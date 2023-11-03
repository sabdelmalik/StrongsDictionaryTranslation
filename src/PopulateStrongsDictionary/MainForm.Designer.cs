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
            menuStrip1 = new MenuStrip();
            updateToolStripMenuItem = new ToolStripMenuItem();
            rebuildReferencesToolStripMenuItem = new ToolStripMenuItem();
            transliterationToolStripMenuItem = new ToolStripMenuItem();
            toolStrip1.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // traceBox
            // 
            traceBox.Dock = DockStyle.Fill;
            traceBox.Location = new Point(0, 55);
            traceBox.Name = "traceBox";
            traceBox.Size = new Size(800, 395);
            traceBox.TabIndex = 1;
            traceBox.Text = "";
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new Size(20, 20);
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripButtonSettings, toolStripButtonBuild });
            toolStrip1.Location = new Point(0, 28);
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
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { updateToolStripMenuItem, rebuildReferencesToolStripMenuItem, transliterationToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 28);
            menuStrip1.TabIndex = 3;
            menuStrip1.Text = "menuStrip1";
            // 
            // updateToolStripMenuItem
            // 
            updateToolStripMenuItem.Name = "updateToolStripMenuItem";
            updateToolStripMenuItem.Size = new Size(72, 24);
            updateToolStripMenuItem.Text = "Update";
            updateToolStripMenuItem.Click += updateToolStripMenuItem_Click;
            // 
            // rebuildReferencesToolStripMenuItem
            // 
            rebuildReferencesToolStripMenuItem.Name = "rebuildReferencesToolStripMenuItem";
            rebuildReferencesToolStripMenuItem.Size = new Size(150, 24);
            rebuildReferencesToolStripMenuItem.Text = "Rebuild References";
            rebuildReferencesToolStripMenuItem.Click += rebuildReferencesToolStripMenuItem_Click;
            // 
            // transliterationToolStripMenuItem
            // 
            transliterationToolStripMenuItem.Name = "transliterationToolStripMenuItem";
            transliterationToolStripMenuItem.Size = new Size(117, 24);
            transliterationToolStripMenuItem.Text = "Transliteration";
            transliterationToolStripMenuItem.Click += transliterationToolStripMenuItem_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(traceBox);
            Controls.Add(toolStrip1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "MainForm";
            Text = "Populate Strongs Dictionary";
            Load += MainForm_Load;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private RichTextBox traceBox;
        private FolderBrowserDialog folderBrowserDialog1;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButtonSettings;
        private ToolStripButton toolStripButtonBuild;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem updateToolStripMenuItem;
        private ToolStripMenuItem rebuildReferencesToolStripMenuItem;
        private ToolStripMenuItem transliterationToolStripMenuItem;
    }
}