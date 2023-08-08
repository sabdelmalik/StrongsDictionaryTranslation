using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using Microsoft.Office.Interop.Excel;
using Npgsql;


namespace PopulateStrongsDictionary
{
    public partial class MainForm : Form
    {
        private NpgsqlDataSource dataSource;
        private NpgsqlConnection connection;

        Languages languages;

        ConfigurationDialog configurationDialog = new ConfigurationDialog();
        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            languages = new Languages(this);
            for(int i = 0; i < languages.SupportedLanguags.Length; i++)
            {
                configurationDialog.AddLanguage(languages.GetLanguage(languages.SupportedLanguags[i]));
            }
        }


        #region Trace
        delegate void TraceDelegate(string text, Color color);
        delegate void ClearTraceDelegate();

        public void clearTrace()
        {
            if (InvokeRequired)
            {
                Invoke(new ClearTraceDelegate(clearTrace));
            }
            else
            {
                traceBox.Clear();
                traceBox.ScrollToCaret();
            }
        }
        public void Trace(string text, Color color)
        {
            if (InvokeRequired)
            {
                Invoke(new TraceDelegate(Trace), new object[] { text, color });
            }
            else
            {
                traceBox.SelectionColor = color;
                if (text.Length > 0)
                {
                    string txt = string.Format("{0}: {1}v", DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss.fff"), text);
                    //traceBox.AppendText(txt);
                    traceBox.SelectedText = text + "\r\n"; // txt;
                }
                else
                {
                    traceBox.AppendText("\r\n");
                }
                traceBox.ScrollToCaret();
                System.Windows.Forms.Application.DoEvents();
            }
        }

        public void TraceError(string method, string text)
        {
            Trace(string.Format("Error: {0}::{1}", method, text), Color.Red);
        }


        #endregion Trace

        private void toolStripButtonSettings_Click(object sender, EventArgs e)
        {
            GetConfiguration();
        }

        private void GetConfiguration()
        {
            configurationDialog.SourceSpreadsheetsFolder = Properties.Settings.Default.sourceSpreadsheetsFolder;
            configurationDialog.SourceTranslatedSpreadsheetsFolder = Properties.Settings.Default.sourceSpreadsheetsFolder;
            configurationDialog.TaggedBibleFile = Properties.Settings.Default.taggedBibleFile;
            configurationDialog.SelectedLanguage = Properties.Settings.Default.selectedLanguage;


            DialogResult result = configurationDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string sourceSpreadsheets = configurationDialog.SourceSpreadsheetsFolder;
                string translatedSpreadsheets = configurationDialog.SourceTranslatedSpreadsheetsFolder;
                string taggedBibleFile = configurationDialog.TaggedBibleFile;

                if (!string.IsNullOrEmpty(sourceSpreadsheets))
                {
                    Properties.Settings.Default.sourceSpreadsheetsFolder = sourceSpreadsheets;
                }
                if (!string.IsNullOrEmpty(translatedSpreadsheets))
                {
                    Properties.Settings.Default.translatedSpreadsheetsFolder = translatedSpreadsheets;
                }
                if (!string.IsNullOrEmpty(taggedBibleFile))
                {
                    Properties.Settings.Default.taggedBibleFile = taggedBibleFile;
                }

                Properties.Settings.Default.selectedLanguage = configurationDialog.SelectedLanguage;

                Properties.Settings.Default.Save();

            }
            //string folderPath = string.Empty;
            //DialogResult res = folderBrowserDialog1.ShowDialog(this);
            //if (res == DialogResult.OK)
            //{
            //    folderPath = folderBrowserDialog1.SelectedPath;

            //    PopulateDataBase(folderPath);

            //}

        }

        private void toolStripButtonBuild_Click(object sender, EventArgs e)
        {
            PopulateDataBase();
        }

        private void PopulateDataBase()
        {
            string sourceSpreadsheets = Properties.Settings.Default.sourceSpreadsheetsFolder;
            string translatedSpreadsheets = Properties.Settings.Default.translatedSpreadsheetsFolder;
            string taggedBibleFile = Properties.Settings.Default.taggedBibleFile;
            int selectedLanguage = Properties.Settings.Default.selectedLanguage;

            if (string.IsNullOrEmpty(sourceSpreadsheets) ||
                string.IsNullOrEmpty(translatedSpreadsheets) ||
                string.IsNullOrEmpty(taggedBibleFile) ||
                selectedLanguage < 1)
            {
                GetConfiguration();
                return;
            }

            clearTrace();

            string dbName = "StrongsDictionary";


            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1Na61A51;Database=" + dbName + "; Include Error Detail=True";
            dataSource = NpgsqlDataSource.Create(connectionString);
            connection = dataSource.OpenConnection();


            Languages langs = new Languages(this);
            langs.PopulateLanguageTable(dataSource);


            BibleBooks bb = new BibleBooks(this);
            bb.PopulateBookNames(dataSource);

            AraSVD svd = new AraSVD(this);
            bool result = svd.LoadBibleFile(taggedBibleFile, true, false);
            if(!result)
            {
                Trace("Load Bible File Faild", Color.Red);
                return;
            }

            svd.BuildStrongs2Arabic();

            svd.LoadBibleVerses(selectedLanguage,dataSource);
            svd.LoadBibleWords(selectedLanguage,dataSource);

            DictionarySpreadSheets dss = new DictionarySpreadSheets(this);
            dss.LoadSourceSpreadsheetsIntoDB(sourceSpreadsheets,dataSource);
            dss.LoadTranslatedSpreadsheetsIntoDB(selectedLanguage, translatedSpreadsheets, dataSource);


            connection.Close();
        }

    }
}