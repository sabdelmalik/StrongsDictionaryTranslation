
namespace DictionaryEditorV2.GlossReferences
{
    partial class WordReferences
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            splitContainerMain = new SplitContainer();
            dgvWords = new DataGridView();
            splitContainerBottom = new SplitContainer();
            dgvVerses = new DataGridView();
            traceBox = new RichTextBox();
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvWords).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainerBottom).BeginInit();
            splitContainerBottom.Panel1.SuspendLayout();
            splitContainerBottom.Panel2.SuspendLayout();
            splitContainerBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvVerses).BeginInit();
            SuspendLayout();
            // 
            // splitContainerMain
            // 
            splitContainerMain.Dock = DockStyle.Fill;
            splitContainerMain.Location = new Point(0, 0);
            splitContainerMain.Name = "splitContainerMain";
            splitContainerMain.Orientation = Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.Controls.Add(dgvWords);
            // 
            // splitContainerMain.Panel2
            // 
            splitContainerMain.Panel2.Controls.Add(splitContainerBottom);
            splitContainerMain.Size = new Size(461, 585);
            splitContainerMain.SplitterDistance = 153;
            splitContainerMain.TabIndex = 0;
            // 
            // dgvWords
            // 
            dgvWords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvWords.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dgvWords.BackgroundColor = Color.White;
            dgvWords.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dgvWords.DefaultCellStyle = dataGridViewCellStyle1;
            dgvWords.Dock = DockStyle.Fill;
            dgvWords.Location = new Point(0, 0);
            dgvWords.Margin = new Padding(3, 2, 3, 2);
            dgvWords.Name = "dgvWords";
            dgvWords.ReadOnly = true;
            dgvWords.RightToLeft = RightToLeft.Yes;
            dgvWords.RowHeadersWidth = 51;
            dgvWords.RowTemplate.Height = 29;
            dgvWords.Size = new Size(461, 153);
            dgvWords.TabIndex = 14;
            // 
            // splitContainerBottom
            // 
            splitContainerBottom.Dock = DockStyle.Fill;
            splitContainerBottom.Location = new Point(0, 0);
            splitContainerBottom.Margin = new Padding(3, 2, 3, 2);
            splitContainerBottom.Name = "splitContainerBottom";
            splitContainerBottom.Orientation = Orientation.Horizontal;
            // 
            // splitContainerBottom.Panel1
            // 
            splitContainerBottom.Panel1.Controls.Add(dgvVerses);
            // 
            // splitContainerBottom.Panel2
            // 
            splitContainerBottom.Panel2.Controls.Add(traceBox);
            splitContainerBottom.Size = new Size(461, 428);
            splitContainerBottom.SplitterDistance = 211;
            splitContainerBottom.SplitterWidth = 3;
            splitContainerBottom.TabIndex = 1;
            // 
            // dgvVerses
            // 
            dgvVerses.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvVerses.BackgroundColor = Color.White;
            dgvVerses.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvVerses.DefaultCellStyle = dataGridViewCellStyle2;
            dgvVerses.Dock = DockStyle.Fill;
            dgvVerses.Location = new Point(0, 0);
            dgvVerses.Margin = new Padding(3, 2, 3, 2);
            dgvVerses.Name = "dgvVerses";
            dgvVerses.ReadOnly = true;
            dgvVerses.RightToLeft = RightToLeft.Yes;
            dgvVerses.RowHeadersWidth = 51;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvVerses.RowsDefaultCellStyle = dataGridViewCellStyle3;
            dgvVerses.RowTemplate.Height = 29;
            dgvVerses.RowTemplate.ReadOnly = true;
            dgvVerses.RowTemplate.Resizable = DataGridViewTriState.False;
            dgvVerses.ShowEditingIcon = false;
            dgvVerses.Size = new Size(461, 211);
            dgvVerses.TabIndex = 14;
            // 
            // traceBox
            // 
            traceBox.Dock = DockStyle.Fill;
            traceBox.Location = new Point(0, 0);
            traceBox.Margin = new Padding(3, 2, 3, 2);
            traceBox.Name = "traceBox";
            traceBox.RightToLeft = RightToLeft.Yes;
            traceBox.Size = new Size(461, 214);
            traceBox.TabIndex = 1;
            traceBox.Text = "";
            // 
            // WordReferences
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(461, 585);
            Controls.Add(splitContainerMain);
            Margin = new Padding(3, 2, 3, 2);
            Name = "WordReferences";
            Text = "Word References";
            Load += WordReferences_Load;
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvWords).EndInit();
            splitContainerBottom.Panel1.ResumeLayout(false);
            splitContainerBottom.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerBottom).EndInit();
            splitContainerBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvVerses).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainerMain;
        private DataGridView dgvWords;
        private SplitContainer splitContainerBottom;
        private DataGridView dgvVerses;
        private RichTextBox traceBox;
    }
}