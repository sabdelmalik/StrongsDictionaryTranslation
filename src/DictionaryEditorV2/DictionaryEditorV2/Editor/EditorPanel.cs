using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;
using static System.Net.Mime.MediaTypeNames;

using System.Security.Cryptography.Xml;
using System.Reflection;
using System.Windows.Forms.Layout;

using DictionaryEditorV2.GlossReferences;
using DictionaryEditorV2.Lexicon;
using System.Runtime.CompilerServices;
using Npgsql;
using DictionaryEditorV2.GoogleAPI;

namespace DictionaryEditorV2.Editor
{
    public partial class EditorPanel : DockContent
    {
        private string dbName = Properties.Settings.Default.Database;
        private NpgsqlDataSource dataSource;
        private NpgsqlConnection connection;

        private LexiconMainForm container;
        private WordReferences wordReferences;
        private LexiconPanel lexicon;

        private string strongsPrefix = string.Empty;

        private System.Timers.Timer tempTimer = null;

        private List<TranslationEntry> testEntries = new List<TranslationEntry>();

        #region Constructors
        public EditorPanel()
        {
            InitializeComponent();
            this.ControlBox = false;
            this.CloseButtonVisible = false;
            this.CloseButton = false;
        }

        public EditorPanel(LexiconMainForm container, LexiconPanel lexicon)
        {
            InitializeComponent();

            this.ControlBox = false;
            this.CloseButtonVisible = false;
            this.CloseButton = false;

            //System.Drawing.Image img = picRedo.Image;
            //img.RotateFlip(RotateFlipType.Rotate180FlipY);
            //picRedo.Image = img;
            //picRedo.Invalidate();

            this.container = container;
            this.lexicon = lexicon;

        }
        #endregion Constructors

        #region Trace
        private void Trace(string text, System.Drawing.Color color,
        [CallerLineNumber] int lineNumber = 0,
        [CallerMemberName] string? caller = null)
        {
            Tracing.TraceInfo(string.Format("{0}#{1}", caller == null ? "???" : caller, lineNumber), text, color);
        }
        private void TraceError(
                string text,
                [CallerLineNumber] int lineNumber = 0,
                [CallerMemberName] string caller = null)
        {
            Tracing.TraceError(string.Format("{0}#{1}", caller == null ? "???" : caller, lineNumber), text);
        }
        #endregion Trace

        private void EditorPanel_Load(object sender, EventArgs e)
        {
            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1Na61A51;Database=" + dbName;
            dataSource = NpgsqlDataSource.Create(connectionString);
            connection = dataSource.CreateConnection();

            panelMain.AutoScroll = false;
            panelMain.HorizontalScroll.Enabled = false;
            panelMain.HorizontalScroll.Visible = false;
            panelMain.HorizontalScroll.Maximum = 0;
            panelMain.VerticalScroll.Enabled = true;
            panelMain.VerticalScroll.Visible = true;
            panelMain.AutoScroll = true;

            lexicon.StrongsChanged += Lexicon_StrongsChanged;

            PopulateTestEntries();
        }

        private void Lexicon_StrongsChanged(object sender, StrongEventArgs e)
        {
            new Thread(() => { UpdateMainPanel(e); }).Start();
        }



