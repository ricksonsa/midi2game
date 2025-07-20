namespace Midi2Game
{
    partial class HelpForm
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
            label1 = new Label();
            label2 = new Label();
            linkLabel2 = new LinkLabel();
            label3 = new Label();
            linkLabel3 = new LinkLabel();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(21, 9);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 0;
            label1.Text = "label1";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(21, 168);
            label2.Name = "label2";
            label2.Size = new Size(838, 15);
            label2.TabIndex = 2;
            label2.Text = "If you find this project useful or would like to help me improve it further, your support on Patreon makes a big difference. Even small contributions helps a lot.";
            // 
            // linkLabel2
            // 
            linkLabel2.AutoSize = true;
            linkLabel2.Location = new Point(21, 183);
            linkLabel2.Name = "linkLabel2";
            linkLabel2.Size = new Size(199, 15);
            linkLabel2.TabIndex = 3;
            linkLabel2.TabStop = true;
            linkLabel2.Text = "https://www.patreon.com/ricksonsa";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(21, 204);
            label3.Name = "label3";
            label3.Size = new Size(346, 15);
            label3.TabIndex = 4;
            label3.Text = "For more information or if you want to contribute to the project:";
            // 
            // linkLabel3
            // 
            linkLabel3.AutoSize = true;
            linkLabel3.Location = new Point(364, 204);
            linkLabel3.Name = "linkLabel3";
            linkLabel3.Size = new Size(228, 15);
            linkLabel3.TabIndex = 3;
            linkLabel3.TabStop = true;
            linkLabel3.Text = "https://github.com/ricksonsa/midi2game";
            // 
            // HelpForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(863, 254);
            Controls.Add(label3);
            Controls.Add(linkLabel3);
            Controls.Add(linkLabel2);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "HelpForm";
            Text = "Midi2Game - Help";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private LinkLabel linkLabel2;
        private Label label3;
        private LinkLabel linkLabel3;
    }
}