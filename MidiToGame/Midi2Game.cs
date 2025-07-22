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
                .ThenBy(g => g.Key)
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
                        return tempoEvent.MicrosecondsPerQuarterNote;
                }
            }
            return 500000.0; // Default tempo (120 BPM)
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
            Dictionary<int, bool> tracks,
            Dictionary<int, byte> noteToKeyValue,
            Dictionary<int, byte> octaveKeysValue,
            CancellationToken token)
        {
            noteToKey = noteToKeyValue;
            octaveKeys = octaveKeysValue;

            var midiFile = new MidiFile(file, false);
            double tempo = GetTempo(midiFile);
            var notes = ExtractNotes(midiFile, tempo, tracks.Where(t => t.Value).Select(t => t.Key), token);

            const int baseNote = 60;

            var tasks = new List<Task>();
            var pressedKeys = new HashSet<byte>();
            var pressedKeysLock = new object();

            await ResetOctavePosition();

            foreach (var (startMs, durationMs, noteNumber) in notes)
            {
                int semitoneOffset = noteNumber - baseNote;
                int octaveShift = semitoneOffset / 12;
                int noteInOctave = semitoneOffset % 12;
                if (noteInOctave < 0) noteInOctave += 12;

                if (!noteToKey.TryGetValue(noteInOctave, out byte key))
                    continue;

                var playNoteTask = Task.Run(async () =>
                {
                    try
                    {
                        if (startMs > 0) await Task.Delay((int)startMs, token);

                        for (int i = 0; i < Math.Abs(octaveShift); i++)
                        {
                            if (token.IsCancellationRequested) return;

                            if (octaveShift > 0)
                            {
                                PressKey(octaveKeys[1]); await Task.Delay(5); ReleaseKey(octaveKeys[1]);
                            }
                            else
                            {
                                PressKey(octaveKeys[0]); await Task.Delay(5); ReleaseKey(octaveKeys[0]);
                            }
                        }

                        lock (pressedKeysLock)
                        {
                            ReleaseKey(key);
                            PressKey(key);
                            pressedKeys.Add(key);
                        }

                        var duration = (int)durationMs < (int)MinResonanceMs ? (int)ResonanceExtensionMs : (int)durationMs;
                        await Task.Delay(duration, token);
                        //ReleaseKey(key);

                        lock (pressedKeysLock)
                        {
                            pressedKeys.Remove(key);
                        }
                    }
                    catch (OperationCanceledException) { }
                    catch (Exception ex) { Debug.WriteLine($"Note task error: {ex.Message}"); }
                }, token);

                tasks.Add(playNoteTask);
            }

            try { await Task.WhenAll(tasks); }
            catch (OperationCanceledException) { }
            catch (Exception ex) { Debug.WriteLine($"Playback error: {ex.Message}"); }
            finally { ReleaseAllKeys(); }
        }

        private async Task ResetOctavePosition()
        {
            for (int i = 0; i < 3; i++)
            {
                PressKey(octaveKeys[0]); await Task.Delay(100); ReleaseKey(octaveKeys[0]); await Task.Delay(100);
            }
        }

        public void ReleaseAllKeys()
        {
            foreach (var key in noteToKey.Values) { try { ReleaseKey(key); } catch { } }
            foreach (var key in octaveKeys.Values) { try { ReleaseKey(key); } catch { } }
        }

        public List<(double startMs, double durationMs, int noteNumber)> ExtractNotes(MidiFile midiFile, double tempo, IEnumerable<int> tracks, CancellationToken cancellationToken)
        {
            var notes = new List<(double, double, int)>();
            var ticksPerQuarter = midiFile.DeltaTicksPerQuarterNote;
            var activeNotes = new Dictionary<int, long>();
            long absoluteTime = 0;

            for (int i = 0; i < midiFile.Events.Count(); i++)
            {
                if (!tracks.Contains(i)) continue;
                absoluteTime = ProcessNotes(tempo, notes, ticksPerQuarter, activeNotes, midiFile.Events[i], cancellationToken);
            }

            return notes;
        }

        const double MinResonanceMs = 2050;
        const double ResonanceExtensionMs = 3000;

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
                    activeNotes[noteOn.NoteNumber] = absoluteTime;
                }
                else if (midiEvent is NoteEvent noteEvent && (noteEvent.CommandCode == MidiCommandCode.NoteOff || (noteEvent is NoteOnEvent on && on.Velocity == 0)))
                {
                    if (activeNotes.TryGetValue(noteEvent.NoteNumber, out var startTime))
                    {
                        double startMs = (startTime * tempo) / (ticksPerQuarter * 1000.0);
                        double endMs = (absoluteTime * tempo) / (ticksPerQuarter * 1000.0);
                        double durationMs = endMs - startMs;

                        if (durationMs < MinResonanceMs)
                            durationMs += ResonanceExtensionMs;

                        notes.Add((startMs, Math.Max(1, durationMs), noteEvent.NoteNumber));
                        activeNotes.Remove(noteEvent.NoteNumber);
                    }
                }
            }

            return absoluteTime;
        }
    }
}
