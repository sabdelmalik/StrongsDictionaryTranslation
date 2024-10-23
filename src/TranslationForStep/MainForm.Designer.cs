namespace TranslationForStep
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
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            grkLexiconStrongsToolStripMenuItem = new ToolStripMenuItem();
            hebLexiconStrongsToolStripMenuItem = new ToolStripMenuItem();
            selectLexiconsFolderToolStripMenuItem = new ToolStripMenuItem();
            openFileDialog1 = new OpenFileDialog();
            traceBox = new RichTextBox();
            folderBrowserDialog1 = new FolderBrowserDialog();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 33);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { grkLexiconStrongsToolStripMenuItem, hebLexiconStrongsToolStripMenuItem, selectLexiconsFolderToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(54, 29);
            fileToolStripMenuItem.Text = "File";
            // 
            // grkLexiconStrongsToolStripMenuItem
            // 
            grkLexiconStrongsToolStripMenuItem.Name = "grkLexiconStrongsToolStripMenuItem";
            grkLexiconStrongsToolStripMenuItem.Size = new Size(286, 34);
            grkLexiconStrongsToolStripMenuItem.Text = "Grk Lexicon Strong's";
            grkLexiconStrongsToolStripMenuItem.Click += GrkLexiconStrongsToolStripMenuItem_Click;
            // 
            // hebLexiconStrongsToolStripMenuItem
            // 
            hebLexiconStrongsToolStripMenuItem.Name = "hebLexiconStrongsToolStripMenuItem";
            hebLexiconStrongsToolStripMenuItem.Size = new Size(286, 34);
            hebLexiconStrongsToolStripMenuItem.Text = "Heb Lexicon Strong's";
            hebLexiconStrongsToolStripMenuItem.Click += hebLexiconStrongsToolStripMenuItem_Click;
            // 
            // selectLexiconsFolderToolStripMenuItem
            // 
            selectLexiconsFolderToolStripMenuItem.Name = "selectLexiconsFolderToolStripMenuItem";
            selectLexiconsFolderToolStripMenuItem.Size = new Size(286, 34);
            selectLexiconsFolderToolStripMenuItem.Text = "Select Lexicons Folder";
            selectLexiconsFolderToolStripMenuItem.Click += selectLexiconsFolderToolStripMenuItem_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // traceBox
            // 
            traceBox.Dock = DockStyle.Fill;
            traceBox.Font = new Font("Segoe UI", 12F);
            traceBox.Location = new Point(0, 33);
            traceBox.Margin = new Padding(4);
            traceBox.Name = "traceBox";
            traceBox.Size = new Size(800, 417);
            traceBox.TabIndex = 4;
            traceBox.Text = "";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(traceBox);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "MainForm";
            Text = "Form1";
            Load += MainForm_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem grkLexiconStrongsToolStripMenuItem;
        private ToolStripMenuItem hebLexiconStrongsToolStripMenuItem;
        private OpenFileDialog openFileDialog1;
        private RichTextBox traceBox;
        private ToolStripMenuItem selectLexiconsFolderToolStripMenuItem;
        private FolderBrowserDialog folderBrowserDialog1;
    }
}
