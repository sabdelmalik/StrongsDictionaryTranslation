using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using Excel = Microsoft.Office.Interop.Excel;

namespace PopulateStrongsDictionary
{
    internal class DictionarySpreadSheets
    {
        private MainForm mainForm;
        public DictionarySpreadSheets(MainForm mf)
        {
            mainForm = mf;
        }


        public void LoadSourceSpreadsheetsIntoDB(string sourceSpreadsheetsFolder, NpgsqlDataSource dataSource)
        {
            try
            {
                var command = dataSource.CreateCommand();

                string tableName = "strongs_numbers";

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
                    mainForm.Trace("Table '" + tableName + "' does not exist!", Color.Red);
                    return;
                }

                command.CommandText = "DELETE FROM public.\"" + tableName + "\";";
                command.ExecuteNonQuery();

                Excel.Application xlApp = new Excel.Application();

                string[] sheets = Directory.GetFiles(sourceSpreadsheetsFolder); // @"C:\Users\samim\Documents\MyProjects\Lexicon\Spreadsheets");
                foreach (string sheet in sheets)
                {
                    string name = Path.GetFileName(sheet);
                    if (!name.ToLower().StartsWith("strong"))
                        continue;
                    bool result = CopySourceSpreadsheetToDb(tableName, xlApp, sheet, command);
                    if (!result)
                        break;
                }
                xlApp.Quit();
                mainForm.Trace("Load Spreadsheets Done!", Color.Green);

            }
            catch (Exception ex)
            {
                mainForm.Trace("LoadSpreadsheetsIntoDB Exception\r\n" + ex.ToString(), Color.Red);
                return;
            }

        }

        private bool CopySourceSpreadsheetToDb (string tableName,Excel.Application xlApp, string sheet, NpgsqlCommand? command)
        {
            mainForm.Trace("Processing => " + sheet, Color.Blue);
            bool result = true;

            string strongsNumber = string.Empty;
            string dStrong = string.Empty;
            string originalWord = string.Empty;
            string englishWord = string.Empty;
            string longText = string.Empty;
            string shortText = string.Empty;

            string cmdText = string.Empty;

            Excel.Workbook xlWorkbook = null;
            try
            {
                xlWorkbook = xlApp.Workbooks.Open(sheet);
                Excel._Worksheet xlWorksheet = (Excel._Worksheet)xlWorkbook.Sheets[1];
                Excel.Range xlRange = xlWorksheet.UsedRange;

                dynamic[,] excelData = xlRange.Value2;
                int rowCount = excelData.GetUpperBound(0);
                int columnCount = excelData.GetUpperBound(1);

                for (int i = 0; i < rowCount / 3; i++)
                {
                    strongsNumber = string.Empty;
                    dStrong = string.Empty;
                    originalWord = string.Empty;
                    englishWord = string.Empty;
                    shortText = string.Empty;

                    for (int r = 1; r <= 3; r++)
                    {
                        int row = i * 3 + r;
                        for (int column = 1; column <= 3; column++)
                        {
                            string cellText = string.Empty;
                            if (excelData[row, column] != null)
                            {
                                cellText = excelData[row, column].ToString().Trim();
                            }
                            switch (r)
                            {
                                case 1:
                                    if (column == 1)
                                    {
                                        if (cellText.Contains("="))
                                        {
                                            string[] parts = cellText.Split('=');
                                            if (parts.Length == 2)
                                            {
                                                string  sn = parts[0].Trim();
                                                strongsNumber = sn;
                                                if (sn.Length > 5)
                                                {
                                                    strongsNumber = sn.Substring(0, 5);
                                                    dStrong = sn.Substring(5);
                                                    //mainForm.Trace(string.Format("{0} reduced to {1}", sn, strongsNumber), Color.Brown);
                                                }
                                                else if (sn.Length < 5)
                                                {
                                                    mainForm.Trace(string.Format("{0} is too short", sn), Color.Brown);
                                                }                                                
                                                originalWord = parts[1].Trim();
                                            }
                                        }
                                    }
                                    else if (column == 2)
                                    {
                                        longText = cellText;
                                    }
                                    break;
                                case 2:
                                    if (column == 2)
                                    {
                                        englishWord = cellText;
                                    }
                                    else if (column == 3)
                                    {
                                        shortText = cellText;
                                    }
                                    break;
                            }
                        }
                    }
                    cmdText = "INSERT INTO public.\"" + tableName + "\" " +
                       "(strongs_number, d_strong, original_word, english_translation, long_text, short_text) " + //, translated_word, translated_short_text) " +
                       "VALUES " +
                       string.Format("('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');", //, '{6}', '{7}');",
                       strongsNumber.Replace("'", "''"),
                       dStrong.Replace("'", "''"),
                       originalWord.Replace("'", "''"),
                       englishWord.Replace("'", "''"),
                       longText.Replace("'", "''"),
                       shortText.Replace("'", "''")
                       );

                    command.CommandText = cmdText;
                        if (strongsNumber != string.Empty)
                            command.ExecuteNonQuery();
                }

                xlWorkbook.Close();
            }
            catch (Exception ex)
            {
                if (xlWorkbook != null)
                    xlWorkbook.Close();

                result = false;
                mainForm.Trace(cmdText, Color.Red);
                mainForm.Trace(ex.ToString(), Color.Red);
            }

            return result;
        }

