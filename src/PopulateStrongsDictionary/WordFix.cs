using Microsoft.Office.Interop.Excel;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace PopulateStrongsDictionary
{
    internal class WordFix
    {
        Dictionary<string, WordEntry> dictionaryWords = new Dictionary<string, WordEntry>();
        Dictionary<string, WordEntry> dictionaryMatchGoogle = new Dictionary<string, WordEntry>();
        Dictionary<string, WordEntry> dictionarySingleWord = new Dictionary<string, WordEntry>();
        Dictionary<string, WordEntry> dictionaryOther = new Dictionary<string, WordEntry>();

        SortedDictionary<string, WordEntry> dictionaryCorrected = new SortedDictionary<string, WordEntry>();


        int matchGoogle = 0;
        int singleWord = 0;
        int total = 0;

        private MainForm mainForm;
        public WordFix(MainForm mainForm) 
        {
            this.mainForm = mainForm;
        }

        public bool Excute(NpgsqlDataSource dataSource)
        {

            bool result = BuildCorrectionDictionaries(dataSource);

            if (result)
            {
                DoCorrections();

                result = ApplyCorrections(dataSource);
            }

            // Report
            string reportPath = @"C:\Users\samim\Documents\MyProjects\Lexicon\programs\StrongsDictionaryTranslation\database\Names2023-08-22.txt";
            using (StreamWriter sw = new StreamWriter(reportPath))
            {
                sw.WriteLine("Strong's #\t,Grk/Heb\tEnglish\tSVD\tText");
                foreach(string strongs in dictionaryCorrected.Keys)
                {
                    WordEntry we = dictionaryCorrected[strongs];
                    sw.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}\t{4}",
                        strongs,
                        we.OriginalWord,
                        we.EnglishTranslation,
                        we.CorrectedWord,
                        we.LongText));

                }
            }
            
            return result;
        }

        private bool ApplyCorrections(NpgsqlDataSource dataSource)
        {
            bool result = true;
            mainForm.Trace(MethodBase.GetCurrentMethod().Name + ": Updating Arabic Words.", Color.Blue);

            string cmdText = string.Empty;
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
                    mainForm.TraceError(MethodBase.GetCurrentMethod().Name, "Table '" + tableName + "' does not exist!");
                    return false;
                }

                foreach (string strongNumber in dictionaryCorrected.Keys)
                {
                    string strongs = strongNumber;
                    string dstrong = "";
                    if (strongNumber.Length == 6)
                    {
                        strongs = strongNumber.Substring(0, 5);
                        dstrong = strongNumber.Substring(5);
                    }

                    WordEntry entry = dictionaryCorrected[strongNumber];

                    cmdText = "UPDATE public.\"" + tableName + "\" " +
                        string.Format("SET translated_word='{0}' ",
                        entry.CorrectedWord) +
                        string.Format("WHERE strongs_number='{0}' AND d_strong='{1}';", strongs, dstrong);

                    command.CommandText = cmdText;
                    command.ExecuteNonQuery();
                }
                mainForm.Trace(MethodBase.GetCurrentMethod().Name + ": Database updated", Color.Blue);
            }
            catch (Exception ex)
            {
                mainForm.TraceError(MethodBase.GetCurrentMethod().Name, cmdText);
                mainForm.TraceError(MethodBase.GetCurrentMethod().Name, ex.ToString());
                result = false;
            }
            return result;
        }

        private bool BuildCorrectionDictionaries(NpgsqlDataSource dataSource)
        {
            bool result = true;
            mainForm.Trace(string.Format("Building Correction Dictionary"), Color.Black);
            try
            {
                NpgsqlCommand command = dataSource.CreateCommand();

                command.CommandText =
                "SELECT " +
                "strongs.strongs_number," +
                "strongs.d_strong,"+
                "strongs.original_word," + 
                "strongs.english_translation," +
                "strongs.transliteration," +
                "google_xlt.translated_word," +
                "svd_words.word," +
                "google_xlt.translated_long_text" +
                " FROM public.\"strongs_numbers\" strongs INNER JOIN public.\"bible_words_references\" svd_words" +
                "    ON strongs.strongs_number = svd_words.strongs_number" +
                "    INNER JOIN public.\"dictionary_translation\" google_xlt" +
                "    ON strongs.strongs_number = google_xlt.strongs_number" +
                " WHERE (strongs.step_united_reason= 'named' AND (strongs.step_type= 'word' OR strongs.step_type= 'verb'))" +
                " OR strongs.step_type IN" +
                " (SELECT DISTINCT  step_type from public.\"strongs_numbers\" WHERE step_united_reason = 'named'" +
                " AND step_type != 'verb' AND step_type != 'word');";

                //mainForm.Trace(command.CommandText, Color.Black);
                NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string strongs_number = (string)reader[0];
                    string d_strong = (string)reader[1];
                    string original_word = (string)reader[2];
                    string english_translation = (string)reader[3];
                    string transliteration = (string)reader[4];
                    string translated_word = (string)reader[5];
                    string word = (string)reader[6];
                    string longText = (string)reader[7];

                    string strongs = strongs_number + d_strong;

                    if (dictionaryWords.Keys.Contains(strongs))
                    {
                        dictionaryWords[strongs].ArabicWords.Add(word);
                    }
                    else
                    {
                        WordEntry wordEntry = new WordEntry(original_word, english_translation, transliteration, translated_word, longText);
                        wordEntry.ArabicWords.Add(word);
                        dictionaryWords.Add(strongs, wordEntry);
                    }
                }

                command.Dispose();
                reader.Close();
                command.Dispose();

            }
            catch (Exception ex)
            {
                mainForm.TraceError(MethodBase.GetCurrentMethod().Name, "GetStrongsEntry Exception\r\n" + ex.ToString());
                result = false;
            }

            return result;
        }

        private void DoCorrections()
        {
            //mainForm.Trace(string.Format("{0}\t{1}", "originel", "Arabic"), Color.Black);
            mainForm.Trace(string.Format("Applying Corrections"), Color.Black);

            foreach (string strongs in dictionaryWords.Keys)
            {
                WordEntry wordEntry = dictionaryWords[strongs];

                if (wordEntry.ArabicWords.Count == 0)
                    continue;

                total++;
                List<string> words = new List<string>();
                foreach (string word in wordEntry.ArabicWords)
                {
                    string clean = Arabic.RemoveDiacritics(word);
                    if (clean == wordEntry.GoogleWord)
                    {
                        matchGoogle++;
                        if (Arabic.IsDiacritic(word[word.Length - 1]))
                            wordEntry.CorrectedWord = word.Substring(0, word.Length - 1);
                        else
                            wordEntry.CorrectedWord = word;

                        wordEntry.LongText.Replace(wordEntry.GoogleWord, wordEntry.CorrectedWord).
                                           Replace(wordEntry.EnglishTranslation, wordEntry.CorrectedWord);

                        dictionaryMatchGoogle.Add(strongs, wordEntry);
                        dictionaryCorrected.Add(strongs, wordEntry);
                        break;
                    }
                    words.Add(word);
                }
                if (!string.IsNullOrEmpty(wordEntry.CorrectedWord))
                    continue;

                if (words.Count == 1)
                {
                    // handle single word
                    wordEntry.CorrectedWord = GetTrimmedWord(words[0], wordEntry.OriginalWord, strongs[0] == 'H');
                    wordEntry.LongText.Replace(wordEntry.GoogleWord, wordEntry.CorrectedWord).
                        Replace(wordEntry.EnglishTranslation, wordEntry.CorrectedWord);
                    singleWord++;
                    dictionarySingleWord.Add(strongs, wordEntry);
                    dictionaryCorrected.Add(strongs, wordEntry);

                    //Trace(string.Format("{0}\t{1}\t{2}", wordEntry.OriginalWord, words[0], wordEntry.CorrectedWord), Color.Black);
                    continue;
                }

                // handle multiple words
                for (int i = 0; i < words.Count; i++)
                {
                    words[i] = GetTrimmedWord(words[i], wordEntry.OriginalWord, strongs[0] == 'H');
                }

                bool match = true;
                Dictionary<string, int> matches = new Dictionary<string, int>(); 
                for (int i = 0; i < words.Count; i++)
                {
                    string word2Match = words[i];
                    string a = Arabic.RemoveDiacritics(word2Match.Trim());

                    for (int j = 0; j < words.Count; j++)
                    {
                        if (i != j)
                        {
                            string b = Arabic.RemoveDiacritics(words[j]).Trim();
                            if(a==b)
                            {
                                if (matches.Keys.Contains(word2Match))
                                    matches[word2Match]++;
                                else
                                    matches.Add(word2Match, 1);;
                            }
                        }
                    }
                }
                string best = string.Empty;
                int score = 0;
                foreach(string w in matches.Keys)
                {
                    if (matches[w] > score) { best = w; score = matches[w]; }
                }

                if(string.IsNullOrEmpty(best))
                {
                    dictionaryOther.Add(strongs, wordEntry);
                }
                else
                {
                    wordEntry.CorrectedWord = best;
                    dictionaryCorrected.Add(strongs, wordEntry);
                }
                //if (!match)
                //{
                //    string o = strongs + "\t" + wordEntry.OriginalWord;
                //    for (int i = 0; i < words.Count; i++)
                //    {
                //        o += "\t" + words[i];
                //    }
                //    //Trace(o, Color.Black);
                //}
            }
        }

        private string GetTrimmedWord(string araWord, string hebOrGrkWord, bool heb)
        {
            string result = araWord;

            string araClean = Arabic.RemoveDiacritics(araWord);
            string hebGrkClean = heb ? Hebrew.RemovePoints(hebOrGrkWord) : hebOrGrkWord;
            try
            {
                if (heb)
                {
                    if ((araClean[0] == 'و' && hebGrkClean[0] != 'ו') ||
                        (araClean[0] == 'ب' && hebGrkClean[0] != 'ב') ||
                        (araClean[0] == 'ل' && hebGrkClean[0] != 'ל'))
                    {
                        araClean = araClean.Substring(1);
                        result = result.Substring(1);
                    }
                }
                else
                {
                    if ((araClean[0] == 'و' && hebGrkClean[0] != 'ω') ||
                        (araClean[0] == 'و' && hebGrkClean[0] != 'ο') ||
                        (araClean[0] == 'ب' && hebGrkClean[0] != 'β') ||
                        (araClean[0] == 'ل' && hebGrkClean[0] != 'λ'))
                    {
                        araClean = araClean.Substring(1);
                        result = result.Substring(1);
                    }
                }
            }
            catch (Exception e)
            {
                mainForm.TraceError(MethodBase.GetCurrentMethod().Name, e.ToString());
            }
            if (Arabic.IsDiacritic(result[0])) // remove any diacretic left over from the waw
                result = result.Substring(1);


            string[] araParts = araClean.Split(new char[] { ' ' });
            string[] hebGrkParts = hebGrkClean.Split(new char[] { ' ' });
            if (araParts.Length != hebGrkParts.Length)
            {
                string[] strings = { "من", "ان", "في", "الى", "عند", "يا", "مثل", "على" };
                if (araParts.Length > 1 && strings.Contains(araParts[0].Trim()))
                {
                    int idx = result.IndexOf(' ');
                    result = result.Substring(idx);
                }
            }

            if (Arabic.IsDiacritic(result[result.Length - 1]))
                result = result.Substring(0, result.Length - 1);

            return result;
        }

    }
    class WordEntry
    {
        List<string> arabicWords = new List<string>();

        public WordEntry(string originalWord,
            string englishTranslation,
            string transliteration,
            string googleWord, string longText)
        {
            OriginalWord = originalWord;
            EnglishTranslation = englishTranslation;
            Transliteration = transliteration;
            GoogleWord = googleWord;
            LongText = longText;
        }

        public string OriginalWord { get; set; }

        public string EnglishTranslation { get; set; }
        public string Transliteration { get; set; }

        public string GoogleWord { get; set; }
        public string CorrectedWord { get; set; }
        public string LongText { get; set; }

        public List<string> ArabicWords
        {
            get
            {
                return arabicWords;
            }
        }

    }
}
