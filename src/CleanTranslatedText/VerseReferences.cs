using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CleanTranslatedText
{
    internal class VerseReferences
    {

        private MainForm mainForm;
        public VerseReferences(MainForm mainForm) {
            this.mainForm = mainForm;
        }

        public void CorrectAbbreviations(string[] arabicLines, bool greek)
        {
            string pattern = @"([\d]{0,1}[ ]*[a-zA-Z\u0620-\u064A]{2,12})[.]{0,1}[ ]*([\d]{1,2}[ ]*\:[ ]*[\d]{1,3})";

            Dictionary<string, string> nameCorrection = otNameCorrection;
            if(greek)
            {
                nameCorrection = ntNameCorrection;
            }

            for (int i = 0; i < arabicLines.Length; i++)
            {
                string arabicLine = arabicLines[i];

                if (i == 149)
                {
                    int x = 0;
                }

                MatchCollection matches = Regex.Matches(arabicLine, pattern);
                if (matches.Count > 0)
                {
                    foreach (Match match in matches)
                    {
                        if (match.Success)
                        {
                            string reference = match.Groups[0].Value; // this is the complete reference
                            string extraRefs = GetExtraReferences(arabicLine, reference);
                            string book = match.Groups[1].Value;
                            string cleanBook = book.Replace(" ", "");
                            if (nameCorrection.ContainsKey(cleanBook))
                            {
                                book = (book[0] == ' '? " ": "") + nameCorrection[cleanBook];
                                string verseNum = match.Groups[2].Value.Replace(" ", "");
                                verseNum = ConvertVerseNumber(verseNum+ extraRefs);
                                arabicLine = arabicLine.Replace(reference + extraRefs, string.Format("{0} {1}", book, verseNum));
                            }
                        }
                    }
                }
                arabicLines[i] = arabicLine;

            }
        }

        public void RemoveApocrypha(string[] arabicLines)
        {
            string pattern = @"([\d]{0,1}[ ]*[a-zA-Z\u0620-\u064A]{2,12})[.]{0,1}[ ]*([\d]{1,2}[ ]*\:[ ]*[\d]{1,3})";

            for (int i = 0; i < arabicLines.Length; i++)
            {
                string arabicLine = arabicLines[i];
                if (i == 337)
                {
                    int x = 0;
                }
                MatchCollection matches = Regex.Matches(arabicLine, pattern);
                if (matches.Count > 0)
                {
                    foreach (Match match in matches)
                    {
                        if (match.Success)
                        {
                            string reference = match.Groups[0].Value; // this is the complete reference
                            string book = match.Groups[1].Value.Replace(" ", "");
                            if (Apocrypha.Contains(book))
                            {
                                // 1. check if it is preceded by Ref
                                Match m = Regex.Match(arabicLine, @"(Ref:[ ]*" + reference + ")");
                                if (m.Success)
                                {
                                    string toRemove = m.Groups[0].Value;
                                    toRemove += GetExtraReferences(arabicLine, toRemove);
                                    arabicLine = arabicLine.Replace(toRemove, "");

                                }
                                else
                                {
                                    m = Regex.Match(arabicLine, @"(المرجع:[ ]*" + reference + ")");
                                    if (m.Success)
                                    {
                                        string toRemove = m.Groups[0].Value;
                                        toRemove += GetExtraReferences(arabicLine, toRemove);
                                        arabicLine = arabicLine.Replace(toRemove, "");
                                    }
                                    else
                                        arabicLine = arabicLine.Replace(reference, "");
                                }
                            }
                        }
                    }
                }
                arabicLines[i] = arabicLine;
            }
        }

        private string GetExtraReferences(string arabicLine, string theReference)
        {
            string extra = string.Empty;

            // are there more verse numbers?
            while (true)
            {
                string reference = theReference + extra;
                int idx = arabicLine.IndexOf(reference + " ");
                if (idx < 0)
                    break;
                // may be 
                string stringToCheck = arabicLine.Substring(idx + reference.Length + 1);
                Match m = Regex.Match(stringToCheck, @"^([0-9\u0660-\u0669]{1,2}\:[0-9\u0660-\u0669]{1,2})");
                if (!m.Success)
                    break;
                extra += (" " + m.Groups[0].Value);
            }

            return extra;    
        }

        private string ConvertVerseNumber(string verseNum)
        {
            return verseNum.
                Replace(":", ":\u200B").
                Replace("0", "٠").
                Replace("1", "١").
                Replace("2", "٢").
                Replace("3", "٣").
                Replace("4", "٤").
                Replace("5", "٥").
                Replace("6", "٦").
                Replace("7", "٧").
                Replace("8", "٨").
                Replace("9", "٩");
        }

        private string[] Apocrypha = {
"1Es",
"1Ma",
"1Mac",
"Mac",
"1أز",
"1أص",
"1إسدرا",
"1اس",
"1ماك",
"1مك",
"2Es",
"2Ma",
"2MA",
"2Mac",
"2أش",
"2ماك",
"2متى",
"2مع",
"2مك",
"٢ماك",
"٢مك",
"3Ma",
"3Mac",
"3ماك",
"3مك",
"4Ma",
"4Mac",
"4ماك",
"4متى",
"4مك",
"Bar",
"Bel",
"Jdt",
"Jdth",
"Pure",
"sir",
"Sir",
"Sol",
"su",
"Su",
"Sus",
"Tob",
"Wis",
"حكم",
"حكم",
"الإنسان",
"الحكمة",
"السيد",
"السير",
"توب",
"حكمة",
"حكيم",
"سيدي",
"سير",
"سي",
"طو",
"طوب",
"طوبى",
"طوبيا",
"فجر",
"فرج",
"بيل",
"سو",
"سوس",
"طب",
"ويسكونسن"
        };

        private Dictionary<string, string> otNameCorrection = new Dictionary<string, string>()
        {
            {"1Sa", "١صم"},
            {"1صم", "١صم"},
            {"2Sa", "٢صم"},
            {"2صم", "٢صم"},
            {"2مل", "٢مل"},
            {"1Ch", "١أخ"},
            {"1أخ", "١أخ"},
            {"1اخ", "١أخ"},
            {"١اخ", "١أخ"},
            {"1أي", "١أخ"},
            {"١أي", "١أخ"},
            {"الساعة", "١أخ"},
            {"1أخبار", "١أخ"},
            {"١أخبار", "١أخ"},
            {"1أيها", "١أخ"},
            {"1الأخبار", "١أخ"},
            {"1Kgs", "١مل"},
            {"2Ch", "٢أخ"},
            {"2أخ", "٢أخ"},
            {"٢اخ", "٢أخ"},
            {"2أخبار", "٢أخ"},
            {"٢أخبار", "٢أخ"},
            {"2أي", "٢أخ"},
            {"٢أي", "٢أخ"},
            {"2الأخبار", "٢أخ"},
            {"Jer", "إر"},
            {"mari", "إر"},
            {"إر", "إر"},
            {"إرميا", "إر"},
            {"Est", "أس"},
            {"أإست", "أس"},
            {"إست", "أس"},
            {"Isa", "إش"},
            {"إش", "إش"},
            {"إشعياء", "إش"},
            {"عيسى", "إش"},
            {"pro", "أم"},
            {"Pro", "أم"},
            {"Pr", "أم"},
            {"أم", "أم"},
            {"أمثال", "أم"},
            {"الأمثال", "أم"},
            {"Job", "أي"},
            {"أيوب", "أي"},
            {"Deu", "تث"},
            {"تث", "تث"},
            {"تثنية", "تث"},
            {"Gen", "تك"},
            {"التكوين", "تك"},
            {"تك", "تك"},
            {"تكوين", "تك"},
            {"Ecc", "جا"},
            {"الجامعة", "جا"},
            {"جامعة", "جا"},
            {"Hab", "حب"},
            {"هاب", "حب"},
            {"Hag", "حج"},
            {"Eze", "حز"},
            {"حز", "حز"},
            {"حزقي", "حز"},
            {"حزقيال", "حز"},
            {"Exo", "خر"},
            {"خروج", "خر"},
            {"dan", "دا"},
            {"Dan", "دا"},
            {"دان", "دا"},
            {"Rut", "را"},
            {"Ruth", "را"},
            {"Zec", "زك"},
            {"زا", "زك"},
            {"زك", "زك"},
            {"زكريا", "زك"},
            {"راع", "را"},
            {"Zep", "صف"},
            {"Amo", "عا"},
            {"عامو", "عا"},
            {"عاموس", "عا"},
            {"عمو", "عا"},
            {"Am", "عا"},
            {"Num", "عد"},
            {"رقم", "عد"},
            {"عدد", "عد"},
            {"نو", "عد"},
            {"عز", "عز"},
            {"Ezr", "عز"},
            {"أوبا", "عو"},
            {"Jdg", "قض"},
            {"القض", "قض"},
            {"القضاء", "قض"},
            {"القضاة", "قض"},
            {"قض", "قض"},
            {"Lev", "لا"},
            {"اللاويين", "لا"},
            {"لا", "لا"},
            {"لاويين", "لا"},
            {"ليف", "لا"},
            {"La", "مرا"},
            {"Psa", "مز"},
            {"مز", "مز"},
            {"مزمور", "مز"},
            {"Mal", "مل"},
            {"ملاخي", "مل"},
            {"tar", "مل"},
            {"Mic", "مي"},
            {"ميكرفون", "مي"},
            {"Nam", "نا"},
            {"نام", "نا"},
            {"Neh", "نح"},
            {"نيه", "نح"},
            {"نحميا", "نح"},
            {"نيح", "نح"},
            {"Sng", "نش"},
            {"سنغ", "نش"},
            {"نش", "نش"},
            {"Hos", "هو"},
            {"هو", "هو"},
            {"Ho", "هو"},
            {"هوس", "هو"},
            {"هوش", "هو"},
            {"هوص", "هو"},
            {"Jos", "يش"},
            {"يش", "يش"},
            {"يشوع", "يش"},
            {"Jol", "يوئ"},
            {"يوئيل", "يوئ"},
            {"يول", "يوئ"},
            {"Jon", "يون"},
            {"يون", "يون"},
            {"يونان", "يون"},
            {"مات", "مت"},
            {"متى", "مت"},
            {"لوقا", "لو"}
        };

        private Dictionary<string, string> ntNameCorrection = new Dictionary<string, string>()
        {
            {"1Pe", "١بط"},
            {"1بط", "١بط"},
            {"1بطرس", "١بط"},
            {"١بطرس", "١بط"},
            {"1بي", "١بط"},
            {"١بط", "١بط"},
            {"1Th", "١تس"},
            {"1تث", "١تس"},
            {"1تس", "١تس"},
            {"١تس", "١تس"},
            {"1تسالونيكي", "١تس"},
            {"١تسالونيكي", "١تس"},
            {"1Ti", "١تي"},
            {"1تي", "١تي"},
            {"1Ch", "١أخ"},
            {"1أخ", "١أخ"},
            {"1أخبار", "١أخ"},
            {"١أخبار", "١أخ"},
            {"1أيها", "١أخ"},
            {"1الأخبار", "١أخ"},
            {"1Ki", "١صم"},
            {"1صم", "١صم"},
            {"1مل", "١صم"},
            {"1ملوك", "١صم"},
            {"1co", "١كو"},
            {"1Co", "١كو"},
            {"1كو", "١كو"},
            {"1Kgs", "١مل"},
            {"3Ki", "١مل"},
            {"3كي", "١مل"},
            {"3مل", "١مل"},
            {"٣مل", "١مل"},
            {"3ملوك", "١مل"},
            {"1Jn", "١يو"},
            {"1Jo", "١يو"},
            {"1يو", "١يو"},
            {"١يوحنا", "١يو"},
            {"1يوحنا", "١يو"},
            {"2Ch", "٢أخ"},
            {"2أخ", "٢أخ"},
            {"2أخبار", "٢أخ"},
            {"٢أخبار", "٢أخ"},
            {"2أي", "٢أخ"},
            {"2الأخبار", "٢أخ"},
            {"2Pe", "٢بط"},
            {"2PE", "٢بط"},
            {"2بط", "٢بط"},
            {"2بطرس", "٢بط"},
            {"2بي", "٢بط"},
            {"2Th", "٢تس"},
            {"2تث", "٢تس"},
            {"2تس", "٢تس"},
            {"2تسالونيكي", "٢تس"},
            {"٢تسالونيكي", "٢تس"},
            {"2Ti", "٢تي"},
            {"2تي", "٢تي"},
            {"2تيم", "٢تي"},
            {"2Ki", "٢صم"},
            {"2مل", "٢صم"},
            {"2Co", "٢كو"},
            {"2كو", "٢كو"},
            {"4Ki", "٢مل"},
            {"4مل", "٢مل"},
            {"4ملوك", "٢مل"},
            {"٤مل", "٢مل"},
            {"٤ملوك", "٢مل"},
            {"2يوحنا", "٢يو"},
            {"Jer", "إر"},
            {"mari", "إر"},
            {"إر", "إر"},
            {"إرميا", "إر"},
            {"Est", "أس"},
            {"أإست", "أس"},
            {"إست", "أس"},
            {"Isa", "إش"},
            {"إش", "إش"},
            {"إشعياء", "إش"},
            {"عيسى", "إش"},
            {"Ac", "أع"},
            {"Act", "أع"},
            {"ACT", "أع"},
            {"أع", "أع"},
            {"أعمال", "أع"},
            {"اع", "أع"},
            {"الفصل", "أع"},
            {"الفعل", "أع"},
            {"القانون", "أع"},
            {"فصل", "أع"},
            {"فعل", "أع"},
            {"قانون", "أع"},
            {"Eph", "أف"},
            {"أف", "أف"},
            {"أفسس", "أف"},
            {"اف", "أف"},
            {"pro", "أم"},
            {"Pro", "أم"},
            {"أم", "أم"},
            {"أمثال", "أم"},
            {"الأمثال", "أم"},
            {"Job", "أي"},
            {"أيوب", "أي"},
            {"Deu", "تث"},
            {"تث", "تث"},
            {"تثنية", "تث"},
            {"Gen", "تك"},
            {"التكوين", "تك"},
            {"تك", "تك"},
            {"تكوين", "تك"},
            {"Tit", "تي"},
            {"الرسالة", "تي"},
            {"العنوان", "تي"},
            {"تي", "تي"},
            {"تيط", "تي"},
            {"Ecc", "جا"},
            {"الجامعة", "جا"},
            {"جامعة", "جا"},
            {"Hab", "حب"},
            {"هاب", "حب"},
            {"Hag", "حج"},
            {"Eze", "حز"},
            {"حز", "حز"},
            {"حزقي", "حز"},
            {"حزقيال", "حز"},
            {"Exo", "خر"},
            {"خروج", "خر"},
            {"dan", "دا"},
            {"Dan", "دا"},
            {"دان", "دا"},
            {"Rut", "را"},
            {"Ruth", "را"},
            {"rom", "رو"},
            {"Rom", "رو"},
            {"رو", "رو"},
            {"روم", "رو"},
            {"رومية", "رو"},
            {"Rev", "رؤ"},
            {"الرؤيا", "رؤ"},
            {"القس", "رؤ"},
            {"رؤ", "رؤ"},
            {"رؤى", "رؤ"},
            {"رؤيا", "رؤ"},
            {"Zec", "زك"},
            {"زا", "زك"},
            {"زك", "زك"},
            {"زكريا", "زك"},
            {"Zep", "صف"},
            {"Amo", "عا"},
            {"عامو", "عا"},
            {"عاموس", "عا"},
            {"عمو", "عا"},
            {"heb", "عب"},
            {"Heb", "عب"},
            {"عب", "عب"},
            {"عبرانيين", "عب"},
            {"Num", "عد"},
            {"رقم", "عد"},
            {"عدد", "عد"},
            {"عز", "عز"},
            {"Gal", "غل"},
            {"GAL", "غل"},
            {"غال", "غل"},
            {"غل", "غل"},
            {"غلا", "غل"},
            {"غلاطية", "غل"},
            {"فاي", "فل"},
            {"فيفي", "فل"},
            {"php", "في"},
            {"Php", "في"},
            {"فب", "في"},
            {"في", "في"},
            {"فيب", "في"},
            {"فيبي", "في"},
            {"فيلبي", "في"},
            {"Jdg", "قض"},
            {"القض", "قض"},
            {"القضاء", "قض"},
            {"القضاة", "قض"},
            {"قض", "قض"},
            {"Col", "كو"},
            {"العقيد", "كو"},
            {"العمود", "كو"},
            {"عقيد", "كو"},
            {"عمود", "كو"},
            {"قول", "كو"},
            {"كو", "كو"},
            {"كول", "كو"},
            {"كولوسي", "كو"},
            {"Lev", "لا"},
            {"اللاويين", "لا"},
            {"لا", "لا"},
            {"لاويين", "لا"},
            {"ليف", "لا"},
            {"Luk", "لو"},
            {"Luke", "لو"},
            {"لو", "لو"},
            {"لوقا", "لو"},
            {"لوك", "لو"},
            {"mat", "مت"},
            {"Mat", "مت"},
            {"MAT", "مت"},
            {"مات", "مت"},
            {"مت", "مت"},
            {"متى", "مت"},
            {"متي", "مت"},
            {"8عن", "مر"},
            {"Mrk", "مر"},
            {"MRK", "مر"},
            {"مرقس", "مر"},
            {"مرك", "مر"},
            {"La", "مرا"},
            {"Psa", "مز"},
            {"مز", "مز"},
            {"مزمور", "مز"},
            {"Mal", "مل"},
            {"tar", "مل"},
            {"Mic", "مي"},
            {"ميكرفون", "مي"},
            {"Nam", "نا"},
            {"Neh", "نح"},
            {"نيه", "نح"},
            {"نحميا", "نح"},
            {"Sng", "نش"},
            {"نش", "نش"},
            {"Hos", "هو"},
            {"هو", "هو"},
            {"هوس", "هو"},
            {"هوش", "هو"},
            {"هوص", "هو"},
            {"Jos", "يش"},
            {"يش", "يش"},
            {"يشوع", "يش"},
            {"Jas", "يع"},
            {"ياس", "يع"},
            {"يع", "يع"},
            {"يعقوب", "يع"},
            {"يهوذا", "يه"},
            {"jhn", "يو"},
            {"Jhn", "يو"},
            {"JHN", "يو"},
            {"Jn", "يو"},
            {"يو", "يو"},
            {"يوحنا", "يو"},
            {"Jol", "يوئ"},
            {"يوئيل", "يوئ"},
            {"يول", "يوئ"},
            {"Jon", "يون"},
            {"يون", "يون"},
            {"يونان", "يون"}
        }; 

    }
}
