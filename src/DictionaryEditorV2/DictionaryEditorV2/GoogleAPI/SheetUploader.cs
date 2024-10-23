/*
 * Sheets API has per-minute quotas, and they're refilled every minute.
 * For example, there's a read request limit of 300 per minute per project.
 * If your app sends 350 requests in one minute, the additional 50 requests exceed
 * the quota and generates a 429: Too many requests HTTP status code response.
 * 
 * Quotas
 * Read requests	Per minute per project	            300
 *                  Per minute per user per project	    60
 * Write requests   Per minute per project	            300
 *                  Per minute per user per project	    60
 * 
 * to speed up requests, Google recommends a 2-MB maximum payload.
 * ================================================================
*/using Google.Apis.Sheets.v4.Data;
using Google.Apis.Sheets.v4;
using Microsoft.VisualBasic.ApplicationServices;
using Npgsql;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace DictionaryEditorV2.GoogleAPI
{
    internal class SheetUploader
    {
        private NpgsqlDataSource dataSource;
        private NpgsqlConnection pgConnection;
        private string dbName = Properties.Settings.Default.Database;

        //        string sheetsCredentialFileName = @"C:\Users\samim\Documents\MyProjects\Lexicon\programs\GoogleCloude\civic-genius-405917-75f68833ff58.json";
        //string sheetId = "1uMjj8v_x2KTHhqPiiqIuclMKOQFxHynerhASXzpmec8";
        //        static string applicationName = "LexiconTranslation";
        // gcp-billing-admins@bible-tools.com
        // sami.abdelmalik@bible-tools.com

        private static SheetUploader instance = null;
        private static readonly object lockObj = new object();

        public static SheetUploader Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = new SheetUploader();
                        }
                    }
                }
                return instance;
            }
        }

        private SheetUploader()
        {
            Initialise();
        }


        #region Trace
        private void Trace(string text, System.Drawing.Color color,
        [CallerLineNumber] int lineNumber = 0,
        [CallerMemberName] string caller = null)
        {
            Tracing.TraceInfo(string.Format("{0}#{1}", caller == null ? "???" : caller, lineNumber),text, color);
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
                pgConnection = dataSource.CreateConnection();
            }
            catch (Exception ex)
            {
                TraceError(ex.Message);
            }
        }

        public int PopulateSheet(CloudSheetDef cloudSheetDef)
        {
            // string sheetId = "1uMjj8v_x2KTHhqPiiqIuclMKOQFxHynerhASXzpmec8";


            // Format sheet
            UserEnteredFormat? userEnteredFormat = SheetsUtils.Instance.GetCellFormat(verticalAlignment: SheetVerticalAlignment.TOP, wrapStrategy: SheetWrapStrategy.WRAP);
            if (userEnteredFormat != null)
            {
                SheetsUtils.Instance.SetCellFormat(cloudSheetDef.SheetId, userEnteredFormat, startRow: 0);
            }

            return CopyStrongsTable(cloudSheetDef.SheetId, cloudSheetDef.Hebrew, cloudSheetDef.StartStrong, cloudSheetDef.LastStrong);
        }

         private int CopyStrongsTable(string sheetId, bool hebrew, int startS, int endS)
        {
            int length = 0;

            UserEnteredFormat? userEnteredFormat = SheetsUtils.Instance.GetCellFormat(backgroundColor: System.Drawing.Color.LightGreen);

            string table1 = "strongs_numbers";
            //Trace("Copying Table " + table, System.Drawing.Color.Blue);

            string cmdText =
               "SELECT " +
               "strongs.strongs_number," +
               "strongs.d_strong," +
               "strongs.original_word," +
               "strongs.english_translation," +
               "strongs.long_text," +
               "xlt.status_id," +
               "xlt.translated_gloss," +
               "xlt.translated_text," +
               "xlt.update_date" +
               " FROM public.\"strongs_numbers\" strongs" +
               " INNER JOIN public.\"translation\" xlt" +
               " ON strongs.strong_id = xlt.strong_id" +
               string.Format(" WHERE strongs.strongs_number LIKE '{0}'", hebrew ? "H%" : "G%") +
               string.Format(" AND substring(strongs.strongs_number,2,4)::INTEGER BETWEEN {0} AND {1} ", startS, endS) +
               " ORDER BY strongs.strongs_number ASC,strongs.d_strong ASC,xlt.update_date ASC;";

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
                return length;
            }
            int rowCounter = 0;
            List<List<object>> rows = new List<List<object>>();
            //string strongEnd = "H0999";
            string currentStrong = string.Empty;


            if (reader != null)
            {
                string lastStrongs = string.Empty;

                while (reader.Read())
                {
                    //// get row from postgresql for this sheet
                    string? strongs_number = reader.GetString(0);
                    string? d_strong = reader.GetString(1);
                    string? original_word = reader.GetString(2);
                    string? english_translation = reader.GetString(3);
                    string? long_text = reader.GetString(4);
                    int statusId = reader.GetInt32(5);
                    string? translated_word = reader.GetString(6);
                    string? translated_long_text = reader.GetString(7);

                    if (strongs_number != null && d_strong != null && original_word != null &&
                        english_translation != null && long_text != null &&
                        translated_word != null && translated_long_text != null)
                    {

                        // Each entry in the table is two rows
                        // the first is the lexicon data
                        // the second is the translation
                        currentStrong = (strongs_number + d_strong).Trim();
                        List<object> row1 = new List<object>();
                        row1.Add(currentStrong);
                        row1.Add(original_word);
                        row1.Add(english_translation);
                        row1.Add(long_text);

                        List<object> row2 = new List<object>();
                        row2.Add((SheetStatus)statusId);
                        row2.Add("");
                        row2.Add(translated_word);
                        row2.Add(translated_long_text.Replace("\u200B", " "));

                        if (currentStrong == lastStrongs)
                        {
                            // this is a newer translation for the same strong's
                            // replace
                            rows[rows.Count - 2] = row1;
                            rows[rows.Count - 1] = row2;
                        }
                        else
                        {
                            rows.Add(row1);
                            rows.Add(row2);
                        }

                        lastStrongs = currentStrong;

                        length += currentStrong.Length;
                        length += original_word.Length;
                        length += english_translation.Length;
                        length += long_text.Length;
                        length += translated_word.Length;
                        length += translated_long_text.Length;

                        //sheetsUtils.UpdateRowData(sheetId, list, rowIndex: rowCounter, columnIndex: 0);
                        //if (userEnteredFormat != null)
                        //{
                        //    sheetsUtils.SetCellFormat(sheetId, userEnteredFormat, startRow: rowCounter, endRow: rowCounter+1);
                        //}

                        rowCounter += 2;

                        //if (rowCounter > 10) { break; }
                    }
                    else
                    {
                        TraceError(string.Format("strongs_number={0}, d_strong={1}, original_word={2}, english_translation={3}, long_text={4}",
                            strongs_number, d_strong, original_word, english_translation, long_text));
                    }

                }
                SheetsUtils.Instance.UpdateRowData(sheetId, rows, rowIndex: 0, columnIndex: 0);
            }

            return length;
        }

    }

}
