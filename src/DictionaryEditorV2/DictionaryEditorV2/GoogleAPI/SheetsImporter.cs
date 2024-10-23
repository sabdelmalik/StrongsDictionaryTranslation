using Google.Apis.Sheets.v4.Data;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DictionaryEditorV2.GoogleAPI
{
    internal class SheetsImporter
    {
        private NpgsqlDataSource dataSource;
        private NpgsqlConnection connection;
        private string dbName = Properties.Settings.Default.Database;

        private static SheetsImporter instance = null;
        private static readonly object lockObj = new object();

        public static SheetsImporter Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = new SheetsImporter();
                        }
                    }
                }
                return instance;
            }
        }

        private SheetsImporter()
        {
            Initialise();
        }


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



        private void Initialise()
        {
            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1Na61A51;Database=" + dbName;

            try
            {
                dataSource = NpgsqlDataSource.Create(connectionString);
                connection = dataSource.CreateConnection();
            }
            catch (Exception ex)
            {
                TraceError(ex.Message);
            }
        }

        public void UpdateTranslation(SheetStatus sheetStatus, string? strongs, string? gloss, string? text, CloudSheetDef cloudSheetDef)
        {
            // fetch associated translation
            // if status, gloss or text is different, we should INSERT
            // otherwise ignore

            string strongs_number = strongs;
            string d_strong = "";
            if (strongs.Length > 5)
            {
                strongs_number = strongs.Substring(0, 5);
                d_strong = strongs.Substring(5, 1);
            }

            string cmdText =
               "SELECT " +
               "strongs.strong_id," +
               "xlt.sheet_id," +
               "xlt.status_id," +
               "xlt.translated_gloss," +
               "xlt.translated_text," +
               "xlt.transliteration" +
               " FROM public.\"strongs_numbers\" strongs" +
               " INNER JOIN public.\"translation\" xlt" +
               " ON strongs.strong_id = xlt.strong_id" +
               string.Format(" WHERE strongs.strongs_number='{0}'", strongs_number) +
               string.Format(" AND strongs.d_strong='{0}' ORDER BY xlt.update_date ASC;", d_strong);

            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();

            NpgsqlDataReader reader = null;
            var command = dataSource.CreateCommand();
            try
            {
                
                command.CommandText = cmdText;
                reader = command.ExecuteReader();
            }
            catch (Exception ex)
            {
                TraceError(cmdText);
                TraceError(ex.Message);
                return;
            }

            long strong_id = -1;
            int sheet_id = -1;
            string transliteration = string.Empty;
            SheetStatus dbSheetStatus = SheetStatus.Pending;
            string dbGloss = string.Empty;
            string dbText = string.Empty;
            string dbTtransliteration = string.Empty;

            try
            {
                if (reader != null)
                {
                    string lastStrongs = string.Empty;

                    while (reader.Read())
                    {
                        strong_id = reader.GetInt64(0);
                        sheet_id = reader.GetInt32(1);
                        dbSheetStatus = (SheetStatus)reader.GetInt32(2);
                        dbGloss = reader.GetString(3);
                        dbText = reader.GetString(4);
                        dbTtransliteration = reader.GetString(5);
                    }
                }



                if (sheetStatus != dbSheetStatus || gloss != dbGloss || text != dbText)
                {
                    string xltTable = "translation";

                    string tgtCmdText = string.Format(
                    "INSERT INTO public.\"{0}\"" +
                    "(strong_id,sheet_id,update_date,updater_name,updater_email,status_id,translated_gloss,translated_text,transliteration)" +
                    " VALUES ({1},{2},'{3}','{4}','{5}',{6},'{7}','{8}','{9}');",
                    xltTable,
                    strong_id,
                    sheet_id,
                    cloudSheetDef.ModifiedTimestamp,
                    cloudSheetDef.LastModifyingUserName,
                    cloudSheetDef.LastModifyingUserEmail,
                    (int)sheetStatus,
                    gloss.Replace("'", "''"),
                    text.Replace("'", "''"),
                    transliteration.Replace("'", "''")
                    );

                    var tgtCommand = dataSource.CreateCommand();

                    tgtCommand.CommandText = tgtCmdText;
                    tgtCommand.ExecuteNonQuery();

                }

                command.Dispose();

                reader.Close();
                reader.DisposeAsync();
                command.Dispose();
                connection.Close();


            }
            catch (Exception ex)
            {
                TraceError(cmdText);
                TraceError(ex.Message);
            }
        }

    }
}
