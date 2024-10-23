using Microsoft.VisualBasic;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DictionaryEditorV2.Lexicon
{
    /// <summary>
    /// This Form displays three list boxes for selecting Bible book, chapter and verse
    /// Any change in the selection in any of the boxes causes a StrongsChanged event to fire.
    /// </summary>
    public partial class LexiconPanel : DockContent
    {
        string strongsPattern = @"([HG])(\d\d\d\d)([a-zA-Z])";

        private string dbName = Properties.Settings.Default.Database;
        private NpgsqlDataSource dataSource;
        private NpgsqlConnection connection;

        int lastH = 9049;
        int lastG = 9996;

        public event StrongEventHandler StrongsChanged;

        public LexiconPanel()
        {
            InitializeComponent();
            this.ControlBox = false;
        }

        private void LexiconEntryPanel_Load(object sender, EventArgs e)
        {
            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1Na61A51;Database=" + dbName;
            dataSource = NpgsqlDataSource.Create(connectionString);
            connection = dataSource.CreateConnection();
        }

        public void Initialise()
        { 
            cbLanguage.SelectedIndexChanged -= CbLanguage_SelectedIndexChanged;
            cbDStrong.SelectedIndexChanged -= CbDStrong_SelectedIndexChanged;

            cbLanguage.Items.AddRange(new object[] { "Hebrew", "Greek" });

            strongsNumberControl.KeyDown += StrongsNumberControl_KeyDown;

            int lang = Properties.Settings.Default.LastStrongsLanguage;
            int strongs = Properties.Settings.Default.LastStrongs;
            string dStrong = Properties.Settings.Default.LastDStrongs;

            if(strongs <1 || lang< 0)
            {
                lang = 0;
                strongs = 1;
                dStrong = "H";
            }

            cbLanguage.SelectedIndex = lang;
            strongsNumberControl.Value = strongs;


            StrongsNumber sn = StrongsNumbers.Instance.GetStrongNumber(lang == 0, strongsNumberControl.Value);
            if (sn == null)
            {
                return;
            }
            cbDStrong.Items.Clear();

            if (sn.HasDStrongs)
            {
                cbDStrong.Items.AddRange(sn.DStrongs.ToArray());
                cbDStrong.SelectedIndex = sn.DStrongs.IndexOf(dStrong);
                cbDStrong.Visible = true;
            }
            else
                cbDStrong.Visible = false;

            cbDStrong.SelectedIndex = cbDStrong.Items.IndexOf(dStrong);

            LoadLexicon(false, null);


            cbLanguage.SelectedIndexChanged += CbLanguage_SelectedIndexChanged;
            cbDStrong.SelectedIndexChanged += CbDStrong_SelectedIndexChanged;

            // new Thread(() => { LoadLexicon(); }).Start();

            // toolStrip1.Cursor = Cursors.Default;
        }

        private void LoadLexicon( bool trace, LexiconEntry entry)
        {
            StrongsNumber sn = StrongsNumbers.Instance.GetStrongNumber(cbLanguage.SelectedIndex == 0, strongsNumberControl.Value);

            string strongsNumber = sn.Strongs;
                string dStrong = sn.HasDStrongs? cbDStrong.SelectedItem.ToString() : string.Empty;
                long strongId = sn.GetStrongID(dStrong);

            string cmdText = string.Empty;
            bool result = true;

            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();

                NpgsqlCommand command = dataSource.CreateCommand();

                command.CommandText =
                   "SELECT  " +
                   "strongs_number,d_strong,original_word,english_translation,long_text,short_text,transliteration " +
                   "FROM public.\"strongs_numbers\" " +
                   string.Format(" WHERE strong_id={0};",
                       strongId);

                NpgsqlDataReader reader = command.ExecuteReader();

                tbOriginal.Text = string.Empty;
                tbEnglish.Text = string.Empty;
                tbLongText.Text = string.Empty;

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
                            }

                        }
                        else
                        {
                            tbOriginal.Text = (string)reader[2];
                            tbEnglish.Text = (string)reader[3];
                            tbLongText.Text = (string)reader[4];
                            //tbShortText.Text = (string)reader[5];
                        }
                    }

                    if (trace)
                    {
                        //Trace(string.Format("{0}", strongsNumber), Color.Black);
                        //Trace(string.Format("Hebrew:\t{0}", tbOriginal.Text), Color.Black);
                        //Trace(string.Format("English:\t{0}", tbEnglish.Text), Color.Black);
                        //Trace(string.Format("Long Text:\t{0}", tbLongText.Text), Color.Black);
                    }

                }
                command.Dispose();

                reader.Close();
                reader.DisposeAsync();
                command.Dispose();
                connection.Close();

                Properties.Settings.Default.LastStrongsLanguage = cbLanguage.SelectedIndex;
                Properties.Settings.Default.LastDStrongs = cbDStrong.Text;
                Properties.Settings.Default.LastStrongs = strongsNumberControl.Value;
                Properties.Settings.Default.Save();


                StrongEventArgs e = new StrongEventArgs(strongId, dStrong, strongsNumber);
                FireStrongsChanged(e);
            }
            catch (Exception ex)
            {
                //Trace("GetStrongsEntry Exception\r\n" + ex.ToString(), Color.Red);
                result = false;
            }

        }

        private void btnPreviousStrong_Click(object sender, EventArgs e)
        {
            GotoPreviousStrong();
        }

        private void btnNextStrong_Click(object sender, EventArgs e)
        {
            GotoNextStrong();

        }
        private void GotoNextStrong()
        {
            ChangeStrongs(true);
        }

        private void GotoPreviousStrong()
        {
            ChangeStrongs(false);
        }
        private void ChangeStrongs(bool next)
        {
            int current = strongsNumberControl.Value;
            bool moveToNext = false;

            if (!StrongsNumbers.Instance.IsValid(cbLanguage.SelectedIndex == 0, current))
            {
                moveToNext = true;
            }
            else if (!cbDStrong.Visible)
            {
                moveToNext = true;
            }
            else
            {
                if (next)
                {
                    if (cbDStrong.SelectedIndex == cbDStrong.Items.Count - 1)
                    {
                        // last d, got to next number
                        moveToNext = true;
                    }
                    else
                        cbDStrong.SelectedIndex++;
                }
                else
                {
                    if (cbDStrong.SelectedIndex == 0)
                    {
                        // first d, got to previous number
                        moveToNext = true;
                    }
                    else
                        cbDStrong.SelectedIndex--;
                }
            }

            if (moveToNext)
                MoveToNewStrongNumber(next);
            else
            {
                LoadLexicon(false, null);
            }
        }

        private void MoveToNewStrongNumber(bool next)
        {
            if (next)
            {
                strongsNumberControl.Increment();
            }
            else
            {
                if (strongsNumberControl.Value == 1)
                {
                    strongsNumberControl.Value = cbLanguage.SelectedIndex == 1 ? lastG : lastH;
                }
                else
                    strongsNumberControl.Decrement();
            }

            int current = strongsNumberControl.Value;
            bool hebrew = (cbLanguage.SelectedIndex == 0);
            while (!StrongsNumbers.Instance.IsValid(hebrew, current))
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
                current = strongsNumberControl.Value;
            }


            StrongsNumber sn = StrongsNumbers.Instance.GetStrongNumber(hebrew, current);
            List<string> chars = sn.DStrongs;

            if (sn.HasDStrongs)
            {
                cbDStrong.Items.Clear();
                cbDStrong.Items.AddRange(chars.ToArray());
                cbDStrong.SelectedIndex = next ? 0 : cbDStrong.Items.Count - 1;
                cbDStrong.Visible = true;
            }
            else
                cbDStrong.Visible = false;

            LoadLexicon(false, null);

        }

        private void StrongsNumberControl_KeyDown(object? sender, KeyEventArgs e)
        {
            int current = strongsNumberControl.Value;
            try
            {
                bool hebrew = cbLanguage.SelectedIndex == 0;
                if (!StrongsNumbers.Instance.IsValid(hebrew, current))
                {
                    MoveToNewStrongNumber(current >= 0);
                }

                StrongsNumber sn = StrongsNumbers.Instance.GetStrongNumber(hebrew, current);
                if (sn.HasDStrongs)
                {
                    cbDStrong.Visible = false;
                }
                else
                {
                    cbDStrong.Items.Clear();
                    cbDStrong.Items.AddRange(sn.DStrongs.ToArray());
                    cbDStrong.SelectedIndex = 0;
                    cbDStrong.Visible = true;
                }

                LoadLexicon(false, null);
            }
            catch (Exception ex)
            {
                //Trace("GetStrongsEntry Exception\r\n" + ex.ToString(), Color.Red);
            }
        }

        private void CbDStrong_SelectedIndexChanged(object? sender, EventArgs e)
        {
            LoadLexicon(false, null);
        }

        private void CbLanguage_SelectedIndexChanged(object? sender, EventArgs e)
        {
            LoadLexicon(false, null);
        }

        public void FireStrongsChanged(StrongEventArgs e)
        {
            if (this.StrongsChanged != null)
            {
                this.StrongsChanged(this, e);
            }
        }


    }


}






