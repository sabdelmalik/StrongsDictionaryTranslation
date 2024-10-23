using DictionaryEditorV2.Lexicon;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryEditorV2
{
    internal class StrongsNumbers
    {
        /// <summary>
        /// key strong's number
        /// value list of disambiguation characters
        /// </summary>
        private SortedDictionary<int, StrongsNumber> greekStrongs = new SortedDictionary<int, StrongsNumber>();
        private SortedDictionary<int, StrongsNumber> hebrewStrongs = new SortedDictionary<int, StrongsNumber>();

        private string dbName = "StrongsDictionaryV2";
        private NpgsqlDataSource dataSource;
        private NpgsqlConnection connection;

        private static StrongsNumbers instance = null;
        private static readonly object lockObj = new object();

        public static StrongsNumbers Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = new StrongsNumbers();
                        }
                    }
                }
                return instance;
            }
        }

        private StrongsNumbers() 
        {
            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1Na61A51;Database=" + dbName;
            dataSource = NpgsqlDataSource.Create(connectionString);
            connection = dataSource.CreateConnection();

            GetStrongsList();
        }



        private void GetStrongsList()
        {
            string cmdText = string.Empty;
            //Trace("Fetching " + strongsNumber, Color.Green);
            long strong_id = 0;
            string strongs = string.Empty;

            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();

                NpgsqlCommand command = dataSource.CreateCommand();

                command.CommandText =
                   "SELECT" +
                   " strong_id, strongs_number,d_strong" +
                   " FROM public.\"strongs_numbers\"" +
                   " ORDER BY strongs_number ASC,d_strong ASC;";

                NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    // G0068, G0184, G5514, G5564
                    SortedDictionary<int, StrongsNumber> sDict;
                    strong_id = (long)reader[0];
                    strongs = (string)reader[1];
                    if (strongs[0] == 'H')
                        sDict = hebrewStrongs;
                    else
                        sDict = greekStrongs;

                    if (strongs == "H9049" || strongs == "G0068")
                    {
                        int x = 0;
                    }

                    int sNum = int.Parse(strongs.Substring(1));
                    string d = (string)reader[2];
                    if (sDict.ContainsKey(sNum))
                    {
                        sDict[sNum].AddDStrong(d, strong_id);
                    }
                    else
                    {
                        StrongsNumber sn = new StrongsNumber(strongs);
                        sn.AddDStrong(d, strong_id);
                        sDict[sNum] = sn;
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
                int x = 0;
                //Trace("GetStrongsEntry Exception\r\n" + ex.ToString(), Color.Red);
            }

        }

        public void Reload()
        {
            hebrewStrongs.Clear();
            greekStrongs.Clear();
            GetStrongsList();
        }

        public StrongsNumber GetStrongNumber(bool hebrew, int strongNum)
        {
            StrongsNumber sn = null;
            try
            {
                sn = hebrew ? hebrewStrongs[strongNum] : greekStrongs[strongNum];
            }
            catch (Exception ex)
            {
            }

            return sn;
        }

        public List<string> GetDStrongNumber(bool hebrew, int strongNum)
        {
            StrongsNumber sn = hebrew? hebrewStrongs[strongNum] : greekStrongs[strongNum];

            return sn.DStrongs;
        }

        public bool IsValid(bool hebrew, int strongNum)
        {
            bool result = hebrew ? hebrewStrongs.ContainsKey(strongNum) :
                                   greekStrongs.ContainsKey(strongNum);

            return result;
        }

    }
}
