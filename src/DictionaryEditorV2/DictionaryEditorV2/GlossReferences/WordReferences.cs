using Npgsql;
using DictionaryEditorV2.Lexicon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DictionaryEditorV2.GlossReferences
{
    public partial class WordReferences : DockContent
    {
        private int targetLanguage = 7;

        private string dbName = Properties.Settings.Default.Database;
        private NpgsqlDataSource dataSource;
        private NpgsqlConnection connection;

        private Dictionary<string, string> words = new Dictionary<string, string>();


        private LexiconMainForm container;
        private LexiconPanel lexicon;


        #region Constructors
        public WordReferences()
        {
            InitializeComponent();
            this.ControlBox = false;
        }

        public WordReferences(LexiconMainForm container, LexiconPanel lexicon)
        {
            InitializeComponent();

            this.ControlBox = false;
            this.CloseButtonVisible = false;
            this.CloseButton = false;

            this.container = container;
            this.lexicon = lexicon;
        }
        #endregion Constructors

        private void WordReferences_Load(object sender, EventArgs e)
        {
            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1Na61A51;Database=" + dbName;
            dataSource = NpgsqlDataSource.Create(connectionString);
            connection = dataSource.CreateConnection();

            dgvWords.CellClick += DgvWords_CellClick;

            if (lexicon != null)
            {
                lexicon.StrongsChanged += Lexicon_StrongsChanged;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="language"></param>
        /// <param name="strongsNumber"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetAssociatedWords(int language, long strong_id, string strongsNumber)
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
                   "SELECT" +
                   " gloss, \"references\" " +
                   " FROM public.\"bible_words_references\"" +
                   string.Format(" WHERE language_id={0} AND strong_id={1};",
                       language,
                       strong_id);

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

            }
            catch (Exception ex)
            {
               // Trace("GetAssociatedWords Exception\r\n" + ex.ToString(), Color.Red);
            }

            return words;
        }

        private void Lexicon_StrongsChanged(object sender, StrongEventArgs e)
        {
           // long strongid = e.StrongsID;
           // string strongsNumber = e.Strongs;

            new Thread(() => { PopulateWordsAndReferences(e.StrongsID, e.Strongs); }).Start();

            //words.Clear();
            //words = GetAssociatedWords(targetLanguage, strongsNumber);
            //PopulateWordsAndReferences();
        }

        private void PopulateWordsAndReferences(long strongid, string strongsNumber)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { PopulateWordsAndReferences(strongid, strongsNumber); }));
            }
            else
            {
                words.Clear();
                words = GetAssociatedWords(targetLanguage, strongid, strongsNumber);

                dgvWords.Rows.Clear();
                dgvWords.ColumnCount = 5;
                string[] associatedWords = words.Keys.ToArray();

                if (associatedWords.Length > 0)
                {
                    int rows = associatedWords.Length / 5;
                    int remain = associatedWords.Length % 5;
                    for (int i = 0; i < rows; i++)
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="references"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        private bool GetBibleVerses(int language, string references)
        {
            //SetTraceRTL(true);

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
                //Trace("GetBibleVerses1 Exception\r\n" + ex.ToString(), Color.Red);
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
                //Trace("GetBibleVerses2 Exception\r\n" + ex.ToString(), Color.Red);
                connection.Close();
                result = false;
            }

            return result;
        }

        #region Words & Verses
        private void DgvWords_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;

            string word = (string)dgvWords[e.ColumnIndex, e.RowIndex].Value;
            //string s = "H" + strongsNumberControl.Text;
            //if (cbLanguage.SelectedIndex == 1)
            //    s = "G" + strongsNumberControl.Text;

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

    }
}
