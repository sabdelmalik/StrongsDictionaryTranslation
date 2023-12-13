using Npgsql;
using System.Xml.Linq;

using BibleTaggingUtil;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;
using System.Net.Sockets;
using System.Security.Cryptography.Xml;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using static System.Windows.Forms.Design.AxImporter;
using Newtonsoft.Json;
using System.Reflection;
using System.Web;
using System.Text.RegularExpressions;
using System.Xml;
using System.Diagnostics.Eventing.Reader;

namespace DictionaryEditor
{
    public partial class MainForm : Form
    {

        private NpgsqlDataSource dataSource;
        private NpgsqlConnection connection;

        private Dictionary<string, string> words = new Dictionary<string, string>();

        private bool initializing = true;

        private bool dirty = false;

        private int returnLang = 0;
        string returnStrong = string.Empty;
        string returnDStrong = string.Empty;

        int lastH = 9049;
        int lastG = 9996;

        /// <summary>
        /// key strong's number
        /// value list of disambiguation characters
        /// </summary>
        private SortedDictionary<int, List<char>> greekStrongs = new SortedDictionary<int, List<char>>();
        private SortedDictionary<int, List<char>> hebrewStrongs = new SortedDictionary<int, List<char>>();

        private int targetLanguage = 7;

        DictionaryDump dictionaryDump;

        public MainForm()
        {
            InitializeComponent();
            strongsNumberControl.KeyDown += StrongsNumberControl_KeyDown;

            this.Shown += MainForm_Shown;
        }

        private void MainForm_Shown(object? sender, EventArgs e)
        {
            initializing = false;
        }

        #region Trace
        delegate void TraceDelegate(string text, Color color);
        delegate void ClearTraceDelegate();
        delegate void SetTraceRTLDelegate(bool rtl);

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

        public void SetTraceRTL(bool rtl)
        {
            if (InvokeRequired)
            {
                Invoke(new SetTraceRTLDelegate(SetTraceRTL), new object[] { rtl });
            }
            else
            {
                traceBox.RightToLeft = rtl ? RightToLeft.Yes : RightToLeft.No;
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
            }
        }

        public void TraceError(string method, string text)
        {
            Trace(string.Format("Error: {0}::{1}", method, text), Color.Red);
        }


        #endregion Trace

        private void Form1_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            dictionaryDump = new DictionaryDump(this);

            tbTranslatedLong.KeyUp += TbTranslatedLong_KeyUp;
            this.KeyDown += MainForm_KeyDown;

            Connect();

            dgvWords.CellClick += DgvWords_CellClick;
            this.KeyUp += Form1_KeyUp;

            GetStrongsList();

            string s = Properties.Settings1.Default.savedStrong;
            if (!string.IsNullOrEmpty(s))
                strongsNumberControl.Text = s;

            string d = Properties.Settings1.Default.savedDStrong;
            if (!string.IsNullOrEmpty(d))
                tbDstrongs.Text = d;

            tbDstrongs.TextChanged += tbDstrongs_TextChanged;
            strongsNumberControl.TextChanged += strongsNumberControl_TextChanged;

            tbTranslatedLong.DoubleClick += TbTranslatedLong_DoubleClick;

            InitialiseRTBContextMenu();

            cbLanguage.SelectedIndex = 0; // Properties.Settings1.Default.savedLanguage; // triggers cbLanguage changed event which starts fetching from the DB

        }

