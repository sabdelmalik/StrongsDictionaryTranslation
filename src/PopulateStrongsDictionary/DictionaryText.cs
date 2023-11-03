using Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic.ApplicationServices;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PopulateStrongsDictionary
{
    internal class DictionaryText : TableBase
    {
        private MainForm mainForm;

        private Dictionary<char, string> arabicLetter2Eng = new Dictionary<char, string>();
        public DictionaryText(MainForm mainForm) : base(mainForm)
        { 
            this.mainForm = mainForm;

            arabicLetter2Eng.Add('ا', "A");
            arabicLetter2Eng.Add('أ', "A");
            arabicLetter2Eng.Add('ب', "B");
            arabicLetter2Eng.Add('ج', "C");
            arabicLetter2Eng.Add('د', "D");
            arabicLetter2Eng.Add('ه', "E");
            arabicLetter2Eng.Add('ف', "F");
            arabicLetter2Eng.Add('ز', "G");
            arabicLetter2Eng.Add('ح', "H");
        }

        internal bool ClearTables(NpgsqlDataSource dataSource)
        {
            bool result = false;
            result = ClearTable("dictionary_translation", dataSource);
            if (result)
            {
                result = this.ClearTable("strongs_numbers", dataSource);
            }

            return result;
        }


        /// <summary>
        /// 
        /// Note: **** disambiguation character (dstrong) is case sensetive
        /// </summary>
        /// <param name="sourceText"></param>
        /// <param name="dataSource"></param>
        internal void LoadSourceTextIntoDB(string sourceText, NpgsqlDataSource dataSource)
        {
            string cmdText = string.Empty;

            string strongsNumber = string.Empty;
            string dStrong = string.Empty;
            string originalWord = string.Empty;
            string englishWord = string.Empty;
            string longText = string.Empty;
            string shortText = string.Empty;

            try
            {
                var command = dataSource.CreateCommand();

                string tableName = "strongs_numbers";

                using (StreamReader sr = new StreamReader(sourceText))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] lineParts = line.Split('\t');
                        if (lineParts[0].Length == 5)
                        {
                            strongsNumber = lineParts[0];
                            dStrong = "";
                        }
                        else if (lineParts[0].Length == 6)
                        {
                            strongsNumber = lineParts[0].Substring(0,5);
                            dStrong = lineParts[0].Substring(5);   // Note: **** disambiguation character (dstrong) is case sensetive
                        }
                        originalWord = lineParts[1];
                        englishWord = lineParts[2];
                        shortText = lineParts[3].Trim();
                        longText = lineParts[4].Trim();

                        cmdText = "INSERT INTO public.\"" + tableName + "\" " +
                                   "(strongs_number, d_strong, original_word, english_translation, long_text, short_text) " + //, translated_word, translated_short_text) " +
                                   "VALUES " +
                                   string.Format("('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');", //, '{6}', '{7}');",
                                   strongsNumber,
                                   dStrong,
                                   originalWord,
                                   englishWord.Replace("'", "''"),
                                   longText.Replace("'", "''"),
                                   shortText.Replace("'", "''")
                                   );

                        command.CommandText = cmdText;
                        if (strongsNumber != string.Empty)
                            command.ExecuteNonQuery();


                    }
                }

            }
            catch(Exception ex)
            {
                mainForm.Trace(cmdText, Color.Red);
                mainForm.TraceError(MethodBase.GetCurrentMethod().Name, ex.ToString());
            }
            mainForm.Trace("Strong's numbers population Done!", Color.Green);

        }

        internal void LoadTranslatedTextIntoDB(int langId, string translatedText, NpgsqlDataSource dataSource)
        {
            string cmdText = string.Empty;

            string strongsNumber = string.Empty;
            string dStrong = string.Empty;
            string originalWord = string.Empty;
            string translatedWord = string.Empty;
            string translatedLongText = string.Empty;
            string translatedShortText = string.Empty;

            try
            {
                var command = dataSource.CreateCommand();

                string tableName = "dictionary_translation";

                using (StreamReader sr = new StreamReader(translatedText))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        
                        string[] lineParts = line.Split('\t');
                        if (lineParts[0].Length == 5)
                        {
                            strongsNumber = lineParts[0];
                            dStrong = "";
                        }
                        else if (lineParts[0].Length == 6)
                        {
                            strongsNumber = lineParts[0].Substring(0, 5);
                            dStrong = lineParts[0].Substring(5); ;
                        }
                        originalWord = lineParts[1];
                        translatedWord = lineParts[2];
                        translatedShortText = lineParts[3].Trim();
                        translatedLongText = lineParts[4].Trim();

                        // Apply Latest Fixes
                        if (strongsNumber[0] == 'G')
                        {
                            translatedLongText = ApplyLatestGreekFixes(translatedLongText, "");
                        }
                        else if (strongsNumber[0] == 'H')
                        {
                            translatedLongText = ApplyLatestHebrewFixes(translatedLongText, "" );
                        }
                        else
                        {
                            mainForm.TraceError(MethodBase.GetCurrentMethod().Name, "Strong's number does not start with G or H");
                            return;
                        }

                        if (!string.IsNullOrEmpty(dStrong) && dStrong[0] > 0x7F)
                        {
                            dStrong = arabicLetter2Eng[dStrong[0]];
                        }

                        cmdText = "INSERT INTO public.\"" + tableName + "\" " +
                               "(language_id, strongs_number, d_strong, translated_word, translated_long_text, translated_short_text) " + //, translated_word, translated_short_text) " +
                               "VALUES " +
                               string.Format("({0}, '{1}', '{2}', '{3}', '{4}', '{5}');", //, '{6}', '{7}');",
                               langId,
                               strongsNumber,
                               dStrong.ToUpper(),
                               translatedWord.Replace("'", "''"),
                               Arabic.AdjustText(translatedLongText).Replace("'", "''"),
                               Arabic.AdjustText(translatedShortText).Replace("'", "''")
                               );

                        command.CommandText = cmdText;
                        if (strongsNumber != string.Empty)
                            command.ExecuteNonQuery();


                    }
                }

            }
            catch (Exception ex)
            {
                mainForm.Trace(cmdText, Color.Red);
                mainForm.TraceError(MethodBase.GetCurrentMethod().Name, ex.ToString());
            }
            mainForm.Trace("Translation population Done!", Color.Green);
        }

        private string ApplyLatestGreekFixes(string translatedLongText, string englishText)
        {
            string result = translatedLongText;

            string strongsPattern = @"\[في الترجمة السبعينية لـ[ ]*([\u0001-\u9999]*);\]";
            string strongsPattern2 = @"\[in LXX for[ ]*([ .,a-z\u0590-\u05FF]*);\]";

            MatchCollection matches = Regex.Matches(result, strongsPattern);
            if (matches.Count > 0)
            {
                MatchCollection matches2 = Regex.Matches(englishText, strongsPattern2);
                if (matches2.Count > 0)
                {
                    if (!string.IsNullOrEmpty(matches[0].Groups[1].Value) && !string.IsNullOrEmpty(matches2[0].Groups[1].Value))
                    {
                        result = result.Replace(matches[0].Groups[1].Value, matches2[0].Groups[1].Value);
                        result = result.Replace("etc.", "الخ.");
                    }
                }

            }

            strongsPattern = @"\[في الترجمة السبعينية بشكل رئيسي لـ[ ]*([\u0001-\u9999]*);\]";
            strongsPattern2 = @"\[in LXX chiefly for[ ]*([ .,a-z\u0590-\u05FF]*);\]";

            matches = Regex.Matches(result, strongsPattern);
            if (matches.Count > 0)
            {
                MatchCollection matches2 = Regex.Matches(englishText, strongsPattern2);
                if (matches2.Count > 0)
                {
                    if (!string.IsNullOrEmpty(matches[0].Groups[1].Value) && !string.IsNullOrEmpty(matches2[0].Groups[1].Value))
                    {
                        result = result.Replace(matches[0].Groups[1].Value, matches2[0].Groups[1].Value);
                        result = result.Replace("etc.", "الخ.");
                    }
                }

            }

            return result;
        }

        private string ApplyLatestHebrewFixes(string translatedLongText, string englishText)
        {
            string result = translatedLongText;

           // result = Regex.Replace(result,@"^[1]\)", "(١)")
                //.Replace("فاسيا", "فَاسِيح")
                //.Replace("تحينة", "تَحِنَّة")
                //.Replace("زيرور", "صَرُورَ")
                //.Replace("إحي", "إِيحِي")
                //.Replace("موبيم", "مُفِّيم")
                //.Replace("جيدائيل", "يَدِيعَئِيل")
                //.Replace("طحان", "تَاحَن")
                //.Replace("آزيل", "آصِيل")
                //.Replace("إليعاد", "أَلِعَاد")
                //.Replace("زمن المملكة المتحدة", "عصر المملكة الموحدة")
                //.Replace("زمن المملكة المنقسمة", "عصر المملكة المنقسمة")
                //.Replace("زمن الاباء", "عصر الاباء")
                //.Replace("عسقلان", "أَشْقَلُون")
                //.Replace("الخليل", "حَبْرُون") 
                //.Replace("المنفى", "السبي")
                //.Replace("القدس", "أورشليم")
                //.Replace("زمن السبي والعودةمنه،", "زمن السبي والعودة منه،")
                //.Replace("البطاركة", "الأباء")
                //.Replace("يعيش في", "عاش في")
                //.Replace("تعيش في", "عاشت في")
                //.Replace("متزوج من", "زوج")
                //.Replace("الأخ غير الأخو", "الأخ غير الشقيق")
                //.Replace("أخ غير الأخو", "أخ غير شقيق")
                ;

            return result;
        }

        private string ApplyLatestHebrewFixesOld(string translatedLongText)
        {
            string result = translatedLongText;

            result = result
                .Replace("الملكية المتحدة", "المملكة المتحدة")
                .Replace("الملكية المنقسمة", "المملكة المنقسمة")
                .Replace("شقيق:", "أخو:")
                .Replace("رجل يعيش", "رجل عاش");

            #region Strong's numbers
            string strongsPattern = @"[hHهح](\d\d\d\d)([a-zA-Z\u0620-\u064A]{0,1})";

            MatchCollection matches = Regex.Matches(result, strongsPattern);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    string number = match.Groups[1].Value;
                    string suffix = match.Groups[2].Value;
                    if(!string.IsNullOrEmpty(suffix ))
                    {
                        suffix = suffix
                            .Replace("أ", "A")
                            .Replace("ب", "B")
                            .Replace("ج", "C")
                            .Replace("د", "D")
                            .Replace("ف", "F")
                            .Replace("ز", "G")
                            .Replace("ه", "G")
                            .Replace("ح", "H")
                            .Replace("ط", "I")
                            .Replace("ك", "K")
                            .Replace("ي", "J")
                            .ToUpper();
                    }
                    string replacement = string.Format("H{0}{1}", number, suffix);
                    result = result.Replace(match.Groups[0].Value, replacement);
                }
            }

            // between parentheses
            strongsPattern = @"\([ ]*[hHهح][ ]*(\d\d\d\d)[ ]*([a-zA-Z\u0620-\u064A]{0,1})[ ]*\)";

            matches = Regex.Matches(result, strongsPattern);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    string number = match.Groups[1].Value;
                    string suffix = match.Groups[2].Value;
                    if (!string.IsNullOrEmpty(suffix))
                    {
                        suffix = suffix
                            .Replace("أ", "A")
                            .Replace("ب", "B")
                            .Replace("ج", "C")
                            .Replace("د", "D")
                            .Replace("ف", "F")
                            .Replace("ز", "G")
                            .Replace("ه", "G")
                            .Replace("ح", "H")
                            .Replace("ط", "I")
                            .Replace("ك", "K")
                            .Replace("ي", "J")
                            .ToUpper();
                    }
                    string replacement = string.Format("(H{0}{1})", number, suffix);
                    result = result.Replace(match.Groups[0].Value, replacement);
                }
            }

            #endregion Strong's numbers

            #region Hebrew verb benyamim
            result = result
        .Replace("(Qal)", "(صيغة فَعَلَ)")     // Pa'al / Qal
        .Replace("(قال)", "(صيغة فَعَلَ)")     // Pa'al / Qal
        .Replace("(بال)", "(صيغة فَعَلَ)")     // Pa'al / Qal
        .Replace("(بيل)", "(صيغة بيل)")      // Pi'el
        .Replace("(هيفيل)", "(صيغة هيفيل)")  // Hiph'el
        .Replace("(قال)", "(صيغة قال)")      // Hitpa'el
        .Replace("(هوفال)", "(صيغة هوفال)")      // Huph'al
        .Replace("(هوبال)", "(صيغة هوفال)")      // Huph'al
        .Replace("(بوال)", "(صيغة بوال)")   // Pu'al
        .Replace("(نيفال)", "(صيغة نيفال)")  // Niph'al
        .Replace("(الشكل)", "(مجازيا)");
            #endregion Hebrew verb benyamim

            #region Numbers and subnumbers

            //                    1         2        3    4     5       6        7
            string pattern1 = @"([ ]+)([\d]{1,2})([ ]*)([^\):])([ ]*)([\d]{1,2})([ ]*)([\) ])"; // 1 a 1
                                                                                                //                    1        2       3         4      5
            string pattern2 = @"[^:]([ ]+)([\d]{1,2})([ ]*)([^\):]{0,1})([ ]*)([\) ])"; // 1) and 1 a)

            matches = Regex.Matches(result, pattern1);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    int possibleStrongNum = 0;
                    string possibleStrong = match.Groups[0].Value;
                    possibleStrong = possibleStrong.Substring(0, possibleStrong.Length - 1);
                    if (possibleStrong.Length >= 3 && int.TryParse(possibleStrong, out possibleStrongNum))
                        continue;

                    string leadingBracket = string.IsNullOrEmpty(match.Groups[1].Value) ? "(" : "   (";

                    string number = match.Groups[2].Value
                        .Replace("0", "٠")
                        .Replace("1", "١")
                        .Replace("2", "٢")
                        .Replace("3", "٣")
                        .Replace("4", "٤")
                        .Replace("5", "٥")
                        .Replace("6", "٦")
                        .Replace("7", "٧")
                        .Replace("8", "٨")
                        .Replace("9", "٩");
                    string secondSpace = string.Empty;
                    string subNumber = match.Groups[4].Value;
                    string subNumber2 = match.Groups[6].Value;
                    string thirdSpace = string.Empty;
                    if (!string.IsNullOrEmpty(subNumber))
                    {
                        secondSpace = "-";
                        subNumber = subNumber.ToLower()
                        .Replace("a", "أ")
                        .Replace("b", "ب")
                        .Replace("c", "ت")
                        .Replace("d", "ث")
                        .Replace("e", "ج")
                        .Replace("f", "ح")
                        .Replace("g", "خ")
                        .Replace("h", "د")
                        .Replace("i", "ذ")
                        .Replace("j", "ر");
                    }

                    if (!string.IsNullOrEmpty(subNumber2))
                    {
                        thirdSpace = "​\u200B";  // ZERO WIDTH SPACE
                        subNumber2 = subNumber2
                       .Replace("0", "٠")
                       .Replace("1", "١")
                       .Replace("2", "٢")
                       .Replace("3", "٣")
                       .Replace("4", "٤")
                       .Replace("5", "٥")
                       .Replace("6", "٦")
                       .Replace("7", "٧")
                       .Replace("8", "٨")
                       .Replace("9", "٩");

                    }
                    string replacement = string.Format("{0}{1}{2}{3}{4}{5})",
                        leadingBracket, number, secondSpace, subNumber, thirdSpace, subNumber2);
                    result = result.Replace(match.Groups[0].Value, replacement);
                    //result = Regex.Replace(result, match.Groups[0].Value, replacement);
                }
            }

            matches = Regex.Matches(result, pattern2);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    int possibleStrongNum = 0;
                    string possibleStrong = match.Groups[0].Value;
                    possibleStrong = possibleStrong.Substring(0, possibleStrong.Length - 1);
                    if (possibleStrong.Length >= 3 && int.TryParse(possibleStrong, out possibleStrongNum))
                        continue;

                    string leadingBracket = string.IsNullOrEmpty(match.Groups[1].Value) ? "(" : "   (";
                    string number = match.Groups[2].Value
                        .Replace("0", "٠")
                        .Replace("1", "١")
                        .Replace("2", "٢")
                        .Replace("3", "٣")
                        .Replace("4", "٤")
                        .Replace("5", "٥")
                        .Replace("6", "٦")
                        .Replace("7", "٧")
                        .Replace("8", "٨")
                        .Replace("9", "٩");
                    string secondSpace = string.Empty;
                    string subNumber = match.Groups[4].Value;
                    string thirdSpace = string.Empty;
                    if (!string.IsNullOrEmpty(subNumber))
                    {
                        secondSpace = "-";
                        subNumber = subNumber.ToLower()
                        .Replace("a", "أ")
                        .Replace("b", "ب")
                        .Replace("c", "ت")
                        .Replace("d", "ث")
                        .Replace("e", "ج")
                        .Replace("f", "ح")
                        .Replace("g", "خ")
                        .Replace("h", "د")
                        .Replace("i", "ذ")
                        .Replace("j", "ر");
                    }
                    string replacement = string.Format("{0}{1}{2}{3})",
                        leadingBracket, number, secondSpace, subNumber);
                    int x = result.IndexOf(match.Groups[0].Value);
                    result = result.Replace(match.Groups[0].Value, replacement);
                }
            }

            #endregion Numbers and subnumbers

            return result;
        }


        private class UpdatedTranslation
        {
            public int Language { get; set; }
            public string Strongs { get; set; }
            public string dStrong { get; set; }
            public string Word { get; set; }
            public string Translation { get; set; }
        }

        internal void UpdateTranslation(NpgsqlDataSource dataSource)
        {
            mainForm.Trace("Updating  Translation ...", Color.Green);

            string cmdText = string.Empty;

            List<UpdatedTranslation> translations = new List<UpdatedTranslation>();

            try
            {
                var command = dataSource.CreateCommand();

                cmdText = "SELECT" +
                    " translation.language_id," +
                    " translation.strongs_number," +
                    " translation.d_strong," +
                    " translation.translated_word," +
                    " translation.translated_long_text," +
                    " strongs.long_text" +
                    " FROM public.\"strongs_numbers\" strongs" +
                    " INNER JOIN public.\"dictionary_translation\" translation" +
                    " ON strongs.strongs_number = translation.strongs_number" +
                    " AND strongs.d_strong = translation.d_strong;";

                command.CommandText = cmdText;
                NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    UpdatedTranslation translation = new UpdatedTranslation();
                    translation.Language = (int)reader[0];
                    translation.Strongs = (string)reader[1];
                    translation.dStrong = (string)reader[2];
                    translation.Word = (string)reader[3];
                    string englishText = (string)reader[5];

                    if (translation.Strongs[0] == 'G')
                    {
                        translation.Translation = ApplyLatestGreekFixes((string)reader[4], englishText);
                    }
                    else if (translation.Strongs[0] == 'H')
                    {
                        translation.Translation = ApplyLatestHebrewFixes((string)reader[4], englishText);
                    }
                    else
                    {
                        mainForm.TraceError(MethodBase.GetCurrentMethod().Name, "Strong's number does not start with G or H");
                        translation.Translation = "Sync Error Strong = + translation.Strongs";
                    }

                    translations.Add(translation);
                }
                reader.Close();
                reader.DisposeAsync();

                foreach (var translation in translations)
                {
                    string tableName = "dictionary_translation";
                    cmdText = "UPDATE public.\"" + tableName + "\" " +
                            string.Format("SET translated_word='{0}', translated_long_text='{1}' ",
                            translation.Word.Replace("'", "''"),
                            translation.Translation.Replace("'", "''")) +
                            string.Format("WHERE strongs_number='{0}' AND d_strong='{1}' AND language_id={2};",
                            translation.Strongs, translation.dStrong, translation.Language);
                    command.CommandText = cmdText;
                    command.ExecuteNonQuery();
                }

                mainForm.Trace("Update  Translation Done!", Color.Green);

            }
            catch (Exception ex)
            {
                mainForm.Trace(cmdText, Color.Red);
                mainForm.TraceError(MethodBase.GetCurrentMethod().Name, ex.ToString());
            }


        }

        internal void SyncUpdatedTranslations(NpgsqlDataSource dataSource)
        {

            mainForm.Trace("Sync Updated Translations Started ...", Color.Green);

            string cmdText = string.Empty;

            List< UpdatedTranslation > translations = new List< UpdatedTranslation >();

            try
            {
                var command = dataSource.CreateCommand();

                cmdText = "SELECT language_id, strongs_number,d_strong,translated_word,translated_text FROM public.\"updated_translation\";";

                command.CommandText = cmdText;
                NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    UpdatedTranslation translation = new UpdatedTranslation();
                    translation.Language = (int)reader[0];
                    translation.Strongs = (string)reader[1];
                    translation.dStrong = (string)reader[2];
                    translation.Word = (string)reader[3];
                    if (string.IsNullOrEmpty(translation.Word) || translation.Word.Contains("??"))
                        continue;
                    if (translation.Strongs[0] == 'G')
                    {
                        translation.Translation = ApplyLatestGreekFixes((string)reader[4], "");
                    }
                    else if (translation.Strongs[0] == 'H')
                    {
                        translation.Translation = ApplyLatestHebrewFixes((string)reader[4], "");
                    }
                    else
                    {
                        mainForm.TraceError(MethodBase.GetCurrentMethod().Name, "Strong's number does not start with G or H");
                        translation.Translation = "Sync Error Strong = + translation.Strongs";
                    }

                    translations.Add(translation);
                }
                reader.Close();
                reader.DisposeAsync();

                foreach ( var translation in translations )
                {
                    string tableName = "dictionary_translation";
                    cmdText = "UPDATE public.\"" + tableName + "\" " +
                            string.Format("SET translated_word='{0}', translated_long_text='{1}' ",
                            translation.Word.Replace("'", "''"),
                            translation.Translation.Replace("'", "''")) +
                            string.Format("WHERE strongs_number='{0}' AND d_strong='{1}' AND language_id={2};",
                            translation.Strongs, translation.dStrong, translation.Language);
                    command.CommandText = cmdText;
                    command.ExecuteNonQuery();
                }

                mainForm.Trace("Sync Updated Translations Done!", Color.Green);


            }
            catch (Exception ex)
            {
                mainForm.Trace(cmdText, Color.Red);
                mainForm.TraceError(MethodBase.GetCurrentMethod().Name, ex.ToString());
            }
        }

    }
}
