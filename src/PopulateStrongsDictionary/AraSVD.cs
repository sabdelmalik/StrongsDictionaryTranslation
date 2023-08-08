using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using BibleTaggingUtil;
using Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic.ApplicationServices;
using Npgsql;
using static System.Net.Mime.MediaTypeNames;

namespace PopulateStrongsDictionary
{
    internal class AraSVD
    {

        /// <summary>
        /// Bible Dictionary
        /// Key: verse reference (xxx c:v) xxx = OSIS book name, v = verse number
        /// </summary>
        protected Dictionary<string, Verse> bible = new Dictionary<string, Verse>();

        /// <summary>
        /// Bible Dictionary
        /// Key: UBS book name
        /// value: loaded Bible book name
        /// </summary>
        protected Dictionary<string, string> bookNames = new Dictionary<string, string>();

        /// <summary>
        /// Key Strong's number
        /// value ArabicReferences instances containing a dictionary of Arabic words and associated verse references
        /// </summary>
        public SortedDictionary<string, ArabicReferences> Strongs2Arabic = new SortedDictionary<string, ArabicReferences>();



        public List<string> bookNamesList = new List<string>();

        private const string referencePattern1 = @"^([0-9A-Za-z]+)\s([0-9]+):([0-9]+)\s*(.*)";
        private const string referencePattern2 = @"^[0-9]+_([0-9A-Za-z]+)\.([0-9]+)\.([0-9]+)\s*(.*)";
        private const string referencePattern3 = @"^([0-9A-Za-z]{3})\.([0-9]+)\.([0-9]+)\s*(.*)";
        private string textReferencePattern = string.Empty;

        protected string bibleName = string.Empty;
        protected int totalVerses = 31104;
        protected int currentVerseCount;

        MainForm mainForm;

        public AraSVD(MainForm mainForm)
        {
            this.mainForm = mainForm;
        }

        public bool LoadBibleVerses(int langId, NpgsqlDataSource dataSource)
        {
            bool result = true;
            try
            {
                string table = "bible_text";
                string cmdText = string.Empty;
                mainForm.Trace("\r\nPolpulating Bible text!", Color.Green);

                using var command = dataSource.CreateCommand();

                bool exists = false;
                command.CommandText = "SELECT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = '" + table + "') AS table_existence;";
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    exists = (bool)reader[0];
                }
                reader.Close();

                if (!exists)
                {
                    mainForm.Trace("Table '" + table + "' does not exist!", Color.Red);
                    return false;
                }

                command.CommandText = "DELETE FROM public.\"" + table + "\";";
                command.ExecuteNonQuery();

                string[] references = bible.Keys.ToArray();
                for (int i = 0; i < bible.Count; i++)
                {
                    string reference = references[i];

                    int bookID = Utils.GetBookIndex(reference);
                    string[] refParts = reference.Split(' ');
                    string[] verseIdParts = refParts[1].Split(':');
                    int chapter = int.Parse(verseIdParts[0]);
                    int verse = int.Parse(verseIdParts[1]);

                    Verse v = bible[reference];
                    string text = Utils.GetVerseText(v, false);

                    cmdText = "INSERT INTO public.\"" + table + "\" " +
                       "(language_id, book_id, chapter_num, verse_num, verse_text) " +
                       "VALUES " +
                       string.Format("({0}, {1}, {2}, {3}, '{4}');",
                       langId,
                       bookID,
                       chapter,
                       verse,
                       text.Replace("'", "''"));
                    command.CommandText = cmdText;
                    command.ExecuteNonQuery();
                }
                mainForm.Trace("Bible text population Done!", Color.Green);
            }
            catch (Exception ex)
            {
                mainForm.Trace("LoadBibleVerses Exception\r\n" + ex.ToString(), Color.Red);
                return false;
            }

            return result;
        }

