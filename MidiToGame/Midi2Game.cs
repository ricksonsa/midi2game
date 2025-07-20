using NAudio.Midi;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MidiToGame
{
    public class Midi2Game
    {
        public int BpmToMidiTempo(double bpm)
        {
            if (bpm <= 0)
                throw new ArgumentOutOfRangeException(nameof(bpm), "BPM must be greater than 0.");

            return (int)(60000000 / bpm);
        }

        public double GetTempo(MidiFile midiFile)
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

        public Dictionary<int, byte> noteToKey = new()
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

        public Dictionary<int, byte> octaveKeys = new()
        {
            { 0, (byte)ConsoleKey.F },
            { 1, (byte)ConsoleKey.R }
        };

        public void PressKey(byte key) => keybd_event(key, 0, 0, UIntPtr.Zero);
        public void ReleaseKey(byte key) => keybd_event(key, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
        public async Task Play(string file, CancellationToken token, Dictionary<int, byte> noteToKeyValue, Dictionary<int, byte> octaveKeysValue, int trackNumber = -1)
        {
            try
            {
                noteToKey = noteToKeyValue;
                octaveKeys = octaveKeysValue;
                var midiFile = new MidiFile(file, false);

                double tempo = GetTempo(midiFile); // in microseconds per quarter
                double bpm = 60000000.0 / tempo;

                var notes = ExtractNotes(midiFile, tempo, trackNumber, token);
                int baseNote = 60; // Middle C (C5)

                var stopwatch = Stopwatch.StartNew();
                var tasks = new List<Task>();

                foreach (var (startMs, durationMs, noteNumber) in notes)
                {
                    token.ThrowIfCancellationRequested();
                    int semitoneOffset = noteNumber - baseNote;
                    int octaveShift = semitoneOffset / 12;
                    int noteInOctave = semitoneOffset % 12;
                    if (noteInOctave < 0) noteInOctave += 12;

                    if (!noteToKey.TryGetValue(noteInOctave, out byte key))
                        continue;

                    var task = Task.Run(async () =>
                    {
                        await Task.Delay((int)startMs, token);
                        if (token.IsCancellationRequested) return;

                        for (int i = 0; i < Math.Abs(octaveShift); i++)
                        {
                            if (octaveShift > 0)
                                PressKey(octaveKeys[1]);
                            else
                                PressKey(octaveKeys[0]);
                        }

                        PressKey(key);
                        await Task.Delay((int)durationMs, token);
                        ReleaseKey(key);

                        for (int i = 0; i < Math.Abs(octaveShift); i++)
                        {
                            if (token.IsCancellationRequested) return;
                            if (octaveShift > 0)
                                ReleaseKey(octaveKeys[1]);
                            else
                                ReleaseKey(octaveKeys[0]);
                        }
                    });
                    tasks.Add(task);
                }

                await Task.WhenAll(tasks);
            }
            catch (TaskCanceledException) { }
        }

        public List<(double startMs, double durationMs, int noteNumber)> ExtractNotes(MidiFile midiFile, double tempo, int trackNumber, CancellationToken cancellationToken)
        {
            var notes = new List<(double, double, int)>();
            var ticksPerQuarter = midiFile.DeltaTicksPerQuarterNote;
            var activeNotes = new Dictionary<int, long>();
            long absoluteTime = 0;

            trackNumber = Math.Clamp(trackNumber, 0, midiFile.Events.Count());

            if (trackNumber == -1)
            {
                foreach (var track in midiFile.Events)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    absoluteTime = ProcessNotes(tempo, notes, ticksPerQuarter, activeNotes, track, cancellationToken);
                }
            }
            else
            {
                absoluteTime = ProcessNotes(tempo, notes, ticksPerQuarter, activeNotes, midiFile.Events[trackNumber], cancellationToken);
            }

            return notes;
        }

        private static long ProcessNotes(double tempo, List<(double, double, int)> notes, int ticksPerQuarter, Dictionary<int, long> activeNotes, IList<MidiEvent>? track, CancellationToken cancellationToken)
        {
            long absoluteTime = 0;
            foreach (var midiEvent in track)
            {
                cancellationToken.ThrowIfCancellationRequested();

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

            return absoluteTime;
        }
    }
}
