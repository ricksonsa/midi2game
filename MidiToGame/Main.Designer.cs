namespace MidiToGame
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            removeFileToolStripMenuItem = new ToolStripMenuItem();
            selectProcessToolStripMenuItem = new ToolStripMenuItem();
            mapKeysToolStripMenuItem = new ToolStripMenuItem();
            selectTrackToolStripMenuItem = new ToolStripMenuItem();
            tracksComboBox = new ToolStripComboBox();
            helpToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            playToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStrip1 = new ContextMenuStrip(components);
            playAllTracksToolStripMenuItem = new ToolStripMenuItem();
            playTrack1ToolStripMenuItem = new ToolStripMenuItem();
            playTrack2ToolStripMenuItem = new ToolStripMenuItem();
            playTrack3ToolStripMenuItem = new ToolStripMenuItem();
            playTracToolStripMenuItem = new ToolStripMenuItem();
            label1 = new Label();
            listBox1 = new ListBox();
            playToolStripMenuItem1 = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, playToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(466, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, removeFileToolStripMenuItem, selectProcessToolStripMenuItem, mapKeysToolStripMenuItem, selectTrackToolStripMenuItem, helpToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(50, 20);
            fileToolStripMenuItem.Text = "Menu";
            fileToolStripMenuItem.Click += fileToolStripMenuItem_Click;
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(180, 22);
            openToolStripMenuItem.Text = "Load File";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click_1;
            // 
            // removeFileToolStripMenuItem
            // 
            removeFileToolStripMenuItem.Enabled = false;
            removeFileToolStripMenuItem.Name = "removeFileToolStripMenuItem";
            removeFileToolStripMenuItem.Size = new Size(180, 22);
            removeFileToolStripMenuItem.Text = "Remove File";
            removeFileToolStripMenuItem.Click += removeFileToolStripMenuItem_Click;
            // 
            // selectProcessToolStripMenuItem
            // 
            selectProcessToolStripMenuItem.Name = "selectProcessToolStripMenuItem";
            selectProcessToolStripMenuItem.Size = new Size(180, 22);
            selectProcessToolStripMenuItem.Text = "Select Process";
            selectProcessToolStripMenuItem.Click += selectProcessToolStripMenuItem_Click;
            // 
            // mapKeysToolStripMenuItem
            // 
            mapKeysToolStripMenuItem.Name = "mapKeysToolStripMenuItem";
            mapKeysToolStripMenuItem.Size = new Size(180, 22);
            mapKeysToolStripMenuItem.Text = "Map Keys";
            mapKeysToolStripMenuItem.Click += mapKeysToolStripMenuItem_Click;
            // 
            // selectTrackToolStripMenuItem
            // 
            selectTrackToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { tracksComboBox });
            selectTrackToolStripMenuItem.Enabled = false;
            selectTrackToolStripMenuItem.Name = "selectTrackToolStripMenuItem";
            selectTrackToolStripMenuItem.Size = new Size(180, 22);
            selectTrackToolStripMenuItem.Text = "Select Track";
            // 
            // tracksComboBox
            // 
            tracksComboBox.DropDownStyle = ComboBoxStyle.Simple;
            tracksComboBox.Name = "tracksComboBox";
            tracksComboBox.Size = new Size(121, 150);
            tracksComboBox.Text = "All Tracks";
            tracksComboBox.SelectedIndexChanged += tracksComboBox_SelectedIndexChanged;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(180, 22);
            helpToolStripMenuItem.Text = "Help";
            helpToolStripMenuItem.Click += helpToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(180, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // playToolStripMenuItem
            // 
            playToolStripMenuItem.Name = "playToolStripMenuItem";
            playToolStripMenuItem.Size = new Size(41, 20);
            playToolStripMenuItem.Text = "Play";
            playToolStripMenuItem.Click += playToolStripMenuItem_Click;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { playAllTracksToolStripMenuItem, playTrack1ToolStripMenuItem, playTrack2ToolStripMenuItem, playTrack3ToolStripMenuItem, playTracToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(150, 114);
            // 
            // playAllTracksToolStripMenuItem
            // 
            playAllTracksToolStripMenuItem.Name = "playAllTracksToolStripMenuItem";
            playAllTracksToolStripMenuItem.Size = new Size(149, 22);
            playAllTracksToolStripMenuItem.Text = "Play All Tracks";
            // 
            // playTrack1ToolStripMenuItem
            // 
            playTrack1ToolStripMenuItem.Name = "playTrack1ToolStripMenuItem";
            playTrack1ToolStripMenuItem.Size = new Size(149, 22);
            playTrack1ToolStripMenuItem.Text = "Play Track 1";
            // 
            // playTrack2ToolStripMenuItem
            // 
            playTrack2ToolStripMenuItem.Name = "playTrack2ToolStripMenuItem";
            playTrack2ToolStripMenuItem.Size = new Size(149, 22);
            playTrack2ToolStripMenuItem.Text = "Play Track 2";
            // 
            // playTrack3ToolStripMenuItem
            // 
            playTrack3ToolStripMenuItem.Name = "playTrack3ToolStripMenuItem";
            playTrack3ToolStripMenuItem.Size = new Size(149, 22);
            playTrack3ToolStripMenuItem.Text = "Play Track 3";
            // 
            // playTracToolStripMenuItem
            // 
            playTracToolStripMenuItem.Name = "playTracToolStripMenuItem";
            playTracToolStripMenuItem.Size = new Size(149, 22);
            playTracToolStripMenuItem.Text = "Play Track 4";
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label1.AutoSize = true;
            label1.Location = new Point(12, 415);
            label1.Name = "label1";
            label1.Size = new Size(0, 15);
            label1.TabIndex = 1;
            // 
            // listBox1
            // 
            listBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(0, 26);
            listBox1.Name = "listBox1";
            listBox1.ScrollAlwaysVisible = true;
            listBox1.Size = new Size(466, 379);
            listBox1.TabIndex = 2;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            listBox1.DragDrop += listBox1_DragDrop;
            // 
            // playToolStripMenuItem1
            // 
            playToolStripMenuItem1.Name = "playToolStripMenuItem1";
            playToolStripMenuItem1.Size = new Size(149, 22);
            playToolStripMenuItem1.Text = "Play";
            // 
            // Main
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(466, 439);
            Controls.Add(listBox1);
            Controls.Add(label1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Main";
            Text = "Main";
            DragDrop += Main_DragDrop;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private Label label1;
        private ListBox listBox1;
        private ToolStripMenuItem selectProcessToolStripMenuItem;
        private ToolStripMenuItem mapKeysToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem playToolStripMenuItem;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem playAllTracksToolStripMenuItem;
        private ToolStripMenuItem playTrack1ToolStripMenuItem;
        private ToolStripMenuItem playTrack2ToolStripMenuItem;
        private ToolStripMenuItem playTrack3ToolStripMenuItem;
        private ToolStripMenuItem playTracToolStripMenuItem;
        private ToolStripMenuItem playToolStripMenuItem1;
        private ToolStripMenuItem removeFileToolStripMenuItem;
        private ToolStripMenuItem selectTrackToolStripMenuItem;
        private ToolStripComboBox tracksComboBox;
    }
}