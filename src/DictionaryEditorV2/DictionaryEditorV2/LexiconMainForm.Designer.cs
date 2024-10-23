
namespace DictionaryEditorV2
{
    partial class LexiconMainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LexiconMainForm));
            dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            vS2013LightTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2013LightTheme();
            vS2013BlueTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2013BlueTheme();
            vS2013DarkTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2013DarkTheme();
            folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog2 = new FolderBrowserDialog();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            importExportSheetsToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            openFileDialog = new OpenFileDialog();
            folderBrowserDialog3 = new FolderBrowserDialog();
            waitCursorAnimation = new PictureBox();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)waitCursorAnimation).BeginInit();
            SuspendLayout();
            // 
            // dockPanel
            // 
            dockPanel.Dock = DockStyle.Fill;
            dockPanel.DockBackColor = Color.FromArgb(41, 57, 85);
            dockPanel.DockBottomPortion = 150D;
            dockPanel.DockLeftPortion = 200D;
            dockPanel.DockRightPortion = 200D;
            dockPanel.DockTopPortion = 150D;
            dockPanel.Font = new Font("Tahoma", 11F, FontStyle.Regular, GraphicsUnit.World);
            dockPanel.Location = new Point(0, 24);
            dockPanel.Margin = new Padding(3, 2, 3, 2);
            dockPanel.Name = "dockPanel";
            dockPanel.RightToLeftLayout = true;
            dockPanel.ShowAutoHideContentOnHover = false;
            dockPanel.Size = new Size(928, 388);
            dockPanel.TabIndex = 0;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, aboutToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(5, 2, 0, 2);
            menuStrip1.Size = new Size(928, 24);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { importExportSheetsToolStripMenuItem, settingsToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(186, 22);
            settingsToolStripMenuItem.Text = "Settings";
            settingsToolStripMenuItem.Click += settingsToolStripMenuItem_Click;
            // 
            // importExportSheetsToolStripMenuItem
            // 
            importExportSheetsToolStripMenuItem.Name = "importExportSheetsToolStripMenuItem";
            importExportSheetsToolStripMenuItem.Size = new Size(186, 22);
            importExportSheetsToolStripMenuItem.Text = "Import/Export Sheets";
            importExportSheetsToolStripMenuItem.Click += importExportSheetsToolStripMenuItem_Click;
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(52, 20);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog1";
            // 
            // waitCursorAnimation
            // 
            waitCursorAnimation.Image = (Image)resources.GetObject("waitCursorAnimation.Image");
            waitCursorAnimation.Location = new Point(362, 152);
            waitCursorAnimation.Name = "waitCursorAnimation";
            waitCursorAnimation.Size = new Size(111, 111);
            waitCursorAnimation.SizeMode = PictureBoxSizeMode.AutoSize;
            waitCursorAnimation.TabIndex = 4;
            waitCursorAnimation.TabStop = false;
            waitCursorAnimation.Visible = false;
            // 
            // LexiconMainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(928, 412);
            Controls.Add(waitCursorAnimation);
            Controls.Add(dockPanel);
            Controls.Add(menuStrip1);
            IsMdiContainer = true;
            MainMenuStrip = menuStrip1;
            Margin = new Padding(3, 2, 3, 2);
            Name = "LexiconMainForm";
            Load += LexiconMainForm_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)waitCursorAnimation).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion


        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
        private WeifenLuo.WinFormsUI.Docking.VS2013LightTheme vS2013LightTheme1;
        private WeifenLuo.WinFormsUI.Docking.VS2013BlueTheme vS2013BlueTheme1;
        private WeifenLuo.WinFormsUI.Docking.VS2013DarkTheme vS2013DarkTheme1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog3;
        private System.Windows.Forms.PictureBox waitCursorAnimation;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem importExportSheetsToolStripMenuItem;
    }
}

