using DictionaryEditorV2.Lexicon;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using WeifenLuo.WinFormsUI.Docking;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DictionaryEditorV2
{
    public partial class SettingsForm : DockContent
    {
        private string dbName = "StrongsDictionaryV2";
        private NpgsqlDataSource dataSource;
        private NpgsqlConnection connection;

        public SettingsForm()
        {
            InitializeComponent();

            // https://learn.microsoft.com/en-us/dotnet/desktop/winforms/advanced/how-to-read-settings-at-run-time-with-csharp?view=netframeworkdesktop-4.8

        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1Na61A51;Database=" + dbName;
            dataSource = NpgsqlDataSource.Create(connectionString);
            connection = dataSource.CreateConnection();

            cbDatabases.Items.AddRange(GetDatabases());

            string database = Properties.Settings.Default.Database;
            if (string.IsNullOrEmpty(database))
            {
                cbDatabases.SelectedIndex = 0;
            }
            else
            {
                cbDatabases.SelectedIndex = cbDatabases.Items.IndexOf(database);
            }

            lblInvlidSheetsCreds.Visible = false;

            string appName = Properties.Settings.Default.SheetsCredentialFileName;
            if (!string.IsNullOrEmpty(appName))
                tbSheetsApplicationName.Text = appName;
            appName = Properties.Settings.Default.DriveCredentialFileName;
            if (!string.IsNullOrEmpty(appName))
                tbDriveApplicationName.Text = appName;

            string credsPath = Properties.Settings.Default.DriveApplicationName;
            if (!string.IsNullOrEmpty(credsPath) && isValidCreds(credsPath))
                tbSheetsCredentialsPath.Text = credsPath;
            else
                lblInvlidSheetsCreds.Visible = true;

            credsPath = Properties.Settings.Default.PostgresqlConfig;
            if (!string.IsNullOrEmpty(credsPath) && isValidCreds(credsPath))
                tbDriveCredentialsPath.Text = credsPath;
            else
                lblInvlidDriveCreds.Visible = true;

            if (string.IsNullOrEmpty(tbSheetsApplicationName.Text) || string.IsNullOrEmpty(tbSheetsCredentialsPath.Text))
                tabControl1.SelectedTab = tabControl1.TabPages["googleSheets"];
            else if (string.IsNullOrEmpty(tbDriveApplicationName.Text) || string.IsNullOrEmpty(tbDriveCredentialsPath.Text))
                tabControl1.SelectedTab = tabControl1.TabPages["googleDrive"];
        }


        public string[] GetDatabases()
        {
            List<string> databases = new List<string>();
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();

                NpgsqlCommand command = dataSource.CreateCommand();

                command.CommandText =
                    "SELECT dbs.datname " +
                    " FROM pg_database dbs" +
                    " INNER JOIN pg_authid dba" +
                    " ON dbs.datdba=dba.oid" +
                    " WHERE dba.rolname='sabdelmalik';";

                NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    databases.Add(reader.GetString(0));
                }
                command.Dispose();

                reader.Close();
                reader.DisposeAsync();
                command.Dispose();
                connection.Close();
            }
            catch (Exception ex)
            {
                //Trace("GetStrongsEntry Exception\r\n" + ex.ToString(), Color.Red);
            }

            return databases.ToArray();
        }

        public bool PeriodicSaveEnabled
        {
            get { return true; }
            //set { cbPeriodicSave.Checked = value; }
        }

        public int SavePeriod
        {
            get
            {
                return 12;
            }
            set
            {
                // nudSavePeriod.Value = value;
            }
        }

        private void cbPeriodicSave_CheckedChanged(object sender, EventArgs e)
        {
            // nudSavePeriod.Enabled = cbPeriodicSave.Checked;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // databse
            // =======================
            Properties.Settings.Default.Database = cbDatabases.Text;
            Properties.Settings.Default.PostgresqlConfig = tbPostgresqlConfig.Text;

            // google sheets
            // =======================
            if (!string.IsNullOrEmpty(tbSheetsApplicationName.Text))
                Properties.Settings.Default.SheetsCredentialFileName = tbSheetsApplicationName.Text;
            else
            {
                MessageBox.Show("Invalid Sheets Application Name.");
                tabControl1.SelectedTab = tabControl1.TabPages["googleSheets"];
                return;
            }

            if (!lblInvlidSheetsCreds.Visible)
                Properties.Settings.Default.DriveApplicationName = tbSheetsCredentialsPath.Text;
            else
            {
                MessageBox.Show("Invalid Sheets Credentials");
                tabControl1.SelectedTab = tabControl1.TabPages["googleSheets"];
                return;
            }

            // google drive
            // =======================

            if (!string.IsNullOrEmpty(tbDriveApplicationName.Text))
                Properties.Settings.Default.DriveCredentialFileName = tbDriveApplicationName.Text;
            else
            {
                MessageBox.Show("Invalid Drive Application Name.");
                tabControl1.SelectedTab = tabControl1.TabPages["googleDrive"];
                return;
            }


            if (!lblInvlidDriveCreds.Visible)
                Properties.Settings.Default.PostgresqlConfig = tbDriveCredentialsPath.Text;
            else
            {
                MessageBox.Show("Invalid Drive Credentials");
                tabControl1.SelectedTab = tabControl1.TabPages["googleDrive"];
                return;
            }

            Properties.Settings.Default.Save();
            this.Close();
            this.DialogResult = DialogResult.OK;
        }

        private void btnSheetsCredentialsPath_Click(object sender, EventArgs e)
        {
            bool invalidLableVisible = lblInvlidSheetsCreds.Visible;

            openFileDialog1.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                if (isValidCreds(path))
                {
                    tbSheetsCredentialsPath.Text = path;
                    invalidLableVisible = false;
                }
            }
            lblInvlidSheetsCreds.Visible = invalidLableVisible;
        }
        private void btnDriveCredentialsPath_Click(object sender, EventArgs e)
        {
            bool invalidLableVisible = lblInvlidDriveCreds.Visible;

            openFileDialog1.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                if (isValidCreds(path))
                {
                    tbDriveCredentialsPath.Text = path;
                    invalidLableVisible = false;
                }
            }
            lblInvlidDriveCreds.Visible = invalidLableVisible;

        }


        private bool isValidCreds(string path)
        {
            bool result = false;

            string content = File.ReadAllText(path);
            result = content.Contains("private_key");

            return result;
        }

        private void btnPostgresqlConfig_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                tbPostgresqlConfig.Text = path;
            }

        }
    }
}
