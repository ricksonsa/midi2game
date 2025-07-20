using NAudio.Midi;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MidiToGame
{
    public class Midi2Game
    {
        private int EstimateBaseNote(List<(double startMs, double durationMs, int noteNumber)> notes)
        {
            return notes
                .GroupBy(n => n.noteNumber)
                .OrderByDescending(g => g.Count())
                .First()
                .Key;
        }

        int EstimateHeldNote(List<(double startMs, double durationMs, int noteNumber)> notes)
        {
            return notes
                .GroupBy(n => n.noteNumber)
                .OrderByDescending(g => g.Sum(n => n.durationMs))
                .First()
                .Key;
        }

        int EstimateBassNote(List<(double startMs, double durationMs, int noteNumber)> notes)
        {
            return notes
                .GroupBy(n => n.noteNumber)
                .OrderByDescending(g => g.Count())
                .ThenBy(g => g.Key) // prefer lower note in tie
                .First()
                .Key;
        }

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

        public async Task PlayAsync(
            string file,
            int trackNumber,
            Dictionary<int, byte> noteToKeyValue,
            Dictionary<int, byte> octaveKeysValue,
            CancellationToken token)
        {
            noteToKey = noteToKeyValue;
            octaveKeys = octaveKeysValue;

            var midiFile = new MidiFile(file, false);
            double tempo = GetTempo(midiFile); // microseconds per quarter note
            var notes = ExtractNotes(midiFile, tempo, trackNumber, token);

            const int baseNote = 60; // Middle C
            const int lagMs = 20;

            var stopwatch = Stopwatch.StartNew();
            var tasks = new List<Task>();
            var pressedKeys = new HashSet<byte>();
            var pressedKeysLock = new object();

            // try reset octave to middle
            await ResetOctavePosition();

            foreach (var (startMs, durationMs, noteNumber) in notes)
            {

                int semitoneOffset = noteNumber - baseNote;
                int octaveShift = semitoneOffset / 12;
                int noteInOctave = semitoneOffset % 12;
                if (noteInOctave < 0) noteInOctave += 12;

                if (!noteToKey.TryGetValue(noteInOctave, out byte key))
                    continue;

                var task = Task.Run(async () =>
                {
                    try
                    {
                        double playTime = startMs - lagMs;
                        double delay = playTime - stopwatch.Elapsed.TotalMilliseconds;
                        if (delay > 0)
                            await Task.Delay((int)delay, token);

                        // Shift octave
                        for (int i = 0; i < Math.Abs(octaveShift); i++)
                        {
                            if (token.IsCancellationRequested) return;

                            if (octaveShift > 0)
                            {
                                ReleaseKey(octaveKeys[0]);
                                ReleaseKey(octaveKeys[1]);
                                PressKey(octaveKeys[1]);
                            }
                            else if (octaveShift < 0)
                            {
                                ReleaseKey(octaveKeys[1]);
                                ReleaseKey(octaveKeys[0]);
                                PressKey(octaveKeys[0]);
                            }
                        }

                        lock (pressedKeysLock)
                        {
                            ReleaseKey(key); // always release before pressing to retrigger
                            PressKey(key);
                            pressedKeys.Add(key);
                        }

                        await Task.Delay((int)durationMs, token);

                        ReleaseKey(key);
                        lock (pressedKeysLock)
                        {
                            pressedKeys.Remove(key);
                        }

                        // Revert octave
                        for (int i = 0; i < Math.Abs(octaveShift); i++)
                        {
                            if (token.IsCancellationRequested) return;

                            if (octaveShift > 0)
                                ReleaseKey(octaveKeys[1]);
                            else if (octaveShift < 0)
                                ReleaseKey(octaveKeys[0]);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        // Task canceled, clean exit
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Note task error: {ex.Message}");
                    }
                }, token);

                tasks.Add(task);
            }

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (OperationCanceledException)
            {
                // Swallow expected cancellation
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Playback error: {ex.Message}");
            }
            finally
            {
                ReleaseAllKeys();
            }
        }

        private async Task ResetOctavePosition()
        {
            PressKey(octaveKeys[0]);
            await Task.Delay(100);
            ReleaseKey(octaveKeys[0]);
            await Task.Delay(100);
            PressKey(octaveKeys[0]);
            await Task.Delay(100);
            PressKey(octaveKeys[1]);
            await Task.Delay(100);
            PressKey(octaveKeys[1]);
        }

        public void ReleaseAllKeys()
        {

            foreach (var d_key in noteToKey.Keys)
            {
                try
                {
                    ReleaseKey(noteToKey[d_key]);

                }
                catch (Exception) { }
            }

            foreach (var d_key in octaveKeys.Keys)
            {
                try
                {
                    ReleaseKey(octaveKeys[d_key]);
                }
                catch (Exception) { }
            }
        }

        public List<(double startMs, double durationMs, int noteNumber)> ExtractNotes(MidiFile midiFile, double tempo, int trackNumber, CancellationToken cancellationToken)
        {
            var notes = new List<(double, double, int)>();
            var ticksPerQuarter = midiFile.DeltaTicksPerQuarterNote;
            var activeNotes = new Dictionary<int, long>();
            long absoluteTime = 0;

            trackNumber = Math.Clamp(trackNumber, -1, midiFile.Events.Count());

            if (trackNumber == -1)
            {
                foreach (var track in midiFile.Events)
                {
                    if (cancellationToken.IsCancellationRequested) continue;

                    absoluteTime = ProcessNotes(tempo, notes, ticksPerQuarter, activeNotes, track, cancellationToken);
                }
            }
            else
            {
                absoluteTime = ProcessNotes(tempo, notes, ticksPerQuarter, activeNotes, midiFile.Events[trackNumber], cancellationToken);
            }

            return notes;
        }

        const double MinResonanceMs = 2000;  // minimum realistic key press duration
        const double ResonanceExtensionMs = 3000; // amount to add to short notes

        private static long ProcessNotes(
            double tempo,
            List<(double startMs, double durationMs, int noteNumber)> notes,
            int ticksPerQuarter,
            Dictionary<int, long> activeNotes,
            IList<MidiEvent>? track,
            CancellationToken cancellationToken)
        {
            long absoluteTime = 0;

            foreach (var midiEvent in track)
            {
                if (cancellationToken.IsCancellationRequested) continue;

                absoluteTime += midiEvent.DeltaTime;

                if (midiEvent is NoteOnEvent noteOn && noteOn.Velocity > 0)
                {
                    // Mark note start
                    activeNotes[noteOn.NoteNumber] = absoluteTime;
                }
                else if (midiEvent is NoteEvent noteEvent)
                {
                    bool isNoteOff = noteEvent.CommandCode == MidiCommandCode.NoteOff
                                     || (noteEvent is NoteOnEvent offEvent && offEvent.Velocity == 0);

                    if (isNoteOff)
                    {
                        if (activeNotes.TryGetValue(noteEvent.NoteNumber, out var startTime))
                        {
                            double startMs = (startTime * tempo) / (ticksPerQuarter * 1000.0);
                            double endMs = (absoluteTime * tempo) / (ticksPerQuarter * 1000.0);
                            double durationMs = endMs - startMs;

                            if (durationMs < MinResonanceMs)
                            {
                                durationMs += ResonanceExtensionMs;
                            }

                            durationMs = Math.Max(1, durationMs); // ensure duration is valid
                            notes.Add((startMs, durationMs, noteEvent.NoteNumber));
                            activeNotes.Remove(noteEvent.NoteNumber);
                        }
                    }
                }
            }

            return absoluteTime;
        }

    }
}
