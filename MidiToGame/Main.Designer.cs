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
            helpToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            tracksToolStripMenuItem = new ToolStripMenuItem();
            playToolStripMenuItem = new ToolStripMenuItem();
            label1 = new Label();
            listBox1 = new ListBox();
            listContextMenu = new ContextMenuStrip(components);
            removeToolStripMenuItem = new ToolStripMenuItem();
            playToolStripMenuItem1 = new ToolStripMenuItem();
            stopText = new Label();
            menuStrip1.SuspendLayout();
            listContextMenu.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, tracksToolStripMenuItem, playToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(466, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, removeFileToolStripMenuItem, selectProcessToolStripMenuItem, mapKeysToolStripMenuItem, helpToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            fileToolStripMenuItem.Click += fileToolStripMenuItem_Click;
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(148, 22);
            openToolStripMenuItem.Text = "Load File";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click_1;
            // 
            // removeFileToolStripMenuItem
            // 
            removeFileToolStripMenuItem.Enabled = false;
            removeFileToolStripMenuItem.Name = "removeFileToolStripMenuItem";
            removeFileToolStripMenuItem.Size = new Size(148, 22);
            removeFileToolStripMenuItem.Text = "Remove File";
            removeFileToolStripMenuItem.Click += removeFileToolStripMenuItem_Click;
            // 
            // selectProcessToolStripMenuItem
            // 
            selectProcessToolStripMenuItem.Name = "selectProcessToolStripMenuItem";
            selectProcessToolStripMenuItem.Size = new Size(148, 22);
            selectProcessToolStripMenuItem.Text = "Select Process";
            selectProcessToolStripMenuItem.Click += selectProcessToolStripMenuItem_Click;
            // 
            // mapKeysToolStripMenuItem
            // 
            mapKeysToolStripMenuItem.Name = "mapKeysToolStripMenuItem";
            mapKeysToolStripMenuItem.Size = new Size(148, 22);
            mapKeysToolStripMenuItem.Text = "Map Keys";
            mapKeysToolStripMenuItem.Click += mapKeysToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(148, 22);
            helpToolStripMenuItem.Text = "Help";
            helpToolStripMenuItem.Click += helpToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(148, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // tracksToolStripMenuItem
            // 
            tracksToolStripMenuItem.Enabled = false;
            tracksToolStripMenuItem.Name = "tracksToolStripMenuItem";
            tracksToolStripMenuItem.Size = new Size(52, 20);
            tracksToolStripMenuItem.Text = "Tracks";
            tracksToolStripMenuItem.Click += tracksToolStripMenuItem_Click;
            // 
            // playToolStripMenuItem
            // 
            playToolStripMenuItem.Enabled = false;
            playToolStripMenuItem.Name = "playToolStripMenuItem";
            playToolStripMenuItem.Size = new Size(41, 20);
            playToolStripMenuItem.Text = "Play";
            playToolStripMenuItem.Click += playToolStripMenuItem_Click;
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
            listBox1.ContextMenuStrip = listContextMenu;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(0, 26);
            listBox1.Name = "listBox1";
            listBox1.ScrollAlwaysVisible = true;
            listBox1.Size = new Size(466, 379);
            listBox1.TabIndex = 2;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            listBox1.DragDrop += listBox1_DragDrop;
            listBox1.DoubleClick += listBox1_DoubleClick;
            // 
            // listContextMenu
            // 
            listContextMenu.Items.AddRange(new ToolStripItem[] { removeToolStripMenuItem });
            listContextMenu.Name = "listContextMenu";
            listContextMenu.Size = new Size(181, 48);
            // 
            // removeToolStripMenuItem
            // 
            removeToolStripMenuItem.Enabled = false;
            removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            removeToolStripMenuItem.Size = new Size(180, 22);
            removeToolStripMenuItem.Text = "Remove";
            removeToolStripMenuItem.Click += removeToolStripMenuItem_Click;
            // 
            // playToolStripMenuItem1
            // 
            playToolStripMenuItem1.Name = "playToolStripMenuItem1";
            playToolStripMenuItem1.Size = new Size(149, 22);
            playToolStripMenuItem1.Text = "Play";
            // 
            // stopText
            // 
            stopText.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            stopText.AutoSize = true;
            stopText.Location = new Point(322, 415);
            stopText.Name = "stopText";
            stopText.Size = new Size(132, 15);
            stopText.TabIndex = 3;
            stopText.Text = "Press backspace to stop";
            stopText.Visible = false;
            // 
            // Main
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(466, 439);
            Controls.Add(stopText);
            Controls.Add(listBox1);
            Controls.Add(label1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Main";
            Text = "Main";
            DragDrop += Main_DragDrop;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            listContextMenu.ResumeLayout(false);
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
        private ToolStripMenuItem playToolStripMenuItem1;
        private ToolStripMenuItem removeFileToolStripMenuItem;
        private Label stopText;
        private ToolStripMenuItem tracksToolStripMenuItem;
        private ContextMenuStrip listContextMenu;
        private ToolStripMenuItem removeToolStripMenuItem;
    }
}