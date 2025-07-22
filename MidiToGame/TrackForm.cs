namespace Midi2Game
{
    public partial class TrackForm : Form
    {
        public event EventHandler<List<int>> OnTrackSelected;
        public List<CheckBox> CheckBoxes { get; set; } = [];
        public TrackForm(Dictionary<int, bool> selectedTracks)
        {
            InitializeComponent();

            flowLayoutPanel1.WrapContents = false;
            flowLayoutPanel1.AutoScroll = true;
            CenterToParent();

            foreach (var track in selectedTracks.Keys)
            {
                CheckBoxes.Add(BuildCheckbox(track, selectedTracks[track]));
            }
        }

        private CheckBox BuildCheckbox(int track, bool check)
        {
            var checkbox = new CheckBox();
            checkbox.Name = $"{track}";
            checkbox.Text = $"Track - {track}";
            checkbox.Checked = check;
            flowLayoutPanel1.Controls.Add(checkbox);
            return checkbox;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Dictionary<int, bool> selected = [];
            foreach (var checkbox in CheckBoxes)
            {
                selected[int.Parse(checkbox.Name)] = checkbox.Checked;
            }

            OnTrackSelected?.Invoke(selected, null);
            Close();
        }
    }
}