        public void LoadTranslatedSpreadsheetsIntoDB(int langId, string translatedSpreadsheetsFolder, NpgsqlDataSource dataSource)
        {
            try
            {
                var command = dataSource.CreateCommand();

                string tableName = "dictionary_translation";

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
                    mainForm.Trace("Table '" + tableName + "' does not exist!", Color.Red);
                    return;
                }

                Excel.Application xlApp = new Excel.Application();

                string[] sheets = Directory.GetFiles(translatedSpreadsheetsFolder);
                foreach (string sheet in sheets)
                {
                    string name = Path.GetFileName(sheet);
                    if (!name.ToLower().StartsWith("strong"))
                        continue;
                    bool result = CopyTranslatedSpreadsheetToDb(langId, tableName, xlApp, sheet, command);
                    if (!result)
                        break;
                }
                xlApp.Quit();
                mainForm.Trace("Update Spreadsheets Done!", Color.Green);
            }
            catch (Exception ex)
            {
                mainForm.Trace("LoadSpreadsheetsIntoDB Exception\r\n" + ex.ToString(), Color.Red);
                return;
            }

        }


        private bool CopyTranslatedSpreadsheetToDb(int langId, string tableName, Excel.Application xlApp, string sheet, NpgsqlCommand? command)
        {
            mainForm.Trace("Processing => " + sheet, Color.Blue);

            bool result = true;

            string strongsNumber = string.Empty;
            string dStrong = string.Empty;
            string originalWord = string.Empty;
            string englishWord = string.Empty;
            string longText = string.Empty;
            string shortText = string.Empty;
            string translatedWord = string.Empty;
            string translatedText = string.Empty;

            string cmdText = string.Empty;

            Excel.Workbook xlWorkbook = null;
            try
            {
                xlWorkbook = xlApp.Workbooks.Open(sheet);
                Excel._Worksheet xlWorksheet = (Excel._Worksheet)xlWorkbook.Sheets[1];
                Excel.Range xlRange = xlWorksheet.UsedRange;

                dynamic[,] excelData = xlRange.Value2;
                int rowCount = excelData.GetUpperBound(0);
                int columnCount = excelData.GetUpperBound(1);

                for (int i = 0; i < rowCount / 3; i++)
                {
                    strongsNumber = string.Empty;
                    dStrong = string.Empty;
                    originalWord = string.Empty;
                    englishWord = string.Empty;
                    shortText = string.Empty;
                    translatedWord = string.Empty;
                    translatedText = string.Empty;

                    for (int r = 1; r <= 3; r++)
                    {
                        int row = i * 3 + r;
                        for (int column = 1; column <= 3; column++)
                        {
                            string cellText = string.Empty;
                            if (excelData[row, column] != null)
                            {
                                cellText = excelData[row, column].ToString().Trim();
                            }
                            switch (r)
                            {
                                case 1:
                                    if (column == 1)
                                    {
                                        if (cellText.Contains("="))
                                        {
                                            string[] parts = cellText.Split('=');
                                            if (parts.Length == 2)
                                            {
                                                string sn = parts[0].Trim();
                                                strongsNumber = sn;
                                                if (sn.Length > 5)
                                                {
                                                    strongsNumber = sn.Substring(0, 5);
                                                    dStrong = sn.Substring(5);
                                                    //mainForm.Trace(string.Format("{0} reduced to {1}", sn, strongsNumber), Color.Brown);
                                                }
                                                else if (sn.Length < 5)
                                                {
                                                    mainForm.Trace(string.Format("{0} is too short", sn), Color.Brown);
                                                }
                                                originalWord = parts[1].Trim();
                                            }
                                        }
                                    }
                                    else if (column == 2)
                                    {
                                        longText = cellText;
                                    }
                                    break;
                                case 2:
                                    if (column == 2)
                                    {
                                        englishWord = cellText;
                                    }
                                    else if (column == 3)
                                    {
                                        shortText = cellText;
                                    }
                                    break;
                                case 3:
                                    if (column == 2)
                                    {
                                        translatedWord = cellText;
                                    }
                                    else if (column == 3)
                                    {
                                        translatedText = cellText;
                                    }

                                    break;
                            }
                        }
                    }

                    cmdText = "INSERT INTO public.\"" + tableName + "\" " +
                           "(language_id, strongs_number, d_strong, translated_word, translated_long_text, translated_short_text) " + //, translated_word, translated_short_text) " +
                           "VALUES " +
                           string.Format("({0}, '{1}', '{2}', '{3}', '{4}', '{5}');", //, '{6}', '{7}');",
                           langId,
                           strongsNumber.Replace("'", "''"),
                           dStrong.Replace("'", "''"),
                           englishWord.Replace("'", "''"),
                           longText.Replace("'", "''"),
                           shortText.Replace("'", "''")
                           );

                    int d = 0;
                    if(dStrong.Length > 0) d = (int)dStrong[0];
                    if (dStrong.Length > 1 || d > 128 || strongsNumber == string.Empty)
                    {
                        if(strongsNumber != string.Empty)
                            mainForm.Trace(cmdText, Color.Red);
                    }
                    else
                    {
                        command.CommandText = cmdText;
                        command.ExecuteNonQuery();
                    }
                }

                xlWorkbook.Close();
            }
            catch (Exception ex)
            {
                if (xlWorkbook != null)
                    xlWorkbook.Close();

                result = false;
                mainForm.Trace(cmdText, Color.Red);
                mainForm.Trace(ex.ToString(), Color.Red);
            }
            return result;
        }

    }
}
