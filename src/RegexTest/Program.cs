using System.Formats.Tar;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace RegexTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();

            string arabic = @"رجل عاش في زمن الأباء، لم يُذكر إلا في تك ٢٢:​٢٤؛ ابن: ناحور (H5152H) ورئومة (H7208)؛ أخو: طيبة (H2875)، طاحش (H8477)، معكة (H4601)؛ الأخ غير الشقيق لـ: عوص (H5780H)، بوز (H0938)، كيموئيل (H7055)، حيسد (H3777)، حزو (H2375)، بيلداش (H6394)، جدلاف (H3044) وبتوئيل (H1328A) § جاهام = حرقابن ناحور أخي إبراهيم وسريته رؤومة";

            string pattern = @"\:.*\("

            //string translatedLongText = "رجل من سبط يهوذا عاش في زمن المملكة المتحدة، ورد ذكره لأول مرة في ١صم ١٦:​٨؛ ابن: جيسيرجل عاش في زمن المملكة المتحدة، ورد ذكره لأول مرة في   ١صم ٧: ١  أبو: أَلِعَازَار (H0499H) وعُزَّة (H5798A) وأَخِيُو (H0283).";
            //                    1         2        3    4     5       6        7

            //string translatedLongText = "ἀγκάлη,-ης, ἡ (ἄγκος, انحناء ), [في الترجمة السبعينية لـ аел, हेक ;] الذراع المنحنية : راجع: لو ٢:​٢٨ (راجع ἐναγκακίζομαι).";
            string translatedLongText = "آخر [في الترجمة السبعينية لـ ᦴẫẫ Ảẫ ẫ ảẫ ảẫ ảấặ pi.، إلخ. ;] للقفز : راجع: أع ٣:​٨ ١٤:​١٠ ; من الماء لينبع، راجع: يو ٤:​١٤ (ملم، VGT، انظر الكلمة).";
            string englishText = "ἅλλομαι    [in LXX for צָלַח, דָּלַג pi., etc. ;]   to leap :  Ref: Act.3:8 14:10 ; of water, to spring up,  Ref: Jhn.4:14  (MM,  VGT , see word).†  (AS)";

            //string result = program.ApplyLatestHebrewFixes(translatedLongText);
            string result = program.ApplyLatestGreekFixes(translatedLongText, englishText);
            Console.WriteLine("Hello, World!");
        }

        public string ApplyLatestGreekFixes(string translatedLongText, string englishText)
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
                    result = result.Replace(matches[0].Groups[1].Value, matches2[0].Groups[1].Value);
                    result = result.Replace("etc.", "الخ.");
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
                    result = result.Replace(matches[0].Groups[1].Value, matches2[0].Groups[1].Value);
                    result = result.Replace("etc.", "الخ.");
                }

            }



            return result;
        }


        public string ApplyLatestHebrewFixes(string translatedLongText)
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


    }
}