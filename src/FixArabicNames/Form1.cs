
using Npgsql;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace FixArabicNames
{
    public partial class Form1 : Form
    {
        private NpgsqlDataSource dataSource;
        private NpgsqlConnection connection;

        private Dictionary<string, Translation> translations = new Dictionary<string, Translation>();

        // H1956
        string testText = @"رجل من سبط لاوي عاش في عصر المملكة المنقسمة، ورد ذكره لأول مرة في ١أخ ٢٥:​٤؛ ابن: هيمان (H1968I)؛ أخو: بقيا (H1232)، متنيا (H4983G)، عزيئيل (H5816J)، شبوئيل (ح 7619 هـ)، يريموث (H3406ل)، حننيا (H2608B)، حناني ( ح 2607 هـ)، إلياثة (H0448)، جدلتي (H1437)، روممتي. عازر (هـ7320)، يشبقاشاه (H3436)، مالوثي (H4413)، محازيوث (هـ4238) § هوثير = وفرةالابن الثالث عشر لهيمان ولاوي قهاتي";

        public Form1()
        {
            InitializeComponent();
        }

        #region Trace
        delegate void TraceDelegate(string text, System.Drawing.Color color);
        delegate void ClearTraceDelegate();

        private void Trace(string text, System.Drawing.Color color,
                [CallerLineNumber] int lineNumber = 0,
                [CallerMemberName] string caller = null)
        {
            Trace(text, color);
        }
        private void TraceError(
                string text,
                [CallerLineNumber] int lineNumber = 0,
                [CallerMemberName] string caller = null)
        {
            TraceError(string.Format("{0}#{1}", caller == null ? "???" : caller, lineNumber), text);
        }

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
        public void Trace(string text, System.Drawing.Color color)
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
            Trace(string.Format("Error: {0}::{1}", method, text), System.Drawing.Color.Red);
        }


        #endregion Trace

        private void Form1_Load(object sender, EventArgs e)
        {
            string dbName = "StrongsDictionary";

            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1Na61A51;Database=" + dbName + "; Include Error Detail=True";
            dataSource = NpgsqlDataSource.Create(connectionString);
            connection = dataSource.OpenConnection();
        }

        private void goToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PopulateDictionary();
            foreach(string key in translations.Keys)
            {
                if (key == "H1956")
                {
                    int x = 0;
                }

                int s = 0;
                bool r = int.TryParse(key.Substring(1,4), out s);
                //if (s > 1400)
                {
                    Translation translation = translations[key];
                    translation.Text = FixTranslation(translation.Text);
                }

            }
            UpdateDictionart();

        }

        private void UpdateDictionart()
        {
            string cmdText = string.Empty;
            try
            {
                var command = dataSource.CreateCommand();

                foreach (var translation in translations.Values)
                {
                    if(translation.Strongs == "H1956") 
                    { 
                        int x = 0;
                    }
                    string tableName = "dictionary_translation";
                    cmdText = "UPDATE public.\"" + tableName + "\" " +
                            string.Format("SET translated_long_text='{0}'",
                            translation.Text.Replace("'", "''")) +
                            string.Format(" WHERE strongs_number='{0}' AND d_strong='{1}' AND language_id={2};",
                            translation.Strongs, translation.dStrong, translation.Language);
                    command.CommandText = cmdText;
                    command.ExecuteNonQuery();
                }

                Trace("Sync Updated Translations Done!", Color.Green);
            }
            catch (Exception ex)
            {
                Trace(cmdText, Color.Red);
                TraceError(ex.ToString());
            }
        }

        private void PopulateDictionary()
        {
            string cmdText = string.Empty;
            try
            {
                var command = dataSource.CreateCommand();

                cmdText =
                    "SELECT language_id, strongs_number,d_strong,translated_word,translated_long_text" +
                    " FROM public.\"dictionary_translation\"" +
                    " WHERE strongs_number LIKE 'H%'" +
                    " ORDER BY strongs_number,d_strong;";

                command.CommandText = cmdText;
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Translation translation = new Translation();

                    translation.Language = (int)reader[0];
                    translation.Strongs = (string)reader[1];
                    translation.dStrong = (string)reader[2];
                    translation.Word = (string)reader[3];
                    translation.Text = (string)reader[4];

                    string strongs = (translation.Strongs + translation.dStrong).Trim();
                    translations[strongs] = translation;
                }
                reader.Close();
                reader.DisposeAsync();


            }
            catch (Exception ex)
            {
                Trace(cmdText, Color.Red);
                TraceError(ex.ToString());
            }

        }

        private string FixTranslation(string line)
        {
            string strongsPattern = @"[:،.و]\s{0,4}([\u0620-\u0669]*)\s{0,4}(\(\s{0,4}([حhHه])\s{0,1}(\d\d\d\d)\s{0,4}([a-zA-Z\u0620-\u064A]{0,4})\s{0,4}\))";

            string result = line.Replace("هـ", "ه");

            MatchCollection matches = Regex.Matches(result, strongsPattern);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    string name = match.Groups[1].Value;
                    string number = match.Groups[4].Value;
                    string suffix = match.Groups[5].Value;
                    string strongsToFix = match.Groups[2].Value;
                    if (!string.IsNullOrEmpty(suffix))
                    {
                        suffix = suffix
                            .Replace("أ", "A")
                            .Replace("ب", "B")
                            .Replace("ج", "C")
                            .Replace("د", "D")
                            .Replace("ف", "F")
                            .Replace("ز", "G")
                            .Replace("ه", "H")
                            .Replace("ح", "H")
                            .Replace("ط", "I")
                            .Replace("ك", "K")
                            .Replace("ي", "J")
                            .Replace("ل", "L")
                            .ToUpper();
                    }
                    string strongs = string.Format("(H{0}{1})", number, suffix).ToUpper();
                    if (strongsToFix != strongs)
                    {
                        result = result.Replace(strongsToFix, strongs);
                    }
                    strongs = string.Format("H{0}{1}", number, suffix).ToUpper();
                    if(!translations.ContainsKey(strongs)) 
                    {
                        string tmp = strongs;
                        strongs = string.Empty;
                        if (tmp.Length == 5)
                        {
                            // see if we can find a key
                            foreach (string k in translations.Keys)
                            {
                                if (k.StartsWith(tmp))
                                {
                                    strongs = k;
                                    break;
                                }
                            }
                        }
                    }
                    if (strongs != string.Empty && !string.IsNullOrEmpty(name))
                    {
                        string correctName = translations[strongs].Word;
                        result = result.Replace(name, correctName);
                    }
                }
            }

            return result;
        }

    }

    internal class Translation
    {
        public int Language { get; set; }
        public string Strongs { get; set; }
        public string dStrong { get; set; }
        public string Word { get; set; }
        public string Text { get; set; }
    }
}
