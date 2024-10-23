using Npgsql;
using System.Runtime.CompilerServices;

namespace DStrongDiscrepancy
{
    public partial class Form1 : Form
    {
        private NpgsqlDataSource sourceDataSource;
        private NpgsqlConnection sourceConnection;

        private SortedDictionary<int, List<char>> greekStrongsGood = new SortedDictionary<int, List<char>>();
        private SortedDictionary<int, List<char>> hebrewStrongsGood = new SortedDictionary<int, List<char>>();

        private SortedDictionary<int, List<char>> greekStrongsBad = new SortedDictionary<int, List<char>>();
        private SortedDictionary<int, List<char>> hebrewStrongsBad = new SortedDictionary<int, List<char>>();

        public Form1()
        {
            InitializeComponent();
        }

        #region Trace
        delegate void TraceDelegate(string text, System.Drawing.Color color);
        delegate void ClearTraceDelegate();

        private void TraceX(string text, System.Drawing.Color color,
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
        public void Trace(string text, System.Drawing.Color color)
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
            Trace(string.Format("Error: {0}::{1}", method, text), System.Drawing.Color.Red);
        }


        #endregion Trace

        /*
H2148 => v *
H2148 => w *
H2148 => x *
H2148 => y *
H2148 => z *

H4918 => y *

H5838 => w *
H5838 => x *
H5838 => y *
H5838 => z *
         */
        private void Form1_Load(object sender, EventArgs e)
        {
            string sourceDB = "StrongsDictionary";
            var sourceConnectionString = "Host=localhost;Port=5432;Username=postgres;Password=1Na61A51;Database=" + sourceDB;
            sourceDataSource = NpgsqlDataSource.Create(sourceConnectionString);
            sourceConnection = sourceDataSource.CreateConnection();

            GetStrongsList("strongs_numbers", greekStrongsGood, hebrewStrongsGood);
            GetStrongsList("dictionary_translation", greekStrongsBad, hebrewStrongsBad);
        }



        private void GetStrongsList(
            string table,
            SortedDictionary<int, List<char>> greekStrongs,
            SortedDictionary<int, List<char>> hebrewStrongs)
        {
            string cmdText = string.Empty;
            //Trace("Fetching " + strongsNumber, Color.Green);

            try
            {
                if (sourceConnection.State == System.Data.ConnectionState.Closed)
                    sourceConnection.Open();

                NpgsqlCommand command = sourceDataSource.CreateCommand();

                command.CommandText =
                   "SELECT" +
                   " strongs_number,d_strong" +
                   " FROM public.\"" + table + "\"" +
                   " ORDER BY strongs_number ASC, d_strong ASC;";

                NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    SortedDictionary<int, List<char>> sDict;
                    string s = (string)reader[0];
                    if (s[0] == 'H')
                        sDict = hebrewStrongs;
                    else
                        sDict = greekStrongs;

                    int sNum = int.Parse(s.Substring(1));
                    string d = (string)reader[1];
                    if (sDict.ContainsKey(sNum))
                    {
                        if (!string.IsNullOrEmpty(d))
                            if (!sDict[sNum].Contains(d[0])) sDict[sNum].Add(d[0]);
                    }
                    else
                    {
                        List<char> list = new List<char>();
                        if (string.IsNullOrEmpty(d))
                        {
                            int x = 0;
                        }
                        if (!string.IsNullOrEmpty(d))
                            list.Add(d[0]);
                        sDict[sNum] = list;
                    }
                }
                command.Dispose();

                reader.Close();
                reader.DisposeAsync();
                command.Dispose();
                sourceConnection.Close();

            }
            catch (Exception ex)
            {
                TraceError("GetStrongsEntry Exception\r\n" + ex.ToString());
            }

        }

        private void reportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Thread(() => { Run(); }).Start();
        }

        private void Run()
        {
            foreach(int strong in greekStrongsGood.Keys)
            {
                List<char> good = greekStrongsGood[strong];
                List<char> bad = greekStrongsBad[strong];

                foreach(char c in good)
                {
                    if(bad.Contains(c)) continue;
                    Trace(string.Format("G{0} => {1}", strong, c), Color.Black);
                }
            }

            foreach (int strong in hebrewStrongsGood.Keys)
            {
                if(strong == 5838)
                {
                    int x = 0;
                }
                List<char> good = hebrewStrongsGood[strong];
                List<char> bad = hebrewStrongsBad[strong];

                foreach (char c in good)
                {
                    if (bad.Contains(c)) continue;
                    Trace(string.Format("H{0} => {1}", strong, c), Color.Black);
                }
            }

            Trace("Done!", Color.Green);
        }
    }
}
