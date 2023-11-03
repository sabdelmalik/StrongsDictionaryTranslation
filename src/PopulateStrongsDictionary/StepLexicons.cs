using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace PopulateStrongsDictionary
{
    public enum StateMachine
    {
        IDLE,
        STRONGS_FOUND,
        REASON_FOUND,
        TYPE_FOUND
    }
    internal class StepLexicons
    {
        MainForm mainForm;
        string lexiconsFolder = string.Empty;


        SortedDictionary<string, LexiconEntry> lexicon = new SortedDictionary<string, LexiconEntry>();
        public StepLexicons(MainForm mainForm)
        {

            this.mainForm = mainForm;

            lexiconsFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "STEP");
        }

        public void LoadLexicons(NpgsqlDataSource dataSource)
        {
            mainForm.Trace(MethodBase.GetCurrentMethod().Name + ": Loading lexicon.", Color.Blue);

            string lexiconHebrewPath = Path.Combine(lexiconsFolder, "lexicon_hebrew.txt");
            string lexiconGreekPath = Path.Combine(lexiconsFolder, "lexicon_greek.txt");

            try
            {

                if (File.Exists(lexiconHebrewPath))
                    LoadLexicon(lexiconHebrewPath, lexicon);

                if (File.Exists(lexiconGreekPath))
                    LoadLexicon(lexiconGreekPath, lexicon);
            }
            catch (Exception ex)
            {
                mainForm.TraceError(MethodBase.GetCurrentMethod().Name, ex.ToString());
                return;
            }
            mainForm.Trace(MethodBase.GetCurrentMethod().Name + ": lexicon.Loaded.\r\n", Color.Blue);

//            ExportLexicon(lexiconsFolder);

            mainForm.Trace(MethodBase.GetCurrentMethod().Name + ": Updating database", Color.Blue);
            string cmdText = string.Empty;
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
                    mainForm.TraceError(MethodBase.GetCurrentMethod().Name, "Table '" + tableName + "' does not exist!");
                    return;
                }

                foreach (string strongNumber in lexicon.Keys)
                {
                    string strongs = strongNumber;
                    string dstrong = "";
                    if (strongNumber.Length == 6)
                    {
                        strongs = strongNumber.Substring(0, 5);
                        dstrong = strongNumber.Substring(5);
                    }

                    LexiconEntry entry = lexicon[strongNumber];

                    cmdText = "UPDATE public.\"" + tableName + "\" " +
                        string.Format("SET step_united_reason='{0}', step_type='{1}', transliteration='{2}', pronunciation='{3}' ",
                        entry.UnitedReason.Replace("'", "''"),
                        entry.StepType.Replace("'", "''"),
                        entry.Transliteration.Replace("'", "''"),
                        entry.Pronunciation.Replace("'", "''")) +
                        string.Format("WHERE strongs_number='{0}';", strongs);

                    command.CommandText = cmdText;
                    command.ExecuteNonQuery();
                }
                mainForm.Trace(MethodBase.GetCurrentMethod().Name + ": Database updated", Color.Blue);
            }
            catch (Exception ex)
            {
                mainForm.TraceError(MethodBase.GetCurrentMethod().Name, cmdText);
                mainForm.TraceError(MethodBase.GetCurrentMethod().Name, ex.ToString());
                return;
            }
        }

        private void ExportLexicon(string lexiconsFolder)
        {
            // strong original english short long
            string path = Path.Combine(lexiconsFolder, "CombinedLexicon.txt");
            using(StreamWriter sw = new StreamWriter(path))
            {
                foreach(string strongs in lexicon.Keys)
                {
                    sw.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}\t{4}",
                        strongs,
                        lexicon[strongs].UnicodeAccented,
                        lexicon[strongs].Gloss,
                        lexicon[strongs].ShortDefinition,
                        lexicon[strongs].Definition));
                } 
            }
        }


        /// <summary>
        /// 
        /// Note: disambiguation character is case sensetive
        /// </summary>
        /// <param name="lexiconPath"></param>
        /// <param name="lexicon"></param>
        private void LoadLexicon(string lexiconPath, SortedDictionary<string, LexiconEntry> lexicon)
        {
            using (StreamReader sr = new StreamReader(lexiconPath))
            {
                string strong = string.Empty;
                string reason = string.Empty;
                string type = string.Empty;
                string unicodeAccented = string.Empty;
                string gloss = string.Empty;
                string transliteration = string.Empty;
                string pronunciation = string.Empty;
                string definition = string.Empty;
                string shortDefinition = string.Empty;

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    Match mTx = Regex.Match(line, @"^\$\=+([GH]\d\d\d\d[A-Z]*)\=");
                    if (mTx.Success)
                    {
                        if (strong != string.Empty)
                        {
                            // this is not the first time. Save the previous Strong's 
                            lexicon.Add(strong, new LexiconEntry(reason, type, unicodeAccented, gloss, transliteration, pronunciation, definition, shortDefinition));
                        }
                        // Strong Number
                        strong = mTx.Groups[1].Value;
                        reason = string.Empty;
                        type = string.Empty;
                        unicodeAccented = string.Empty;
                        gloss = string.Empty;
                        transliteration = string.Empty;
                        pronunciation = string.Empty;
                        definition = string.Empty;
                        shortDefinition = string.Empty;
                        continue;
                    }

                    mTx = Regex.Match(line, @"^\@STEP_UnitedReason\s*\=\s*(.+)");
                    if (mTx.Success)
                    {
                        reason = mTx.Groups[1].Value;
                        continue;
                    }

                    mTx = Regex.Match(line, @"^\@STEP_Type\s*\=\s*(.+)");
                    if (mTx.Success)
                    {
                        type = mTx.Groups[1].Value;
                        continue;
                    }

                    mTx = Regex.Match(line, @"^\@STEPUnicodeAccented\s*\=\s*(.+)");
                    if (mTx.Success)
                    {
                        unicodeAccented = mTx.Groups[1].Value;
                        continue;
                    }

                    mTx = Regex.Match(line, @"^\@StepGloss\s*\=\s*(.+)");
                    if (mTx.Success)
                    {
                        gloss = mTx.Groups[1].Value;
                        continue;
                    }

                    mTx = Regex.Match(line, @"^\@StrTranslit\s*\=\s*(.+)");
                    if (mTx.Success)
                    {
                        transliteration = mTx.Groups[1].Value;
                        continue;
                    }

                    mTx = Regex.Match(line, @"^\@StrPronunc\s*\=\s*(.+)");
                    if (mTx.Success)
                    {
                        pronunciation = mTx.Groups[1].Value;
                        continue;
                    }

                    mTx = Regex.Match(line, @"^\@MounceShortDef\s*\=\s*(.+)");
                    if (mTx.Success)
                    {
                        shortDefinition = CleanText(mTx.Groups[1].Value);
                        continue;
                    }

                    mTx = Regex.Match(line, @"^\@MounceMedDef\s*\=\s*(.+)");
                    if (mTx.Success)
                    {
                        definition = CleanText(mTx.Groups[1].Value);
                        continue;
                    }
                    mTx = Regex.Match(line, @"^\@BdbMedDef\s*\=\s*(.+)");
                    if (mTx.Success)
                    {
                        definition = CleanText(mTx.Groups[1].Value);
                        continue;
                    }
                }
                // this is the last entry 
                lexicon.Add(strong, new LexiconEntry(reason, type, unicodeAccented, gloss, transliteration, pronunciation, definition, shortDefinition));
            }
        }


        /// <summary>
        /// Replace  <ref ></ref> tag with Ref:
        /// remove all other HTML tags
        /// convert Gen.1.1 to Gen 1:1
        /// convert Gen. 1:1 to Gen 1:1
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string CleanText(string text)
        {
            string refPattern = @"<[Rr][Ee][Ff]\s*\=[0-9a-zA-Z ""():_+=;.,'\t-]*>";

            string definition = Regex.Replace(text, refPattern, " Ref: ");
            definition = Regex.Replace(definition, @"<[/0-9a-zA-Z =.']*>", " ");
            while (true)
            {
                // convert Gen.1.1 to Gen 1:1
                Match match = Regex.Match(definition, @"([a-zA-Z0-9]+\.\d+\.\d+)");
                if (!match.Success) break;

                string x = match.Groups[1].Value;
                string reference = Regex.Replace(x, @"^([a-zA-Z0-9]+)\.([0-9]+)\.([0-9]+)",
                                    m => string.Format("{0} {1}:{2}", m.Groups[1].Value, m.Groups[2].Value, m.Groups[3].Value));
                definition = definition.Replace(x, reference);
            }
            while (true)
            {
                // convert Gen. 1:1 to Gen 1:1
                Match match = Regex.Match(definition, @"([a-zA-Z0-9]+\.\s\d+\:\d+)");
                if (!match.Success) break;

                string x = match.Groups[1].Value;
                string reference = Regex.Replace(x, @"^([a-zA-Z0-9]+)\.\ ([0-9]+\:[0-9]+)",
                                    m => string.Format("{0} {1}", m.Groups[1].Value, m.Groups[2].Value));
                definition = definition.Replace(x, reference);
            }

            return definition;
        }
    }


    class LexiconEntry
    {
        public LexiconEntry(string reason, string type, string unicodeAccented, string gloss, string transliteration, string pronunciation, string definition, string shortDefinition)
        {
            UnitedReason = reason;
            StepType = type;
            UnicodeAccented = unicodeAccented;
            Gloss = gloss;
            Transliteration = transliteration;
            Pronunciation = pronunciation;
            Definition = definition;
            ShortDefinition = shortDefinition;
        }

        public string UnitedReason { get; set; }
        public string StepType { get; set; }
        public string UnicodeAccented { get; set; }

        public string Gloss { get; set; }

        public string Transliteration { get; set; }
        public string Pronunciation { get; set; }

        public string Definition { get; set; }
        public string ShortDefinition { get; set; }



    }
}