        private void PopulateTestEntries()
        {
            DateTime today = DateTime.Today;
            DateTime yesterday = today.AddDays(-1);
            DateTime beforeYesterday = today.AddDays(-2);

            testEntries.Add(new TranslationEntry
                (
                    10,
                    "G0001 -G0999",
                    beforeYesterday,
                    "Sami Abdel Malik",
                    "sami.abdelmalik@gmail.com",
                    "Pending",
                    "أَبِيَأَثَار",
                    "Ἀβιάθαρ , ὁ,  (بالعبرية: ᴴᴰᴺ)، أبيثار (راجع: ١صم ٢١:​١): راجع: مر ٢:​٢٦ .",
                    "Ἀβιαθάρ"
                ));

            testEntries.Add(new TranslationEntry
                (
                    10,
                    "G0001 -G0999",
                    yesterday,
                    "Sami Abdel Malik",
                    "sami.abdelmalik@gmail.com",
                    "Reviewed",
                    "أَبِيَأَثَار",
                    "Ἀβιάθαρ , ὁ,  (بالعبرية: ᴴᴰᴺ)، أبيثار (راجع: ١صم ٢١:​١): راجع: مر ٢:​٢٦ .",
                    "Ἀβιαθάρ"
                ));
            testEntries.Add(new TranslationEntry
                (
                    10,
                    "G0001 -G0999",
                    today,
                    "Sami Abdel Malik",
                    "sami.abdelmalik@gmail.com",
                    "Approved",
                    "أَبِيَأَثَار",
                    "Ἀβιάθαρ , ὁ,  (بالعبرية: ᴴᴰᴺ)، أبيثار (راجع: ١صم ٢١:​١): راجع: مر ٢:​٢٦ .",
                    "Ἀβιαθάρ"
                ));

        }
        private void TempTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //            container.FindVerse(cbTagToFind.Text);
        }



