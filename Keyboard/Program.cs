using NAudio.Midi;
using System.Diagnostics;
using System.Runtime.InteropServices;

class Program
{
    public static int BpmToMidiTempo(double bpm)
    {
        if (bpm <= 0)
            throw new ArgumentOutOfRangeException(nameof(bpm), "BPM must be greater than 0.");

        return (int)(60000000 / bpm);
    }

    [DllImport("user32.dll")]
    static extern bool SetForegroundWindow(IntPtr hWnd);
    internal static bool BringWindowToForeground()
    {
        Process[] procs = Process.GetProcessesByName("SCUM");

        if (procs.Length > 0)
        {
            if (procs[0].MainWindowHandle != IntPtr.Zero)
            {
                SetForegroundWindow(procs[0].MainWindowHandle);
                return true;
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    static double GetTempo(MidiFile midiFile)
    {
        foreach (var track in midiFile.Events)
        {
            foreach (var midiEvent in track)
            {
                if (midiEvent is TempoEvent tempoEvent)
                {
                    return tempoEvent.MicrosecondsPerQuarterNote;
                }
            }
        }

        return 500000.0; // Default tempo if none is specified (120 BPM)
    }

    [DllImport("user32.dll")]
    private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
    const int KEYEVENTF_KEYUP = 0x0002;

    static Dictionary<int, byte> noteToKey = new()
    {
        { 0, (byte)ConsoleKey.T },
        { 1, (byte)ConsoleKey.Y },
        { 2, (byte)ConsoleKey.U },
        { 3, (byte)ConsoleKey.I },
        { 4, (byte)ConsoleKey.O },
        { 5, (byte)ConsoleKey.B },
        { 6, (byte)ConsoleKey.H },
        { 7, (byte)ConsoleKey.J },
        { 8, (byte)ConsoleKey.K },
        { 9, (byte)ConsoleKey.L },
        { 10, (byte)ConsoleKey.X },
        { 11, (byte)ConsoleKey.N }
    };
    static byte octaveUpKey = (byte)ConsoleKey.R;
    static byte octaveDownKey = (byte)ConsoleKey.F;

    static void PressKey(byte key) => keybd_event(key, 0, 0, UIntPtr.Zero);
    static void ReleaseKey(byte key) => keybd_event(key, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
    static async Task Main(string[] args)
    {
        await Task.Delay(3000);
        var midiFile = new MidiFile("wonderwall.mid", false);

        double tempo = GetTempo(midiFile); // in microseconds per quarter
        double bpm = 60000000.0 / tempo;
        Console.WriteLine($"Detected BPM: {bpm:0.##}");

        var notes = ExtractNotes(midiFile, tempo);
        int baseNote = 60; // Middle C (C5)



        var stopwatch = Stopwatch.StartNew();

        foreach (var (startMs, durationMs, noteNumber) in notes)
        {
            int semitoneOffset = noteNumber - baseNote;
            int octaveShift = semitoneOffset / 12;
            int noteInOctave = semitoneOffset % 12;
            if (noteInOctave < 0) noteInOctave += 12;

            if (!noteToKey.TryGetValue(noteInOctave, out byte key))
                continue;

            _ = Task.Run(async () =>
            {
                await Task.Delay((int)startMs);

                for (int i = 0; i < Math.Abs(octaveShift); i++)
                {
                    if (octaveShift > 0)
                        PressKey(octaveUpKey);
                    else
                        PressKey(octaveDownKey);
                }

                PressKey(key);
                await Task.Delay((int)durationMs);
                ReleaseKey(key);

                for (int i = 0; i < Math.Abs(octaveShift); i++)
                {
                    if (octaveShift > 0)
                        ReleaseKey(octaveUpKey);
                    else
                        ReleaseKey(octaveDownKey);
                }
            });
        }

        Console.WriteLine("Playing... Press any key to exit.");
        Console.ReadKey();
    }

    static List<(double startMs, double durationMs, int noteNumber)> ExtractNotes(MidiFile midiFile, double tempo)
    {
        var notes = new List<(double, double, int)>();
        var ticksPerQuarter = midiFile.DeltaTicksPerQuarterNote;
        var activeNotes = new Dictionary<int, long>();
        long absoluteTime = 0;

        foreach (var track in midiFile.Events)
        {
            absoluteTime = 0;
            foreach (var midiEvent in track)
            {
                absoluteTime += midiEvent.DeltaTime;

                if (midiEvent is NoteOnEvent noteOn && noteOn.Velocity > 0)
                {
                    activeNotes[noteOn.NoteNumber] = absoluteTime;
                }
                else if (midiEvent is NoteEvent noteEvent)
                {
                    if (noteEvent.CommandCode == MidiCommandCode.NoteOff ||
                        (noteEvent is NoteOnEvent noteOnOff && noteOnOff.Velocity == 0))
                    {
                        if (activeNotes.TryGetValue(noteEvent.NoteNumber, out var startTime))
                        {
                            double startMs = (startTime * tempo) / (ticksPerQuarter * 1000);
                            double endMs = (absoluteTime * tempo) / (ticksPerQuarter * 1000);
                            notes.Add((startMs, endMs - startMs, noteEvent.NoteNumber));
                            activeNotes.Remove(noteEvent.NoteNumber);
                        }
                    }
                }
            }
        }

        return notes;
    }
}
