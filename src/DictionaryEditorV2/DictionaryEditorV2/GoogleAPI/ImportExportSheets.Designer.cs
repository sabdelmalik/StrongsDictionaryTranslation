namespace DictionaryEditorV2.GoogleAPI
{
    partial class ImportExportSheets
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
            lbSheets = new ListBox();
            splitContainer1 = new SplitContainer();
            splitContainer2 = new SplitContainer();
            traceBox = new RichTextBox();
            btnImport = new Button();
            btnExport = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            SuspendLayout();
            // 
            // lbSheets
            // 
            lbSheets.Dock = DockStyle.Fill;
            lbSheets.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbSheets.ForeColor = SystemColors.WindowText;
            lbSheets.FormattingEnabled = true;
            lbSheets.ItemHeight = 21;
            lbSheets.Location = new Point(0, 0);
            lbSheets.Name = "lbSheets";
            lbSheets.SelectionMode = SelectionMode.MultiExtended;
            lbSheets.Size = new Size(156, 450);
            lbSheets.TabIndex = 0;
            lbSheets.SelectedIndexChanged += lbSheets_SelectedIndexChanged;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(lbSheets);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Size = new Size(654, 450);
            splitContainer1.SplitterDistance = 156;
            splitContainer1.TabIndex = 1;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(traceBox);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.BackColor = SystemColors.Window;
            splitContainer2.Panel2.Controls.Add(btnImport);
            splitContainer2.Panel2.Controls.Add(btnExport);
            splitContainer2.Size = new Size(494, 450);
            splitContainer2.SplitterDistance = 329;
            splitContainer2.TabIndex = 0;
            // 
            // traceBox
            // 
            traceBox.Dock = DockStyle.Fill;
            traceBox.Location = new Point(0, 0);
            traceBox.Name = "traceBox";
            traceBox.ReadOnly = true;
            traceBox.Size = new Size(494, 329);
            traceBox.TabIndex = 4;
            traceBox.Text = "";
            // 
            // btnImport
            // 
            btnImport.BackColor = SystemColors.MenuHighlight;
            btnImport.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnImport.ForeColor = SystemColors.Window;
            btnImport.Location = new Point(94, 34);
            btnImport.Name = "btnImport";
            btnImport.Size = new Size(109, 47);
            btnImport.TabIndex = 0;
            btnImport.Text = "Import";
            btnImport.UseVisualStyleBackColor = false;
            btnImport.Click += btnImport_Click;
            // 
            // btnExport
            // 
            btnExport.BackColor = SystemColors.MenuHighlight;
            btnExport.FlatAppearance.BorderSize = 5;
            btnExport.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 192, 192);
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnExport.ForeColor = SystemColors.Window;
            btnExport.Location = new Point(275, 34);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(109, 47);
            btnExport.TabIndex = 0;
            btnExport.Text = "Export";
            btnExport.UseVisualStyleBackColor = false;
            btnExport.Click += btnExport_Click;
            // 
            // ImportExportSheets
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(654, 450);
            Controls.Add(splitContainer1);
            Name = "ImportExportSheets";
            Text = "ImportExportSheets";
            Load += ImportExportSheets_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ListBox lbSheets;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private RichTextBox traceBox;
        private Button btnImport;
        private Button btnExport;
    }
}