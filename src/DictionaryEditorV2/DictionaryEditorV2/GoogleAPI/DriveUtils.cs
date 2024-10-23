

using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using SheetData = Google.Apis.Sheets.v4.Data;

using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using DriveData = Google.Apis.Drive.v3.Data;

using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Npgsql;
using Microsoft.VisualBasic.ApplicationServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using DictionaryEditorV2.Lexicon;

namespace DictionaryEditorV2.GoogleAPI
{
    internal class DriveUtils
    {
        private DriveService? driveService = null;

        string credentialFileName = Properties.Settings.Default.PostgresqlConfig;
        string applicationName = Properties.Settings.Default.DriveCredentialFileName;

        private static DriveUtils? instance = null;
        private static readonly object lockObj = new object();

        string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1Na61A51;Database=" + Properties.Settings.Default.Database;

        public static DriveUtils Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = new DriveUtils();
                        }
                    }
                }
                return instance;
            }
        }

        private DriveUtils()
        {
        }

        #region Trace
        private void Trace(string text, System.Drawing.Color color,
        [CallerLineNumber] int lineNumber = 0,
        [CallerMemberName] string? caller = null)
        {
            Tracing.TraceInfo(string.Format("{0}#{1}", caller == null ? "???" : caller, lineNumber), text);
        }
        private void TraceError(
                string text,
                [CallerLineNumber] int lineNumber = 0,
                [CallerMemberName] string? caller = null)
        {
            Tracing.TraceError(string.Format("{0}#{1}", caller == null ? "???" : caller, lineNumber), text);
        }
        #endregion Trace

        #region Public Methods

        #region Initializers
        public DriveService? InitialiseDriveService()
        {
            try
            {
                string[] Scopes = { DriveService.Scope.Drive };
                var driveCred = GoogleCredential.FromStream(new FileStream(credentialFileName, FileMode.Open)).CreateScoped(Scopes);
                driveService = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = driveCred,
                    ApplicationName = applicationName,
                });
            }
            catch (Exception ex)
            {
                TraceError(ex.Message);
            }

            return driveService;
        }
        #endregion Initializers

        public IList<DriveData.File>? GetFileList(string fileProperties)
        {
            IList<DriveData.File>? files = null;
            if (driveService != null)
                try
                {
                    FilesResource.ListRequest listRequest = driveService.Files.List();
                    listRequest.Corpora = "user";
                    listRequest.OrderBy = "name";
                    listRequest.PageSize = 100;
                    listRequest.Fields = "nextPageToken, files("+ fileProperties +")";

                    // List files.
                    files = listRequest.Execute().Files;
                }
                catch (Exception ex)
                {
                    TraceError(ex.Message);
                }

            return files;
        }

        #endregion Public Methods



        public bool Export()
        {
            bool result = false;

            try
            {
                List<CloudSheetDef> sheetDefs = GetCloudSheets();
                if(sheetDefs.Count > 0)
                    result = PopulateDbWithSheetInfo(sheetDefs);
            }
            catch (Exception ex)
            {
                TraceError(ex.Message);
            }

            return result;
        }

        public List<CloudSheetDef> GetCloudSheets()
        {
            List<CloudSheetDef> result = new List<CloudSheetDef>();
            try
            {
                DriveService? service = InitialiseDriveService();
                IList<DriveData.File>? files = GetFileList("id, name,  mimeType, lastModifyingUser, modifiedTime");

                if (files != null)
                    foreach (DriveData.File file in files)
                    {
                        if (file.Name.ToLower() == "lexicon")
                            continue;
                        string id = file.Id;
                        string name = file.Name;
                        string mimeType = file.MimeType;
                        string lastModifyingUserName = file.LastModifyingUser.DisplayName;
                        string lastModifyingUserEmail = file.LastModifyingUser.EmailAddress;
                        DateTimeOffset? modifiedDateTimeOff = file.ModifiedTimeDateTimeOffset;
                        DateTime modifiedUtcDateTime = modifiedDateTimeOff.Value.UtcDateTime;
                        string modifiedUtcTimestamp = modifiedUtcDateTime.ToString("yyyy-MM-dd HH:mm:ss");

                        bool hebrew = false;
                        int startStrong = 0;
                        int lastStrong = 0;

                        string pattern = @"([HG])(\d\d\d\d)\s{0,5}\-\s{0,5}([HG])(\d\d\d\d)";
                        Match match = Regex.Match(name, pattern);
                        if (match != null)
                        {
                            string prefix1 = match.Groups[1].Value;
                            string start = match.Groups[2].Value;
                            string prefix2 = match.Groups[3].Value;
                            string last = match.Groups[4].Value;
                            if (prefix1 != prefix2)
                            {
                                TraceError(string.Format("Prefixes do not match for '{0}'", name));
                                continue;
                            }
                            hebrew = prefix1 == "H" ? true : false;
                            startStrong = int.Parse(start);
                            lastStrong = int.Parse(last);

                        }
                        else
                        {
                            TraceError(string.Format("Could not parse '{0}'", name));
                            continue;
                        }
                        result.Add(new CloudSheetDef(
                            id,
                            name,
                            lastModifyingUserName,
                            lastModifyingUserEmail,
                            modifiedUtcTimestamp,
                            mimeType, hebrew, startStrong, lastStrong));
                    }
            }
            catch (Exception ex)
            {
                TraceError(ex.Message);
            }

            return result;
        }


        private bool PopulateDbWithSheetInfo(List<CloudSheetDef> sheetDefs)
        {
            bool result = false;

            NpgsqlConnection? connection = null;
            NpgsqlDataSource? dataSource = null;

            string cmdText = string.Empty;

            try
            {
                dataSource = NpgsqlDataSource.Create(connectionString);
                connection = dataSource.CreateConnection();

                string tableName = "google_sheet";

                result = ClearTable(tableName, dataSource);
                if (result)
                {
                    var command = dataSource.CreateCommand();

                    // Key: starting strong number
                    // value: sheet definition

                    SortedDictionary<int, CloudSheetDef> dictG = new SortedDictionary<int, CloudSheetDef>();
                    SortedDictionary<int, CloudSheetDef> dictH = new SortedDictionary<int, CloudSheetDef>();

                    foreach (CloudSheetDef sheetDef in sheetDefs)
                    {
                        SortedDictionary<int, CloudSheetDef> dict;
                        if (sheetDef.Hebrew) dict = dictH; else dict = dictG;

                        dict.Add(sheetDef.StartStrong, sheetDef);
                    }
                    int[] indices = dictG.Keys.ToArray();
                    int i;

                    // Populate google_sheet table with the Greek sheets info
                    for (i = 0; i < indices.Length; i++)
                    {
                        CloudSheetDef sheetDef = dictG[indices[i]];
                        string startStrong = string.Empty;
                        string lastStrong = string.Empty;
                        string d = string.Empty;

                        List<string> dList = StrongsNumbers.Instance.GetDStrongNumber(false, sheetDef.LastStrong);
                        if (dList.Count > 1)
                            d += dList[dList.Count - 1];
                        startStrong = string.Format("G{0:d4}{1}", sheetDef.StartStrong, dList[0]);
                        lastStrong = string.Format("G{0:d4}{1}", sheetDef.LastStrong, d);


                        cmdText = "INSERT INTO public.\"" + tableName + "\" " +
                                  "(sheet_id, id, name, first_strong, last_strong, translation_language, last_updated) VALUES " +
                                   string.Format("({0}, '{1}', '{2}', '{3}', '{4}', 7, '{5}');",
                                   i + 1,
                                   sheetDef.SheetId,
                                   sheetDef.Name,
                                   startStrong,
                                   lastStrong,
                                   sheetDef.ModifiedTimestamp);

                        command.CommandText = cmdText;
                        command.ExecuteNonQuery();

                    }

                    // Populate google_sheet table with the Hebrew sheets info
                    indices = dictH.Keys.ToArray();
                    for (int j = 0; j < indices.Length; j++)
                    {
                        CloudSheetDef sheetDef = dictH[indices[j]];

                        string startStrong = string.Empty;
                        string lastStrong = string.Empty;

                        string d = string.Empty;

                        List<string> dList = StrongsNumbers.Instance.GetDStrongNumber(true, sheetDef.LastStrong);
                        if (dList.Count > 1)
                            d += dList[dList.Count - 1];
                        startStrong = string.Format("H{0:d4}{1}", sheetDef.StartStrong, dList[0]);
                        lastStrong = string.Format("H{0:d4}{1}", sheetDef.LastStrong, d);

                        cmdText = "INSERT INTO public.\"" + tableName + "\" " +
                                  "(sheet_id, id, name, first_strong, last_strong, translation_language, last_updated) VALUES " +
                                   string.Format("({0}, '{1}', '{2}', '{3}', '{4}', 7, '{5}');",
                                   j + i + 1,
                                   sheetDef.SheetId,
                                   sheetDef.Name,
                                   startStrong,
                                   lastStrong,
                                   sheetDef.ModifiedTimestamp);

                        command.CommandText = cmdText;
                        command.ExecuteNonQuery();

                    }
                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                TraceError(cmdText);
                TraceError(ex.ToString());
            }
            finally
            {
                if (connection != null)
                    connection.Close();
                if (dataSource != null)
                    dataSource.Dispose();
            }

            return result;
        }

        public Dictionary<string, DbSheetDef> GetSheetInfoFromDB()
        {
            Dictionary<string, DbSheetDef> sheetDbInfo = new Dictionary<string, DbSheetDef>();

            NpgsqlConnection? connection = null;
            NpgsqlDataSource? dataSource = null;
            string cmdText = string.Empty;

            try
            {
                dataSource = NpgsqlDataSource.Create(connectionString);
                connection = dataSource.CreateConnection();

                var command = dataSource.CreateCommand();

                cmdText = "SELECT" +
                          " id, name, first_strong, last_strong, last_updated" +
                          " FROM public.\"google_sheet\"" +
                          " WHERE translation_language = 7;";

                command.CommandText = cmdText;
                NpgsqlDataReader reader = command.ExecuteReader();

                // Output rows
                while (reader.Read())
                {
                    string id = reader.GetString(0);
                    string name = reader.GetString(1);
                    string first_strong = reader.GetString(2);
                    string last_strong = reader.GetString(3);
                    DateTime last_updated = reader.GetDateTime(4);
                    sheetDbInfo[name] = new DbSheetDef(id, name, first_strong, last_strong, last_updated);
                }
                reader.Close();
                reader.DisposeAsync();
                command.Dispose();
            }
            catch (Exception ex)
            {
                TraceError(cmdText);
                TraceError(ex.ToString());
            }
            finally
            {
                if (connection != null)
                    connection.Close();
                if (dataSource != null)
                    dataSource.Dispose();
            }

            return sheetDbInfo;
        }
        protected bool ClearTable(string tableName, NpgsqlDataSource dataSource)
        {
            Trace("Clearing " + tableName, System.Drawing.Color.Green);

            bool result = true;
            string cmdText = string.Empty;

            try
            {
                var command = dataSource.CreateCommand();

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
                    TraceError("Table '" + tableName + "' does not exist!");
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

    }

}
