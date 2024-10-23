using Google.Protobuf.Collections;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using Npgsql;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;
using System.Xml;
using System.Xml.Linq;
using static Npgsql.Replication.PgOutput.Messages.RelationMessage;

namespace Postgresql2MySql
{
    public partial class MainForm : Form
    {
        MySqlConnection mysqlConnection;
        private NpgsqlDataSource dataSource;
        private NpgsqlConnection pgConnection;

        string mysqlConnectionString = "server=127.0.0.1;uid=root;pwd=1Na61A51;database=StrongsDictionary";
        string pgConnectionString = "Host=localhost;Port=5432;Username=postgres;Password=1Na61A51;Database=StrongsDictionary; Include Error Detail=True";
        public MainForm()
        {
            InitializeComponent();
        }

        #region Trace
        delegate void TraceDelegate(string text, Color color);
        delegate void ClearTraceDelegate();

        private void Trace(string text, Color color,
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

        private void Form1_Load(object sender, EventArgs e)
        {

            try
            {
                dataSource = NpgsqlDataSource.Create(pgConnectionString);
                pgConnection = dataSource.OpenConnection();
            }
            catch (Exception ex)
            {
                TraceError(ex.Message);
            }

            try
            {
                mysqlConnection = new MySqlConnection();
                mysqlConnection.ConnectionString = mysqlConnectionString;
                mysqlConnection.Open();
            }
            catch (Exception ex)
            {
                TraceError(ex.Message);
            }

            DropTables();
            CreateTables();
            CopyTables();

            if (mysqlConnection != null)
                mysqlConnection.Close();
            if (pgConnection != null)
                pgConnection.Close();

        }

        private void DropTables()
        {
            string[] tablseNames = {
                // order is important because of forign key constraints
                "bible_text",
                "dictionary_translation",
                "language",
                "book",
                "updated_translation",
                "strongs_numbers CASCADE",
                "book CASCADE",
                "bible_words_references CASCADE",
                "strongs_references"
                };

            for (int i = 0; i < tablseNames.Length; i++)
            {
                DropTable(tablseNames[i]);
            }

        }

        private void DropTable(string table)
        {
            try
            {
                using var cmd = new MySqlCommand();
                cmd.Connection = mysqlConnection;

                cmd.CommandText = "DROP TABLE IF EXISTS " + table;
                cmd.ExecuteNonQuery();

                cmd.Dispose();
            }
            catch (Exception ex)
            {
                TraceError(ex.Message);
            }
        }

        private void CreateTables()
        {
            foreach (string tableSql in TableDefs.Defs)
            {
                CreateTable(tableSql);
            }

        }

        private void CreateTable(string creationString)
        {
            try
            {
                using var cmd = new MySqlCommand();
                cmd.Connection = mysqlConnection;

                cmd.CommandText = creationString;
                cmd.ExecuteNonQuery();

                cmd.Dispose();
            }
            catch (Exception ex)
            {
                TraceError(ex.Message);
            }
        }


        private void CopyTables()
        {
            string tablename;
            string[] columns;

            tablename = "language";
            columns = new string[] { "id", "name", "iso_639_1", "iso_639_2" };
            CopyTable(tablename, columns);

            tablename = "book";
            columns = new string[] { "id", "usfm_name", "osis_name", "full_name" };
            CopyTable(tablename, columns);

            tablename = "strongs_numbers";
            columns = new string[] {
                    "strongs_number",
                    "d_strong",
                    "original_word",
                    "english_translation",
                    "long_text",
                    "short_text",
                    "step_united_reason",
                    "step_type",
                    "transliteration",
                    "pronunciation"
                };
            CopyTable(tablename, columns);

            tablename = "dictionary_translation";
            columns = new string[] {"language_id", "strongs_number", "d_strong",
                                    "transliteration", "translated_word", "translated_long_text",
                                    "translated_short_text", "reviewed", "reviewer_initials",
                                    "approved", "approver_initials"};
            CopyTable(tablename, columns);

            tablename = "bible_text";
            columns = new string[] { "language_id", "book_id", "chapter_num", "verse_num", "verse_text" };
            CopyTable(tablename, columns);

            tablename = "bible_words_references";
            columns = new string[] { "language_id", "strongs_number", "d_strong", "word", "reference" };
            CopyTable(tablename, columns);

            tablename = "strongs_references";
            columns = new string[] { "strongs_number", "d_strong", "reference" };
            CopyTable(tablename, columns);

            tablename = "updated_translation";
            columns = new string[] { 
                "language_id",
                "strongs_number",
                "d_strong",
                "transliteration",
                "translated_word",
                "translated_text",
                "reviewer_initials",
                "approved",
                "approver_initials"
            };
            CopyTable(tablename, columns);
        }

        private void CopyTable(string table, string[] columns)
        {
            Trace("Copying Table " + table, Color.Blue);
            string cNames = String.Join(",", columns);

            string cmdText = string.Format("SELECT {0} FROM public.\"{1}\";", cNames.Replace("reference", "\"references\""), table);
            NpgsqlDataReader reader = null;
            try
            {

                var command = dataSource.CreateCommand();
                command.CommandText = cmdText;
                reader = command.ExecuteReader();
            }
            catch (Exception ex)
            {
                TraceError(cmdText);
                TraceError(ex.Message);
                return;
            }
            if (reader != null)
                while (reader.Read())
                {
                    // get row from postgresql
                    string vals = string.Empty;
                    for (int i = 0; i < columns.Length; i++)
                    {
                        var val = reader[i];
                        if (val is int)
                        {
                            vals += val.ToString() + ",";
                        }
                        else if (val is bool)
                        {
                            vals += ((bool)val) ? "true," : "false,";
                        }
                        else
                        {
                            if (val.ToString().Contains("οὐρανός , -οῦ,"))
                            {
                                int x = 0;
                            }
                            vals += ("'" + 
                                val.ToString()
                                .Replace("'","\\'") + 
                                "',");
                        }
                    }
                    vals = vals.Trim(',');
                    // inser row into sql
                    string insert = string.Format("INSERT INTO {0}({1}) VALUES({2});", table, cNames, vals);
                    try
                    {
                        MySqlCommand sqlCommand = new MySqlCommand();
                        sqlCommand.CommandText = insert;
                        sqlCommand.Connection = mysqlConnection;
                        MySqlDataReader MyReader = sqlCommand.ExecuteReader();     // Here our query will be executed and data saved into the database.
                        while (MyReader.Read())
                        {
                        }
                        MyReader.DisposeAsync();
                    }
                    catch (Exception ex)
                    {
                        TraceError(insert);
                        TraceError(ex.Message);
                        return;
                    }
                }

        }


        }
    }