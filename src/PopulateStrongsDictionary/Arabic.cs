using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PopulateStrongsDictionary
{
    internal class Arabic
    {
        public static string AdjustText(string text)
        {
            return text;
            string result = text.ToLower().
                Replace("الاسم الصحيح", "إسم علم").
                Replace("المرجع", "انظر").
                Replace("مرجع", "انظر").
            Replace("mk", "مرقس").
            Replace("mk", "مرقس").
                Replace("act", "أع").
                Replace("روم ", "رو ").
                Replace("1كو", "١كو").
                Replace("1 كو", "١كو").
                Replace("1co", "١كو").
                Replace("2 كو", "٢كو").
                Replace("2كو", "٢كو").
                Replace("2co", "٢كو").
                Replace("غلا ", "غل ").
                Replace("php", "فيلبي").
            Replace("col", "كولوسي").
                Replace("1th", "١تس").
                Replace("2th", "٢تس").
                 Replace("1 تي", "١تي").
                 Replace("1Ti", "١تي").
                Replace("2 تي", "٢تي").
                Replace("2ti", "٢تي").
                Replace("ti", "تي").
            Replace("phlm.", "فليمون").
            Replace("١ بي", "١بط").
            Replace("2 بط", "٢بط").
            Replace("2pe", "٢بط").
            Replace("1يو", "١يو").
            Replace("2ي", "٢يو").
            Replace("(AS)", "").
            Replace("indecl.;", "").
            Replace("indecl.", "").
            Replace("indecl,", "").
            Replace("indecl", "").
            Replace("†", "").Trim();


            int idx = result.IndexOf("قانون");
            while (idx > 0)
            {
                if ((result.Length > (idx + 6)) && char.IsDigit(result[idx + 6]))
                {
                    result = result.Substring(0, idx) + "أعمال" + result.Substring(idx + 5);
                    idx = result.IndexOf("قانون", idx + 6);
                }
                else
                    break;
            }

            idx = result.IndexOf("القس");
            while (idx > 0)
            {
                if ((result.Length > (idx + 5)) && char.IsDigit(result[idx + 5]))
                {
                    result = result.Substring(0, idx) + "رؤيا" + result.Substring(idx + 4);
                    idx = result.IndexOf("القس", idx + 4);
                }
                else
                    break;
            }

            // fix strong numbers where H & G were translated to Arabic letters
            while (true)
            {
                // convert Gen.1.1 to Gen 1:1
                Match match = Regex.Match(result, @"([حhgهز])[ ]{0,1}([0-9]{4})[ ]{0,1}([a-z\u0620-\u064A]{0,1})");
                if (!match.Success) break;

                string x = match.Groups[1].Value;
                string y = match.Groups[2].Value;
                string z = match.Groups[3].Value;

                switch (x)
                {
                    case "h":
                    case "ح":
                    case "ه": y = "H" + y; break;
                    case "g":
                    case "ز": y = "G" + y; break;
                }
                if (!string.IsNullOrEmpty(z))
                {
                    if (z[0] >= 'a' && z[0] <= 'z')
                        y += z.ToUpper();
                    else
                        switch (z)
                        {
                            case "أ": y += "A"; break;
                            case "ب": y += "B"; break;
                            case "ج": y += "C"; break;
                            case "د": y += "D"; break;
                            case "ف": y += "F"; break;
                            case "ز": y += "G"; break;
                            case "ه":
                            case "ح": y += "H"; break;
                            case "ط": y += "I"; break;
                            case "ك": y += "K"; break;
                            default: y += z; break;
                        }

                }


                result = result.Replace(match.Groups[0].Value, y);
            }

            // convert verse references Arabic numerals to Hindu numerals 
            while (true)
            {
                // find referance Gen 1:1
                Match match = Regex.Match(result, @"([a-zA-Z0-9\u0620-\u064A]+\s[0-9]+\:[ ​]*[0-9]+)");
                if (!match.Success) break;

                string referenceA = match.Groups[1].Value;
                // replace numerals
                string referenceH = referenceA.
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
                    Replace("9", "٩").
                    Replace(":", ":");

                result = result.Replace(referenceA, referenceH);
            }



            return result;
        }

        public static bool IsDiacritic(char c)
        {
            char[] diacritics =
            {
                '\u064B',  // ARABIC FATHATAN
                '\u064C',  // ARABIC DAMMATAN
                '\u064D',  // ARABIC KASRATAN
                '\u064E',  // ARABIC FATHA
                '\u064F',  // ARABIC DAMMA
                '\u0650',  // ARABIC KASRA
                '\u0651',  // ARABIC SHADDA
                '\u0652',  // ARABIC SUKUN
                '\u0653',  // Madda
                '\u0654',  // Hamza above
                '\u0655',  // Hamza below
                '\u0656',  // 
                '\u0657',
                '\u0658',
                '\u0659',
                '\u065A',
                '\u065B',
                '\u065C',
                '\u065D',
                '\u065E',
                '\u065F'
                };

            return diacritics.Contains(c);

        }

        public static string RemoveDiacritics(string word)
        {
            // Remove diacretics
            string clean = word.
                Replace("\u064B", "").  // ARABIC FATHATAN
                Replace("\u064C", "").  // ARABIC DAMMATAN
                Replace("\u064D", "").  // ARABIC KASRATAN
                Replace("\u064E", "").  // ARABIC FATHA
                Replace("\u064F", "").  // ARABIC DAMMA
                Replace("\u0650", "").  // ARABIC KASRA
                Replace("\u0651", "").  // ARABIC SHADDA
                Replace("\u0652", "").  // ARABIC SUKUN
                Replace("\u0653", "").  // Madda
                Replace("\u0654", "").  // Hamza above
                Replace("\u0655", "").  // Hamza below
                Replace("\u0656", "").  // 
                Replace("\u0657", "").
                Replace("\u0658", "").
                Replace("\u0659", "").
                Replace("\u065A", "").
                Replace("\u065B", "").
                Replace("\u065C", "").
                Replace("\u065D", "").
                Replace("\u065E", "").
                Replace("\u065F", "");
            // Reduce variances of Alef to just one form
            clean = clean.
                Replace("\u0622", "\u0627").  //ALEF WITH MADDA ABOVE
                Replace("\u0623", "\u0627").  //ALEF WITH HAMZA ABOVE
                Replace("\u0625", "\u0627").  //ALEF WITH HAMZA BELOW
                Replace("\u0671", "\u0627");  //ALEF WASLA
                                              // remove unnecessary punctuation
            clean = clean.
                Replace(".", "").
                Replace("«", "").
                Replace("»", "").
                Replace(":", "").
                Replace("؟", "").
                Replace("،", "");


            return clean;
        }

        public const char NUMBER_SIGN = '\u0600';   // ؀
        public const char SIGN_SANAH = '\u0601';    // ؁
        public const char FOOTNOTE_MARKER = '\u0602';   // ؂
        public const char SIGN_SAFHA = '\u0603';    // ؃
        public const char SIGN_SAMVAT = '\u0604';   // ؄
        public const char NUMBER_MARK_ABOVE = '\u0605'; // ؅
        public const char INDIC_CUBE_ROOT = '\u0606';   // ؆
        public const char INDIC_FOURTH_ROOT = '\u0607'; // ؇
        public const char RAY = '\u0608';   // ؈
        public const char INDIC_PER_MILLE_SIGN = '\u0609';  // ؉
        public const char INDIC_PER_TEN_THOUSAND_SIGN = '\u060A';   // ؊
        public const char AFGHANI_SIGN = '\u060B';  // ؋
        public const char COMMA = '\u060C'; // ،
        public const char DATE_SEPARATOR = '\u060D';    // ؍
        public const char POETIC_VERSE_SIGN = '\u060E'; // ؎
        public const char SIGN_MISRA = '\u060F';    // ؏
        public const char SIGN_SALLALLAHOU_ALAYHE_WASSALLAM = '\u0610'; // ؐ
        public const char SIGN_ALAYHE_ASSALLAM = '\u0611';  // ؑ
        public const char SIGN_RAHMATULLAH_ALAYHE = '\u0612';   // ؒ
        public const char SIGN_RADI_ALLAHOU_ANHU = '\u0613';    // ؓ
        public const char SIGN_TAKHALLUS = '\u0614';    // ؔ
        public const char SMALL_HIGH_TAH = '\u0615';    // ؕ
        public const char SMALL_HIGH_LIGATURE_ALEF_WITH_LAM_WITH_YEH = '\u0616';    // ؖ
        public const char SMALL_HIGH_ZAIN = '\u0617';   // ؗ
        public const char SMALL_FATHA = '\u0618';   // ؘ
        public const char SMALL_DAMMA = '\u0619';   // ؙ
        public const char SMALL_KASRA = '\u061A';   // ؚ
        public const char SEMICOLON = '\u061B'; // ؛
        public const char LETTER_MARK = '\u061C';   // ؜
        public const char END_OF_TEXT_MARK = '\u061D';  // ؝
        public const char TRIPLE_DOT_PUNCTUATION_MARK = '\u061E';   // ؞
        public const char QUESTION_MARK = '\u061F'; // ؟
        public const char LETTER_KASHMIRI_YEH = '\u0620';   // ؠ
        public const char LETTER_HAMZA = '\u0621';  // ء
        public const char LETTER_ALEF_WITH_MADDA_ABOVE = '\u0622';  // آ
        public const char LETTER_ALEF_WITH_HAMZA_ABOVE = '\u0623';  // أ
        public const char LETTER_WAW_WITH_HAMZA_ABOVE = '\u0624';   // ؤ
        public const char LETTER_ALEF_WITH_HAMZA_BELOW = '\u0625';  // إ
        public const char LETTER_YEH_WITH_HAMZA_ABOVE = '\u0626';   // ئ
        public const char LETTER_ALEF = '\u0627';   // ا
        public const char LETTER_BEH = '\u0628';    // ب
        public const char LETTER_TEH_MARBUTA = '\u0629';    // ة
        public const char LETTER_TEH = '\u062A';    // ت
        public const char LETTER_THEH = '\u062B';   // ث
        public const char LETTER_JEEM = '\u062C';   // ج
        public const char LETTER_HAH = '\u062D';    // ح
        public const char LETTER_KHAH = '\u062E';   // خ
        public const char LETTER_DAL = '\u062F';    // د
        public const char LETTER_THAL = '\u0630';   // ذ
        public const char LETTER_REH = '\u0631';    // ر
        public const char LETTER_ZAIN = '\u0632';   // ز
        public const char LETTER_SEEN = '\u0633';   // س
        public const char LETTER_SHEEN = '\u0634';  // ش
        public const char LETTER_SAD = '\u0635';    // ص
        public const char LETTER_DAD = '\u0636';    // ض
        public const char LETTER_TAH = '\u0637';    // ط
        public const char LETTER_ZAH = '\u0638';    // ظ
        public const char LETTER_AIN = '\u0639';    // ع
        public const char LETTER_GHAIN = '\u063A';  // غ
        public const char LETTER_KEHEH_WITH_TWO_DOTS_ABOVE = '\u063B';  // ػ
        public const char LETTER_KEHEH_WITH_THREE_DOTS_BELOW = '\u063C';    // ؼ
        public const char LETTER_FARSI_YEH_WITH_INVERTED_V = '\u063D';  // ؽ
        public const char LETTER_FARSI_YEH_WITH_TWO_DOTS_ABOVE = '\u063E';  // ؾ
        public const char LETTER_FARSI_YEH_WITH_THREE_DOTS_ABOVE = '\u063F';    // ؿ
        public const char TATWEEL = '\u0640';   // ـ
        public const char LETTER_FEH = '\u0641';    // ف
        public const char LETTER_QAF = '\u0642';    // ق
        public const char LETTER_KAF = '\u0643';    // ك
        public const char LETTER_LAM = '\u0644';    // ل
        public const char LETTER_MEEM = '\u0645';   // م
        public const char LETTER_NOON = '\u0646';   // ن
        public const char LETTER_HEH = '\u0647';    // ه
        public const char LETTER_WAW = '\u0648';    // و
        public const char LETTER_ALEF_MAKSURA = '\u0649';   // ى
        public const char LETTER_YEH = '\u064A';    // ي
        public const char FATHATAN = '\u064B';  // ً
        public const char DAMMATAN = '\u064C';  // ٌ
        public const char KASRATAN = '\u064D';  // ٍ
        public const char FATHA = '\u064E'; // َ
        public const char DAMMA = '\u064F'; // ُ
        public const char KASRA = '\u0650'; // ِ
        public const char SHADDA = '\u0651';    // ّ
        public const char SUKUN = '\u0652'; // ْ
        public const char MADDAH_ABOVE = '\u0653';  // ٓ
        public const char HAMZA_ABOVE = '\u0654';   // ٔ
        public const char HAMZA_BELOW = '\u0655';   // ٕ
        public const char SUBSCRIPT_ALEF = '\u0656';    // ٖ
        public const char INVERTED_DAMMA = '\u0657';    // ٗ
        public const char MARK_NOON_GHUNNA = '\u0658';  // ٘
        public const char ZWARAKAY = '\u0659';  // ٙ
        public const char VOWEL_SIGN_SMALL_V_ABOVE = '\u065A';  // ٚ
        public const char VOWEL_SIGN_INVERTED_SMALL_V_ABOVE = '\u065B'; // ٛ
        public const char VOWEL_SIGN_DOT_BELOW = '\u065C';  // ٜ
        public const char REVERSED_DAMMA = '\u065D';    // ٝ
        public const char FATHA_WITH_TWO_DOTS = '\u065E';   // ٞ
        public const char WAVY_HAMZA_BELOW = '\u065F';  // ٟ';	// ARABIC-INDIC_DIGIT_ZERO = '\u0660';	// ٠';	// ARABIC-INDIC_DIGIT_ONE = '\u0661';	// ١';	// ARABIC-INDIC_DIGIT_TWO = '\u0662';	// ٢';	// ARABIC-INDIC_DIGIT_THREE = '\u0663';	// ٣';	// ARABIC-INDIC_DIGIT_FOUR = '\u0664';	// ٤';	// ARABIC-INDIC_DIGIT_FIVE = '\u0665';	// ٥';	// ARABIC-INDIC_DIGIT_SIX = '\u0666';	// ٦';	// ARABIC-INDIC_DIGIT_SEVEN = '\u0667';	// ٧';	// ARABIC-INDIC_DIGIT_EIGHT = '\u0668';	// ٨';	// ARABIC-INDIC_DIGIT_NINE = '\u0669';	// ٩
        public const char PERCENT_SIGN = '\u066A';  // ٪
        public const char DECIMAL_SEPARATOR = '\u066B'; // ٫
        public const char THOUSANDS_SEPARATOR = '\u066C';   // ٬
        public const char FIVE_POINTED_STAR = '\u066D'; // ٭
        public const char LETTER_DOTLESS_BEH = '\u066E';    // ٮ
        public const char LETTER_DOTLESS_QAF = '\u066F';    // ٯ
        public const char LETTER_SUPERSCRIPT_ALEF = '\u0670';   // ٰ
        public const char LETTER_ALEF_WASLA = '\u0671'; // ٱ
        public const char LETTER_ALEF_WITH_WAVY_HAMZA_ABOVE = '\u0672'; // ٲ
        public const char LETTER_ALEF_WITH_WAVY_HAMZA_BELOW = '\u0673'; // ٳ
        public const char LETTER_HIGH_HAMZA = '\u0674'; // ٴ
        public const char LETTER_HIGH_HAMZA_ALEF = '\u0675';    // ٵ
        public const char LETTER_HIGH_HAMZA_WAW = '\u0676'; // ٶ
        public const char LETTER_U_WITH_HAMZA_ABOVE = '\u0677'; // ٷ
        public const char LETTER_HIGH_HAMZA_YEH = '\u0678'; // ٸ
        public const char LETTER_TTEH = '\u0679';   // ٹ
        public const char LETTER_TTEHEH = '\u067A'; // ٺ
        public const char LETTER_BEEH = '\u067B';   // ٻ
        public const char LETTER_TEH_WITH_RING = '\u067C';  // ټ
        public const char LETTER_TEH_WITH_THREE_DOTS_ABOVE_DOWNWARDS = '\u067D';    // ٽ
        public const char LETTER_PEH = '\u067E';    // پ
        public const char LETTER_TEHEH = '\u067F';  // ٿ
        public const char LETTER_BEHEH = '\u0680';  // ڀ
        public const char LETTER_HAH_WITH_HAMZA_ABOVE = '\u0681';   // ځ
        public const char LETTER_HAH_WITH_TWO_DOTS_VERTICAL_ABOVE = '\u0682';   // ڂ
        public const char LETTER_NYEH = '\u0683';   // ڃ
        public const char LETTER_DYEH = '\u0684';   // ڄ
        public const char LETTER_HAH_WITH_THREE_DOTS_ABOVE = '\u0685';  // څ
        public const char LETTER_TCHEH = '\u0686';  // چ
        public const char LETTER_TCHEHEH = '\u0687';    // ڇ
        public const char LETTER_DDAL = '\u0688';   // ڈ
        public const char LETTER_DAL_WITH_RING = '\u0689';  // ډ
        public const char LETTER_DAL_WITH_DOT_BELOW = '\u068A'; // ڊ
        public const char LETTER_DAL_WITH_DOT_BELOW_AND_SMALL_TAH = '\u068B';   // ڋ
        public const char LETTER_DAHAL = '\u068C';  // ڌ
        public const char LETTER_DDAHAL = '\u068D'; // ڍ
        public const char LETTER_DUL = '\u068E';    // ڎ
        public const char LETTER_DAL_WITH_THREE_DOTS_ABOVE_DOWNWARDS = '\u068F';    // ڏ
        public const char LETTER_DAL_WITH_FOUR_DOTS_ABOVE = '\u0690';   // ڐ
        public const char LETTER_RREH = '\u0691';   // ڑ
        public const char LETTER_REH_WITH_SMALL_V = '\u0692';   // ڒ
        public const char LETTER_REH_WITH_RING = '\u0693';  // ړ
        public const char LETTER_REH_WITH_DOT_BELOW = '\u0694'; // ڔ
        public const char LETTER_REH_WITH_SMALL_V_BELOW = '\u0695'; // ڕ
        public const char LETTER_REH_WITH_DOT_BELOW_AND_DOT_ABOVE = '\u0696';   // ږ
        public const char LETTER_REH_WITH_TWO_DOTS_ABOVE = '\u0697';    // ڗ
        public const char LETTER_JEH = '\u0698';    // ژ
        public const char LETTER_REH_WITH_FOUR_DOTS_ABOVE = '\u0699';   // ڙ
        public const char LETTER_SEEN_WITH_DOT_BELOW_AND_DOT_ABOVE = '\u069A';  // ښ
        public const char LETTER_SEEN_WITH_THREE_DOTS_BELOW = '\u069B'; // ڛ
        public const char LETTER_SEEN_WITH_THREE_DOTS_BELOW_AND_THREE_DOTS_ABOVE = '\u069C';    // ڜ
        public const char LETTER_SAD_WITH_TWO_DOTS_BELOW = '\u069D';    // ڝ
        public const char LETTER_SAD_WITH_THREE_DOTS_ABOVE = '\u069E';  // ڞ
        public const char LETTER_TAH_WITH_THREE_DOTS_ABOVE = '\u069F';  // ڟ
        public const char LETTER_AIN_WITH_THREE_DOTS_ABOVE = '\u06A0';  // ڠ
        public const char LETTER_DOTLESS_FEH = '\u06A1';    // ڡ
        public const char LETTER_FEH_WITH_DOT_MOVED_BELOW = '\u06A2';   // ڢ
        public const char LETTER_FEH_WITH_DOT_BELOW = '\u06A3'; // ڣ
        public const char LETTER_VEH = '\u06A4';    // ڤ
        public const char LETTER_FEH_WITH_THREE_DOTS_BELOW = '\u06A5';  // ڥ
        public const char LETTER_PEHEH = '\u06A6';  // ڦ
        public const char LETTER_QAF_WITH_DOT_ABOVE = '\u06A7'; // ڧ
        public const char LETTER_QAF_WITH_THREE_DOTS_ABOVE = '\u06A8';  // ڨ
        public const char LETTER_KEHEH = '\u06A9';  // ک
        public const char LETTER_SWASH_KAF = '\u06AA';  // ڪ
        public const char LETTER_KAF_WITH_RING = '\u06AB';  // ګ
        public const char LETTER_KAF_WITH_DOT_ABOVE = '\u06AC'; // ڬ
        public const char LETTER_NG = '\u06AD'; // ڭ
        public const char LETTER_KAF_WITH_THREE_DOTS_BELOW = '\u06AE';  // ڮ
        public const char LETTER_GAF = '\u06AF';    // گ
        public const char LETTER_GAF_WITH_RING = '\u06B0';  // ڰ
        public const char LETTER_NGOEH = '\u06B1';  // ڱ
        public const char LETTER_GAF_WITH_TWO_DOTS_BELOW = '\u06B2';    // ڲ
        public const char LETTER_GUEH = '\u06B3';   // ڳ
        public const char LETTER_GAF_WITH_THREE_DOTS_ABOVE = '\u06B4';  // ڴ
        public const char LETTER_LAM_WITH_SMALL_V = '\u06B5';   // ڵ
        public const char LETTER_LAM_WITH_DOT_ABOVE = '\u06B6'; // ڶ
        public const char LETTER_LAM_WITH_THREE_DOTS_ABOVE = '\u06B7';  // ڷ
        public const char LETTER_LAM_WITH_THREE_DOTS_BELOW = '\u06B8';  // ڸ
        public const char LETTER_NOON_WITH_DOT_BELOW = '\u06B9';    // ڹ
        public const char LETTER_NOON_GHUNNA = '\u06BA';    // ں
        public const char LETTER_RNOON = '\u06BB';  // ڻ
        public const char LETTER_NOON_WITH_RING = '\u06BC'; // ڼ
        public const char LETTER_NOON_WITH_THREE_DOTS_ABOVE = '\u06BD'; // ڽ
        public const char LETTER_HEH_DOACHASHMEE = '\u06BE';    // ھ
        public const char LETTER_TCHEH_WITH_DOT_ABOVE = '\u06BF';   // ڿ
        public const char LETTER_HEH_WITH_YEH_ABOVE = '\u06C0'; // ۀ
        public const char LETTER_HEH_GOAL = '\u06C1';   // ہ
        public const char LETTER_HEH_GOAL_WITH_HAMZA_ABOVE = '\u06C2';  // ۂ
        public const char LETTER_TEH_MARBUTA_GOAL = '\u06C3';   // ۃ
        public const char LETTER_WAW_WITH_RING = '\u06C4';  // ۄ
        public const char LETTER_KIRGHIZ_OE = '\u06C5'; // ۅ
        public const char LETTER_OE = '\u06C6'; // ۆ
        public const char LETTER_U = '\u06C7';  // ۇ
        public const char LETTER_YU = '\u06C8'; // ۈ
        public const char LETTER_KIRGHIZ_YU = '\u06C9'; // ۉ
        public const char LETTER_WAW_WITH_TWO_DOTS_ABOVE = '\u06CA';    // ۊ
        public const char LETTER_VE = '\u06CB'; // ۋ
        public const char LETTER_FARSI_YEH = '\u06CC';  // ی
        public const char LETTER_YEH_WITH_TAIL = '\u06CD';  // ۍ
        public const char LETTER_YEH_WITH_SMALL_V = '\u06CE';   // ێ
        public const char LETTER_WAW_WITH_DOT_ABOVE = '\u06CF'; // ۏ
        public const char LETTER_E = '\u06D0';  // ې
        public const char LETTER_YEH_WITH_THREE_DOTS_BELOW = '\u06D1';  // ۑ
        public const char LETTER_YEH_BARREE = '\u06D2'; // ے
        public const char LETTER_YEH_BARREE_WITH_HAMZA_ABOVE = '\u06D3';    // ۓ
        public const char FULL_STOP = '\u06D4'; // ۔
        public const char LETTER_AE = '\u06D5'; // ە
        public const char SMALL_HIGH_LIGATURE_SAD_WITH_LAM_WITH_ALEF_MAKSURA = '\u06D6';    // ۖ
        public const char SMALL_HIGH_LIGATURE_QAF_WITH_LAM_WITH_ALEF_MAKSURA = '\u06D7';    // ۗ
        public const char SMALL_HIGH_MEEM_INITIAL_FORM = '\u06D8';  // ۘ
        public const char SMALL_HIGH_LAM_ALEF = '\u06D9';   // ۙ
        public const char SMALL_HIGH_JEEM = '\u06DA';   // ۚ
        public const char SMALL_HIGH_THREE_DOTS = '\u06DB'; // ۛ
        public const char SMALL_HIGH_SEEN = '\u06DC';   // ۜ
        public const char END_OF_AYAH = '\u06DD';   // ۝
        public const char START_OF_RUB_EL_HIZB = '\u06DE';  // ۞
        public const char SMALL_HIGH_ROUNDED_ZERO = '\u06DF';   // ۟
        public const char SMALL_HIGH_UPRIGHT_RECTANGULAR_ZERO = '\u06E0';   // ۠
        public const char SMALL_HIGH_DOTLESS_HEAD_OF_KHAH = '\u06E1';   // ۡ
        public const char SMALL_HIGH_MEEM_ISOLATED_FORM = '\u06E2'; // ۢ
        public const char SMALL_LOW_SEEN = '\u06E3';    // ۣ
        public const char SMALL_HIGH_MADDA = '\u06E4';  // ۤ
        public const char SMALL_WAW = '\u06E5'; // ۥ
        public const char SMALL_YEH = '\u06E6'; // ۦ
        public const char SMALL_HIGH_YEH = '\u06E7';    // ۧ
        public const char SMALL_HIGH_NOON = '\u06E8';   // ۨ
        public const char PLACE_OF_SAJDAH = '\u06E9';   // ۩
        public const char EMPTY_CENTRE_LOW_STOP = '\u06EA'; // ۪
        public const char EMPTY_CENTRE_HIGH_STOP = '\u06EB';    // ۫
        public const char ROUNDED_HIGH_STOP_WITH_FILLED_CENTRE = '\u06EC';  // ۬
        public const char SMALL_LOW_MEEM = '\u06ED';    // ۭ
        public const char LETTER_DAL_WITH_INVERTED_V = '\u06EE';    // ۮ
        public const char LETTER_REH_WITH_INVERTED_V = '\u06EF';    // ۯ';	// EXTENDED_ARABIC-INDIC_DIGIT_ZERO = '\u06F0';	// ۰';	// EXTENDED_ARABIC-INDIC_DIGIT_ONE = '\u06F1';	// ۱';	// EXTENDED_ARABIC-INDIC_DIGIT_TWO = '\u06F2';	// ۲';	// EXTENDED_ARABIC-INDIC_DIGIT_THREE = '\u06F3';	// ۳';	// EXTENDED_ARABIC-INDIC_DIGIT_FOUR = '\u06F4';	// ۴';	// EXTENDED_ARABIC-INDIC_DIGIT_FIVE = '\u06F5';	// ۵';	// EXTENDED_ARABIC-INDIC_DIGIT_SIX = '\u06F6';	// ۶';	// EXTENDED_ARABIC-INDIC_DIGIT_SEVEN = '\u06F7';	// ۷';	// EXTENDED_ARABIC-INDIC_DIGIT_EIGHT = '\u06F8';	// ۸';	// EXTENDED_ARABIC-INDIC_DIGIT_NINE = '\u06F9';	// ۹
        public const char LETTER_SHEEN_WITH_DOT_BELOW = '\u06FA';   // ۺ
        public const char LETTER_DAD_WITH_DOT_BELOW = '\u06FB'; // ۻ
        public const char LETTER_GHAIN_WITH_DOT_BELOW = '\u06FC';   // ۼ
        public const char SIGN_SINDHI_AMPERSAND = '\u06FD'; // ۽
        public const char SIGN_SINDHI_POSTPOSITION_MEN = '\u06FE';  // ۾
        public const char LETTER_HEH_WITH_INVERTED_V = '\u06FF';	// ۿ';	// 
    }
}
