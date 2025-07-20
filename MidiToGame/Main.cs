using Midi2Game;
using NAudio.Midi;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MidiToGame
{
    public partial class Main : Form
    {
        public static Main instance;
        private List<string> FilePaths = [];
        public int Track = -1;
        Process? SelectedProcess;
        Midi2Game Midi2Game = new();
        Task Midi2GameTask;
        bool IsPlaying = false;
        CancellationTokenSource CancellationTokenSource = new();
        System.Windows.Forms.Timer Timer = new System.Windows.Forms.Timer();

        // Hook handle
        private static IntPtr _hookID = IntPtr.Zero;
        private static LowLevelKeyboardProc _proc = HookCallback;

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
            { 1, (byte)ConsoleKey.R },
        };

        private delegate IntPtr LowLevelKeyboardProc(
       int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            const int WM_KEYDOWN = 0x0100;

            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                // Detect Backspace key
                if (vkCode == (int)Keys.Back)
                {
                    Main.instance.Stop();
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        // Windows API imports and constants
        private const int WH_KEYBOARD_LL = 13;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk,
            int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        public Main()
        {
            instance = this;
            InitializeComponent();
            listBox1.Items.Clear();

            Timer.Interval = 200;
            Timer.Tick += Timer_Tick;
            Timer.Start();

            this.DragEnter += MainForm_DragEnter;
            this.DragDrop += MainForm_DragDrop;

            CreateKeysFileIfNotExists();
            CreateSongsFileOrLoadIt();

            _hookID = SetHook(_proc);

        }

        // Unhook when form closes
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            UnhookWindowsHookEx(_hookID);
            base.OnFormClosing(e);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule!)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }
        private void CreateSongsFileOrLoadIt()
        {

            try
            {
                var lines = new List<string>();

                using (StreamReader reader = new("files"))
                {
                    while (reader.ReadLine() is { } line)
                    {
                        if (!File.Exists(line)) continue;
                        FilePaths.Add(line);
                        listBox1.Items.Add(Path.GetFileName(line));
                    }
                }

            }
            catch (Exception ex)
            {
                FilePaths.Clear();
            }

        }

        private void CreateKeysFileIfNotExists()
        {
            if (!File.Exists("keys"))
            {
                CreateKeysFile();
            }
            else
            {
                var lines = new List<string>();

                using (StreamReader reader = new("keys"))
                {
                    while (reader.ReadLine() is { } line)
                    {
                        lines.Add(line);
                    }
                }

                try
                {
                    SetKeysFromList(lines);
                }
                catch (Exception)
                {
                    File.Delete("keys");
                    CreateKeysFile();
                    return;
                }

            }
        }

        private void CreateKeysFile()
        {
            List<string> lines = [];
            lines.Add($"0=T");
            lines.Add($"1=Y");
            lines.Add($"2=U");
            lines.Add($"3=I");
            lines.Add($"4=O");
            lines.Add($"5=B");
            lines.Add($"6=H");
            lines.Add($"7=J");
            lines.Add($"8=K");
            lines.Add($"9=L");
            lines.Add($"10=X");
            lines.Add($"11=N");
            lines.Add($"OctaveUp=R");
            lines.Add($"OctaveDown=F");

            using (FileStream fs = File.Create("keys"))
            using (StreamWriter writer = new(fs))
            {
                foreach (var line in lines)
                {
                    writer.WriteLine(line);
                }
            }

            noteToKey = new()
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

            octaveKeys = new()
                {
                    { 0, (byte)ConsoleKey.F },
                    { 1, (byte)ConsoleKey.R },
                };
        }

        private void SetKeysFromList(List<string> lines)
        {
            noteToKey[0] = KeyHelper.GetKeyByte(GetNoteLine(lines, "0"));
            noteToKey[1] = KeyHelper.GetKeyByte(GetNoteLine(lines, "1"));
            noteToKey[2] = KeyHelper.GetKeyByte(GetNoteLine(lines, "2"));
            noteToKey[3] = KeyHelper.GetKeyByte(GetNoteLine(lines, "3"));
            noteToKey[4] = KeyHelper.GetKeyByte(GetNoteLine(lines, "4"));
            noteToKey[5] = KeyHelper.GetKeyByte(GetNoteLine(lines, "5"));
            noteToKey[6] = KeyHelper.GetKeyByte(GetNoteLine(lines, "6"));
            noteToKey[7] = KeyHelper.GetKeyByte(GetNoteLine(lines, "7"));
            noteToKey[8] = KeyHelper.GetKeyByte(GetNoteLine(lines, "8"));
            noteToKey[9] = KeyHelper.GetKeyByte(GetNoteLine(lines, "9"));
            noteToKey[10] = KeyHelper.GetKeyByte(GetNoteLine(lines, "10"));
            noteToKey[11] = KeyHelper.GetKeyByte(GetNoteLine(lines, "11"));

            octaveKeys[0] = KeyHelper.GetKeyByte(GetNoteLine(lines, "OctaveDown"));
            octaveKeys[1] = KeyHelper.GetKeyByte(GetNoteLine(lines, "OctaveUp"));
        }

        private string GetNoteLine(List<string> lines, string value)
        {
            try
            {
                var line = lines.FirstOrDefault(line => line.StartsWith(value));
                if (line is null)
                {
                    File.Delete("keys");
                    CreateKeysFileIfNotExists();
                    throw new Exception("Invalid key value");
                }
                return line.Split("=")[1];
            }
            catch (Exception)
            {
                throw new Exception("Invalid key value");
            }

        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (Midi2GameTask == null || Midi2GameTask.IsFaulted || Midi2GameTask.IsCompleted)
            {
                HasStopped();
                return;
            }
        }

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        internal bool BringWindowToForeground()
        {
            if (SelectedProcess is null) return false;
            Process[] procs = Process.GetProcessesByName(SelectedProcess.ProcessName);

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

        private void HasStarted()
        {
            IsPlaying = true;
            playToolStripMenuItem.Text = "Stop";
            stopText.Visible = true;
        }

        private void HasStopped()
        {
            Text = $"Midi2Game";
            IsPlaying = false;
            playToolStripMenuItem.Text = "Play";
            stopText.Visible = false;
        }

        private void Play(string file)
        {
            if (!IsPlaying)
            {
                if (SelectedProcess == null)
                {
                    SelectProcessPrompt();
                    return;
                }

                Text = $"Midi2Game - Playing {Path.GetFileName(file)}";
                HasStarted();

                if (SelectedProcess != null)
                {
                    BringWindowToForeground();
                    Thread.Sleep(600);
                }

                Midi2GameTask = Midi2Game.PlayAsync(file, Track, noteToKey, octaveKeys, CancellationTokenSource.Token);
            }
            else
            {
                Stop();
            }

        }

        private void Stop()
        {
            CancellationTokenSource.Cancel();
            CancellationTokenSource = new CancellationTokenSource();
            Midi2Game.ReleaseAllKeys();
            HasStopped();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void openToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "*.mid|";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                foreach (var filePath in openFileDialog.FileNames)
                {
                    AddFile(filePath);
                }
            }
        }

        private void AddFile(string filePath)
        {
            if (FilePaths.Contains(filePath)) { return; }
            FilePaths.Add(filePath);
            listBox1.Items.Add(Path.GetFileName(filePath));

            File.WriteAllText("files", string.Join(Environment.NewLine, FilePaths));
        }

        private void selectProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectProcessPrompt();
        }

        private void SelectProcessPrompt()
        {
            var selectProcessForm = new SelectProcessForm();
            selectProcessForm.OnProcessSelected += (file, e) =>
            {
                SelectedProcess = Process.GetProcessesByName(file.ToString()).FirstOrDefault();
                label1.Text = $"Binded to process: {SelectedProcess.ProcessName}";
                selectProcessForm.Close();
            };
            selectProcessForm.ShowDialog(this);
        }

        private void SelectProcessForm_OnProcessSelected(object? sender, string e)
        {
            if (sender is null) return;
            SelectedProcess = Process.GetProcessesByName(sender.ToString()).FirstOrDefault();
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            Debug.WriteLine(sender.ToString());
        }

        private void Main_DragDrop(object sender, DragEventArgs e)
        {
            Debug.WriteLine(sender.ToString());

        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            // Check if the data contains files
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Allow copy effect
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            // Get the dropped file paths
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                if (fileInfo.Extension != ".mid") continue;
                AddFile(file);
            }
        }

        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var index = listBox1.SelectedIndex;
            if (index == -1) return;
            Play(FilePaths[index]);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Track = -1;
            selectTrackToolStripMenuItem.Text = "All Tracks";
            removeFileToolStripMenuItem.Enabled = true;
            selectTrackToolStripMenuItem.Enabled = true;
            var index = listBox1.SelectedIndex;
            if (index == -1) return;
            var midiFile = new MidiFile(FilePaths[index], false);
            tracksComboBox.Items.Clear();
            tracksComboBox.Items.Add("All Tracks");
            for (int i = 0; i < midiFile.Events.Count(); i++)
            {
                tracksComboBox.Items.Add($"Track - {i}");
            }
        }

        private void tracksComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = tracksComboBox.SelectedItem;

            if (selected == null || selected.ToString() == "All Tracks")
            {
                Track = -1;
                return;
            }

            Track = int.Parse(selected.ToString()!.Split("- ")[1]);
        }

        private void removeFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var index = listBox1.SelectedIndex;
            if (index == -1) return;
            listBox1.Items.RemoveAt(index);
            FilePaths.RemoveAt(index);
        }

        private void mapKeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var mapKeyForm = new MapKeyForm();
            mapKeyForm.OnKeysMapped += (config, e) =>
            {
                var keys = config as List<string>;
                SetKeysFromList(keys!);
                mapKeyForm.Close();
            };
            mapKeyForm.ShowDialog(this);
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var helpForm = new HelpForm();
            helpForm.ShowDialog(this);
        }
    }
}
