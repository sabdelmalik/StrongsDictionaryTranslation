using Npgsql;
using System.Drawing;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;

namespace CopyDbToV2
{
    public partial class MainForm : Form
    {

        private NpgsqlDataSource sourceDataSource;
        private NpgsqlConnection sourceConnection;

        private NpgsqlDataSource targetDataSource;
        private NpgsqlConnection targetConnection;

        private SortedDictionary<int, List<char>> greekStrongs = new SortedDictionary<int, List<char>>();
        private SortedDictionary<int, List<char>> hebrewStrongs = new SortedDictionary<int, List<char>>();


        /// <summary>
        /// key dstrong's number
        /// value strong_id in V2
        /// </summary>
        private Dictionary<string, int> strongDict = new Dictionary<string, int>();

        private Sheets sheets;


        public MainForm()
        {
            InitializeComponent();
        }

        #region Trace
        delegate void TraceDelegate(string text, System.Drawing.Color color);
        delegate void ClearTraceDelegate();

        private void TraceX(string text, System.Drawing.Color color,
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

        private void MainForm_Load(object sender, EventArgs e)
        {
            sheets = new Sheets(this);

            string sourceDB = "StrongsDictionary";
            string targetDB = "StrongsDictionaryV2";

            var sourceConnectionString = "Host=localhost;Port=5432;Username=postgres;Password=1Na61A51;Database=" + sourceDB;
            var targetConnectionString = "Host=localhost;Port=5432;Username=postgres;Password=1Na61A51;Database=" + targetDB;

            sourceDataSource = NpgsqlDataSource.Create(sourceConnectionString);
            sourceConnection = sourceDataSource.CreateConnection();

            targetDataSource = NpgsqlDataSource.Create(targetConnectionString);
            targetConnection = targetDataSource.CreateConnection();

        }

        private bool ProcessTranslationStatusTable()
        {
            bool result = true;

            string tgtTable = "translation_status";

            string tgtCmdText = string.Empty;


            try
            {
                ClearTargetTable(tgtTable);
                if (targetConnection.State == System.Data.ConnectionState.Closed)
                    targetConnection.Open();

                var tgtCommand = targetDataSource.CreateCommand();

                string[] st = { "Pending", "Reviewed", "Approved" };

                int id = 0;
                string status = string.Empty;


                for (int i = 0; i < st.Length; i++)
                {
                    id++;
                    status = st[i];

                    tgtCmdText = string.Format(
                    "INSERT INTO public.\"{0}\"" +
                    " (id, status)" +
                    " VALUES ({1},'{2}');",
                    tgtTable,
                    id,
                    status);

                    tgtCommand.CommandText = tgtCmdText;
                    tgtCommand.ExecuteNonQuery();
                }
                tgtCommand.Dispose();

                Trace(MethodBase.GetCurrentMethod().Name + " Done!", System.Drawing.Color.Black);
            }
            catch (Exception ex)
            {
                TraceError(ex.Message);
                result = false;
            }
            finally
            {
                sourceConnection.Close();
                targetConnection.Close();
            }

            return result;
        }
        private bool ProcessLanguageTable()
        {
            bool result = true;

            string srcTable = "language";
            string tgtTable = "language";

            string srcCmdText = string.Empty;
            string tgtCmdText = string.Empty;

            try
            {
                ClearTargetTable(tgtTable);

                if (sourceConnection.State == System.Data.ConnectionState.Closed)
                    sourceConnection.Open();

                if (targetConnection.State == System.Data.ConnectionState.Closed)
                    targetConnection.Open();

                var srcCommand = sourceDataSource.CreateCommand();
                var tgtCommand = targetDataSource.CreateCommand();

                srcCmdText = string.Format(
                    "SELECT " +
                    " id, name, iso_639_1, iso_639_2" +
                    " FROM public.\"{0}\";", srcTable);

                srcCommand.CommandText = srcCmdText;
                NpgsqlDataReader reader = srcCommand.ExecuteReader();

                while (reader.Read())
                {
                    int id = (int)reader[0];
                    string name = (string)reader[1];
                    string iso_639_1 = (string)reader[2];
                    string iso_639_2 = (string)reader[3];

                    tgtCmdText = string.Format(
                    "INSERT INTO public.\"{0}\"" +
                    " (id, name, iso_639_1, iso_639_2)" +
                    " VALUES ({1}, '{2}', '{3}', '{4}');",
                    tgtTable,
                    id, name, iso_639_1, iso_639_2);

                    tgtCommand.CommandText = tgtCmdText;
                    tgtCommand.ExecuteNonQuery();
                }
                reader.Close();
                reader.DisposeAsync();

                srcCommand.Dispose();
                tgtCommand.Dispose();

                Trace(MethodBase.GetCurrentMethod().Name + " Done!", System.Drawing.Color.Black);
            }
            catch (Exception ex)
            {
                TraceError(ex.Message);
                result = false;
            }
            finally
            {
                sourceConnection.Close();
                targetConnection.Close();
            }

            return result;
        }
        private bool ProcessBookTable()
        {
            bool result = true;

            string srcTable = "book";
            string tgtTable = "book";

            string srcCmdText = string.Empty;
            string tgtCmdText = string.Empty;

            try
            {
                ClearTargetTable(tgtTable);
                if (sourceConnection.State == System.Data.ConnectionState.Closed)
                    sourceConnection.Open();

                if (targetConnection.State == System.Data.ConnectionState.Closed)
                    targetConnection.Open();

                var srcCommand = sourceDataSource.CreateCommand();
                var tgtCommand = targetDataSource.CreateCommand();

                srcCmdText = string.Format(
                    "SELECT id, usfm_name, osis_name, full_name" +
                    " FROM public.\"{0}\";", srcTable);

                srcCommand.CommandText = srcCmdText;
                NpgsqlDataReader reader = srcCommand.ExecuteReader();

                while (reader.Read())
                {
                    int id = (int)reader[0];
                    string usfm_name = (string)reader[1];
                    string osis_name = (string)reader[2];
                    string full_name = (string)reader[3];

                    tgtCmdText = string.Format(
                    "INSERT INTO public.\"{0}\"" +
                    "(id, usfm_name, osis_name, full_name)" +
                    " VALUES ({1}, '{2}', '{3}', '{4}');",
                    tgtTable,
                    id, usfm_name, osis_name, full_name);

                    tgtCommand.CommandText = tgtCmdText;
                    tgtCommand.ExecuteNonQuery();
                }
                reader.Close();
                reader.DisposeAsync();

                srcCommand.Dispose();
                tgtCommand.Dispose();

                Trace(MethodBase.GetCurrentMethod().Name + " Done!", System.Drawing.Color.Black);
            }
            catch (Exception ex)
            {
                TraceError(ex.Message);
                result = false;
            }
            finally
            {
                sourceConnection.Close();
                targetConnection.Close();
            }

            return result;
        }
        private bool ProcessStrongsTable()
        {
            bool result = true;

            string srcTable = "strongs_numbers";
            string tgtTable = "strongs_numbers";

            string srcCmdText = string.Empty;
            string tgtCmdText = string.Empty;

            SortedDictionary<int, StrongsNumber> dict = new SortedDictionary<int, StrongsNumber>();

            try
            {
                ClearTargetTable(tgtTable);
                if (sourceConnection.State == System.Data.ConnectionState.Closed)
                    sourceConnection.Open();

                if (targetConnection.State == System.Data.ConnectionState.Closed)
                    targetConnection.Open();

                var srcCommand = sourceDataSource.CreateCommand();
                var tgtCommand = targetDataSource.CreateCommand();

                srcCmdText = string.Format(
                    "SELECT " +
                    "strongs_number,d_strong,original_word,english_translation,long_text,step_united_reason,step_type,transliteration,pronunciation" +
                    " FROM public.\"{0}\"" +
                    " ORDER BY strongs_number ASC, d_strong ASC;",
                    srcTable);

                srcCommand.CommandText = srcCmdText;
                NpgsqlDataReader reader = srcCommand.ExecuteReader();

                int idx = 1;
                while (reader.Read())
                {
                    StrongsNumber sn = new StrongsNumber();

                    sn.strongs_number = (string)reader[0];
                    sn.d_strong = (string)reader[1];
                    sn.original_word = (string)reader[2];
                    sn.english_translation = (string)reader[3];
                    sn.long_text = (string)reader[4];
                    sn.step_united_reason = (string)reader[5];
                    sn.step_type = (string)reader[6];
                    sn.transliteration = (string)reader[7];
                    sn.pronunciation = (string)reader[8];

                    dict.Add(idx++, sn);
                }
                reader.Close();
                reader.DisposeAsync();

                srcCommand.Dispose();

                foreach (int indx in dict.Keys)
                {
                    StrongsNumber sn = dict[indx];

                    tgtCmdText = string.Format(
                    "INSERT INTO public.\"{0}\"" +
                    " (strong_id, strongs_number,d_strong,original_word,english_translation,long_text,step_united_reason,step_type,transliteration,pronunciation)" +
                    " VALUES ({1}, '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}');",
                    tgtTable,
                    indx,
                    sn.strongs_number,
                    sn.d_strong,
                    sn.original_word.Replace("'", "''"),
                    sn.english_translation.Replace("'", "''"),
                    sn.long_text.Replace("'", "''"),
                    sn.step_united_reason.Replace("'", "''"),
                    sn.step_type.Replace("'", "''"),
                    sn.transliteration.Replace("'", "''"),
                    sn.pronunciation.Replace("'", "''")
                    );

                    tgtCommand.CommandText = tgtCmdText;
                    tgtCommand.ExecuteNonQuery();

                    string dStrong = (sn.strongs_number + sn.d_strong).Trim();
                    strongDict.Add(dStrong, indx);
                }
                tgtCommand.Dispose();

                Trace(MethodBase.GetCurrentMethod().Name + " Done!", System.Drawing.Color.Black);
            }
            catch (Exception ex)
            {
                TraceError(ex.Message);
                result = false;
            }
            finally
            {
                sourceConnection.Close();
                targetConnection.Close();
            }

            return result;
        }
        private bool ProcessBibleTextTable()
        {
            bool result = true;
            string srcTable = "bible_text";
            string tgtTable = "bible_text";

            string srcCmdText = string.Empty;
            string tgtCmdText = string.Empty;

            try
            {
                ClearTargetTable(tgtTable);
                if (sourceConnection.State == System.Data.ConnectionState.Closed)
                    sourceConnection.Open();

                if (targetConnection.State == System.Data.ConnectionState.Closed)
                    targetConnection.Open();

                var srcCommand = sourceDataSource.CreateCommand();
                var tgtCommand = targetDataSource.CreateCommand();

                srcCmdText = string.Format(
                    "SELECT " +
                    " language_id,book_id,chapter_num,verse_num,verse_text" +
                    " FROM public.\"{0}\";", srcTable);

                srcCommand.CommandText = srcCmdText;
                NpgsqlDataReader reader = srcCommand.ExecuteReader();

                while (reader.Read())
                {
                    int language_id = (int)reader[0];
                    int book_id = (int)reader[1];
                    int chapter_num = (int)reader[2];
                    int verse_num = (int)reader[3];
                    string verse_text = (string)reader[4];

                    tgtCmdText = string.Format(
                        "INSERT INTO public.\"{0}\"" +
                        " (language_id,book_id,chapter_num,verse_num,verse_text)" +
                        " VALUES ({1},{2},{3},{4},'{5}');",
                        tgtTable,
                        language_id,
                        book_id,
                        chapter_num,
                        verse_num,
                        verse_text.Replace("'", "''"));

                    tgtCommand.CommandText = tgtCmdText;
                    tgtCommand.ExecuteNonQuery();
                }
                reader.Close();
                reader.DisposeAsync();

                srcCommand.Dispose();
                tgtCommand.Dispose();

                Trace(MethodBase.GetCurrentMethod().Name + " Done!", System.Drawing.Color.Black);
            }
            catch (Exception ex)
            {
                TraceError(ex.Message);
                result = false;
            }
            finally
            {
                sourceConnection.Close();
                targetConnection.Close();
            }

            return result;
        }
        private bool ProcessGoogleSheetTable()
        {
            bool result = true;

            string srcTable = "google_sheet";
            string tgtTable = "google_sheet";

            string srcCmdText = string.Empty;
            string tgtCmdText = string.Empty;

            try
            {
                ClearTargetTable(tgtTable);
                if (sourceConnection.State == System.Data.ConnectionState.Closed)
                    sourceConnection.Open();

                if (targetConnection.State == System.Data.ConnectionState.Closed)
                    targetConnection.Open();

                var srcCommand = sourceDataSource.CreateCommand();
                var tgtCommand = targetDataSource.CreateCommand();

                srcCmdText = string.Format(
                    "SELECT " +
                    " sheet_id,id,name,first_strong,last_strong,translation_language,last_updated" +
                    " FROM public.\"{0}\";", srcTable);

                srcCommand.CommandText = srcCmdText;
                NpgsqlDataReader reader = srcCommand.ExecuteReader();

                while (reader.Read())
                {

                    int sheet_id = (int)reader[0];
                    string id = (string)reader[1];
                    string name = (string)reader[2];
                    string first_strong = (string)reader[3];
                    string last_strong = (string)reader[4];
                    int translation_language = (int)reader[5];
                    DateTime last_updated = (DateTime)reader[6];

                    bool res = sheets.Add(sheet_id, id, name, first_strong, last_strong, translation_language, last_updated);
                    if (!res)
                    {
                        result = false;
                    }

                    tgtCmdText = string.Format(
                        "INSERT INTO public.\"{0}\"" +
                        " (sheet_id,id,name,first_strong,last_strong,translation_language,last_updated)" +
                        " VALUES ({1},'{2}','{3}','{4}','{5}',{6},'{7}');",
                        tgtTable,
                        sheet_id,
                        id,
                        name,
                        first_strong,
                        last_strong,
                        translation_language,
                        last_updated.ToString("yyyy-MM-dd HH:mm:ss")
                        );


                    tgtCommand.CommandText = tgtCmdText;
                    tgtCommand.ExecuteNonQuery();
                }
                reader.Close();
                reader.DisposeAsync();

                srcCommand.Dispose();
                tgtCommand.Dispose();

                Trace(MethodBase.GetCurrentMethod().Name + " Done!", System.Drawing.Color.Black);
            }
            catch (Exception ex)
            {
                TraceError(ex.Message);
                result = false;
            }
            finally
            {
                sourceConnection.Close();
                targetConnection.Close();
            }
            return result;
        }
        private bool ProcessTranslationTable()
        {
            bool result = true;  
            
            string srcTable = "dictionary_translation";
            string tgtTable = "translation";

            string srcCmdText = string.Empty;
            string tgtCmdText = string.Empty;

            try
            {
                ClearTargetTable(tgtTable);
                if (sourceConnection.State == System.Data.ConnectionState.Closed)
                    sourceConnection.Open();

                if (targetConnection.State == System.Data.ConnectionState.Closed)
                    targetConnection.Open();

                var srcCommand = sourceDataSource.CreateCommand();
                var tgtCommand = targetDataSource.CreateCommand();

                srcCmdText = string.Format(
                    "SELECT " +
                    " strongs_number,d_strong,transliteration,translated_word,translated_long_text" +
                    " FROM public.\"{0}\";", srcTable);

                srcCommand.CommandText = srcCmdText;
                NpgsqlDataReader reader = srcCommand.ExecuteReader();

                while (reader.Read())
                {
                    string strongs_number = (string) reader[0];
                    string d_strong = (string) reader[1];
                    string transliteration = (string) reader[2];
                    string translated_gloss = (string) reader[3];
                    string translated_text = (string) reader[4];

                    if(strongs_number == "H3000")
                    {
                        int x = 0;
                    }
                    string dStrong = (strongs_number + d_strong).Trim();
                    int strong_id = strongDict[dStrong];
                    int sheet_id = sheets.GetSheet(dStrong);
                    DateTime update_date = DateTime.Now ;
                    string updater_name = "Sami Abdel Malik";
                    string updater_email = "sami.abdelmalik@gmail.com";
                    int status_id = 1;

                    if(strong_id == 17456)
                    {
                        int x = 0;
                    }

                tgtCmdText = string.Format(
                    "INSERT INTO public.\"{0}\"" +
                    "(strong_id,sheet_id,update_date,updater_name,updater_email,status_id,translated_gloss,translated_text,transliteration)" +
                    " VALUES ({1},{2},'{3}','{4}','{5}',{6},'{7}','{8}','{9}');",
                    tgtTable,
                    strong_id,
                    sheet_id,
                    update_date.ToString("yyyy-MM-dd HH:mm:ss"),
                    updater_name,
                    updater_email,
                    status_id,
                    translated_gloss.Replace("'","''"),
                    translated_text.Replace("'", "''"),
                    transliteration.Replace("'", "''")
                    );

                    tgtCommand.CommandText = tgtCmdText;
                    tgtCommand.ExecuteNonQuery();
                }
                reader.Close();
                reader.DisposeAsync();

                srcCommand.Dispose();
                tgtCommand.Dispose();

                Trace(MethodBase.GetCurrentMethod().Name + " Done!", System.Drawing.Color.Black);
            }
            catch (Exception ex)
            {
                TraceError(tgtCmdText);
                TraceError(ex.Message);
                result = false;
            }
            finally
            {
                sourceConnection.Close();
                targetConnection.Close();
            }

            return result;
        }
        private bool ProcessReferencesTable()
        {
            bool result = true;

            string srcTable = "bible_words_references";
            string tgtTable = "bible_words_references";

            string srcCmdText = string.Empty;
            string tgtCmdText = string.Empty;



            try
            {
                ClearTargetTable(tgtTable);

                GetStrongsList();

                if (sourceConnection.State == System.Data.ConnectionState.Closed)
                    sourceConnection.Open();

                if (targetConnection.State == System.Data.ConnectionState.Closed)
                    targetConnection.Open();

                var srcCommand = sourceDataSource.CreateCommand();
                var tgtCommand = targetDataSource.CreateCommand();

                srcCmdText = string.Format(
                    "SELECT" +
                    " language_id,strongs_number,d_strong,word,\"references\"" +
                    " FROM public.\"{0}\";", srcTable);

                srcCommand.CommandText = srcCmdText;
                NpgsqlDataReader reader = srcCommand.ExecuteReader();

                while (reader.Read())
                {
                    int language_id = (int)reader[0];
                    string strongs_number = (string)reader[1];
                    string d_strong = reader[2].ToString();
                    string word = ((string)reader[3]);
                    string references = (string)reader[4];

                    SortedDictionary<int, List<char>> sDict;
                    int st = int.Parse(strongs_number.Substring(1));
                    if (strongs_number[0] == 'H')
                        sDict = hebrewStrongs;
                    else
                        sDict = greekStrongs;

                    List<char> list = sDict[st];
                    d_strong = list[0].ToString();
                    string dStrong = (strongs_number + d_strong).Trim();

                    int strong_id = strongDict[dStrong];

                    tgtCmdText = string.Format(
                    "INSERT INTO public.\"{0}\"" +
                    " (strong_id,language_id,gloss,\"references\")" +
                    " VALUES ({1},{2},'{3}','{4}');",
                    tgtTable,
                    strong_id, language_id, word, references);

                    tgtCommand.CommandText = tgtCmdText;
                    tgtCommand.ExecuteNonQuery();
                }
                reader.Close();
                reader.DisposeAsync();

                srcCommand.Dispose();
                tgtCommand.Dispose();

                Trace(MethodBase.GetCurrentMethod().Name + " Done!", System.Drawing.Color.Black);
            }
            catch (Exception ex)
            {
                TraceError(ex.Message);
                result = false;
            }
            finally
            {
                sourceConnection.Close();
                targetConnection.Close();
            }

            return result;
        }


        private void GetStrongsList()
        {
            string cmdText = string.Empty;
            //Trace("Fetching " + strongsNumber, Color.Green);

            try
            {
                if (sourceConnection.State == System.Data.ConnectionState.Closed)
                    sourceConnection.Open();

                NpgsqlCommand command = sourceDataSource.CreateCommand();

                command.CommandText =
                   "SELECT" +
                   " strongs_number,d_strong" +
                   " FROM public.\"strongs_numbers\"" +
                   " ORDER BY strongs_number ASC, d_strong ASC;";

                NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    SortedDictionary<int, List<char>> sDict;
                    string s = (string)reader[0];
                    if (s[0] == 'H')
                        sDict = hebrewStrongs;
                    else
                        sDict = greekStrongs;

                    int sNum = int.Parse(s.Substring(1));
                    string d = (string)reader[1];
                    if (sDict.ContainsKey(sNum))
                    {
                        if (!string.IsNullOrEmpty(d))
                            if (!sDict[sNum].Contains(d[0])) sDict[sNum].Add(d[0]);
                    }
                    else
                    {
                        List<char> list = new List<char>();
                        if (string.IsNullOrEmpty(d))
                        {
                            int x = 0;
                        }
                        if (!string.IsNullOrEmpty(d))
                            list.Add(d[0]);
                        sDict[sNum] = list;
                    }
                }
                command.Dispose();

                reader.Close();
                reader.DisposeAsync();
                command.Dispose();
                sourceConnection.Close();

            }
            catch (Exception ex)
            {
                TraceError("GetStrongsEntry Exception\r\n" + ex.ToString());
            }

        }

