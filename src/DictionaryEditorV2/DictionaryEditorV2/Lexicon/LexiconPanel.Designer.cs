
namespace DictionaryEditorV2.Lexicon
{
    partial class LexiconPanel
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
            tableLayoutPanel7 = new TableLayoutPanel();
            tbLongText = new RichTextBox();
            tableLayoutPanel8 = new TableLayoutPanel();
            cbLanguage = new ComboBox();
            tbOriginal = new TextBox();
            strongsNumberControl = new MyNumericControl();
            btnPreviousStrong = new Button();
            btnNextStrong = new Button();
            cbDStrong = new ComboBox();
            tbEnglish = new TextBox();
            tableLayoutPanel7.SuspendLayout();
            tableLayoutPanel8.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel7
            // 
            tableLayoutPanel7.ColumnCount = 1;
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel7.Controls.Add(tbLongText, 0, 1);
            tableLayoutPanel7.Controls.Add(tableLayoutPanel8, 0, 0);
            tableLayoutPanel7.Dock = DockStyle.Fill;
            tableLayoutPanel7.Location = new Point(0, 0);
            tableLayoutPanel7.Margin = new Padding(3, 2, 3, 2);
            tableLayoutPanel7.Name = "tableLayoutPanel7";
            tableLayoutPanel7.RowCount = 2;
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel7.Size = new Size(700, 338);
            tableLayoutPanel7.TabIndex = 3;
            // 
            // tbLongText
            // 
            tbLongText.Dock = DockStyle.Fill;
            tbLongText.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            tbLongText.Location = new Point(3, 47);
            tbLongText.Margin = new Padding(3, 2, 3, 2);
            tbLongText.Name = "tbLongText";
            tbLongText.ReadOnly = true;
            tbLongText.Size = new Size(694, 289);
            tbLongText.TabIndex = 0;
            tbLongText.Text = "";
            // 
            // tableLayoutPanel8
            // 
            tableLayoutPanel8.ColumnCount = 8;
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10.7249069F));
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13.8990774F));
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.646302F));
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 19.6141472F));
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5401926F));
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel8.Controls.Add(cbLanguage, 0, 0);
            tableLayoutPanel8.Controls.Add(tbOriginal, 5, 0);
            tableLayoutPanel8.Controls.Add(strongsNumberControl, 2, 0);
            tableLayoutPanel8.Controls.Add(btnPreviousStrong, 1, 0);
            tableLayoutPanel8.Controls.Add(btnNextStrong, 4, 0);
            tableLayoutPanel8.Controls.Add(cbDStrong, 3, 0);
            tableLayoutPanel8.Controls.Add(tbEnglish, 7, 0);
            tableLayoutPanel8.Dock = DockStyle.Fill;
            tableLayoutPanel8.Location = new Point(3, 2);
            tableLayoutPanel8.Margin = new Padding(3, 2, 3, 2);
            tableLayoutPanel8.Name = "tableLayoutPanel8";
            tableLayoutPanel8.RowCount = 1;
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel8.Size = new Size(694, 41);
            tableLayoutPanel8.TabIndex = 1;
            // 
            // cbLanguage
            // 
            cbLanguage.Dock = DockStyle.Fill;
            cbLanguage.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            cbLanguage.FormattingEnabled = true;
            cbLanguage.Location = new Point(3, 2);
            cbLanguage.Margin = new Padding(3, 2, 3, 2);
            cbLanguage.Name = "cbLanguage";
            cbLanguage.Size = new Size(60, 29);
            cbLanguage.TabIndex = 0;
            cbLanguage.SelectedIndexChanged += CbLanguage_SelectedIndexChanged;
            // 
            // tbOriginal
            // 
            tbOriginal.Dock = DockStyle.Fill;
            tbOriginal.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            tbOriginal.Location = new Point(287, 2);
            tbOriginal.Margin = new Padding(3, 2, 3, 2);
            tbOriginal.Name = "tbOriginal";
            tbOriginal.Size = new Size(116, 29);
            tbOriginal.TabIndex = 3;
            // 
            // strongsNumberControl
            // 
            strongsNumberControl.Dock = DockStyle.Fill;
            strongsNumberControl.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            strongsNumberControl.Location = new Point(105, 2);
            strongsNumberControl.Margin = new Padding(3, 2, 3, 2);
            strongsNumberControl.Name = "strongsNumberControl";
            strongsNumberControl.Size = new Size(80, 29);
            strongsNumberControl.TabIndex = 1;
            strongsNumberControl.Text = "0001";
            strongsNumberControl.Value = 1;
            // 
            // btnPreviousStrong
            // 
            btnPreviousStrong.Font = new Font("Snap ITC", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPreviousStrong.Location = new Point(69, 3);
            btnPreviousStrong.Name = "btnPreviousStrong";
            btnPreviousStrong.Size = new Size(30, 30);
            btnPreviousStrong.TabIndex = 5;
            btnPreviousStrong.Text = "-";
            btnPreviousStrong.UseVisualStyleBackColor = true;
            btnPreviousStrong.Click += btnPreviousStrong_Click;
            // 
            // btnNextStrong
            // 
            btnNextStrong.FlatStyle = FlatStyle.System;
            btnNextStrong.Font = new Font("Wingdings 2", 12F, FontStyle.Bold, GraphicsUnit.Point, 2);
            btnNextStrong.Location = new Point(251, 3);
            btnNextStrong.Name = "btnNextStrong";
            btnNextStrong.Size = new Size(30, 30);
            btnNextStrong.TabIndex = 5;
            btnNextStrong.Text = "Ë";
            btnNextStrong.UseVisualStyleBackColor = true;
            btnNextStrong.Click += btnNextStrong_Click;
            // 
            // cbDStrong
            // 
            cbDStrong.Dock = DockStyle.Fill;
            cbDStrong.FlatStyle = FlatStyle.System;
            cbDStrong.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            cbDStrong.FormattingEnabled = true;
            cbDStrong.Location = new Point(191, 3);
            cbDStrong.Name = "cbDStrong";
            cbDStrong.Size = new Size(54, 29);
            cbDStrong.TabIndex = 4;
            cbDStrong.SelectedIndexChanged += CbDStrong_SelectedIndexChanged;
            // 
            // tbEnglish
            // 
            tbEnglish.Dock = DockStyle.Fill;
            tbEnglish.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            tbEnglish.Location = new Point(487, 2);
            tbEnglish.Margin = new Padding(3, 2, 3, 2);
            tbEnglish.Name = "tbEnglish";
            tbEnglish.Size = new Size(204, 29);
            tbEnglish.TabIndex = 3;
            // 
            // LexiconPanel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 338);
            Controls.Add(tableLayoutPanel7);
            Margin = new Padding(3, 2, 3, 2);
            Name = "LexiconPanel";
            Text = "VerseSelectionPanel";
            Load += LexiconEntryPanel_Load;
            tableLayoutPanel7.ResumeLayout(false);
            tableLayoutPanel8.ResumeLayout(false);
            tableLayoutPanel8.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.RichTextBox tbLongText;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.ComboBox cbLanguage;
        private MyNumericControl strongsNumberControl;
        private System.Windows.Forms.TextBox tbOriginal;
        private System.Windows.Forms.TextBox tbEnglish;
        private ComboBox cbDStrong;
        private Button btnPreviousStrong;
        private Button btnNextStrong;
    }
}