        #region Reference verse events
        private void Cms_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string tag = e.ClickedItem.Text;
            //browser.NavigateToTag((testament == TestamentEnum.OLD ? "H" : "G") + tag);
        }

        #endregion Reference verse events

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void UpdateMainPanel(StrongEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { UpdateMainPanel(e); }));
            }
            else {
                //ClearMainPanel();

                List<TranslationEntry> translationEntries = GetTranslationEntries(e);

                List<TableLayoutPanel> newTables = new List<TableLayoutPanel>();

                foreach (TranslationEntry entry in translationEntries) // testEntries)
                {
                    newTables.Add(GetTableEntry(entry));
                }
                UpdateTableEntries(newTables);
            }
        }

        private List<TranslationEntry> GetTranslationEntries(StrongEventArgs e)
        {
            List<TranslationEntry> translationEntries = new List<TranslationEntry>();

            string cmdText =
                   "SELECT " +
                   "xlt.strong_id," +
                   "xlt.update_date," +
                   "xlt.updater_name," +
                   "xlt.updater_email," +
                   "xlt.translated_gloss," +
                   "xlt.translated_text," +
                   "xlt.transliteration," +
                   "sta.status," +
                   "sht.name" +
                   " FROM public.\"translation\" xlt" +
                   " INNER JOIN public.\"translation_status\" sta" +
                        " ON xlt.status_id = sta.id" +
                   " INNER JOIN public.\"google_sheet\" sht" +
                        " ON xlt.sheet_id = sht.sheet_id" +
                   string.Format(" WHERE xlt.strong_id={0}", e.StrongsID) +
                   " ORDER BY xlt.update_date ASC;";

            NpgsqlDataReader reader = null;
            try
            {
                var command = dataSource.CreateCommand();
                command.CommandText = cmdText;
                reader = command.ExecuteReader();

                if (reader != null)
                {
                    string lastStrongs = string.Empty;

                    while (reader.Read())
                    {
                        long strong_id = reader.GetInt64(0);
                        DateTime update_date = reader.GetDateTime(1);
                        string updater_name = reader.GetString(2);
                        string updater_email = reader.GetString(3);
                        string translated_gloss = reader.GetString(4);
                        string translated_text = reader.GetString(5);
                        string transliteration = reader.GetString(6);
                        string status = reader.GetString(7);
                        string sheet_name = reader.GetString(8);

                        translationEntries.Add(new TranslationEntry(strong_id,sheet_name,update_date,updater_name,
                            updater_email,status,translated_gloss,translated_text,transliteration));
                    }
                }



            }
            catch (Exception ex)
            {
                TraceError(cmdText);
                TraceError(ex.Message);
            }



            return translationEntries;
        }

        private void ClearMainPanel()
        {
            Control[] ctrls = new Control[panelMain.Controls.Count];

            panelMain.Controls.CopyTo(ctrls, 0);
            panelMain.Controls.Clear();

            panelMain.Controls.Add(ctrls[ctrls.Length -1]);

        }
        private TableLayoutPanel GetTableEntry(TranslationEntry entry)
        {
            RichTextBox richTextBox1 = new RichTextBox()
            {
                Dock = DockStyle.Fill,
                RightToLeft = RightToLeft.Yes,
                Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 13.2f, FontStyle.Bold),
                Text = entry.Translated_text,
                Name = entry.Status
            };
            richTextBox1.TextChanged += RichTextBox1_TextChanged;

            int[] widths = new int[] { 10, 8, 20, 20, 34, 15, 15 };

            TableLayoutPanel detailsTable = new TableLayoutPanel()
            {
                ColumnCount = widths.Length,
                RowCount = 1,
                Dock = DockStyle.Fill
            };


            for (int i = 0; i < widths.Length; i++)
            {
                detailsTable.ColumnStyles.Add(new ColumnStyle());
            }


            for (int j = 0; j < widths.Length; j++)
            {
                ColumnStyle style = detailsTable.ColumnStyles[j];
                style.SizeType = SizeType.Percent;

                TextBox txtBox = new TextBox()
                {
                    Dock = DockStyle.Fill,
                    Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 10f, FontStyle.Bold)
                };

                switch (j)
                {
                    case 0:
                        style.Width = widths[j];
                        txtBox.Text = entry.Sheet_name;
                        detailsTable.Controls.Add(txtBox, j, 0);
                        break;
                   case 1:
                        style.Width = widths[j];
                        txtBox.Text = entry.Status;
                        detailsTable.Controls.Add(txtBox, j, 0);
                        break;
                    case 2:
                        style.Width = widths[j];
                        txtBox.Text = entry.Updater_name;
                        detailsTable.Controls.Add(txtBox, j, 0);
                        break;
                    case 3:
                        style.Width = widths[j];
                        txtBox.Text = entry.Update_date.ToString("yyyy-MM-dd hh:mm:ss");
                        detailsTable.Controls.Add(txtBox, j, 0);
                        break;
                    case 4:
                        style.Width = widths[j];
                        break;
                    case 5:
                        style.Width = widths[j];
                        txtBox.Text = entry.Transliteration;
                        detailsTable.Controls.Add(txtBox, j, 0);
                        break;
                    case 6:
                        style.Width = widths[j];
                        txtBox.Text = entry.Translated_gloss;
                        detailsTable.Controls.Add(txtBox, j, 0);
                        break;
                }

            }

            TableLayoutPanel containerTable = new TableLayoutPanel()
            {
                ColumnCount = 1,
                RowCount = 2,
                Dock = DockStyle.Top,
                Height = 200
            };
            containerTable.RowStyles.Add(new RowStyle() { SizeType = SizeType.Absolute, Height=35});
            containerTable.RowStyles.Add(new RowStyle() { SizeType = SizeType.AutoSize});

            containerTable.Controls.Add(detailsTable, 0, 0);
            containerTable.Controls.Add(richTextBox1, 0, 1);

            return containerTable;
        }

        private void RichTextBox1_TextChanged(object? sender, EventArgs e)
        {
            var x = sender;
        }

        private void AddTableEntry(TableLayoutPanel table)
        { 
            //var last = panelMain.Controls[panelMain.Controls.Count-1];
            Control[] ctrls = new Control[panelMain.Controls.Count];

            panelMain.Controls.CopyTo(ctrls, 0);
            panelMain.Controls.Clear();

            panelMain.Controls.Add(table);
            for (int i = 0; i < ctrls.Length; i++)
            {
                panelMain.Controls.Add(ctrls[i]);
            }


        }


        private void UpdateTableEntries(List<TableLayoutPanel> entries)
        {
            //var last = panelMain.Controls[panelMain.Controls.Count-1];
            Control[] ctrls = new Control[panelMain.Controls.Count];

            panelMain.Controls.CopyTo(ctrls, 0);
            panelMain.Controls.Clear();

            panelMain.Controls.AddRange(entries.ToArray());
            panelMain.Controls.Add(ctrls[ctrls.Length-1]);
 

        }
    }
}
