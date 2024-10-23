
namespace DictionaryEditorV2
{
    partial class SettingsForm
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
            btnCancel = new Button();
            btnOK = new Button();
            label1 = new Label();
            cbDatabases = new ComboBox();
            tabControl1 = new TabControl();
            database = new TabPage();
            googleSheets = new TabPage();
            lblInvlidSheetsCreds = new Label();
            tbSheetsApplicationName = new TextBox();
            btnSheetsCredentialsPath = new Button();
            tbSheetsCredentialsPath = new TextBox();
            label3 = new Label();
            label2 = new Label();
            googleDrive = new TabPage();
            lblInvlidDriveCreds = new Label();
            tbDriveApplicationName = new TextBox();
            btnDriveCredentialsPath = new Button();
            tbDriveCredentialsPath = new TextBox();
            label5 = new Label();
            label6 = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            openFileDialog1 = new OpenFileDialog();
            btnPostgresqlConfig = new Button();
            tbPostgresqlConfig = new TextBox();
            label4 = new Label();
            tabControl1.SuspendLayout();
            database.SuspendLayout();
            googleSheets.SuspendLayout();
            googleDrive.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.None;
            btnCancel.Location = new Point(69, 11);
            btnCancel.Margin = new Padding(3, 2, 3, 2);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(82, 26);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            btnOK.Anchor = AnchorStyles.None;
            btnOK.Location = new Point(290, 11);
            btnOK.Margin = new Padding(3, 2, 3, 2);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(82, 26);
            btnOK.TabIndex = 4;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 21);
            label1.Name = "label1";
            label1.Size = new Size(55, 15);
            label1.TabIndex = 5;
            label1.Text = "Database";
            // 
            // cbDatabases
            // 
            cbDatabases.FormattingEnabled = true;
            cbDatabases.Location = new Point(110, 13);
            cbDatabases.Name = "cbDatabases";
            cbDatabases.Size = new Size(258, 23);
            cbDatabases.TabIndex = 6;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(database);
            tabControl1.Controls.Add(googleSheets);
            tabControl1.Controls.Add(googleDrive);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(3, 3);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(442, 215);
            tabControl1.TabIndex = 7;
            // 
            // database
            // 
            database.Controls.Add(btnPostgresqlConfig);
            database.Controls.Add(tbPostgresqlConfig);
            database.Controls.Add(label4);
            database.Controls.Add(label1);
            database.Controls.Add(cbDatabases);
            database.Location = new Point(4, 24);
            database.Name = "database";
            database.Padding = new Padding(3);
            database.Size = new Size(434, 187);
            database.TabIndex = 0;
            database.Text = "Database";
            database.UseVisualStyleBackColor = true;
            // 
            // googleSheets
            // 
            googleSheets.Controls.Add(lblInvlidSheetsCreds);
            googleSheets.Controls.Add(tbSheetsApplicationName);
            googleSheets.Controls.Add(btnSheetsCredentialsPath);
            googleSheets.Controls.Add(tbSheetsCredentialsPath);
            googleSheets.Controls.Add(label3);
            googleSheets.Controls.Add(label2);
            googleSheets.Location = new Point(4, 24);
            googleSheets.Name = "googleSheets";
            googleSheets.Padding = new Padding(3);
            googleSheets.Size = new Size(434, 187);
            googleSheets.TabIndex = 3;
            googleSheets.Text = "Google Sheets";
            googleSheets.UseVisualStyleBackColor = true;
            // 
            // lblInvlidSheetsCreds
            // 
            lblInvlidSheetsCreds.AutoSize = true;
            lblInvlidSheetsCreds.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblInvlidSheetsCreds.ForeColor = Color.Red;
            lblInvlidSheetsCreds.Location = new Point(129, 97);
            lblInvlidSheetsCreds.Name = "lblInvlidSheetsCreds";
            lblInvlidSheetsCreds.Size = new Size(182, 21);
            lblInvlidSheetsCreds.TabIndex = 9;
            lblInvlidSheetsCreds.Text = "Invalid Credential File!";
            lblInvlidSheetsCreds.Visible = false;
            // 
            // tbSheetsApplicationName
            // 
            tbSheetsApplicationName.Location = new Point(129, 26);
            tbSheetsApplicationName.Name = "tbSheetsApplicationName";
            tbSheetsApplicationName.Size = new Size(258, 23);
            tbSheetsApplicationName.TabIndex = 9;
            // 
            // btnSheetsCredentialsPath
            // 
            btnSheetsCredentialsPath.BackColor = SystemColors.ButtonFace;
            btnSheetsCredentialsPath.BackgroundImageLayout = ImageLayout.Stretch;
            btnSheetsCredentialsPath.Font = new Font("Wingdings", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 2);
            btnSheetsCredentialsPath.Location = new Point(393, 66);
            btnSheetsCredentialsPath.Name = "btnSheetsCredentialsPath";
            btnSheetsCredentialsPath.Size = new Size(33, 31);
            btnSheetsCredentialsPath.TabIndex = 8;
            btnSheetsCredentialsPath.Text = "1";
            btnSheetsCredentialsPath.UseVisualStyleBackColor = false;
            btnSheetsCredentialsPath.Click += btnSheetsCredentialsPath_Click;
            // 
            // tbSheetsCredentialsPath
            // 
            tbSheetsCredentialsPath.Location = new Point(129, 71);
            tbSheetsCredentialsPath.Name = "tbSheetsCredentialsPath";
            tbSheetsCredentialsPath.Size = new Size(258, 23);
            tbSheetsCredentialsPath.TabIndex = 7;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 29);
            label3.Name = "label3";
            label3.Size = new Size(103, 15);
            label3.TabIndex = 8;
            label3.Text = "Application Name";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 74);
            label2.Name = "label2";
            label2.Size = new Size(97, 15);
            label2.TabIndex = 6;
            label2.Text = "Credentials JSON";
            // 
            // googleDrive
            // 
            googleDrive.Controls.Add(lblInvlidDriveCreds);
            googleDrive.Controls.Add(tbDriveApplicationName);
            googleDrive.Controls.Add(btnDriveCredentialsPath);
            googleDrive.Controls.Add(tbDriveCredentialsPath);
            googleDrive.Controls.Add(label5);
            googleDrive.Controls.Add(label6);
            googleDrive.Location = new Point(4, 24);
            googleDrive.Name = "googleDrive";
            googleDrive.Padding = new Padding(3);
            googleDrive.Size = new Size(434, 187);
            googleDrive.TabIndex = 4;
            googleDrive.Text = "Google Drive";
            googleDrive.UseVisualStyleBackColor = true;
            // 
            // lblInvlidDriveCreds
            // 
            lblInvlidDriveCreds.AutoSize = true;
            lblInvlidDriveCreds.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblInvlidDriveCreds.ForeColor = Color.Red;
            lblInvlidDriveCreds.Location = new Point(130, 98);
            lblInvlidDriveCreds.Name = "lblInvlidDriveCreds";
            lblInvlidDriveCreds.Size = new Size(182, 21);
            lblInvlidDriveCreds.TabIndex = 14;
            lblInvlidDriveCreds.Text = "Invalid Credential File!";
            lblInvlidDriveCreds.Visible = false;
            // 
            // tbDriveApplicationName
            // 
            tbDriveApplicationName.Location = new Point(130, 27);
            tbDriveApplicationName.Name = "tbDriveApplicationName";
            tbDriveApplicationName.Size = new Size(258, 23);
            tbDriveApplicationName.TabIndex = 15;
            // 
            // btnDriveCredentialsPath
            // 
            btnDriveCredentialsPath.BackColor = SystemColors.ButtonFace;
            btnDriveCredentialsPath.BackgroundImageLayout = ImageLayout.Stretch;
            btnDriveCredentialsPath.Font = new Font("Wingdings", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 2);
            btnDriveCredentialsPath.Location = new Point(394, 67);
            btnDriveCredentialsPath.Name = "btnDriveCredentialsPath";
            btnDriveCredentialsPath.Size = new Size(33, 31);
            btnDriveCredentialsPath.TabIndex = 12;
            btnDriveCredentialsPath.Text = "1";
            btnDriveCredentialsPath.UseVisualStyleBackColor = false;
            btnDriveCredentialsPath.Click += btnDriveCredentialsPath_Click;
            // 
            // tbDriveCredentialsPath
            // 
            tbDriveCredentialsPath.Location = new Point(130, 72);
            tbDriveCredentialsPath.Name = "tbDriveCredentialsPath";
            tbDriveCredentialsPath.Size = new Size(258, 23);
            tbDriveCredentialsPath.TabIndex = 11;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(7, 30);
            label5.Name = "label5";
            label5.Size = new Size(103, 15);
            label5.TabIndex = 13;
            label5.Text = "Application Name";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(7, 75);
            label6.Name = "label6";
            label6.Size = new Size(97, 15);
            label6.TabIndex = 10;
            label6.Text = "Credentials JSON";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(tabControl1, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 80.71429F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 19.2857151F));
            tableLayoutPanel1.Size = new Size(448, 275);
            tableLayoutPanel1.TabIndex = 8;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(btnCancel, 0, 0);
            tableLayoutPanel2.Controls.Add(btnOK, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 224);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(442, 48);
            tableLayoutPanel2.TabIndex = 8;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnPostgresqlConfig
            // 
            btnPostgresqlConfig.BackColor = SystemColors.ButtonFace;
            btnPostgresqlConfig.BackgroundImageLayout = ImageLayout.Stretch;
            btnPostgresqlConfig.Font = new Font("Wingdings", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 2);
            btnPostgresqlConfig.Location = new Point(385, 47);
            btnPostgresqlConfig.Name = "btnPostgresqlConfig";
            btnPostgresqlConfig.Size = new Size(33, 31);
            btnPostgresqlConfig.TabIndex = 11;
            btnPostgresqlConfig.Text = "1";
            btnPostgresqlConfig.UseVisualStyleBackColor = false;
            btnPostgresqlConfig.Click += btnPostgresqlConfig_Click;
            // 
            // tbPostgresqlConfig
            // 
            tbPostgresqlConfig.Location = new Point(110, 52);
            tbPostgresqlConfig.Name = "tbPostgresqlConfig";
            tbPostgresqlConfig.Size = new Size(258, 23);
            tbPostgresqlConfig.TabIndex = 10;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(3, 55);
            label4.Name = "label4";
            label4.Size = new Size(93, 15);
            label4.TabIndex = 9;
            label4.Text = "Postgresql JSON";
            // 
            // SettingsForm
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(448, 275);
            Controls.Add(tableLayoutPanel1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "SettingsForm";
            Text = "SettingsForm";
            Load += SettingsForm_Load;
            tabControl1.ResumeLayout(false);
            database.ResumeLayout(false);
            database.PerformLayout();
            googleSheets.ResumeLayout(false);
            googleSheets.PerformLayout();
            googleDrive.ResumeLayout(false);
            googleDrive.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private Label label1;
        private ComboBox cbDatabases;
        private TabControl tabControl1;
        private TabPage database;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private OpenFileDialog openFileDialog1;
        private TabPage googleSheets;
        private TextBox tbSheetsCredentialsPath;
        private Label label2;
        private Button btnSheetsCredentialsPath;
        private Label lblInvlidSheetsCreds;
        private TextBox tbSheetsApplicationName;
        private Label label3;
        private TabPage googleDrive;
        private Label lblInvlidDriveCreds;
        private TextBox tbDriveApplicationName;
        private Button btnDriveCredentialsPath;
        private TextBox tbDriveCredentialsPath;
        private Label label5;
        private Label label6;
        private Button btnPostgresqlConfig;
        private TextBox tbPostgresqlConfig;
        private Label label4;
    }
}