        public bool LoadBibleWords(int langId, NpgsqlDataSource dataSource)
        {
            bool result = true;
            try
            {
                string table = "bible_words_references";
                string cmdText = string.Empty;
                mainForm.Trace("\r\nPolpulating Bible word references!", Color.Green);

                using var command = dataSource.CreateCommand();

                bool exists = false;
                command.CommandText = "SELECT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = '" + table + "') AS table_existence;";
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    exists = (bool)reader[0];
                }
                reader.Close();

                if (!exists)
                {
                    mainForm.Trace("Table '" + table + "' does not exist!", Color.Red);
                    return false;
                }

                command.CommandText = "DELETE FROM public.\"" + table + "\";";
                command.ExecuteNonQuery();

                string[] strongs = Strongs2Arabic.Keys.ToArray();
                List<string> wordList = new List<string>();
                for (int i = 0; i < strongs.Length; i++)
                {
                    if (strongs[i] == "H0000" || strongs[i] == "G0000")
                        continue;

                    ArabicReferences ar = Strongs2Arabic[strongs[i]];

                    bool first = true;
                    wordList.Clear();
                    foreach (string araWord in ar.AR.Keys)
                    {
                        #region remove punctuation characters
                        string cleanWord = araWord;

                        string temp = cleanWord.
                            Replace("،", "").Replace(":", "").
                            Replace("!", "").Replace(",", "").
                            Replace("«", "").Replace("»", "").
                            Replace("؟", "").Replace(".", "");
                        if (temp.Length > 0)
                            cleanWord = temp;

                        #endregion remove punctuation characters

                        #region remove unnecessary Arabic words
                        if (cleanWord.Length > 6) cleanWord = cleanWord.
                                Replace("كنت ", "").
                                Replace("كانوا ", "");
                        if (cleanWord.Length > 5) cleanWord = cleanWord.
                                Replace("قد ", "").
                                Replace("قد ", "").
                                Replace("في ", "").
                                Replace("كانت ", "").
                                Replace("كان ", "");
                        #endregion remove unnecessary Arabic words

                        if (wordList.Contains(cleanWord))
                            continue;

                        wordList.Add(cleanWord);

                        string refrs = string.Empty;
                        foreach (string r in ar.AR[araWord])
                        {
                            refrs = refrs + r + ", ";
                        }
                        refrs = refrs.Substring(0, refrs.Length - 1);
                        if (strongs[i].Length > 5)
                        {
                            int x = 0;
                        }
                        cmdText = "INSERT INTO public.\"" + table + "\" " +
                           "(strongs_number, language_id, word, \"references\") " +
                           "VALUES " +
                           string.Format("('{0}', {1}, '{2}', '{3}');",
                           strongs[i],
                           langId,
                           cleanWord,
                           refrs);
                        command.CommandText = cmdText;
                        command.ExecuteNonQuery();

                    }

                }
                mainForm.Trace("Bible word references population Done!", Color.Green);
            }
            catch (Exception ex)
            {
                mainForm.Trace("LoadBibleWords Exception\r\n" + ex.ToString(), Color.Red);
                return false;
            }

