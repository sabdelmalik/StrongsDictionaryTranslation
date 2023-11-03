namespace PopulateStrongsDictionary
{
    partial class ConfigurationDialog
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
            label2 = new Label();
            label3 = new Label();
            tbTaggedBible = new TextBox();
            btnTaggedBible = new Button();
            textBox4 = new TextBox();
            button4 = new Button();
            folderBrowserDialog1 = new FolderBrowserDialog();
            openFileDialog1 = new OpenFileDialog();
            btnDone = new Button();
            btnCancel = new Button();
            label5 = new Label();
            comboBoxLanguage = new ComboBox();
            label4 = new Label();
            groupBox1 = new GroupBox();
            rbtnTabSeparatedText = new RadioButton();
            rbtnSpreadsheets = new RadioButton();
            btnSourceTabSeparatedText = new Button();
            label6 = new Label();
            tbSourceTabSeparatedText = new TextBox();
            tbSourceSpreadsheets = new TextBox();
            label7 = new Label();
            label1 = new Label();
            tbTranslatedSpreadsheets = new TextBox();
            tbTranslatedTabSeparatedText = new TextBox();
            btnSourceSpreadsheets = new Button();
            btnTranslatedSpreadsheets = new Button();
            btnTranslatedTabSeparatedText = new Button();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(2, 91);
            label2.Name = "label2";
            label2.Size = new Size(169, 20);
            label2.TabIndex = 0;
            label2.Text = "Translated Spreadsheets";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(10, 283);
            label3.Name = "label3";
            label3.Size = new Size(96, 20);
            label3.TabIndex = 0;
            label3.Text = "Tagged Bible";
            // 
            // tbTaggedBible
            // 
            tbTaggedBible.Location = new Point(234, 280);
            tbTaggedBible.Name = "tbTaggedBible";
            tbTaggedBible.Size = new Size(507, 27);
            tbTaggedBible.TabIndex = 1;
            // 
            // btnTaggedBible
            // 
            btnTaggedBible.BackColor = SystemColors.ButtonFace;
            btnTaggedBible.BackgroundImage = Properties.Resources.ellipsisTX;
            btnTaggedBible.BackgroundImageLayout = ImageLayout.Stretch;
            btnTaggedBible.Location = new Point(753, 278);
            btnTaggedBible.Name = "btnTaggedBible";
            btnTaggedBible.Size = new Size(33, 31);
            btnTaggedBible.TabIndex = 2;
            btnTaggedBible.UseVisualStyleBackColor = false;
            btnTaggedBible.Click += btnTaggedBible_Click;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(234, 383);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(507, 27);
            textBox4.TabIndex = 1;
            // 
            // button4
            // 
            button4.BackColor = SystemColors.ButtonFace;
            button4.BackgroundImage = Properties.Resources.ellipsisTX;
            button4.BackgroundImageLayout = ImageLayout.Stretch;
            button4.Location = new Point(753, 381);
            button4.Name = "button4";
            button4.Size = new Size(33, 31);
            button4.TabIndex = 2;
            button4.UseVisualStyleBackColor = false;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnDone
            // 
            btnDone.DialogResult = DialogResult.OK;
            btnDone.Location = new Point(536, 416);
            btnDone.Name = "btnDone";
            btnDone.Size = new Size(94, 29);
            btnDone.TabIndex = 3;
            btnDone.Text = "Done";
            btnDone.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(156, 416);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(94, 29);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(10, 386);
            label5.Name = "label5";
            label5.Size = new Size(116, 20);
            label5.TabIndex = 0;
            label5.Text = "CorrectedWords";
            // 
            // comboBoxLanguage
            // 
            comboBoxLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxLanguage.FormattingEnabled = true;
            comboBoxLanguage.Location = new Point(234, 332);
            comboBoxLanguage.Name = "comboBoxLanguage";
            comboBoxLanguage.Size = new Size(507, 28);
            comboBoxLanguage.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(10, 335);
            label4.Name = "label4";
            label4.Size = new Size(74, 20);
            label4.TabIndex = 0;
            label4.Text = "Language";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(rbtnTabSeparatedText);
            groupBox1.Controls.Add(rbtnSpreadsheets);
            groupBox1.Controls.Add(btnSourceTabSeparatedText);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(tbSourceTabSeparatedText);
            groupBox1.Controls.Add(tbSourceSpreadsheets);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(tbTranslatedSpreadsheets);
            groupBox1.Controls.Add(tbTranslatedTabSeparatedText);
            groupBox1.Controls.Add(btnSourceSpreadsheets);
            groupBox1.Controls.Add(btnTranslatedSpreadsheets);
            groupBox1.Controls.Add(btnTranslatedTabSeparatedText);
            groupBox1.Location = new Point(10, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(786, 231);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            groupBox1.Text = "Dictionary";
            // 
            // rbtnTabSeparatedText
            // 
            rbtnTabSeparatedText.AutoSize = true;
            rbtnTabSeparatedText.Location = new Point(439, 13);
            rbtnTabSeparatedText.Name = "rbtnTabSeparatedText";
            rbtnTabSeparatedText.Size = new Size(156, 24);
            rbtnTabSeparatedText.TabIndex = 11;
            rbtnTabSeparatedText.TabStop = true;
            rbtnTabSeparatedText.Text = "Tab Separated Text";
            rbtnTabSeparatedText.UseVisualStyleBackColor = true;
            rbtnTabSeparatedText.CheckedChanged += rbtnTabSeparatedText_CheckedChanged;
            // 
            // rbtnSpreadsheets
            // 
            rbtnSpreadsheets.AutoSize = true;
            rbtnSpreadsheets.Location = new Point(224, 13);
            rbtnSpreadsheets.Name = "rbtnSpreadsheets";
            rbtnSpreadsheets.Size = new Size(118, 24);
            rbtnSpreadsheets.TabIndex = 10;
            rbtnSpreadsheets.TabStop = true;
            rbtnSpreadsheets.Text = "Spreadsheets";
            rbtnSpreadsheets.UseVisualStyleBackColor = true;
            rbtnSpreadsheets.CheckedChanged += rbtnSpreadsheets_CheckedChanged;
            // 
            // btnSourceTabSeparatedText
            // 
            btnSourceTabSeparatedText.BackColor = SystemColors.ButtonFace;
            btnSourceTabSeparatedText.BackgroundImage = Properties.Resources.ellipsisTX;
            btnSourceTabSeparatedText.BackgroundImageLayout = ImageLayout.Stretch;
            btnSourceTabSeparatedText.Location = new Point(742, 127);
            btnSourceTabSeparatedText.Name = "btnSourceTabSeparatedText";
            btnSourceTabSeparatedText.Size = new Size(33, 31);
            btnSourceTabSeparatedText.TabIndex = 9;
            btnSourceTabSeparatedText.UseVisualStyleBackColor = false;
            btnSourceTabSeparatedText.Click += btnSourceTabSeparatedText_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(2, 171);
            label6.Name = "label6";
            label6.Size = new Size(207, 20);
            label6.TabIndex = 0;
            label6.Text = "Translated Tab Separated Text";
            // 
            // tbSourceTabSeparatedText
            // 
            tbSourceTabSeparatedText.Location = new Point(225, 127);
            tbSourceTabSeparatedText.Name = "tbSourceTabSeparatedText";
            tbSourceTabSeparatedText.Size = new Size(507, 27);
            tbSourceTabSeparatedText.TabIndex = 8;
            // 
            // tbSourceSpreadsheets
            // 
            tbSourceSpreadsheets.Location = new Point(224, 47);
            tbSourceSpreadsheets.Name = "tbSourceSpreadsheets";
            tbSourceSpreadsheets.Size = new Size(507, 27);
            tbSourceSpreadsheets.TabIndex = 1;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(2, 131);
            label7.Name = "label7";
            label7.Size = new Size(184, 20);
            label7.TabIndex = 7;
            label7.Text = "Source Tab Separated Text";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(2, 51);
            label1.Name = "label1";
            label1.Size = new Size(146, 20);
            label1.TabIndex = 0;
            label1.Text = "Source Spreadsheets";
            // 
            // tbTranslatedSpreadsheets
            // 
            tbTranslatedSpreadsheets.Location = new Point(224, 87);
            tbTranslatedSpreadsheets.Name = "tbTranslatedSpreadsheets";
            tbTranslatedSpreadsheets.Size = new Size(507, 27);
            tbTranslatedSpreadsheets.TabIndex = 1;
            // 
            // tbTranslatedTabSeparatedText
            // 
            tbTranslatedTabSeparatedText.Location = new Point(224, 167);
            tbTranslatedTabSeparatedText.Name = "tbTranslatedTabSeparatedText";
            tbTranslatedTabSeparatedText.Size = new Size(507, 27);
            tbTranslatedTabSeparatedText.TabIndex = 1;
            // 
            // btnSourceSpreadsheets
            // 
            btnSourceSpreadsheets.BackColor = SystemColors.ButtonFace;
            btnSourceSpreadsheets.BackgroundImage = Properties.Resources.ellipsisTX;
            btnSourceSpreadsheets.BackgroundImageLayout = ImageLayout.Stretch;
            btnSourceSpreadsheets.Location = new Point(742, 47);
            btnSourceSpreadsheets.Name = "btnSourceSpreadsheets";
            btnSourceSpreadsheets.Size = new Size(33, 31);
            btnSourceSpreadsheets.TabIndex = 2;
            btnSourceSpreadsheets.UseVisualStyleBackColor = false;
            btnSourceSpreadsheets.Click += btnSourceSpreadsheets_Click;
            // 
            // btnTranslatedSpreadsheets
            // 
            btnTranslatedSpreadsheets.BackColor = SystemColors.ButtonFace;
            btnTranslatedSpreadsheets.BackgroundImage = Properties.Resources.ellipsisTX;
            btnTranslatedSpreadsheets.BackgroundImageLayout = ImageLayout.Stretch;
            btnTranslatedSpreadsheets.Location = new Point(742, 87);
            btnTranslatedSpreadsheets.Name = "btnTranslatedSpreadsheets";
            btnTranslatedSpreadsheets.Size = new Size(33, 31);
            btnTranslatedSpreadsheets.TabIndex = 2;
            btnTranslatedSpreadsheets.UseVisualStyleBackColor = false;
            btnTranslatedSpreadsheets.Click += btnTranslatedSpreadsheets_Click;
            // 
            // btnTranslatedTabSeparatedText
            // 
            btnTranslatedTabSeparatedText.BackColor = SystemColors.ButtonFace;
            btnTranslatedTabSeparatedText.BackgroundImage = Properties.Resources.ellipsisTX;
            btnTranslatedTabSeparatedText.BackgroundImageLayout = ImageLayout.Stretch;
            btnTranslatedTabSeparatedText.Location = new Point(742, 167);
            btnTranslatedTabSeparatedText.Name = "btnTranslatedTabSeparatedText";
            btnTranslatedTabSeparatedText.Size = new Size(33, 31);
            btnTranslatedTabSeparatedText.TabIndex = 2;
            btnTranslatedTabSeparatedText.UseVisualStyleBackColor = false;
            btnTranslatedTabSeparatedText.Click += btnTranslatedTabSeparatedText_Click;
            // 
            // ConfigurationDialog
            // 
            AcceptButton = btnDone;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonFace;
            CancelButton = btnCancel;
            ClientSize = new Size(983, 748);
            Controls.Add(comboBoxLanguage);
            Controls.Add(btnCancel);
            Controls.Add(btnDone);
            Controls.Add(button4);
            Controls.Add(btnTaggedBible);
            Controls.Add(textBox4);
            Controls.Add(label5);
            Controls.Add(tbTaggedBible);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(groupBox1);
            Name = "ConfigurationDialog";
            Text = "Configuration";
            Load += ConfigurationDialog_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label2;
        private Label label3;
        private TextBox tbTaggedBible;
        private Button btnTaggedBible;
        private TextBox textBox4;
        private Button button4;
        private FolderBrowserDialog folderBrowserDialog1;
        private OpenFileDialog openFileDialog1;
        private Button btnDone;
        private Button btnCancel;
        private Label label5;
        private ComboBox comboBoxLanguage;
        private Label label4;
        private GroupBox groupBox1;
        private Button btnSourceTabSeparatedText;
        private Label label6;
        private TextBox tbSourceTabSeparatedText;
        private TextBox tbSourceSpreadsheets;
        private Label label7;
        private Label label1;
        private TextBox tbTranslatedSpreadsheets;
        private TextBox tbTranslatedTabSeparatedText;
        private Button btnSourceSpreadsheets;
        private Button btnTranslatedSpreadsheets;
        private Button btnTranslatedTabSeparatedText;
        private RadioButton rbtnTabSeparatedText;
        private RadioButton rbtnSpreadsheets;
    }
}