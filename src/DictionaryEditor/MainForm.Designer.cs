namespace DictionaryEditor
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
            strongsNumberControl = new MyNumericControl();
            pbUP = new PictureBox();
            pbDown = new PictureBox();
            cbLanguage = new ComboBox();
            label1 = new Label();
            tbOriginal = new TextBox();
            tbEnglish = new TextBox();
            tbShortText = new TextBox();
            tbLongText = new TextBox();
            contextMenuStrip1 = new ContextMenuStrip(components);
            tbTranslatedShort = new TextBox();
            tbTranslatedWord = new TextBox();
            dgvWords = new DataGridView();
            dgvVerses = new DataGridView();
            tbDstrongs = new TextBox();
            folderBrowserDialog1 = new FolderBrowserDialog();
            mainSplitContainer = new SplitContainer();
            leftSplitContainer = new SplitContainer();
            leftBottomSplitContainer = new SplitContainer();
            tbTranslatedLong = new RichTextBox();
            panel1 = new Panel();
            tbArabicTxLt = new TextBox();
            btnStopSearch = new Button();
            btnNext = new Button();
            btnSearch = new Button();
            tbSearchStart = new TextBox();
            tbSearch = new TextBox();
            btnZspace = new Button();
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
            ((System.ComponentModel.ISupportInitialize)leftSplitContainer).BeginInit();
            leftSplitContainer.Panel1.SuspendLayout();
            leftSplitContainer.Panel2.SuspendLayout();
            leftSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)leftBottomSplitContainer).BeginInit();
            leftBottomSplitContainer.Panel1.SuspendLayout();
            leftBottomSplitContainer.Panel2.SuspendLayout();
            leftBottomSplitContainer.SuspendLayout();
            panel1.SuspendLayout();
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
            traceBox.Name = "traceBox";
            traceBox.RightToLeft = RightToLeft.Yes;
            traceBox.Size = new Size(709, 273);
            traceBox.TabIndex = 1;
            traceBox.Text = "";
            // 
            // strongsNumberControl
            // 
            strongsNumberControl.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            strongsNumberControl.Location = new Point(123, 36);
            strongsNumberControl.Margin = new Padding(3, 4, 3, 4);
            strongsNumberControl.Name = "strongsNumberControl";
            strongsNumberControl.Size = new Size(71, 34);
            strongsNumberControl.TabIndex = 3;
            strongsNumberControl.Text = "0001";
            strongsNumberControl.TextAlign = HorizontalAlignment.Center;
            // 
            // pbUP
            // 
            pbUP.Image = (Image)resources.GetObject("pbUP.Image");
            pbUP.Location = new Point(249, 36);
            pbUP.Margin = new Padding(3, 4, 3, 4);
            pbUP.Name = "pbUP";
            pbUP.Size = new Size(35, 13);
            pbUP.SizeMode = PictureBoxSizeMode.StretchImage;
            pbUP.TabIndex = 6;
            pbUP.TabStop = false;
            pbUP.Click += pbUP_Click;
            // 
            // pbDown
            // 
            pbDown.Image = (Image)resources.GetObject("pbDown.Image");
            pbDown.Location = new Point(248, 53);
            pbDown.Margin = new Padding(3, 4, 3, 4);
            pbDown.Name = "pbDown";
            pbDown.Size = new Size(35, 13);
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
            cbLanguage.Location = new Point(3, 36);
            cbLanguage.Margin = new Padding(3, 4, 3, 4);
            cbLanguage.MaxDropDownItems = 2;
            cbLanguage.Name = "cbLanguage";
            cbLanguage.Size = new Size(114, 36);
            cbLanguage.TabIndex = 7;
            cbLanguage.SelectedIndexChanged += cbLanguage_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(208, 32);
            label1.TabIndex = 8;
            label1.Text = "Strong's Number";
            // 
            // tbOriginal
            // 
            tbOriginal.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            tbOriginal.ForeColor = Color.Blue;
            tbOriginal.Location = new Point(300, 32);
            tbOriginal.Margin = new Padding(3, 4, 3, 4);
            tbOriginal.Name = "tbOriginal";
            tbOriginal.ReadOnly = true;
            tbOriginal.Size = new Size(188, 34);
            tbOriginal.TabIndex = 9;
            // 
            // tbEnglish
            // 
            tbEnglish.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            tbEnglish.ForeColor = Color.Blue;
            tbEnglish.Location = new Point(494, 32);
            tbEnglish.Margin = new Padding(3, 4, 3, 4);
            tbEnglish.Name = "tbEnglish";
            tbEnglish.ReadOnly = true;
            tbEnglish.Size = new Size(181, 34);
            tbEnglish.TabIndex = 9;
            // 
            // tbShortText
            // 
            tbShortText.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            tbShortText.Location = new Point(774, 804);
            tbShortText.Margin = new Padding(3, 4, 3, 4);
            tbShortText.Multiline = true;
            tbShortText.Name = "tbShortText";
            tbShortText.ReadOnly = true;
            tbShortText.ScrollBars = ScrollBars.Vertical;
            tbShortText.Size = new Size(296, 72);
            tbShortText.TabIndex = 10;
            tbShortText.Visible = false;
            // 
            // tbLongText
            // 
            tbLongText.ContextMenuStrip = contextMenuStrip1;
            tbLongText.Dock = DockStyle.Fill;
            tbLongText.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            tbLongText.ForeColor = Color.Blue;
            tbLongText.Location = new Point(0, 0);
            tbLongText.Margin = new Padding(3, 4, 3, 4);
            tbLongText.Multiline = true;
            tbLongText.Name = "tbLongText";
            tbLongText.ReadOnly = true;
            tbLongText.ScrollBars = ScrollBars.Vertical;
            tbLongText.Size = new Size(930, 329);
            tbLongText.TabIndex = 10;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.ShowImageMargin = false;
            contextMenuStrip1.Size = new Size(36, 4);
            // 
            // tbTranslatedShort
            // 
            tbTranslatedShort.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            tbTranslatedShort.Location = new Point(491, 804);
            tbTranslatedShort.Margin = new Padding(3, 4, 3, 4);
            tbTranslatedShort.Multiline = true;
            tbTranslatedShort.Name = "tbTranslatedShort";
            tbTranslatedShort.RightToLeft = RightToLeft.Yes;
            tbTranslatedShort.ScrollBars = ScrollBars.Vertical;
            tbTranslatedShort.Size = new Size(254, 72);
            tbTranslatedShort.TabIndex = 10;
            tbTranslatedShort.Visible = false;
            // 
            // tbTranslatedWord
            // 
            tbTranslatedWord.Dock = DockStyle.Right;
            tbTranslatedWord.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            tbTranslatedWord.ForeColor = Color.Black;
            tbTranslatedWord.Location = new Point(715, 0);
            tbTranslatedWord.Margin = new Padding(3, 4, 3, 4);
            tbTranslatedWord.Name = "tbTranslatedWord";
            tbTranslatedWord.RightToLeft = RightToLeft.Yes;
            tbTranslatedWord.Size = new Size(215, 34);
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
            dgvWords.Name = "dgvWords";
            dgvWords.ReadOnly = true;
            dgvWords.RightToLeft = RightToLeft.Yes;
            dgvWords.RowHeadersWidth = 51;
            dgvWords.RowTemplate.Height = 29;
            dgvWords.Size = new Size(709, 363);
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
            dgvVerses.Size = new Size(709, 275);
            dgvVerses.TabIndex = 14;
            // 
            // tbDstrongs
            // 
            tbDstrongs.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            tbDstrongs.Location = new Point(200, 35);
            tbDstrongs.Name = "tbDstrongs";
            tbDstrongs.Size = new Size(33, 34);
            tbDstrongs.TabIndex = 15;
            // 
            // mainSplitContainer
            // 
            mainSplitContainer.Dock = DockStyle.Fill;
            mainSplitContainer.Location = new Point(0, 28);
            mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            mainSplitContainer.Panel1.Controls.Add(leftSplitContainer);
            // 
            // mainSplitContainer.Panel2
            // 
            mainSplitContainer.Panel2.Controls.Add(rightSplitContainer);
            mainSplitContainer.Size = new Size(1643, 919);
            mainSplitContainer.SplitterDistance = 930;
            mainSplitContainer.TabIndex = 16;
            // 
            // leftSplitContainer
            // 
            leftSplitContainer.Dock = DockStyle.Fill;
            leftSplitContainer.Location = new Point(0, 0);
            leftSplitContainer.Name = "leftSplitContainer";
            leftSplitContainer.Orientation = Orientation.Horizontal;
            // 
            // leftSplitContainer.Panel1
            // 
            leftSplitContainer.Panel1.Controls.Add(label1);
            leftSplitContainer.Panel1.Controls.Add(tbDstrongs);
            leftSplitContainer.Panel1.Controls.Add(cbLanguage);
            leftSplitContainer.Panel1.Controls.Add(strongsNumberControl);
            leftSplitContainer.Panel1.Controls.Add(tbEnglish);
            leftSplitContainer.Panel1.Controls.Add(pbUP);
            leftSplitContainer.Panel1.Controls.Add(tbOriginal);
            leftSplitContainer.Panel1.Controls.Add(pbDown);
            leftSplitContainer.Panel1MinSize = 10;
            // 
            // leftSplitContainer.Panel2
            // 
            leftSplitContainer.Panel2.Controls.Add(leftBottomSplitContainer);
            leftSplitContainer.Size = new Size(930, 919);
            leftSplitContainer.SplitterDistance = 88;
            leftSplitContainer.TabIndex = 0;
            // 
            // leftBottomSplitContainer
            // 
            leftBottomSplitContainer.Dock = DockStyle.Fill;
            leftBottomSplitContainer.Location = new Point(0, 0);
            leftBottomSplitContainer.Name = "leftBottomSplitContainer";
            leftBottomSplitContainer.Orientation = Orientation.Horizontal;
            // 
            // leftBottomSplitContainer.Panel1
            // 
            leftBottomSplitContainer.Panel1.Controls.Add(tbLongText);
            // 
            // leftBottomSplitContainer.Panel2
            // 
            leftBottomSplitContainer.Panel2.Controls.Add(tbTranslatedLong);
            leftBottomSplitContainer.Panel2.Controls.Add(panel1);
            leftBottomSplitContainer.Size = new Size(930, 827);
            leftBottomSplitContainer.SplitterDistance = 329;
            leftBottomSplitContainer.TabIndex = 0;
            // 
            // tbTranslatedLong
            // 
            tbTranslatedLong.Dock = DockStyle.Fill;
            tbTranslatedLong.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point);
            tbTranslatedLong.Location = new Point(0, 40);
            tbTranslatedLong.Name = "tbTranslatedLong";
            tbTranslatedLong.RightToLeft = RightToLeft.Yes;
            tbTranslatedLong.Size = new Size(930, 454);
            tbTranslatedLong.TabIndex = 10;
            tbTranslatedLong.Text = "";
            // 
            // panel1
            // 
            panel1.Controls.Add(tbArabicTxLt);
            panel1.Controls.Add(btnStopSearch);
            panel1.Controls.Add(btnNext);
            panel1.Controls.Add(btnSearch);
            panel1.Controls.Add(tbSearchStart);
            panel1.Controls.Add(tbSearch);
            panel1.Controls.Add(tbTranslatedWord);
            panel1.Controls.Add(btnZspace);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(930, 40);
            panel1.TabIndex = 12;
            // 
            // tbArabicTxLt
            // 
            tbArabicTxLt.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            tbArabicTxLt.Location = new Point(480, 1);
            tbArabicTxLt.Name = "tbArabicTxLt";
            tbArabicTxLt.RightToLeft = RightToLeft.Yes;
            tbArabicTxLt.Size = new Size(239, 34);
            tbArabicTxLt.TabIndex = 13;
            // 
            // btnStopSearch
            // 
            btnStopSearch.Location = new Point(420, 6);
            btnStopSearch.Name = "btnStopSearch";
            btnStopSearch.Size = new Size(54, 29);
            btnStopSearch.TabIndex = 13;
            btnStopSearch.Text = "Stop";
            btnStopSearch.UseVisualStyleBackColor = true;
            btnStopSearch.Click += btnStopSearch_Click;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(367, 5);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(57, 29);
            btnNext.TabIndex = 13;
            btnNext.Text = "Next";
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Click += btnNext_Click;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(306, 5);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(67, 29);
            btnSearch.TabIndex = 13;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // tbSearchStart
            // 
            tbSearchStart.Location = new Point(100, 7);
            tbSearchStart.Name = "tbSearchStart";
            tbSearchStart.Size = new Size(72, 27);
            tbSearchStart.TabIndex = 13;
            // 
            // tbSearch
            // 
            tbSearch.Location = new Point(178, 8);
            tbSearch.Name = "tbSearch";
            tbSearch.Size = new Size(122, 27);
            tbSearch.TabIndex = 13;
            // 
            // btnZspace
            // 
            btnZspace.Dock = DockStyle.Left;
            btnZspace.Location = new Point(0, 0);
            btnZspace.Name = "btnZspace";
            btnZspace.Size = new Size(94, 40);
            btnZspace.TabIndex = 11;
            btnZspace.Text = "Z Space";
            btnZspace.UseVisualStyleBackColor = true;
            btnZspace.Click += btnZspace_Click;
            // 
            // rightSplitContainer
            // 
            rightSplitContainer.Dock = DockStyle.Fill;
            rightSplitContainer.Location = new Point(0, 0);
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
            rightSplitContainer.Size = new Size(709, 919);
            rightSplitContainer.SplitterDistance = 363;
            rightSplitContainer.TabIndex = 0;
            // 
            // rightBottomSplitContainer
            // 
            rightBottomSplitContainer.Dock = DockStyle.Fill;
            rightBottomSplitContainer.Location = new Point(0, 0);
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
            rightBottomSplitContainer.Size = new Size(709, 552);
            rightBottomSplitContainer.SplitterDistance = 275;
            rightBottomSplitContainer.TabIndex = 0;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1, generateJSONToolStripMenuItem, saveCurrentToolStripMenuItem, backToolStripMenuItem, toolStripMenuItem2 });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1643, 28);
            menuStrip1.TabIndex = 17;
            menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(14, 24);
            // 
            // generateJSONToolStripMenuItem
            // 
            generateJSONToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { withDStrongToolStripMenuItem, withuStrongToolStripMenuItem });
            generateJSONToolStripMenuItem.Name = "generateJSONToolStripMenuItem";
            generateJSONToolStripMenuItem.Size = new Size(122, 24);
            generateJSONToolStripMenuItem.Text = "Generate JSON";
            // 
            // withDStrongToolStripMenuItem
            // 
            withDStrongToolStripMenuItem.Name = "withDStrongToolStripMenuItem";
            withDStrongToolStripMenuItem.Size = new Size(180, 26);
            withDStrongToolStripMenuItem.Text = "With dStrong";
            withDStrongToolStripMenuItem.Click += withDStrongToolStripMenuItem_Click;
            // 
            // withuStrongToolStripMenuItem
            // 
            withuStrongToolStripMenuItem.Name = "withuStrongToolStripMenuItem";
            withuStrongToolStripMenuItem.Size = new Size(180, 26);
            withuStrongToolStripMenuItem.Text = "WithuStrong";
            withuStrongToolStripMenuItem.Click += withuStrongToolStripMenuItem_Click;
            // 
            // saveCurrentToolStripMenuItem
            // 
            saveCurrentToolStripMenuItem.Name = "saveCurrentToolStripMenuItem";
            saveCurrentToolStripMenuItem.Size = new Size(106, 24);
            saveCurrentToolStripMenuItem.Text = "Save Current";
            saveCurrentToolStripMenuItem.Click += saveCurrentToolStripMenuItem_Click;
            // 
            // backToolStripMenuItem
            // 
            backToolStripMenuItem.Name = "backToolStripMenuItem";
            backToolStripMenuItem.Size = new Size(54, 24);
            backToolStripMenuItem.Text = "Back";
            backToolStripMenuItem.Click += backToolStripMenuItem_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.DropDownItems.AddRange(new ToolStripItem[] { csvWithdStrongToolStripMenuItem, csvWithuStrongToolStripMenuItem });
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(113, 24);
            toolStripMenuItem2.Text = "Generate CSV";
            // 
            // csvWithdStrongToolStripMenuItem
            // 
            csvWithdStrongToolStripMenuItem.Name = "csvWithdStrongToolStripMenuItem";
            csvWithdStrongToolStripMenuItem.Size = new Size(180, 26);
            csvWithdStrongToolStripMenuItem.Text = "With dStrong";
            csvWithdStrongToolStripMenuItem.Click += csvWithdStrongToolStripMenuItem_Click;
            // 
            // csvWithuStrongToolStripMenuItem
            // 
            csvWithuStrongToolStripMenuItem.Name = "csvWithuStrongToolStripMenuItem";
            csvWithuStrongToolStripMenuItem.Size = new Size(180, 26);
            csvWithuStrongToolStripMenuItem.Text = "WithuStrong";
            csvWithuStrongToolStripMenuItem.Click += csvWithuStrongToolStripMenuItem_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1643, 947);
            Controls.Add(mainSplitContainer);
            Controls.Add(tbTranslatedShort);
            Controls.Add(tbShortText);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
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
            leftSplitContainer.Panel1.ResumeLayout(false);
            leftSplitContainer.Panel1.PerformLayout();
            leftSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)leftSplitContainer).EndInit();
            leftSplitContainer.ResumeLayout(false);
            leftBottomSplitContainer.Panel1.ResumeLayout(false);
            leftBottomSplitContainer.Panel1.PerformLayout();
            leftBottomSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)leftBottomSplitContainer).EndInit();
            leftBottomSplitContainer.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
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
        private MyNumericControl strongsNumberControl;
        private PictureBox pbUP;
        private PictureBox pbDown;
        private ComboBox cbLanguage;
        private Label label1;
        private TextBox tbOriginal;
        private TextBox tbEnglish;
        private TextBox tbShortText;
        private TextBox tbLongText;
        private TextBox tbTranslatedShort;
        private TextBox tbTranslatedWord;
        private DataGridView dgvWords;
        private DataGridView dgvVerses;
        private TextBox tbDstrongs;
        private FolderBrowserDialog folderBrowserDialog1;
        private SplitContainer mainSplitContainer;
        private SplitContainer rightSplitContainer;
        private SplitContainer leftSplitContainer;
        private SplitContainer leftBottomSplitContainer;
        private SplitContainer rightBottomSplitContainer;
        private RichTextBox tbTranslatedLong;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem generateJSONToolStripMenuItem;
        private ToolStripMenuItem saveCurrentToolStripMenuItem;
        private ToolStripMenuItem backToolStripMenuItem;
        private Panel panel1;
        private Button btnZspace;
        private ContextMenuStrip contextMenuStrip1;
        private Button btnStopSearch;
        private Button btnSearch;
        private TextBox tbSearchStart;
        private TextBox tbSearch;
        private Button btnNext;
        private ToolStripMenuItem withDStrongToolStripMenuItem;
        private ToolStripMenuItem withuStrongToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem csvWithdStrongToolStripMenuItem;
        private ToolStripMenuItem csvWithuStrongToolStripMenuItem;
        private TextBox tbArabicTxLt;
    }
}