            return result;
        }

        public bool LoadBibleFile(string textFilePath, bool newBible, bool more)
        {
            if (newBible)
            {
                bible.Clear();
                bookNames.Clear();
                bookNamesList.Clear();
                currentVerseCount = 0;
            }
            return LoadBibleFileInternal(textFilePath, more);
        }

        private bool LoadBibleFileInternal(string textFilePath, bool more)
        {
            //Tracing.TraceEntry(MethodBase.GetCurrentMethod().Name, textFilePath, more);
            mainForm.Trace(string.Format("[{0}]: {1}", MethodBase.GetCurrentMethod().Name, textFilePath), Color.Brown);
            bool result = false;

            if (File.Exists(textFilePath))
            {
                result = true;
                using (var fileStream = new FileStream(textFilePath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        while (reader.Peek() >= 0)
                        {
                            var line = reader.ReadLine().Trim(' ');
                            if (!string.IsNullOrEmpty(line))
                            {
                                if (string.IsNullOrEmpty(textReferencePattern))
                                {
                                    //if (!SetSearchPattern(line, out textReferencePattern))
                                    //    continue;
                                    SetSearchPattern(line, out textReferencePattern);
                                }
                                if (!string.IsNullOrEmpty(textReferencePattern))
                                {
                                    AddBookName(line);
                                }
                                ParseLine(line);
                            }
                        }

                    }
                }

            }

            if (!more && !(new int[] { 66, 39, 27 }).Contains(bookNamesList.Count))
            {
                //Tracing.TraceError(MethodBase.GetCurrentMethod().Name, string.Format("{0}:Book Names Count = {1}. Was expecting 66, 39 or 27",
                //                        Path.GetFileName(textFilePath), bookNamesList.Count));
                mainForm.Trace(string.Format("[{0}]: {1}:Book Names Count = {2}. Was expecting 66, 39 or 27", MethodBase.GetCurrentMethod().Name, Path.GetFileName(textFilePath), bookNamesList.Count), Color.Red);
                return false;
            }


            if (bookNamesList.Count == 66 || bookNamesList.Count == 39)
            {
                for (int i = 0; i < bookNamesList.Count; i++)
                {
                    bookNames.Add(BibleBooks.ubsNames[i], bookNamesList[i]);
                }
            }
            else if (bookNamesList.Count == 27)
            {
                for (int i = 0; i < bookNamesList.Count; i++)
                {
                    bookNames.Add(BibleBooks.ubsNames[i + 39], bookNamesList[i]);
                }
            }
            return result;
        }

        public void BuildStrongs2Arabic()
        {
            try
            {

                string[] references = bible.Keys.ToArray();
                for (int i = 0; i < bible.Count; i++)
                {
                    string reference = references[i];
                    if(reference == "Jos 12:3")
                    {
                        int x = 0;
                    }

                    int bookID = Utils.GetBookIndex(reference);
                    string[] refParts = reference.Split(' ');
                    string[] verseIdParts = refParts[1].Split(':');
                    int chapter = int.Parse(verseIdParts[0]);
                    int verse = int.Parse(verseIdParts[1]);

                    Verse v = bible[reference];
                    BibleTestament bibleTestament = Utils.GetTestament(reference);
                    string text = Utils.GetVerseText(v, false);
                    for (int j = 0; j < v.Count; j++)
                    {
                        VerseWord vw = v[j];
                        if (vw.Word.Length == 1 && vw.Word[0] == '\u00AD')
                        {
                            if (vw.Strong.Length == 0 || (vw.Strong.Length == 1 && vw.Strong[0] == ""))
                                continue;
                        }

                        string cleanWord = vw.Word;

                        for (int k = 0; k < vw.Strong.Length; k++)
                        {
                            string st = vw.Strong[k];
                            if (string.IsNullOrEmpty(st))
                                continue;

                            st = (bibleTestament == BibleTestament.OT ? "H" : "G") + st;


                            if (Strongs2Arabic.ContainsKey(st))
                            {
                                ArabicReferences ar = Strongs2Arabic[st];
                                ar.Add(cleanWord, reference);
                            }
                            else
                            {
                                ArabicReferences ar = new ArabicReferences();
                                ar.Add(cleanWord, reference);
                                Strongs2Arabic.Add(st, ar);
                            }
                        }
                    }

                }
                mainForm.Trace("BuildStrongs2Arabic Done!", Color.Green);
            }
            catch (Exception ex)
            {
                mainForm.Trace("LoadBibleToDB Exception\r\n" + ex.ToString(), Color.Red);
                return;
            }
        }

        public int BookCount
        {
            get
            {
                return bookNamesList.Count;
            }
        }
        public Dictionary<string, Verse> Bible
        { get { return bible; } }


        public string this[string ubsName]
        {
            get
            {
                string bookName = string.Empty;
                try
                {
                    if (bookNames.Count > 0)
                        bookName = bookNames[ubsName];
                }
                catch (Exception ex)
                {
                    //Tracing.TraceException(MethodBase.GetCurrentMethod().Name, ex.Message);
                    mainForm.Trace(string.Format("[{0}]: {1}", MethodBase.GetCurrentMethod().Name, ex.Message), Color.Red);

                }
                return bookName;

            }
        }

        private bool AddBookName(string line)
        {
            if (line.ToLower().Contains("gen"))
            {
                int o = 0;
            }
            Match mTx = Regex.Match(line, textReferencePattern);
            if (!mTx.Success)
            {
                //Tracing.TraceError(MethodBase.GetCurrentMethod().Name, "Could not detect text reference: " + line);
                mainForm.Trace(string.Format("[{0}]: Could not detect text reference: {1}", MethodBase.GetCurrentMethod().Name, line), Color.Red);
                return false;
            }

            String book = mTx.Groups[1].Value;
            if (!bookNamesList.Contains(book))
                bookNamesList.Add(book);

            return true;
        }

        private bool SetSearchPattern(string line, out string referancePattern)
        {

            Match mTx = Regex.Match(line, referencePattern1);
            if (mTx.Success)
            {
                referancePattern = referencePattern1;
                return true;
            }

            mTx = Regex.Match(line, referencePattern2);
            if (mTx.Success)
            {
                referancePattern = referencePattern2;
                return true;
            }

            mTx = Regex.Match(line, referencePattern3);
            if (mTx.Success)
            {
                referancePattern = referencePattern3;
                return true;
            }

            //Tracing.TraceError(MethodBase.GetCurrentMethod().Name, "Could not detect reference pattern: " + line);
            mainForm.Trace(string.Format("[{0}]: Could not detect reference pattern: {1}", MethodBase.GetCurrentMethod().Name, line), Color.Red);
            referancePattern = string.Empty;
            return false;
        }

        protected virtual void ParseLine(string line)
        {

            Match mTx = Regex.Match(line, @"^([0-9A-Za-z]+)\s([0-9]+):([0-9]+)\s*(.*)");

            /*            // find spcae between book and Chapter
                        int spaceB = line.IndexOf(' ');
                        // find spcae between Verse number and Text
                        int spaceV = line.IndexOf(' ', spaceB + 1);
                        if (spaceV == -1)
                        {
                            throw new Exception(string.Format("Ill formed verse line!"));
                        }

                        string book = line.Substring(0, spaceB);
                        string reference = line.Substring(0, spaceV);
                        string verse = line.Substring(spaceV + 1);*/

                    string book = mTx.Groups[1].Value;
            string chapter = mTx.Groups[2].Value;
            string verseNo = mTx.Groups[3].Value;
            string verse = mTx.Groups[4].Value;
            string reference = string.Format("{0} {1}:{2}", book, chapter, verseNo);
            if (reference == "Jos 12:3")
            {
                int x = 0;
            }


            BibleTestament testament = Utils.GetTestament(reference);

            string[] verseParts = verse.Split(' ');
            List<string> words = new List<string>();
            List<string> tags = new List<string>();
            string tempWord = string.Empty;
            string tmpTag = string.Empty;
            for (int i = 0; i < verseParts.Length; i++)
            {
                string versePart = verseParts[i].Trim();
                if (string.IsNullOrEmpty(versePart))
                    continue; // some extra space
                if (i == 0 || versePart[0] != '<') // add i == 0 test because a verse can not start with a tag.
                {
                    if (!string.IsNullOrEmpty(tmpTag))
                        tags.Add(tmpTag);
                    tmpTag = string.Empty;
                    tempWord += (string.IsNullOrEmpty(tempWord)) ? verseParts[i] : (" " + verseParts[i]);
                    if (i == verseParts.Length - 1)
                    {
                        // last word
                        words.Add(tempWord);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(tempWord))
                        words.Add(tempWord);
                    tempWord = string.Empty;
                    if (verseParts[i] == "<>")
                    {
                        tmpTag = "<>";
                    }
                    else
                    {
                        tmpTag += (string.IsNullOrEmpty(tmpTag)) ? verseParts[i] : (" " + verseParts[i]);
                        tmpTag = tmpTag.Replace(".", "").Replace("?", "").Trim();
                        if (!string.IsNullOrEmpty(tmpTag))
                        {
                            string[] pts = tmpTag.Split(' ');
                            tmpTag = string.Empty;
                            foreach (string t in pts)
                            {
                                if (t.StartsWith('<'))
                                {
                                    int x = t.IndexOf(">");
                                    if (x > 0)
                                    {
                                        string t1 = t.Substring(1, x - 1);
                                        string t2 = "0000" + t1;
                                        tmpTag += "<" + t2.Substring(t2.Length - 4) + "> ";
                                    }
                                }
                                else
                                    tmpTag += t + " ";
                            }
                        }
                        tmpTag = tmpTag.Trim();
                        if (i == verseParts.Length - 1)
                        {
                            // last word
                            if (tmpTag.EndsWith('.'))
                                tmpTag.Remove(tmpTag.Length - 1, 1);
                            tags.Add(tmpTag);
                        }
                    }
                }
            }

            if (words.Count == (tags.Count + 1)) // last word was not tagged
                tags.Add(string.Empty);
            string[] vWords = words.ToArray();
            string[] vTags = tags.ToArray();
            if (vWords.Length != vTags.Length)
            {
                throw new Exception(string.Format("Word Count = {0}, Tags Count {1}", vWords, vTags.Length));
            }

            // remove <> from tags
            for (int i = 0; i < vTags.Length; i++)
                vTags[i] = vTags[i].Replace("<", "").Replace(">", "");

            Verse verseWords = new Verse();
            for (int i = 0; i < vWords.Length; i++)
            {
                string[] splitTags = vTags[i].Split(' ');
                verseWords[i] = new VerseWord(vWords[i], splitTags, reference);
            }

            bible.Add(reference, verseWords);
            currentVerseCount++;

        }
    }

    internal class ArabicReferences
    {
        /// <summary>
        /// key: Arabic word
        /// value: verse references
        /// </summary>
        public SortedDictionary<string, List<string>> AR = new SortedDictionary<string, List<string>>();
        public void Add(string araWord, string reference)
        {
            if (AR.Keys.Contains(araWord))
            {
                AR[araWord].Add(reference);
            }
            else
            {
                List<string> list = new List<string>();
                list.Add(reference);
                AR.Add(araWord, list);
            }

        }
    }

}