        private bool ClearTargetTable(string tableName)
        {
            Trace("Clearing " + tableName, Color.Green);

            bool result = true;
            string cmdText = string.Empty;

            try
            {
                var command = targetDataSource.CreateCommand();

                bool exists = false;
                command.CommandText = "SELECT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = '" + tableName + "') AS table_existence;";
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    exists = (bool)reader[0];
                }
                reader.Close();

                if (!exists)
                {
                    Trace("Table '" + tableName + "' does not exist!", Color.Red);
                    return false;
                }

                command.CommandText = "DELETE FROM public.\"" + tableName + "\";";
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                TraceError(ex.ToString());
                result = false;
            }

            return result;
        }

        private void createDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Thread(() => { Run(); }).Start();
        }

        public void Run()
        { 
            bool result = CreateDBFromSQLFile(@"C:\Users\samim\Documents\MyProjects\Lexicon\programs\StrongsDictionaryTranslation\database\Version2\StrongsDictionaryV2.sql");

            if (result) result = ProcessTranslationStatusTable();
            if (result) result = ProcessLanguageTable();
            if (result) result = ProcessBookTable();
            if (result) result = ProcessGoogleSheetTable();
            if (result) result = ProcessStrongsTable();
            if (result) result = ProcessBibleTextTable();
            if (result) result = ProcessTranslationTable();
            if (result) result = ProcessReferencesTable();

            Trace("*** All Done! ***", System.Drawing.Color.Black);

        }

        private bool CreateDBFromSQLFile(string sqlFilePath)
        { 
            bool result = true;

            try
            {
                string sqlScript = File.ReadAllText(sqlFilePath);
                var createdb_cmd = new NpgsqlCommand(sqlScript, targetConnection);
                targetConnection.Open();
                createdb_cmd.ExecuteNonQuery();
                targetConnection.Close();
                Trace(MethodBase.GetCurrentMethod().Name + " Done!", System.Drawing.Color.Black);
            }
            catch (Exception ex)
            {
                TraceError(ex.ToString());
                result = false;
            }

            return result;

        }
    }

    internal class StrongsNumber
    {
        public string strongs_number { get; set; }
        public string d_strong { get; set; }
        public string original_word { get; set; }
        public string english_translation { get; set; }
        public string long_text { get; set; }
        public string step_united_reason { get; set; }
        public string step_type { get; set; }
        public string transliteration { get; set; }
        public string pronunciation { get; set; }
    }


}
