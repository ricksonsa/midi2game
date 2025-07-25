﻿namespace Midi2Game
{
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
            label1.Text =
                @"
🔍 MIDI File Loader: Load and list multiple .mid files.
🎯 Game Window Targeting: Select a running Windows process; when playback starts, the game window is brought to focus automatically.
🎼 Track Selection: Choose to play a specific track or all tracks from a MIDI file.
🎹 Custom Key Mapping: Map each semitone (note) to a specific keyboard key.
⬆️⬇️ Octave Shift Support: Define keys to shift octaves up or down during playback.
🕒 Tempo-Aware Playback: Accurate timing with support for tempo changes.
🧠 Async Note Execution: Supports polyphony (melodies and harmonies).
⏸️ Playback Control: Start, stop, and replay playback from the UI.
🛠️ Open Source: Made for the community, by the community.
";
        }
    }
}
