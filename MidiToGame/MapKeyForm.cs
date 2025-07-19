namespace MidiToGame
{
    public partial class MapKeyForm : Form
    {
        public event EventHandler<List<string>> OnKeysMapped;
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

        public MapKeyForm()
        {
            InitializeComponent();

            try
            {
                var lines = new List<string>();

                using (StreamReader reader = new("keys"))
                {
                    while (reader.ReadLine() is { } line)
                    {
                        var keyValue = line.Split('=');
                        var c_key = keyValue[0];
                        var value = keyValue[1];

                        if (Enum.TryParse(typeof(Keys), value, true, out object? keyObj) && keyObj is Keys key)
                        {
                            byte keyCode = (byte)key;
                            if (c_key == "OctaveUp")
                            {
                                octaveKeys[1] = keyCode;
                            }
                            else if (c_key == "OctaveDown")
                            {
                                octaveKeys[0] = keyCode;
                            }
                            else
                            {
                                noteToKey[int.Parse(c_key)] = keyCode;
                            }
                        }
                        else
                        {
                            throw new Exception("Invalid Key");
                        }
                        switch (c_key)
                        {
                            case "0":
                                textBox1.Text = key.ToString();
                                break;
                            case "1":
                                textBox2.Text = key.ToString();
                                break;
                            case "2":
                                textBox3.Text = key.ToString();
                                break;
                            case "3":
                                textBox4.Text = key.ToString();
                                break;
                            case "4":
                                textBox5.Text = key.ToString();
                                break;
                            case "5":
                                textBox6.Text = key.ToString();
                                break;
                            case "6":
                                textBox7.Text = key.ToString();
                                break;
                            case "7":
                                textBox8.Text = key.ToString();
                                break;
                            case "8":
                                textBox9.Text = key.ToString();
                                break;
                            case "9":
                                textBox10.Text = key.ToString();
                                break;
                            case "10":
                                textBox11.Text = key.ToString();
                                break;
                            case "11":
                                textBox12.Text = key.ToString();
                                break;
                            case "OctaveUp":
                                octaveUpTextBox.Text = key.ToString();
                                break;
                            case "OctaveDown":
                                octaveDownTextBox.Text = key.ToString();
                                break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                File.Delete("keys");
                throw;
            }
        }

        public void SaveFile()
        {
            if (!File.Exists("keys"))
            {
                File.Create("keys");
            }
            List<string> lines = [];
            lines.Add($"0={textBox1.Text}");
            lines.Add($"1={textBox2.Text}");
            lines.Add($"2={textBox3.Text}");
            lines.Add($"3={textBox4.Text}");
            lines.Add($"4={textBox5.Text}");
            lines.Add($"5={textBox6.Text}");
            lines.Add($"6={textBox7.Text}");
            lines.Add($"7={textBox8.Text}");
            lines.Add($"8={textBox9.Text}");
            lines.Add($"9={textBox10.Text}");
            lines.Add($"10={textBox11.Text}");
            lines.Add($"11={textBox12.Text}");
            lines.Add($"OctaveUp={octaveUpTextBox.Text}");
            lines.Add($"OctaveDown={octaveDownTextBox.Text}");
            File.WriteAllText("keys", string.Join(Environment.NewLine, lines));
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveFile();
            try
            {
                List<string> config = [];
                config.Add($"0={KeyHelper.GetKeyByte(textBox1.Text)}");
                config.Add($"1={KeyHelper.GetKeyByte(textBox2.Text)}");
                config.Add($"2={KeyHelper.GetKeyByte(textBox3.Text)}");
                config.Add($"3={KeyHelper.GetKeyByte(textBox4.Text)}");
                config.Add($"4={KeyHelper.GetKeyByte(textBox5.Text)}");
                config.Add($"5={KeyHelper.GetKeyByte(textBox6.Text)}");
                config.Add($"6={KeyHelper.GetKeyByte(textBox7.Text)}");
                config.Add($"7={KeyHelper.GetKeyByte(textBox8.Text)}");
                config.Add($"8={KeyHelper.GetKeyByte(textBox9.Text)}");
                config.Add($"9={KeyHelper.GetKeyByte(textBox10.Text)}");
                config.Add($"10={KeyHelper.GetKeyByte(textBox11.Text)}");
                config.Add($"11={KeyHelper.GetKeyByte(textBox12.Text)}");

                config.Add($"OctaveDown={KeyHelper.GetKeyByte(octaveDownTextBox.Text)}");
                config.Add($"OctaveUp={KeyHelper.GetKeyByte(octaveUpTextBox.Text)}");
                OnKeysMapped?.Invoke(config, null);
            }
            catch (Exception) { }

        }

    }
}