        private void MainForm_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
                GoBack();
            }
        }

        private void TbTranslatedLong_KeyUp(object? sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Q)
            {
                string text = tbTranslatedLong.SelectedText;
                if (text.Length > 0)
                {
                    tbTranslatedLong.SelectedText = string.Format("\"{0}\". ", text);
                }
            }
            else if (e.Control && e.KeyCode == Keys.Oem1)
            {
                SendKeys.Send(":");
                SendKeys.Send("\u200B");
            }
        }

        private void InitialiseRTBContextMenu()
        {
            ToolStripMenuItem tsmiUndo = new ToolStripMenuItem("Undo");
            tsmiUndo.Click += (sender, e) => tbTranslatedLong.Undo();
            contextMenuStrip1.Items.Add(tsmiUndo);

            ToolStripMenuItem tsmiRedo = new ToolStripMenuItem("Redo");
            tsmiRedo.Click += (sender, e) => tbTranslatedLong.Redo();
            contextMenuStrip1.Items.Add(tsmiRedo);

            contextMenuStrip1.Items.Add(new ToolStripSeparator());

            ToolStripMenuItem tsmiCut = new ToolStripMenuItem("Cut");
            tsmiCut.Click += (sender, e) => tbTranslatedLong.Cut();
            contextMenuStrip1.Items.Add(tsmiCut);

            ToolStripMenuItem tsmiCopy = new ToolStripMenuItem("Copy");
            tsmiCopy.Click += (sender, e) => tbTranslatedLong.Copy();
            contextMenuStrip1.Items.Add(tsmiCopy);

            ToolStripMenuItem tsmiPaste = new ToolStripMenuItem("Paste");
            tsmiPaste.Click += (sender, e) => tbTranslatedLong.Paste();
            contextMenuStrip1.Items.Add(tsmiPaste);

            ToolStripMenuItem tsmiDelete = new ToolStripMenuItem("Delete");
            tsmiDelete.Click += (sender, e) => tbTranslatedLong.SelectedText = "";
            contextMenuStrip1.Items.Add(tsmiDelete);

            contextMenuStrip1.Items.Add(new ToolStripSeparator());

            ToolStripMenuItem tsmiSelectAll = new ToolStripMenuItem("Select All");
            tsmiSelectAll.Click += (sender, e) => tbTranslatedLong.SelectAll();
            contextMenuStrip1.Items.Add(tsmiSelectAll);

            contextMenuStrip1.Opening += (sender, e) =>
            {
                tsmiUndo.Enabled = !tbTranslatedLong.ReadOnly && tbTranslatedLong.CanUndo;
                tsmiRedo.Enabled = !tbTranslatedLong.ReadOnly && tbTranslatedLong.CanRedo;
                tsmiCut.Enabled = !tbTranslatedLong.ReadOnly && tbTranslatedLong.SelectionLength > 0;
                tsmiCopy.Enabled = tbTranslatedLong.SelectionLength > 0;
                tsmiPaste.Enabled = !tbTranslatedLong.ReadOnly && Clipboard.ContainsText();
                tsmiDelete.Enabled = !tbTranslatedLong.ReadOnly && tbTranslatedLong.SelectionLength > 0;
                tsmiSelectAll.Enabled = tbTranslatedLong.TextLength > 0 && tbTranslatedLong.SelectionLength < tbTranslatedLong.TextLength;
            };

            tbTranslatedLong.ContextMenuStrip = contextMenuStrip1;
        }

        private void TbTranslatedLong_DoubleClick(object? sender, EventArgs e)
        {
            string strong = tbTranslatedLong.SelectedText;
            Match match = Regex.Match(strong, @"([HG])([0-9]{4})([A-Z]{0,1})");
            if (match.Success)
            {
                string strongs = match.Groups[2].Value;
                int strongNum = 0;
                if (!int.TryParse(strongs, out strongNum))
                    return;

                int language = match.Groups[1].Value == "H" ? 0 : 1;
                string d = match.Groups[3].Value;
                if (string.IsNullOrEmpty(d))
                {
                    // did this strong number has an entry with out d suffix?
                    List<char> chars;
                    SortedDictionary<int, List<char>> sDict;
                    if (language == 1) sDict = greekStrongs;
                    else sDict = hebrewStrongs;

                    if (sDict.ContainsKey(strongNum))
                    {
                        chars = sDict[strongNum];

                        if (chars.Count > 0)
                            d = chars[0].ToString();
                    }

                }
                int temp = 0;
                if (int.TryParse(strongs, out temp))
                {
                    tbDstrongs.TextChanged -= tbDstrongs_TextChanged;
                    cbLanguage.SelectedIndexChanged -= cbLanguage_SelectedIndexChanged;

                    // save the current strong's so that we can return to it
                    if (returnStrong == string.Empty)
                    {
                        returnLang = cbLanguage.SelectedIndex;
                        returnStrong = strongsNumberControl.Text;
                        returnDStrong = tbDstrongs.Text;
                    }

                    // Go to the double clicked Strong
                    cbLanguage.SelectedIndex = language;
                    tbDstrongs.Text = d;

                    tbDstrongs.TextChanged += tbDstrongs_TextChanged;
                    cbLanguage.SelectedIndexChanged += cbLanguage_SelectedIndexChanged;

                    strongsNumberControl.Text = strongs; // this should trigger the event
                }
            }
        }
        private void backToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GoBack();
        }

        private void GoBack()
        {
            if (returnStrong != string.Empty)
            {
                tbDstrongs.TextChanged -= tbDstrongs_TextChanged;
                cbLanguage.SelectedIndexChanged -= cbLanguage_SelectedIndexChanged;

                // Go to the saved Strongs
                tbDstrongs.Text = returnDStrong;
                cbLanguage.SelectedIndex = returnLang; // this should trigger the event

                tbDstrongs.TextChanged += tbDstrongs_TextChanged;
                cbLanguage.SelectedIndexChanged += cbLanguage_SelectedIndexChanged;
                //strongsNumberControl.TextChanged += strongsNumberControl_TextChanged;
                strongsNumberControl.Text = returnStrong;

                returnLang = 0;
                returnStrong = string.Empty;
                returnDStrong = string.Empty;


            }
        }



        #region Strong's Number Increment / decrement

        private string previouslanguage = string.Empty;
        private string previousStrongs = string.Empty;
        private string previousDStrongs = string.Empty;
        private bool fetchStarted = false;

        private void tbDstrongs_TextChanged(object sender, EventArgs e)
        {
            HandleStrongChanged();
        }

        private void strongsNumberControl_TextChanged(object sender, EventArgs e)
        {
            if (strongsNumberControl.TextLength == 4)
                HandleStrongChanged();
        }

        private void HandleStrongChanged()
        {
            // before fetching the new strong
            tbTranslatedWord.TextChanged -= tbTranslatedWord_TextChanged;
            tbTranslatedLong.TextChanged -= tbTranslatedLong_TextChanged;

            if (dirty)
            {
                //                if (!fetchStarted ) //&& !initializing)
                SaveCurrentRecord(previouslanguage + previousStrongs, previousDStrongs);
                //                previouslanguage = (cbLanguage.SelectedIndex == 1) ? "G" : "H";
                //                previousDStrongs = tbDstrongs.Text.Substring(0, 1);
                //                previousStrongs = strongsNumberControl.Text;
                dirty = false;
            }

            //            fetchStarted = true;
            FetchStrongs();
            //            fetchStarted = false;

            Properties.Settings1.Default.savedLanguage = cbLanguage.SelectedIndex;
            Properties.Settings1.Default.savedDStrong = tbDstrongs.Text;
            Properties.Settings1.Default.savedStrong = strongsNumberControl.Text;
            Properties.Settings1.Default.Save();

            string x = Properties.Settings1.Default.savedDStrong;

            tbTranslatedWord.TextChanged += tbTranslatedWord_TextChanged;
            tbTranslatedLong.TextChanged += tbTranslatedLong_TextChanged;
        }

        private void Form1_KeyUp(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.PageUp || e.KeyCode == Keys.Up)
            {
                GotoNextStrong();
                pbUP.Focus(); // move focus away from strongsNumberControl
                //SM FetchStrongs();

            }
            if (e.KeyCode == Keys.PageDown || e.KeyCode == Keys.Down)
            {
                GotoPreviousStrong();
                pbUP.Focus(); // move focus away from strongsNumberControl
                //SM FetchStrongs();

            }
        }


        private void pbUP_Click(object sender, EventArgs e)
        {
            GotoNextStrong();
            //SM FetchStrongs();
        }

        private void pbDown_Click(object sender, EventArgs e)
        {
            GotoPreviousStrong();
            //SM FetchStrongs();
        }

        private void FetchStrongs()
        {
            dgvVerses.Rows.Clear();

            string strongsNumber = "H" + strongsNumberControl.Text;
            if (cbLanguage.SelectedIndex == 1)
                strongsNumber = "G" + strongsNumberControl.Text;

            string dStrong = tbDstrongs.Text;
            if (!string.IsNullOrEmpty(dStrong))
                dStrong = dStrong.Substring(0, 1);

            GetStrongsEntry(strongsNumber, dStrong, false, null);
            GetDictionaryTranslation(targetLanguage, strongsNumber, dStrong, false, null);

            words.Clear();
            words = GetAssociatedWords(targetLanguage, strongsNumber, dStrong);

            dgvWords.Rows.Clear();
            dgvWords.ColumnCount = 5;
            string[] associatedWords = words.Keys.ToArray();

            if (associatedWords.Length > 0)
            {
                int rows = associatedWords.Length / 5;
                int remain = associatedWords.Length % 5;
                for (int i = 0; i < rows; i += 5)
                {
                    string[] arr = new string[5];
                    arr[0] = associatedWords[i];
                    arr[1] = associatedWords[i + 1];
                    arr[2] = associatedWords[i + 2];
                    arr[3] = associatedWords[i + 3];
                    arr[4] = associatedWords[i + 4];
                    dgvWords.Rows.Add(arr);
                }

                string[] arrR = new string[5];
                int offset = rows * 5;
                for (int i = 0; i < remain; i++)
                {
                    arrR[i] = associatedWords[i + offset];
                }
                dgvWords.Rows.Add(arrR);

                string references = string.Empty;
                for (int i = 0; i < associatedWords.Length; i++)
                {
                    //references = GetWordReferences(s, word);
                    references = words[associatedWords[i]];
                    if (references != string.Empty)
                        break;
                }

                if (references != string.Empty)
                    if (!GetBibleVerses(targetLanguage, references))
                    {
                        // try again
                        GetBibleVerses(targetLanguage, references);
                    }
            }
        }


        private void StrongsNumberControl_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                List<char>? chars = null;
                int current = int.Parse(strongsNumberControl.Text);
                if (cbLanguage.SelectedIndex == 1)
                {
                    if (greekStrongs.ContainsKey(current))
                        chars = greekStrongs[current];
                    else
                    {

                        MoveToNewStrongNumber(current > 0);
                        return;
                    }
                }
                else
                {
                    if (hebrewStrongs.ContainsKey(current))
                        chars = hebrewStrongs[current];
                    else
                    {
                        MoveToNewStrongNumber(current > 0);
                        return;
                    }
                }

                if (chars != null && chars.Count > 0)
                {
                    tbDstrongs.Text = chars[0].ToString();
                    pbUP.Focus(); // move focus away from strongsNumberControl
                    HandleStrongChanged();
                }
                else
                {
                    TraceError("Strong's number not found;");
                }

                //SM FetchStrongs();
            }
        }


        private void cbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<char> chars;
            int current = int.Parse(strongsNumberControl.Text);

            SortedDictionary<int, List<char>> sDict;
            if (cbLanguage.SelectedIndex == 1) sDict = greekStrongs;
            else sDict = hebrewStrongs;

            if (sDict.ContainsKey(current))
                chars = sDict[current];
            else
                chars = new List<char>();

            if (chars.Count > 0)
                tbDstrongs.Text = chars[0].ToString();

            HandleStrongChanged();

            //SM FetchStrongs();
        }

        private void ChangeStrongs(bool next)
        {
            List<char> chars = null;
            int current = int.Parse(strongsNumberControl.Text);
            if (cbLanguage.SelectedIndex == 1)
            {
                if (greekStrongs.ContainsKey(current))
                    chars = greekStrongs[current];
                else
                {
                    MoveToNewStrongNumber(next);
                    return;
                }
            }
            else
            {
                if (hebrewStrongs.ContainsKey(current))
                    chars = hebrewStrongs[current];
                else
                {
                    MoveToNewStrongNumber(next);
                    return;
                }
            }

            if (chars != null && chars.Count < 2)
            {
                MoveToNewStrongNumber(next);
            }
            else
            {
                char currentD = tbDstrongs.Text[0];
                int i = 0;
                for (i = 0; i < chars.Count; i++)
                {
                    if ((chars[i] == currentD))
                        break;
                }
                if (next)
                {
                    if (i == chars.Count - 1)
                    {
                        // last d, got to next number
                        MoveToNewStrongNumber(next);
                    }
                    else
                        tbDstrongs.Text = chars[i + 1].ToString();
                }
                else
                {
                    if (i == 0)
                    {
                        // first d, got to previous number
                        MoveToNewStrongNumber(next);
                    }
                    else
                        tbDstrongs.Text = chars[i - 1].ToString();
                }
            }

        }

        private void MoveToNewStrongNumber(bool next)
        {
            List<char> chars;
            if (next) strongsNumberControl.Increment();
            else strongsNumberControl.Decrement();

            int current = int.Parse(strongsNumberControl.Text);
            if (cbLanguage.SelectedIndex == 1)
            {
                while (!greekStrongs.ContainsKey(current))
                {
                    if (next)
                    {
                        strongsNumberControl.Increment();
                        if (current > lastG)
                            strongsNumberControl.Text = "0001";
                    }
                    else
                    {
                        strongsNumberControl.Decrement();
                        if (current < 1)
                            strongsNumberControl.Text = lastG.ToString();
                    }
                    current = int.Parse(strongsNumberControl.Text);
                }
                chars = greekStrongs[current];
            }
            else
            {
                while (!hebrewStrongs.ContainsKey(current))
                {
                    if (next)
                    {
                        strongsNumberControl.Increment();
                        if (current > lastH)
                            strongsNumberControl.Text = "0001";
                    }
                    else
                    {
                        strongsNumberControl.Decrement();
                         if (current < 1)
                            strongsNumberControl.Text = lastH.ToString();
                   }
                   current = int.Parse(strongsNumberControl.Text);
                }
                chars = hebrewStrongs[current];
            }
            if (chars.Count > 0)
            {
                if (next) tbDstrongs.Text = chars[0].ToString();
                else tbDstrongs.Text = chars[chars.Count - 1].ToString();
            }
            else tbDstrongs.Text = " ";

        }
        private void GotoNextStrong()
        {
            ChangeStrongs(true);
        }

        private void GotoPreviousStrong()
        {
            ChangeStrongs(false);
        }


        private void TbStrongsNumber_KeyPress(object? sender, KeyPressEventArgs e)
        {
            // ignore non-numeric
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        #endregion Strong's Number Increment / decrement


        #region Database
        private void Connect()
        {
            string dbName = "StrongsDictionary";

            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1Na61A51;Database=" + dbName;
            dataSource = NpgsqlDataSource.Create(connectionString);
            connection = dataSource.OpenConnection();

            GetStrongsList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="references"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        private bool GetBibleVerses(int language, string references)
        {
            SetTraceRTL(true);

            /// <summary>
            /// key: book id
            /// value: Dictionary
            ///         Key: chapter
            ///         value: verse_num=??? (where clause)
            /// </summary>
            Dictionary<int, Dictionary<int, string>> books = new Dictionary<int, Dictionary<int, string>>();

            string commandText = string.Empty;

            string refs = references;
            if (references.EndsWith(","))
            {
                refs = refs.Substring(0, refs.Length - 1);
            }

            try
            {
                string[] refsParts = refs.Split(',');
                foreach (string refPart in refsParts)
                {
                    string[] p2 = refPart.Trim().Split(" ");
                    //string book = p2[0];
                    string[] p3 = p2[1].Split(":");
                    int chapter = int.Parse(p3[0].Trim());
                    int verse = int.Parse(p3[1]);
                    int bookID = Utils.GetBookIndex(refPart.Trim());
                    if (books.ContainsKey(bookID))
                    {
                        Dictionary<int, string> chapters = books[bookID];
                        if (chapters.ContainsKey(chapter))
                        {
                            chapters[chapter] += string.Format(" OR verse_num={0}", verse);
                        }
                        else
                        {
                            chapters.Add(chapter, string.Format("verse_num={0}", verse));
                        }
                    }
                    else
                    {
                        Dictionary<int, string> chapters = new Dictionary<int, string>();
                        chapters.Add(chapter, string.Format("verse_num={0}", verse));
                        books.Add(bookID, chapters);
                    }

                }
            }
            catch (Exception ex)
            {
                Trace("GetBibleVerses1 Exception\r\n" + ex.ToString(), Color.Red);
                return false;
            }

            string cmdText = string.Empty;
            bool result = true;
            //Trace("Fetching " + strongsNumber, Color.Green);
            dgvVerses.Rows.Clear();

            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();

                foreach (int bookID in books.Keys)
                {
                    foreach (int chapter in books[bookID].Keys)
                    {
                        NpgsqlCommand command = dataSource.CreateCommand();
                        commandText =
                           "SELECT  " +
                           "book_id, chapter_num, verse_num, verse_text " +
                           "FROM public.\"bible_text\" " +
                           string.Format(" WHERE language_id={0} AND book_id={1} AND chapter_num={2} AND ({3});",
                               language, bookID, chapter, books[bookID][chapter]);

                        command.CommandText = commandText;
                        NpgsqlDataReader reader = command.ExecuteReader();

                        dgvVerses.ColumnCount = 2;
                        dgvVerses.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dgvVerses.Columns[1].DefaultCellStyle = new DataGridViewCellStyle();
                        dgvVerses.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                        // Output rows
                        while (reader.Read())
                        {
                            string[] arr = new string[2];
                            arr[0] = string.Format("{0} {1}:{2}", Constants.usfmNames[(int)reader[0]], reader[1], reader[2]);
                            arr[1] = (string)reader[3];

                            dgvVerses.Rows.Add(arr);
                            //Trace(string.Format("{0} {1}:{2} {3}", Constants.usfmNames[(int)reader[0]], reader[1], reader[2], reader[3]), Color.DarkBlue);
                        }
                        reader.Close();
                        reader.DisposeAsync();
                        command.Dispose();
                        connection.Close();
                    }

                }

            }
            catch (Exception ex)
            {
                Trace("GetBibleVerses2 Exception\r\n" + ex.ToString(), Color.Red);
                connection.Close();
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="language"></param>
        /// <param name="strongsNumber"></param>
        /// <param name="dStrong"></param>
        /// <param name="trace"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        public bool GetDictionaryTranslation(int language, string strongsNumber, string dStrong, bool trace, LexiconEntry entry)
        {
            string cmdText = string.Empty;
            bool result = true;
            //Trace("Fetching " + strongsNumber, Color.Green);

            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();

                NpgsqlCommand command = dataSource.CreateCommand();

                command.CommandText =
                   "SELECT  " +
                   "translated_word,translated_long_text,translated_short_text, transliteration " +
                   "FROM public.\"dictionary_translation\" " +
                   string.Format(" WHERE language_id={0} AND strongs_number='{1}' AND d_strong='{2}';",
                       language,
                       strongsNumber,
                       dStrong.Trim());

                NpgsqlDataReader reader = command.ExecuteReader();


                tbTranslatedWord.TextChanged -= tbTranslatedWord_TextChanged;
                tbTranslatedLong.TextChanged -= tbTranslatedLong_TextChanged;

                if (entry == null)
                {
                    tbTranslatedWord.Text = string.Empty;
                    tbTranslatedLong.Text = string.Empty;
                    tbTranslatedShort.Text = string.Empty;
                    tbArabicTxLt.Text = string.Empty;
                }

                while (reader.Read())
                {
                    string s = (string)reader[0];

                    if (entry != null)
                    {
                        if (entry.Gloss != string.Empty)
                        {
                            if (entry.Gloss == (string)reader[0])
                            {
                                entry.Definition += " * " + (string)reader[1];
                            }
                        }
                        else
                        {
                            entry.Gloss = (string)reader[0];
                            entry.Definition = (string)reader[1];
                            entry.Transliteration = (string)reader[3];
                        }
                    }
                    else
                    {
                        if (strongsNumber[0] == 'H')
                            tbOriginal.RightToLeft = RightToLeft.Yes;
                        else
                            tbOriginal.RightToLeft = RightToLeft.No;
                        if (tbTranslatedWord.Text != string.Empty)
                        {
                            if (tbTranslatedWord.Text == (string)reader[0])
                            {
                                tbTranslatedLong.Text += " * " + (string)reader[1];
                                tbTranslatedShort.Text += " * " + (string)reader[2];
                            }
                        }
                        else
                        {
                            tbTranslatedWord.Text = (string)reader[0];
                            tbTranslatedLong.Text = (string)reader[1];
                            tbTranslatedShort.Text = (string)reader[2];
                            tbArabicTxLt.Text = (string)reader[3];
                        }
                    }

                    if (trace)
                    {
                        Trace("Translation", Color.Green);
                        Trace(string.Format("Word:\t{0}", tbTranslatedWord.Text), Color.Green);
                        Trace(string.Format("Long Text:\t{0}", tbTranslatedLong.Text), Color.Green);
                        Trace(string.Format("Short Text\t{0}\r\n", tbTranslatedShort.Text), Color.Green);
                        Trace("================================", Color.Black);
                    }

                    // TODO: we may get more than one raw because of d strongs
                    //       Need to deal with dstrong
                    //                    command.Dispose();
                    //                    break;
                }

                tbTranslatedWord.TextChanged += tbTranslatedWord_TextChanged;
                tbTranslatedLong.TextChanged += tbTranslatedLong_TextChanged;

                command.Dispose();

                reader.Close();
                reader.DisposeAsync();
                command.Dispose();
                connection.Close();

            }
            catch (Exception ex)
            {
                Trace("GetDictionaryTranslation Exception\r\n" + ex.ToString(), Color.Red);
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetStrongsList()
        {
            string cmdText = string.Empty;
            //Trace("Fetching " + strongsNumber, Color.Green);

            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();

                NpgsqlCommand command = dataSource.CreateCommand();

                command.CommandText =
                   "SELECT  " +
                   "strongs_number,d_strong " +
                   "FROM public.\"strongs_numbers\";";

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
                connection.Close();

            }
            catch (Exception ex)
            {
                Trace("GetStrongsEntry Exception\r\n" + ex.ToString(), Color.Red);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strongsNumber"></param>
        /// <param name="dStrong"></param>
        /// <param name="trace"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        public bool GetStrongsEntry(string strongsNumber, string dStrong, bool trace, LexiconEntry entry)
        {
            string cmdText = string.Empty;
            bool result = true;
            //Trace("Fetching " + strongsNumber, Color.Green);

            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();

                NpgsqlCommand command = dataSource.CreateCommand();

                command.CommandText =
                   "SELECT  " +
                   "strongs_number,d_strong,original_word,english_translation,long_text,short_text,transliteration " +
                   "FROM public.\"strongs_numbers\" " +
                   string.Format(" WHERE strongs_number='{0}' AND d_strong='{1}';",
                       strongsNumber, dStrong.Trim());

                NpgsqlDataReader reader = command.ExecuteReader();

                tbOriginal.Text = string.Empty;
                tbEnglish.Text = string.Empty;
                tbLongText.Text = string.Empty;
                tbShortText.Text = string.Empty;

                while (reader.Read())
                {
                    string s = (string)reader[0];
                    if (s[0] == 'H')
                        tbOriginal.RightToLeft = RightToLeft.Yes;
                    else
                        tbOriginal.RightToLeft = RightToLeft.No;

                    if (entry != null)
                    {
                        if (entry.UnicodeAccented != string.Empty)
                        {
                            // concatenete dStrongs if required
                            if (tbOriginal.Text == (string)reader[2])
                            {
                                entry.UnicodeAccented += " * " + (string)reader[4];
                            }

                        }
                        else
                        {
                            entry.UnicodeAccented = (string)reader[2];
                            entry.English = (string)reader[3];
                            entry.Transliteration = (string)reader[6];
                        }
                    }
                    else
                    {
                        if (tbOriginal.Text != string.Empty)
                        {
                            // concatenete dStrongs if required
                            if (tbOriginal.Text == (string)reader[2])
                            {
                                tbLongText.Text += " * " + (string)reader[4];
                                tbShortText.Text += " * " + (string)reader[5];
                            }

                        }
                        else
                        {
                            tbOriginal.Text = (string)reader[2];
                            tbEnglish.Text = (string)reader[3];
                            tbLongText.Text = (string)reader[4];
                            tbShortText.Text = (string)reader[5];
                        }
                    }

                    if (trace)
                    {
                        Trace(string.Format("{0}", strongsNumber), Color.Black);
                        Trace(string.Format("Hebrew:\t{0}", tbOriginal.Text), Color.Black);
                        Trace(string.Format("English:\t{0}", tbEnglish.Text), Color.Black);
                        Trace(string.Format("Long Text:\t{0}", tbLongText.Text), Color.Black);
                        Trace(string.Format("Short Text\t{0}\r\n", tbShortText.Text), Color.Black);
                    }


                    // TODO: we may get more than one raw because of d strongs
                    //       Need to deal with dstrong
                    //                    command.Dispose();
                    //                    break;
                }
                command.Dispose();

                reader.Close();
                reader.DisposeAsync();
                command.Dispose();
                connection.Close();

            }
            catch (Exception ex)
            {
                Trace("GetStrongsEntry Exception\r\n" + ex.ToString(), Color.Red);
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="language"></param>
        /// <param name="strongsNumber"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetAssociatedWords(int language, string strongsNumber, string dStrong)
        {
            /// <summary>
            /// key: word
            /// value: associated references
            /// </summary>
            Dictionary<string, string> words = new Dictionary<string, string>();

            string cmdText = string.Empty;
            bool result = true;
            //Trace("Fetching " + strongsNumber, Color.Green);

            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();

                NpgsqlCommand command = dataSource.CreateCommand();

                command.CommandText =
                   "SELECT  " +
                   "word, \"references\" " +
                   "FROM public.\"bible_words_references\" " +
                   string.Format("WHERE language_id={0} AND strongs_number='{1}';",
                       language,
                       strongsNumber);

                NpgsqlDataReader reader = command.ExecuteReader();

                // Output rows
                while (reader.Read())
                {
                    string word = (string)reader[0];
                    string refs = (string)reader[1];
                    if (string.IsNullOrEmpty(word) || string.IsNullOrEmpty(refs))
                        continue;
                    if (words.ContainsKey(word))
                    {
                        words[word] = words[word] + refs;
                    }
                    else
                    {
                        words.Add(word, refs);
                    }
                }
                reader.Close();
                reader.DisposeAsync();
                command.Dispose();
                connection.Close();

                //string output = string.Empty;
                //foreach (string word in words)
                //{
                //    output += word + ", ";
                //}
                //if (string.IsNullOrEmpty(output))
                //{
                //    Trace(string.Format("No words for {0}", strongsNumber), Color.Red);
                //}
                //else
                //{
                //    output = output.Substring(0, output.Length - 1);
                //    //Trace(string.Format("{0}", output), Color.Black);
                //}
                //Trace("================================", Color.Black);

            }
            catch (Exception ex)
            {
                Trace("GetAssociatedWords Exception\r\n" + ex.ToString(), Color.Red);
            }

            return words;
        }

        #endregion Database

        #region Words & Verses
        private void DgvWords_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;

            string word = (string)dgvWords[e.ColumnIndex, e.RowIndex].Value;
            string s = "H" + strongsNumberControl.Text;
            if (cbLanguage.SelectedIndex == 1)
                s = "G" + strongsNumberControl.Text;

            //string references = GetWordReferences(s, word);
            if (!string.IsNullOrEmpty(word))
            {
                string references = words[word];
                if (references != string.Empty)
                    GetBibleVerses(targetLanguage, references);
            }
        }

        private void DgvVerses_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.Value == null) return;

            if (dgvWords.SelectedCells.Count != 1) return;

            string word = dgvWords.SelectedCells[0].Value.ToString();
            if (string.IsNullOrEmpty(word)) return;

            Font font = e.CellStyle.Font;
            StringFormat sf = StringFormat.GenericTypographic;
            sf.FormatFlags = sf.FormatFlags | StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.DisplayFormatControl | StringFormatFlags.DirectionRightToLeft;
            e.PaintBackground(e.CellBounds, true);

            SolidBrush br = new SolidBrush(Color.White);
            if (((int)e.State & (int)DataGridViewElementStates.Selected) == 0)
                br.Color = Color.Black;

            string text = e.Value.ToString();
            SizeF textSize = e.Graphics.MeasureString(text, font, e.CellBounds.Width, sf);

            int keyPos = text.IndexOf(word, StringComparison.OrdinalIgnoreCase);
            if (keyPos >= 0)
            {
                SizeF textMetricSize = new SizeF(0, 0);
                if (keyPos >= 1)
                {
                    string textMetric = text.Substring(0, keyPos);
                    textMetricSize = e.Graphics.MeasureString(textMetric, font, e.CellBounds.Width, sf);
                }

                SizeF keySize = e.Graphics.MeasureString(text.Substring(keyPos, word.Length), font, e.CellBounds.Width, sf);
                float left = e.CellBounds.Left + (keyPos <= 0 ? 0 : textMetricSize.Width) + 2;
                RectangleF keyRect = new RectangleF(left, e.CellBounds.Top + 1, keySize.Width, e.CellBounds.Height - 2);

                var fillBrush = new SolidBrush(Color.Yellow);
                e.Graphics.FillRectangle(fillBrush, keyRect);
                fillBrush.Dispose();
            }
            StringFormat format = new StringFormat(StringFormatFlags.DirectionRightToLeft);
            e.Graphics.DrawString(text, font, br, new PointF(e.CellBounds.Right + 2, e.CellBounds.Top + (e.CellBounds.Height - textSize.Height) / 2), sf);
            e.Handled = true;

            br.Dispose();
        }
        #endregion Words & Verses



        private void SaveCurrentRecord(string strongsNumber, string dStrong)
        {
            string cmdText = string.Empty;

            try
            {
                //string strongsNumber = ((cbLanguage.SelectedIndex == 1) ? "G" : "H") + strongsNumberControl.Text;
                //string dStrong = tbDstrongs.Text.Substring(0, 1);

                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();

                var command = dataSource.CreateCommand();

                //string tableName = "updated_translation";

                //bool exists = false;
                //command.CommandText = string.Format("SELECT id from public.\"{0}\" WHERE strongs_number='{1}' AND d_strong='{2}';",
                //    tableName,
                //    strongsNumber,
                //    dStrong);

                //NpgsqlDataReader reader = command.ExecuteReader();
                //while (reader.Read())
                //{
                //    exists = (long)reader[0] > 0;
                //}
                //reader.Close();

                //if (exists)
                //{

                //    cmdText = "UPDATE public.\"" + tableName + "\" " +
                //        string.Format("SET translated_word='{0}', translated_text='{1}', reviewer_initials='{2}' ",
                //        tbTranslatedWord.Text.Replace("'", "''"),
                //        tbTranslatedLong.Text.Replace("'", "''"),
                //        Environment.UserName) +
                //        string.Format("WHERE strongs_number='{0}' AND d_strong='{1}' AND language_id={2};",
                //        strongsNumber, dStrong, targetLanguage);

                //    command.CommandText = cmdText;
                //    command.ExecuteNonQuery();
                //}
                //else
                //{
                //    cmdText = "INSERT INTO public.\"" + tableName + "\" " +
                //   "(language_id, strongs_number, d_strong, translated_word, translated_text, reviewer_initials) " +
                //       "VALUES " +
                //       string.Format("({0}, '{1}', '{2}', '{3}', '{4}', '{5}');",
                //       targetLanguage,
                //       strongsNumber,
                //       dStrong,
                //       tbTranslatedWord.Text.Replace("'", "''"),
                //       tbTranslatedLong.Text.Replace("'", "''"),
                //       Environment.UserName);

                //    command.CommandText = cmdText;
                //    command.ExecuteNonQuery();

                //}


                string tableName = "dictionary_translation";
                cmdText = "UPDATE public.\"" + tableName + "\" " +
                        string.Format("SET translated_word='{0}', translated_long_text='{1}' ",
                        tbTranslatedWord.Text.Replace("'", "''"),
                        tbTranslatedLong.Text.Replace("'", "''")) +
                        string.Format("WHERE strongs_number='{0}' AND d_strong='{1}' AND language_id={2};",
                        strongsNumber, dStrong, targetLanguage);
                command.CommandText = cmdText;
                command.ExecuteNonQuery();


                command.Dispose();
                connection.Close();

                //    mainForm.Trace(MethodBase.GetCurrentMethod().Name + ": Database updated", Color.Blue);
            }
            catch (Exception ex)
            {
                TraceError(MethodBase.GetCurrentMethod().Name, cmdText);
                TraceError(MethodBase.GetCurrentMethod().Name, ex.ToString());
            }

        }


        private void tbTranslatedWord_TextChanged(object sender, EventArgs e)
        {
            dirty = true;
            previouslanguage = (cbLanguage.SelectedIndex == 1) ? "G" : "H";
            previousDStrongs = tbDstrongs.Text.Substring(0, 1);
            previousStrongs = strongsNumberControl.Text;

        }

        private void tbTranslatedLong_TextChanged(object sender, EventArgs e)
        {
            dirty = true;
            previouslanguage = (cbLanguage.SelectedIndex == 1) ? "G" : "H";
            previousDStrongs = tbDstrongs.Text.Substring(0, 1);
            previousStrongs = strongsNumberControl.Text;

        }

        private void withDStrongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateJSON(true);
        }


        private void withuStrongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateJSON(false);
        }

        private void GenerateJSON(bool outputDStrong)
        {

            DialogResult result = folderBrowserDialog1.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                bool supressReferences = true;
                result = MessageBox.Show("Include References?", "References", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes) supressReferences = false;

                string folder = folderBrowserDialog1.SelectedPath;
                string outFile = Path.Combine(folder, "AraSVDStrongsLexicon.json");

                dictionaryDump.DumpJSON(targetLanguage, outFile, outputDStrong, supressReferences, hebrewStrongs, greekStrongs);
            }
            Trace("Done!", Color.Green);
        }

        private void saveCurrentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dStrongs = tbDstrongs.Text.Substring(0, 1);
            string strongs = ((cbLanguage.SelectedIndex == 1) ? "G" : "H") + strongsNumberControl.Text;

            SaveCurrentRecord(strongs, dStrongs);
        }

        private void btnZspace_Click(object sender, EventArgs e)
        {
            tbTranslatedLong.Focus();
            SendKeys.Send("\u200B");
        }


        bool stopSearch = false;

        private void btnNext_Click(object sender, EventArgs e)
        {
            Search(true);
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search(false);
        }

        private void Search(bool next)
        {
            if (string.IsNullOrEmpty(tbSearch.Text))
                return;

            string searchText = tbSearch.Text;

            string cmdText = string.Empty;

            try
            {

                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();

                var command = dataSource.CreateCommand();

                string tableName = "dictionary_translation";

                long id = 0;

                string strongNum = tbSearchStart.Text;
                string dStrong = string.Empty;
                if (!string.IsNullOrEmpty(strongNum))
                {
                    if (strongNum.Length < 5 || strongNum.Length > 6)
                        strongNum = string.Empty;
                    else if (strongNum[0] != 'H' && strongNum[0] != 'G')
                        strongNum = string.Empty;
                    else if (strongNum.Length == 6)
                    {
                        dStrong = strongNum.Substring(5);
                        strongNum = strongNum.Substring(0, 5);
                    }

                }
                if (!string.IsNullOrEmpty(strongNum))
                {
                    command.CommandText =
                   "SELECT id FROM public.\"" + tableName + "\" Where strongs_number='" + strongNum + "'" +
                   ((dStrong == string.Empty) ? ";" :
                   " AND d_strong='" + dStrong + "';");
                }
                else
                {
                    command.CommandText =
                   "SELECT MIN(id) FROM public.\"" + tableName + "\";";
                }
                NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    id = (long)reader[0];
                    break;
                }
                reader.Close();
                reader.DisposeAsync();
                command.Dispose();


                if (next && !string.IsNullOrEmpty(strongNum))
                    id++;

                //============================
                command = dataSource.CreateCommand();
                command.CommandText =
               "SELECT  " +
               "strongs_number, d_strong, translated_long_text " +
               "FROM public.\"" + tableName + "\" " +
                string.Format("WHERE id >= {0} ORDER BY strongs_number ASC;", id);

                reader = command.ExecuteReader();

                while (reader.Read() && !stopSearch)
                {
                    string text = removeDiacritics((string)reader[2]);
                    if (text.Contains(removeDiacritics(searchText)))
                    {
                        string strongNumber = (string)reader[0];
                        dStrong = (string)reader[1];
                        tbSearchStart.Text = (strongNumber + dStrong).Trim();

                        tbDstrongs.TextChanged -= tbDstrongs_TextChanged;
                        cbLanguage.SelectedIndexChanged -= cbLanguage_SelectedIndexChanged;

                        tbDstrongs.Text = dStrong;
                        cbLanguage.SelectedIndex = strongNumber[0] == 'G' ? 1 : 0;

                        tbDstrongs.TextChanged += tbDstrongs_TextChanged;
                        cbLanguage.SelectedIndexChanged += cbLanguage_SelectedIndexChanged;

                        strongsNumberControl.Text = strongNumber.Substring(1, 4);
                        break;
                    }
                }
                reader.Close();
                reader.DisposeAsync();
                command.Dispose();
                connection.Close();

                stopSearch = false;
            }
            catch (Exception ex)
            {
                TraceError(MethodBase.GetCurrentMethod().Name, cmdText);
                TraceError(MethodBase.GetCurrentMethod().Name, ex.ToString());
            }
        }

        private void btnStopSearch_Click(object sender, EventArgs e)
        {
            stopSearch = true;
        }


        private string removeDiacritics(string text)
        {
            // Remove diacretics
            string clean = text.
                Replace("\u064B", "").  // ARABIC FATHATAN
                Replace("\u064C", "").  // ARABIC DAMMATAN
                Replace("\u064D", "").  // ARABIC KASRATAN
                Replace("\u064E", "").  // ARABIC FATHA
                Replace("\u064F", "").  // ARABIC DAMMA
                Replace("\u0650", "").  // ARABIC KASRA
                Replace("\u0651", "").  // ARABIC SHADDA
                Replace("\u0652", "").  // ARABIC SUKUN
                Replace("\u0653", "").  // Madda
                Replace("\u0654", "").  // Hamza above
                Replace("\u0655", "").  // Hamza below
                Replace("\u0656", "").  // 
                Replace("\u0657", "").
                Replace("\u0658", "").
                Replace("\u0659", "").
                Replace("\u065A", "").
                Replace("\u065B", "").
                Replace("\u065C", "").
                Replace("\u065D", "").
                Replace("\u065E", "").
                Replace("\u065F", "");
            // Reduce variances of Alef to just one form
            clean = clean.
                Replace("\u0622", "\u0627").  //ALEF WITH MADDA ABOVE
                Replace("\u0623", "\u0627").  //ALEF WITH HAMZA ABOVE
                Replace("\u0625", "\u0627").  //ALEF WITH HAMZA BELOW
                Replace("\u0671", "\u0627");  //ALEF WASLA

            return clean;
        }

        private void csvWithdStrongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateCSV(true);
        }

        private void csvWithuStrongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateCSV(false);
        }

        private void GenerateCSV(bool outputDStrong)
        {

            DialogResult result = folderBrowserDialog1.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                bool supressReferences = true;
                result = MessageBox.Show("Include References?", "References", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes) supressReferences = false;

                string folder = folderBrowserDialog1.SelectedPath;
                string outFile = Path.Combine(folder, "AraSVDStrongsLexicon.txt");

                dictionaryDump.DumpCSV(targetLanguage, outFile, outputDStrong, supressReferences, hebrewStrongs, greekStrongs);
            }
            Trace("Done!", Color.Green);
        }

    }

    public class LexiconEntry
    {

        public LexiconEntry()
        {
            Clear();
        }
        public void Clear()
        {
            UnicodeAccented = string.Empty;
            Transliteration = string.Empty;
            Gloss = string.Empty;
            Definition = string.Empty;
            English = string.Empty;
        }

        public string UnicodeAccented { get; set; }
        public string Transliteration { get; set; }
        public string English { get; set; }
        public string Gloss { get; set; }
        public string Definition { get; set; }

        public string StrongsNum { get; set; }
        public string dStrong { get; set; }
        public string References { get; set; }

    }

    public class LexiconJsonObject
    {
        public string Strongs { get; set; }
        public string Hebrew { get; set; }
        public string English { get; set; }
        public string Transliteration { get; set; }
        public string Arabic { get; set; }
        public string Definition { get; set; }
        public string References { get; set; }
    }

}