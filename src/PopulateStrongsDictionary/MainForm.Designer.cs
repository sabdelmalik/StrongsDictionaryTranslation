namespace PopulateStrongsDictionary
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            traceBox = new RichTextBox();
            folderBrowserDialog1 = new FolderBrowserDialog();
            toolStrip1 = new ToolStrip();
            toolStripButtonSettings = new ToolStripButton();
            toolStripButtonBuild = new ToolStripButton();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // traceBox
            // 
            traceBox.Dock = DockStyle.Fill;
            traceBox.Location = new Point(0, 0);
            traceBox.Name = "traceBox";
            traceBox.Size = new Size(800, 450);
            traceBox.TabIndex = 1;
            traceBox.Text = "";
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new Size(20, 20);
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripButtonSettings, toolStripButtonBuild });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(800, 27);
            toolStrip1.TabIndex = 2;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonSettings
            // 
            toolStripButtonSettings.BackgroundImage = Properties.Resources.cog_128;
            toolStripButtonSettings.BackgroundImageLayout = ImageLayout.Stretch;
            toolStripButtonSettings.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButtonSettings.Image = Properties.Resources.cog_128;
            toolStripButtonSettings.ImageTransparentColor = Color.Magenta;
            toolStripButtonSettings.Name = "toolStripButtonSettings";
            toolStripButtonSettings.Size = new Size(29, 24);
            toolStripButtonSettings.Text = "toolStripButton1";
            toolStripButtonSettings.ToolTipText = "Settings";
            toolStripButtonSettings.Click += toolStripButtonSettings_Click;
            // 
            // toolStripButtonBuild
            // 
            toolStripButtonBuild.BackColor = SystemColors.Control;
            toolStripButtonBuild.BackgroundImage = Properties.Resources.arrow_37_128;
            toolStripButtonBuild.BackgroundImageLayout = ImageLayout.Stretch;
            toolStripButtonBuild.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButtonBuild.Image = Properties.Resources.arrow_37_128;
            toolStripButtonBuild.ImageTransparentColor = Color.LightSteelBlue;
            toolStripButtonBuild.Name = "toolStripButtonBuild";
            toolStripButtonBuild.Size = new Size(29, 24);
            toolStripButtonBuild.ToolTipText = "Build";
            toolStripButtonBuild.Click += toolStripButtonBuild_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(toolStrip1);
            Controls.Add(traceBox);
            Name = "MainForm";
            Text = "Form1";
            Load += MainForm_Load;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private RichTextBox traceBox;
        private FolderBrowserDialog folderBrowserDialog1;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButtonSettings;
        private ToolStripButton toolStripButtonBuild;
    }
}