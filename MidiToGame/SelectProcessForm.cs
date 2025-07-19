using System.Diagnostics;

namespace MidiToGame
{
    public partial class SelectProcessForm : Form
    {
        public List<Process> Processes = [];
        public event EventHandler<string> OnProcessSelected;

        public SelectProcessForm()
        {
            InitializeComponent();
            Processes = [.. Process.GetProcesses()];
            FilterProcesses(string.Empty);

        }

        private void FilterProcesses(string filter)
        {
            if (!string.IsNullOrEmpty(filter) || !string.IsNullOrWhiteSpace(filter))
            {
                Processes = Processes.Where(p => p.ProcessName.ToLower().StartsWith(filter.ToLower())).ToList();
            }
            else
            {
                Processes = Process.GetProcesses().ToList();
            }

            foreach (var process in Processes.DistinctBy(p => p.ProcessName).OrderBy(p => p.ProcessName))
            {
                listBox1.Items.Add(process.ProcessName);
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {

            listBox1.Items.Clear();
            FilterProcesses(textBox1.Text);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnProcessSelected?.Invoke(listBox1.SelectedItem, null);
        }
    }
}
