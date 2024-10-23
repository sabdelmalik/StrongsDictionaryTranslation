namespace DictionaryEditorV1
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
                if (dataSource != null) { dataSource.Dispose(); }
                if (connection != null) { connection.Close(); }
                if (connection != null) { connection.Dispose(); }
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            traceBox = new RichTextBox();
            pbUP = new PictureBox();
            pbDown = new PictureBox();
            cbLanguage = new ComboBox();
            label1 = new Label();
            tbOriginal = new TextBox();
            tbEnglish = new TextBox();
            contextMenuStrip1 = new ContextMenuStrip(components);
            tbTranslatedWord = new TextBox();
            dgvWords = new DataGridView();
            dgvVerses = new DataGridView();
            tbDstrongs = new TextBox();
            folderBrowserDialog1 = new FolderBrowserDialog();
            mainSplitContainer = new SplitContainer();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            tbTranslatedLong = new RichTextBox();
            panel1 = new Panel();
            tbUpdateStatus = new TextBox();
            tbUpdater = new TextBox();
            tbSheetName = new TextBox();
            tbLastUpdated = new TextBox();
            tbArabicTxLt = new TextBox();
            panel2 = new Panel();
            strongsNumberControl = new MyNumericControl();
            tbLongText = new RichTextBox();
            rightSplitContainer = new SplitContainer();
            rightBottomSplitContainer = new SplitContainer();
            menuStrip1 = new MenuStrip();
            toolStripMenuItem1 = new ToolStripMenuItem();
            generateJSONToolStripMenuItem = new ToolStripMenuItem();
            withDStrongToolStripMenuItem = new ToolStripMenuItem();
            withuStrongToolStripMenuItem = new ToolStripMenuItem();
            saveCurrentToolStripMenuItem = new ToolStripMenuItem();
            backToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripMenuItem();
            csvWithdStrongToolStripMenuItem = new ToolStripMenuItem();
            csvWithuStrongToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)pbUP).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvWords).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvVerses).BeginInit();
            ((System.ComponentModel.ISupportInitialize)mainSplitContainer).BeginInit();
            mainSplitContainer.Panel1.SuspendLayout();
            mainSplitContainer.Panel2.SuspendLayout();
            mainSplitContainer.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)rightSplitContainer).BeginInit();
            rightSplitContainer.Panel1.SuspendLayout();
            rightSplitContainer.Panel2.SuspendLayout();
            rightSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)rightBottomSplitContainer).BeginInit();
            rightBottomSplitContainer.Panel1.SuspendLayout();
            rightBottomSplitContainer.Panel2.SuspendLayout();
            rightBottomSplitContainer.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // traceBox
            // 
            traceBox.Dock = DockStyle.Fill;
            traceBox.Location = new Point(0, 0);
            traceBox.Margin = new Padding(3, 2, 3, 2);
            traceBox.Name = "traceBox";
            traceBox.RightToLeft = RightToLeft.Yes;
            traceBox.Size = new Size(621, 206);
            traceBox.TabIndex = 1;
            traceBox.Text = "";
            // 
            // pbUP
            // 
            pbUP.Image = (Image)resources.GetObject("pbUP.Image");
            pbUP.Location = new Point(232, 34);
            pbUP.Name = "pbUP";
            pbUP.Size = new Size(31, 8);
            pbUP.SizeMode = PictureBoxSizeMode.StretchImage;
            pbUP.TabIndex = 6;
            pbUP.TabStop = false;
            pbUP.Click += pbUP_Click;
            // 
            // pbDown
            // 
            pbDown.Image = (Image)resources.GetObject("pbDown.Image");
            pbDown.Location = new Point(231, 47);
            pbDown.Name = "pbDown";
            pbDown.Size = new Size(31, 8);
            pbDown.SizeMode = PictureBoxSizeMode.StretchImage;
            pbDown.TabIndex = 0;
            pbDown.TabStop = false;
            pbDown.Click += pbDown_Click;
            // 
            // cbLanguage
            // 
            cbLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
            cbLanguage.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            cbLanguage.FormattingEnabled = true;
            cbLanguage.Items.AddRange(new object[] { "Hebrew", "Greek" });
            cbLanguage.Location = new Point(5, 34);
            cbLanguage.MaxDropDownItems = 2;
            cbLanguage.Name = "cbLanguage";
            cbLanguage.Size = new Size(100, 29);
            cbLanguage.TabIndex = 7;
            cbLanguage.SelectedIndexChanged += cbLanguage_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(10, 0);
            label1.Name = "label1";
            label1.Size = new Size(166, 25);
            label1.TabIndex = 8;
            label1.Text = "Strong's Number";
            // 
            // tbOriginal
            // 
            tbOriginal.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            tbOriginal.ForeColor = Color.Blue;
            tbOriginal.Location = new Point(304, 27);
            tbOriginal.Name = "tbOriginal";
            tbOriginal.ReadOnly = true;
            tbOriginal.Size = new Size(165, 29);
            tbOriginal.TabIndex = 9;
            // 
            // tbEnglish
            // 
            tbEnglish.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            tbEnglish.ForeColor = Color.Blue;
            tbEnglish.Location = new Point(494, 16);
            tbEnglish.Name = "tbEnglish";
            tbEnglish.ReadOnly = true;
            tbEnglish.Size = new Size(159, 29);
            tbEnglish.TabIndex = 9;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.ShowImageMargin = false;
            contextMenuStrip1.Size = new Size(36, 4);
            // 
            // tbTranslatedWord
            // 
            tbTranslatedWord.Dock = DockStyle.Right;
            tbTranslatedWord.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            tbTranslatedWord.ForeColor = Color.Black;
            tbTranslatedWord.Location = new Point(652, 0);
            tbTranslatedWord.Name = "tbTranslatedWord";
            tbTranslatedWord.RightToLeft = RightToLeft.Yes;
            tbTranslatedWord.Size = new Size(149, 29);
            tbTranslatedWord.TabIndex = 9;
            // 
            // dgvWords
            // 
            dgvWords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvWords.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dgvWords.BackgroundColor = Color.White;
            dgvWords.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
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
            dgvWords.Size = new Size(621, 270);
            dgvWords.TabIndex = 13;
            // 
            // dgvVerses
            // 
            dgvVerses.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvVerses.BackgroundColor = Color.White;
            dgvVerses.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
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
            dgvVerses.Size = new Size(621, 204);
            dgvVerses.TabIndex = 14;
            // 
            // tbDstrongs
            // 
            tbDstrongs.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            tbDstrongs.Location = new Point(198, 27);
            tbDstrongs.Margin = new Padding(3, 2, 3, 2);
            tbDstrongs.Name = "tbDstrongs";
            tbDstrongs.Size = new Size(29, 29);
            tbDstrongs.TabIndex = 15;
            // 
            // mainSplitContainer
            // 
            mainSplitContainer.Dock = DockStyle.Fill;
            mainSplitContainer.Location = new Point(0, 24);
            mainSplitContainer.Margin = new Padding(3, 2, 3, 2);
            mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            mainSplitContainer.Panel1.Controls.Add(tableLayoutPanel1);
            // 
            // mainSplitContainer.Panel2
            // 
            mainSplitContainer.Panel2.Controls.Add(rightSplitContainer);
            mainSplitContainer.Size = new Size(1438, 686);
            mainSplitContainer.SplitterDistance = 813;
            mainSplitContainer.TabIndex = 16;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 2);
            tableLayoutPanel1.Controls.Add(panel2, 0, 0);
            tableLayoutPanel1.Controls.Add(tbLongText, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(3, 2, 3, 2);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 15.3427639F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.8411331F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50.8161049F));
            tableLayoutPanel1.Size = new Size(813, 686);
            tableLayoutPanel1.TabIndex = 13;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(tbTranslatedLong, 0, 1);
            tableLayoutPanel2.Controls.Add(panel1, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 339);
            tableLayoutPanel2.Margin = new Padding(3, 2, 3, 2);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 16.9197388F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 83.08026F));
            tableLayoutPanel2.Size = new Size(807, 345);
            tableLayoutPanel2.TabIndex = 14;
            // 
            // tbTranslatedLong
            // 
            tbTranslatedLong.Dock = DockStyle.Fill;
            tbTranslatedLong.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point);
            tbTranslatedLong.Location = new Point(3, 60);
            tbTranslatedLong.Margin = new Padding(3, 2, 3, 2);
            tbTranslatedLong.Name = "tbTranslatedLong";
            tbTranslatedLong.RightToLeft = RightToLeft.Yes;
            tbTranslatedLong.Size = new Size(801, 283);
            tbTranslatedLong.TabIndex = 10;
            tbTranslatedLong.Text = "";
            // 
            // panel1
            // 
            panel1.Controls.Add(tbUpdateStatus);
            panel1.Controls.Add(tbUpdater);
            panel1.Controls.Add(tbSheetName);
            panel1.Controls.Add(tbLastUpdated);
            panel1.Controls.Add(tbArabicTxLt);
            panel1.Controls.Add(tbTranslatedWord);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(3, 2);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(801, 53);
            panel1.TabIndex = 12;
            // 
            // tbUpdateStatus
            // 
            tbUpdateStatus.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            tbUpdateStatus.Location = new Point(409, 0);
            tbUpdateStatus.Margin = new Padding(3, 2, 3, 2);
            tbUpdateStatus.Name = "tbUpdateStatus";
            tbUpdateStatus.RightToLeft = RightToLeft.Yes;
            tbUpdateStatus.Size = new Size(103, 29);
            tbUpdateStatus.TabIndex = 13;
            // 
            // tbUpdater
            // 
            tbUpdater.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            tbUpdater.Location = new Point(182, 0);
            tbUpdater.Margin = new Padding(3, 2, 3, 2);
            tbUpdater.Name = "tbUpdater";
            tbUpdater.RightToLeft = RightToLeft.Yes;
            tbUpdater.Size = new Size(222, 29);
            tbUpdater.TabIndex = 13;
            // 
            // tbSheetName
            // 
            tbSheetName.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            tbSheetName.Location = new Point(5, 28);
            tbSheetName.Margin = new Padding(3, 2, 3, 2);
            tbSheetName.Name = "tbSheetName";
            tbSheetName.RightToLeft = RightToLeft.Yes;
            tbSheetName.Size = new Size(158, 29);
            tbSheetName.TabIndex = 13;
            // 
            // tbLastUpdated
            // 
            tbLastUpdated.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            tbLastUpdated.Location = new Point(8, 0);
            tbLastUpdated.Margin = new Padding(3, 2, 3, 2);
            tbLastUpdated.Name = "tbLastUpdated";
            tbLastUpdated.RightToLeft = RightToLeft.Yes;
            tbLastUpdated.Size = new Size(158, 29);
            tbLastUpdated.TabIndex = 13;
            // 
            // tbArabicTxLt
            // 
            tbArabicTxLt.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            tbArabicTxLt.Location = new Point(526, 2);
            tbArabicTxLt.Margin = new Padding(3, 2, 3, 2);
            tbArabicTxLt.Name = "tbArabicTxLt";
            tbArabicTxLt.RightToLeft = RightToLeft.Yes;
            tbArabicTxLt.Size = new Size(116, 29);
            tbArabicTxLt.TabIndex = 13;
            // 
            // panel2
            // 
            panel2.Controls.Add(strongsNumberControl);
            panel2.Controls.Add(pbUP);
            panel2.Controls.Add(pbDown);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(tbOriginal);
            panel2.Controls.Add(cbLanguage);
            panel2.Controls.Add(tbEnglish);
            panel2.Controls.Add(tbDstrongs);
            panel2.Location = new Point(3, 2);
            panel2.Margin = new Padding(3, 2, 3, 2);
            panel2.Name = "panel2";
            panel2.Size = new Size(654, 86);
            panel2.TabIndex = 15;
            // 
            // strongsNumberControl
            // 
            strongsNumberControl.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            strongsNumberControl.Location = new Point(111, 33);
            strongsNumberControl.Name = "strongsNumberControl";
            strongsNumberControl.Size = new Size(63, 29);
            strongsNumberControl.TabIndex = 16;
            strongsNumberControl.Tag = "";
            strongsNumberControl.Text = "0001";
            // 
            // tbLongText
            // 
            tbLongText.Dock = DockStyle.Fill;
            tbLongText.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point);
            tbLongText.Location = new Point(3, 107);
            tbLongText.Margin = new Padding(3, 2, 3, 2);
            tbLongText.Name = "tbLongText";
            tbLongText.ReadOnly = true;
            tbLongText.Size = new Size(807, 228);
            tbLongText.TabIndex = 16;
            tbLongText.Text = "";
            // 
            // rightSplitContainer
            // 
            rightSplitContainer.Location = new Point(0, 0);
            rightSplitContainer.Margin = new Padding(3, 2, 3, 2);
            rightSplitContainer.Name = "rightSplitContainer";
            rightSplitContainer.Orientation = Orientation.Horizontal;
            // 
            // rightSplitContainer.Panel1
            // 
            rightSplitContainer.Panel1.Controls.Add(dgvWords);
            // 
            // rightSplitContainer.Panel2
            // 
            rightSplitContainer.Panel2.Controls.Add(rightBottomSplitContainer);
            rightSplitContainer.Size = new Size(621, 686);
            rightSplitContainer.SplitterDistance = 270;
            rightSplitContainer.SplitterWidth = 3;
            rightSplitContainer.TabIndex = 0;
            // 
            // rightBottomSplitContainer
            // 
            rightBottomSplitContainer.Dock = DockStyle.Fill;
            rightBottomSplitContainer.Location = new Point(0, 0);
            rightBottomSplitContainer.Margin = new Padding(3, 2, 3, 2);
            rightBottomSplitContainer.Name = "rightBottomSplitContainer";
            rightBottomSplitContainer.Orientation = Orientation.Horizontal;
            // 
            // rightBottomSplitContainer.Panel1
            // 
            rightBottomSplitContainer.Panel1.Controls.Add(dgvVerses);
            // 
            // rightBottomSplitContainer.Panel2
            // 
            rightBottomSplitContainer.Panel2.Controls.Add(traceBox);
            rightBottomSplitContainer.Size = new Size(621, 413);
            rightBottomSplitContainer.SplitterDistance = 204;
            rightBottomSplitContainer.SplitterWidth = 3;
            rightBottomSplitContainer.TabIndex = 0;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1, generateJSONToolStripMenuItem, saveCurrentToolStripMenuItem, backToolStripMenuItem, toolStripMenuItem2 });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(5, 2, 0, 2);
            menuStrip1.Size = new Size(1438, 24);
            menuStrip1.TabIndex = 17;
            menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(12, 20);
            // 
            // generateJSONToolStripMenuItem
            // 
            generateJSONToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { withDStrongToolStripMenuItem, withuStrongToolStripMenuItem });
            generateJSONToolStripMenuItem.Name = "generateJSONToolStripMenuItem";
            generateJSONToolStripMenuItem.Size = new Size(97, 20);
            generateJSONToolStripMenuItem.Text = "Generate JSON";
            // 
            // withDStrongToolStripMenuItem
            // 
            withDStrongToolStripMenuItem.Name = "withDStrongToolStripMenuItem";
            withDStrongToolStripMenuItem.Size = new Size(144, 22);
            withDStrongToolStripMenuItem.Text = "With dStrong";
            withDStrongToolStripMenuItem.Click += withDStrongToolStripMenuItem_Click;
            // 
            // withuStrongToolStripMenuItem
            // 
            withuStrongToolStripMenuItem.Name = "withuStrongToolStripMenuItem";
            withuStrongToolStripMenuItem.Size = new Size(144, 22);
            withuStrongToolStripMenuItem.Text = "WithuStrong";
            withuStrongToolStripMenuItem.Click += withuStrongToolStripMenuItem_Click;
            // 
            // saveCurrentToolStripMenuItem
            // 
            saveCurrentToolStripMenuItem.Name = "saveCurrentToolStripMenuItem";
            saveCurrentToolStripMenuItem.Size = new Size(86, 20);
            saveCurrentToolStripMenuItem.Text = "Save Current";
            saveCurrentToolStripMenuItem.Click += saveCurrentToolStripMenuItem_Click;
            // 
            // backToolStripMenuItem
            // 
            backToolStripMenuItem.Name = "backToolStripMenuItem";
            backToolStripMenuItem.Size = new Size(44, 20);
            backToolStripMenuItem.Text = "Back";
            backToolStripMenuItem.Click += backToolStripMenuItem_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.DropDownItems.AddRange(new ToolStripItem[] { csvWithdStrongToolStripMenuItem, csvWithuStrongToolStripMenuItem });
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(90, 20);
            toolStripMenuItem2.Text = "Generate CSV";
            // 
            // csvWithdStrongToolStripMenuItem
            // 
            csvWithdStrongToolStripMenuItem.Name = "csvWithdStrongToolStripMenuItem";
            csvWithdStrongToolStripMenuItem.Size = new Size(144, 22);
            csvWithdStrongToolStripMenuItem.Text = "With dStrong";
            csvWithdStrongToolStripMenuItem.Click += csvWithdStrongToolStripMenuItem_Click;
            // 
            // csvWithuStrongToolStripMenuItem
            // 
            csvWithuStrongToolStripMenuItem.Name = "csvWithuStrongToolStripMenuItem";
            csvWithuStrongToolStripMenuItem.Size = new Size(144, 22);
            csvWithuStrongToolStripMenuItem.Text = "WithuStrong";
            csvWithuStrongToolStripMenuItem.Click += csvWithuStrongToolStripMenuItem_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1438, 710);
            Controls.Add(mainSplitContainer);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(3, 2, 3, 2);
            Name = "MainForm";
            Text = "Strong's Dictionary Editor";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pbUP).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvWords).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvVerses).EndInit();
            mainSplitContainer.Panel1.ResumeLayout(false);
            mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)mainSplitContainer).EndInit();
            mainSplitContainer.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            rightSplitContainer.Panel1.ResumeLayout(false);
            rightSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)rightSplitContainer).EndInit();
            rightSplitContainer.ResumeLayout(false);
            rightBottomSplitContainer.Panel1.ResumeLayout(false);
            rightBottomSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)rightBottomSplitContainer).EndInit();
            rightBottomSplitContainer.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private RichTextBox traceBox;
        private PictureBox pbUP;
        private PictureBox pbDown;
        private ComboBox cbLanguage;
        private Label label1;
        private TextBox tbOriginal;
        private TextBox tbEnglish;
        private TextBox tbTranslatedWord;
        private DataGridView dgvWords;
        private DataGridView dgvVerses;
        private TextBox tbDstrongs;
        private FolderBrowserDialog folderBrowserDialog1;
        private SplitContainer mainSplitContainer;
        private SplitContainer rightSplitContainer;
        private SplitContainer rightBottomSplitContainer;
        private RichTextBox tbTranslatedLong;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem generateJSONToolStripMenuItem;
        private ToolStripMenuItem saveCurrentToolStripMenuItem;
        private ToolStripMenuItem backToolStripMenuItem;
        private Panel panel1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem withDStrongToolStripMenuItem;
        private ToolStripMenuItem withuStrongToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem csvWithdStrongToolStripMenuItem;
        private ToolStripMenuItem csvWithuStrongToolStripMenuItem;
        private TextBox tbArabicTxLt;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Panel panel2;
        private RichTextBox tbLongText;
        private TextBox tbLastUpdated;
        private TextBox tbUpdateStatus;
        private TextBox tbUpdater;
        private TextBox tbSheetName;
        private MyNumericControl strongsNumberControl;
    }
}