using System.Windows.Forms;
using System.Reflection;
using DictionaryEditorV2.Lexicon;
using Npgsql;
using DictionaryEditorV2.Editor;
using System.Text;
using DictionaryEditorV2;

namespace TranslationForStep
{
    public partial class MainForm : Form
    {
        private List<string> hebrewlist = new List<string>();
        private List<string> greeklist = new List<string>();

        StrongsDictionary strongsDictionary = null;

        public MainForm()
        {
            InitializeComponent();
        }

        #region Trace
        delegate void TraceDelegate(string text, Color color);

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
                    //string txt = string.Format("{0}: {1}\r\n", DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss.fff"), text);
                    string txt = string.Format("{0}\r\n", text);
                    //traceBox.AppendText(txt);
                    traceBox.SelectedText = txt;
                }
                else
                {
                    traceBox.AppendText("\r\n");
                }
                traceBox.ScrollToCaret();
            }
        }

        public void TraceError(string? method, string text)
        {
            if (method == null)
                method = "Unknown Method";
            Trace(string.Format("Error: {0}::{1}", method, text), Color.Red);
        }


        #endregion Trace

        private void MainForm_Load(object sender, EventArgs e)
        {
            strongsDictionary = new StrongsDictionary(this);
        }

        private void GrkLexiconStrongsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReadList(OpenStrongsListFile("Select Greek Lexicon Strong's file"), greeklist);
            foreach (string strongs in greeklist)
            {
                StrongsNumber sn = strongsDictionary.GetStrongs(strongs);
                if (sn == null)
                    TraceError(MethodBase.GetCurrentMethod().Name, string.Format("GetStrongs({0}) returned null", strongs));
                else if(strongs.Length > 5 && sn.DStrongs.Count == 0)
                    TraceError(MethodBase.GetCurrentMethod().Name, string.Format("[{0}] not found", strongs));
                else
                {
                    string strongsNumber = sn.Strongs;
                    if (sn.DStrongs.Count == 0)
                    {
                        long strongId = sn.GetStrongID(string.Empty);
                        List<TranslationEntry> te = strongsDictionary.GetTranslationEntries(strongId);
                    }
                    else
                    {
                        foreach(string dStrong in sn.DStrongs)
                        {
                            long strongId = sn.GetStrongID(dStrong);
                            List<TranslationEntry> te = strongsDictionary.GetTranslationEntries(strongId);
                        }
                    }
                    
                }
            }
        }

        private void hebLexiconStrongsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReadList(OpenStrongsListFile("Select Hebrew Lexicon Strong's file"), hebrewlist);
            foreach (string strongs in hebrewlist)
            {
                StrongsNumber sn = strongsDictionary.GetStrongs(strongs);
                if (sn == null)
                    TraceError(MethodBase.GetCurrentMethod().Name, string.Format("GetStrongs({0}) returned null", strongs));
                else if (strongs.Length > 5 && sn.DStrongs.Count == 0)
                    TraceError(MethodBase.GetCurrentMethod().Name, string.Format("[{0}] not found", strongs));
                else
                {
                    string strongsNumber = sn.Strongs;
                    if (sn.DStrongs.Count == 0)
                    {
                        long strongId = sn.GetStrongID(string.Empty);
                        List<TranslationEntry> te = strongsDictionary.GetTranslationEntries(strongId);
                    }
                    else
                    {
                        foreach (string dStrong in sn.DStrongs)
                        {
                            long strongId = sn.GetStrongID(dStrong);
                            List<TranslationEntry> te = strongsDictionary.GetTranslationEntries(strongId);
                        }
                    }

                }
            }
        }

        private void selectLexiconsFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog1.Description =
                        "Select the directory containing lexicon_greek & lexicon_hebrew texts.";

            // Do not allow the user to create new files via the FolderBrowserDialog.
            this.folderBrowserDialog1.ShowNewFolderButton = false;

            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderName = folderBrowserDialog1.SelectedPath;
                string greekStrongsPath = Path.Combine(folderName, "lexicon_greek.txt.strong.tsv");
                if (!File.Exists(greekStrongsPath))
                {
                    TraceError(MethodBase.GetCurrentMethod().Name, string.Format("Not Found: {0}", greekStrongsPath));
                    return;
                }
                string hebrewStrongsPath = Path.Combine(folderName, "lexicon_hebrew.txt.strong.tsv");
                if (!File.Exists(hebrewStrongsPath))
                {
                    TraceError(MethodBase.GetCurrentMethod().Name, string.Format("Not Found: {0}", hebrewStrongsPath));
                    return;
                }

                StrongsNumbers.Instance.Reload();

                string hebrewOutPath = Path.Combine(folderName, "HebrewDef_ar.tsv");
                ProcessList(hebrewOutPath, GetList(hebrewStrongsPath, 'H'));

                string greekOutPath = Path.Combine(folderName, "GreekDef_ar.tsv");
                ProcessList(greekOutPath, GetList(greekStrongsPath, 'G'));

                Trace("Done!", Color.Green);
            }
        }

        private void ProcessList(string outPath, List<string> list)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(new FileStream(outPath, FileMode.Create), Encoding.UTF8))
                {
                    foreach (string strongs in list)
                    {
                        if (strongs == "H0099")
                        {
                            int y = 0;
                        }
                        string output = string.Empty;
                        StrongsNumber sn = strongsDictionary.GetStrongs(strongs);
                        if (sn == null)
                            TraceError(MethodBase.GetCurrentMethod().Name, string.Format("GetStrongs({0}) returned null", strongs));
                        else if (strongs.Length > 5 && sn.DStrongs.Count == 0)
                            TraceError(MethodBase.GetCurrentMethod().Name, string.Format("[{0}] not found", strongs));
                        else
                        {
                            string strongsNumber = sn.Strongs;
                            string listStrong = strongs.Substring(0, 5);
                            string listD = string.Empty;
                            if (strongs.Length > 5)
                                listD = strongs.Substring(5).Trim();
                            if (listStrong != strongsNumber)
                                TraceError(MethodBase.GetCurrentMethod().Name, string.Format("List Strong[{0}] != DB Strong [{1]", strongs, strongsNumber));
                            else if (sn.DStrongs.Count == 0)
                            {
                                if (strongs.Length == 5)
                                {
                                    long strongId = sn.GetStrongID(string.Empty);
                                    output = ProcessStrong(strongs, strongsDictionary.GetTranslationEntries(strongId));
                                }
                                else
                                {
                                    TraceError(MethodBase.GetCurrentMethod().Name, string.Format("List Strong[{0}] != DB Strong [{1] (extra D)", strongs, strongsNumber));
                                }
                            }
                            else
                            {
                                try
                                {
                                    if (string.IsNullOrEmpty(listD))
                                    {
                                        long strongId = sn.GetStrongID(sn.DStrongs[0]);
                                        output = ProcessStrong(strongs, strongsDictionary.GetTranslationEntries(strongId));
                                    }
                                    else if (sn.DStrongs.Contains(listD))
                                    {
                                        long strongId = sn.GetStrongID(listD);
                                        output = ProcessStrong(strongs, strongsDictionary.GetTranslationEntries(strongId));
                                    }
                                    else
                                    {
                                        TraceError(MethodBase.GetCurrentMethod().Name, string.Format("List Strong[{0}] != DB Strong [{1}] (missing D)", strongs, strongsNumber));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    int x = 0;
                                }

                            }
                        }
                        sw.WriteLine(output.Replace("\n"," ").Replace("\r"," "));
                    }
                }
            }
            catch (Exception ex)
            {
                int x = 0;
            } 
        }

        private string ProcessStrong(string strongs, List<TranslationEntry> te)
        {
            string output = string.Empty;
            try
            {
                string translatedGloss = string.Empty;
                string translatedText = string.Empty;
                int status = 0;
                foreach (TranslationEntry entry in te)
                {
                    int st = 0;
                    string sheetStatus = entry.Status.Trim();
                    switch(sheetStatus.ToLower())
                    {
                        case "pending": st = 1; break;
                        case "reviewed": st = 2; break;
                        case "approved": st = 3; break;
                    }
                    if (st > status)
                    {
                        status = st;
                        translatedGloss = entry.Translated_gloss;
                        translatedText = entry.Translated_text;
                    }
                }

                if(string.IsNullOrEmpty(translatedText))
                {
                    translatedText = "MISSING";
                    TraceError(MethodBase.GetCurrentMethod().Name, string.Format("[{0}] missing Text.", strongs));
                }
                output = string.Format("{0}\tGloss\t{1}\tText\t{2}", strongs,translatedGloss, translatedText.Replace("\t", " "));
            }
            catch (Exception ex)
            {
            }
            return output;
        }

        private void ReadList(string filePath, List<string> list)
        {
            try
            {
                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    list.Clear();
                    int lineNum = 1;
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        while (!sr.EndOfStream)
                        {
                            string? line = sr.ReadLine();
                            if (!string.IsNullOrEmpty(line))
                            {
                                if (line.Length > 5 && (line[0] == 'G' || line[0] == 'H'))
                                {
                                    list.Add(line);
                                }
                                else
                                {
                                    TraceError(MethodBase.GetCurrentMethod().Name, string.Format("Incorrect strong at Line {0} of {1}", lineNum, filePath));
                                    list.Clear();
                                    break;
                                }
                            }
                            else
                                TraceError(MethodBase.GetCurrentMethod().Name, string.Format("Empty or null Line {0} of {1}", lineNum, filePath));

                            lineNum++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TraceError(MethodBase.GetCurrentMethod().Name, string.Format("File [{0}] caused an exception [{1}]", filePath, ex.Message));
            }
        }
        private List<string> GetList(string filePath, char prefix)
        {
            List<string> list = new List<string>();
            try
            {
                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    int lineNum = 1;
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        while (!sr.EndOfStream)
                        {
                            string? line = sr.ReadLine();
                            if (!string.IsNullOrEmpty(line))
                            {
                                if (line[0] == prefix)
                                {
                                    list.Add(line);
                                }
                                else
                                {
                                    TraceError(MethodBase.GetCurrentMethod().Name, string.Format("Incorrect strong at Line {0} of {1}", lineNum, filePath));
                                    list.Clear();
                                    break;
                                }
                            }
                            else
                                TraceError(MethodBase.GetCurrentMethod().Name, string.Format("Empty or null Line {0} of {1}", lineNum, filePath));

                            lineNum++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TraceError(MethodBase.GetCurrentMethod().Name, string.Format("File [{0}] caused an exception [{1}]", filePath, ex.Message));
                list.Clear();
            }

            return list;
        }

        private string OpenStrongsListFile(string title)
        {
            string filePath = string.Empty;

            openFileDialog1.Filter = "tsv files (*.tsv)|*.tsv|All files (*.*)|*.*";
            openFileDialog1.Title = "title";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                filePath = openFileDialog1.FileName;
            }

            return filePath;
        }
    }
}
