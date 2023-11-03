using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using Microsoft.Office.Interop.Excel;
using Npgsql;
using System.Text.RegularExpressions;

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
            for (int i = 0; i < languages.SupportedLanguags.Length; i++)
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
            configurationDialog.DictionaryToUse = Properties.Settings.Default.dictionarySpreadsheets ? DictionaryType.SPREADSHEETS : DictionaryType.TAB_SEPARATED_TEXT;
            configurationDialog.SourceSpreadsheetsFolder = Properties.Settings.Default.sourceSpreadsheetsFolder;
            configurationDialog.TranslatedSpreadsheetsFolder = Properties.Settings.Default.translatedSpreadsheetsFolder;
            configurationDialog.SourceTabSeparatedText = Properties.Settings.Default.sourceTabSeparatedText;
            configurationDialog.TranslatedTabSeparatedText = Properties.Settings.Default.translatedTabSeparatedText;
            configurationDialog.TaggedBibleFile = Properties.Settings.Default.taggedBibleFile;
            configurationDialog.SelectedLanguage = Properties.Settings.Default.selectedLanguage;


            DialogResult result = configurationDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                bool dictionarySpreadsheets = configurationDialog.DictionaryToUse == DictionaryType.SPREADSHEETS;
                string sourceSpreadsheets = configurationDialog.SourceSpreadsheetsFolder;
                string translatedSpreadsheets = configurationDialog.TranslatedSpreadsheetsFolder;
                string sourceText = configurationDialog.SourceTabSeparatedText;
                string translatedText = configurationDialog.TranslatedTabSeparatedText;
                string taggedBibleFile = configurationDialog.TaggedBibleFile;

                Properties.Settings.Default.dictionarySpreadsheets = dictionarySpreadsheets;
                if (!string.IsNullOrEmpty(sourceSpreadsheets))
                {
                    Properties.Settings.Default.sourceSpreadsheetsFolder = sourceSpreadsheets;
                }
                if (!string.IsNullOrEmpty(translatedSpreadsheets))
                {
                    Properties.Settings.Default.translatedSpreadsheetsFolder = translatedSpreadsheets;
                }
                if (!string.IsNullOrEmpty(translatedText))
                {
                    Properties.Settings.Default.translatedTabSeparatedText = translatedText;
                }
                if (!string.IsNullOrEmpty(sourceText))
                {
                    Properties.Settings.Default.sourceTabSeparatedText = sourceText;
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
            DialogResult result = MessageBox.Show("Confirm Rebuild", "Confirmation", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                toolStripButtonBuild.Enabled = false;
                PopulateDataBase();
                toolStripButtonBuild.Enabled = true;
            }
        }

        private void PopulateDataBase()
        {
            bool dictionarySpreadsheets = Properties.Settings.Default.dictionarySpreadsheets;
            string sourceSpreadsheets = Properties.Settings.Default.sourceSpreadsheetsFolder;
            string translatedSpreadsheets = Properties.Settings.Default.translatedSpreadsheetsFolder;
            string sourceText = Properties.Settings.Default.sourceTabSeparatedText;
            string translatedText = Properties.Settings.Default.translatedTabSeparatedText;
            string taggedBibleFile = Properties.Settings.Default.taggedBibleFile;
            int selectedLanguage = Properties.Settings.Default.selectedLanguage;

            if ((dictionarySpreadsheets && (string.IsNullOrEmpty(sourceSpreadsheets) ||
                                            string.IsNullOrEmpty(translatedSpreadsheets))) ||
                (!dictionarySpreadsheets && (string.IsNullOrEmpty(sourceText) ||
                                            string.IsNullOrEmpty(translatedText))) ||
                string.IsNullOrEmpty(taggedBibleFile) ||
                selectedLanguage < 1)
            {
                GetConfiguration();
                return;
            }

            clearTrace();

            new Thread(() =>
            {
                Run(
                     dictionarySpreadsheets,
                     sourceSpreadsheets,
                     translatedSpreadsheets,
                     sourceText,
                     translatedText,
                     taggedBibleFile,
                     selectedLanguage);
            }).Start();
        }

        public void Run(
                        bool dictionarySpreadsheets,
                        string sourceSpreadsheets,
                        string translatedSpreadsheets,
                        string sourceText,
                        string translatedText,
                        string taggedBibleFile,
                        int selectedLanguage)
        {
            string dbName = "StrongsDictionary";


            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1Na61A51;Database=" + dbName + "; Include Error Detail=True";
            dataSource = NpgsqlDataSource.Create(connectionString);
            connection = dataSource.OpenConnection();



            Languages langs = new Languages(this);
            BibleBooks bb = new BibleBooks(this);
            AraSVD svd = new AraSVD(this);
            DictionarySpreadSheets dss = new DictionarySpreadSheets(this);
            DictionaryText dt = new DictionaryText(this);
            StepLexicons lexicons = new StepLexicons(this); // this is just an update

            WordFix wf = new WordFix(this);
            WordFixAll wfa = new WordFixAll(this);

            // Clear database tables * ordr is important

            if (dictionarySpreadsheets)
            {
                if (!dss.ClearTables(dataSource)) return;
            }
            else
            {
                if (!dt.ClearTables(dataSource)) return;
            }

            if (!svd.ClearTables(dataSource)) return;
            if (!langs.ClearTables(dataSource)) return;
            if (!bb.ClearTables(dataSource)) return;


            // Populate database tables
            langs.PopulateLanguageTable(dataSource);
            bb.PopulateBookNames(dataSource);

            if (dictionarySpreadsheets)
            {
                dss.LoadSourceSpreadsheetsIntoDB(sourceSpreadsheets, dataSource);
                dss.LoadTranslatedSpreadsheetsIntoDB(selectedLanguage, translatedSpreadsheets, dataSource);
            }
            else
            {
                dt.LoadSourceTextIntoDB(sourceText, dataSource);
                dt.LoadTranslatedTextIntoDB(selectedLanguage, translatedText, dataSource);
            }
            lexicons.LoadLexicons(dataSource);

            bool result = svd.LoadBibleFile(taggedBibleFile, true, false);
            if (!result)
            {
                Trace("Load Bible File Faild", Color.Red);
                return;
            }

            svd.BuildStrongs2Arabic();
            svd.LoadBibleVerses(selectedLanguage, dataSource);
            svd.LoadBibleWords(selectedLanguage, dataSource);

            wf.Excute(dataSource);

            dt.SyncUpdatedTranslations(dataSource);

            connection.Close();

            Trace("All Done!", Color.Green);
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Confirm Update", "Confirmation", MessageBoxButtons.OKCancel);

            if (result != DialogResult.OK)
                return;

            string dbName = "StrongsDictionary";


            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1Na61A51;Database=" + dbName + "; Include Error Detail=True";
            dataSource = NpgsqlDataSource.Create(connectionString);
            connection = dataSource.OpenConnection();

            DictionaryText dt = new DictionaryText(this);
            dt.UpdateTranslation(dataSource);
            dt.SyncUpdatedTranslations(dataSource);

            connection.Close();

            Trace("All Done!", Color.Green);
        }

        private void rebuildReferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Confirm Rebuild References", "Confirmation", MessageBoxButtons.OKCancel);

            if (dialogResult != DialogResult.OK)
                return;

            string sourceText = Properties.Settings.Default.sourceTabSeparatedText;
            string translatedText = Properties.Settings.Default.translatedTabSeparatedText;
            string taggedBibleFile = Properties.Settings.Default.taggedBibleFile;
            int selectedLanguage = Properties.Settings.Default.selectedLanguage;

            clearTrace();
            Trace("Rebuilding References", Color.Green);

            string dbName = "StrongsDictionary";


            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1Na61A51;Database=" + dbName + "; Include Error Detail=True";
            dataSource = NpgsqlDataSource.Create(connectionString);
            connection = dataSource.OpenConnection();

            AraSVD svd = new AraSVD(this);

            if (!svd.ClearTables(dataSource)) return;


            bool result = svd.LoadBibleFile(taggedBibleFile, true, false);
            if (!result)
            {
                Trace("Load Bible File Faild", Color.Red);
                return;
            }

            svd.BuildStrongs2Arabic();
            svd.LoadBibleVerses(selectedLanguage, dataSource);
            svd.LoadBibleWords(selectedLanguage, dataSource);

            connection.Close();

            Trace("All Done!", Color.Green);
        }

        private void transliterationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Confirm Translitration", "Confirmation", MessageBoxButtons.OKCancel);

            if (result != DialogResult.OK)
                return;

            string dbName = "StrongsDictionary";


            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1Na61A51;Database=" + dbName + "; Include Error Detail=True";
            dataSource = NpgsqlDataSource.Create(connectionString);
            connection = dataSource.OpenConnection();

            Transliteration.TransliterateAra trx = new Transliteration.TransliterateAra(this);
            trx.Transliterate(dataSource);
 
            connection.Close();

            Trace("All Done!", Color.Green);

        }
    }
}