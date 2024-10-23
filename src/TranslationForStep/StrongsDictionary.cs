using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Npgsql;

using DictionaryEditorV2.Lexicon;
using DictionaryEditorV2;
using System.Reflection;
using DictionaryEditorV2.Editor;

namespace TranslationForStep
{
    internal class StrongsDictionary
    {
        private string dbName = "StrongsDictionaryV2";
        private NpgsqlDataSource dataSource;
        private NpgsqlConnection connection;


        private SortedDictionary<string, long> dStrongs = new SortedDictionary<string, long>();

        private MainForm mainForm;
        public StrongsDictionary(MainForm main)
        {
            mainForm = main;
            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1Na61A51;Database=" + dbName;
            dataSource = NpgsqlDataSource.Create(connectionString);
            connection = dataSource.CreateConnection();
        }

        public StrongsNumber GetStrongs(string strongs)
        {
            bool hebrew = strongs[0] == 'H' ? true : false;
            int strongsValue = 0;
            bool goodStrongs = int.TryParse(strongs.Substring(1, 4), out strongsValue);
            StrongsNumber sn = null;
            if (goodStrongs)
                sn = StrongsNumbers.Instance.GetStrongNumber(hebrew, strongsValue);
            else
                mainForm.TraceError(MethodBase.GetCurrentMethod().Name, string.Format("[{0}] bad format", strongs));

            return sn;
        }

        public List<TranslationEntry> GetTranslationEntries(long strongId)
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
                   string.Format(" WHERE xlt.strong_id={0}", strongId) +
                   " ORDER BY xlt.update_date ASC;";

            NpgsqlDataReader reader = null;
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();

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

                        translationEntries.Add(new TranslationEntry(strong_id, sheet_name, update_date, updater_name,
                            updater_email, status, translated_gloss, translated_text, transliteration));
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
                mainForm.TraceError(MethodBase.GetCurrentMethod().Name, ex.Message);

                //TraceError(cmdText);
                //TraceError(ex.Message);
            }



            return translationEntries;
        }

    }
}
