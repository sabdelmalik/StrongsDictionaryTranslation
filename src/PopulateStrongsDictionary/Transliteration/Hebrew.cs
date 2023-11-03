using BibleTaggingUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PopulateStrongsDictionary.Transliteration
{
    // https://github.com/opensiddur

    // https://en.wikipedia.org/wiki/Romanization_of_Hebrew


    // https://unicode.org/charts/PDF/U0590.pdf
    // Hebrew unicode '\u0590' to '\u05FF'

    // Hebrew Cantillation Marks        '\u0591' to '\u05AF'
    // Hebrew Points and punctuation    '\u05B0' to '\u05C3'
    // Hebrew Puncta extraordinaria     '\u05C4' to '\u05C5'
    // Points and punctuation           '\u05C6' to '\u05C7'
    // Hebrew consonanta                '\u05D0' to '\u05EA'

    internal class Hebrew
    {
        Dictionary<char, char> simpleTranslator = new Dictionary<char, char>();
        Dictionary<char, char> niquodTranslator = new Dictionary<char, char>();

        #region Hebrew Points and punctuation
        public const char POINT_SHEVA = '\u05B0';             // two vertical dote below
        public const char POINT_HATAF_SEGOL = '\u05B1';     // segol sheva
        public const char POINT_HATAF_PATAH = '\u05B2';     // patah sheva
        public const char POINT_HATAF_QAMATS = '\u05B3';    // qamats sheva
        public const char POINT_HIRIQ = '\u05B4';           // dot below
        public const char POINT_TSERE = '\u05B5';           // two horizontal dots below
        public const char POINT_SEGOL = '\u05B6';
        public const char POINT_PATAH = '\u05B7';           // Short A vowel
        public const char POINT_QAMATS = '\u05B8';          // Long A vowel or an O if closed un-acented syllable
        public const char POINT_HOLAM = '\u05B9';
        public const char POINT_HOLAM_HASER_FOR_VAV = '\u05BA';
        public const char POINT_QUBUTS = '\u05BB';
        public const char POINT_DAGESH_OR_MAPIQ = '\u05BC';
        public const char POINT_METEG = '\u05BD';
        public const char PUNCTUATION_MAQAF = '\u05BE';
        public const char POINT_RAFE = '\u05BF';
        public const char PUNCTUATION_PASEQ = '\u05C0';
        public const char POINT_SHIN_DOT = '\u05C1';
        public const char POINT_SIN_DOT = '\u05C2';
        public const char PUNCTUATION_SOF_PASUQ = '\u05C3';
        public const char POINT_QAMATS_QATAN = '\u05C7';

        public const char LETTER_YOD = '\u05D9';
        #endregion Hebrew Points and punctuation

        MainForm mainForm;
        public Hebrew(MainForm mainForm)
        {
            this.mainForm = mainForm;

            simpleTranslator.Add('א', 'ا');
            simpleTranslator.Add('ב', 'ڤ');
            simpleTranslator.Add('ג', 'ج');
            simpleTranslator.Add('ד', 'د');
            simpleTranslator.Add('ה', 'ه');
            simpleTranslator.Add('ו', 'و');
            simpleTranslator.Add('ז', 'ز');
            simpleTranslator.Add('ח', 'ح');
            simpleTranslator.Add('ט', 'ط');
            simpleTranslator.Add('י', 'ي');
            simpleTranslator.Add('כ', 'خ');
            simpleTranslator.Add('ך', 'خ');
            simpleTranslator.Add('ל', 'ل');
            simpleTranslator.Add('מ', 'م');
            simpleTranslator.Add('ם', 'م');
            simpleTranslator.Add('ן', 'ن');
            simpleTranslator.Add('נ', 'ن');
            simpleTranslator.Add('ס', 'س');
            simpleTranslator.Add('ע', 'ع');
            simpleTranslator.Add('פ', 'ف');
            simpleTranslator.Add('ף', 'ف');
            simpleTranslator.Add('ץ', 'ص');
            simpleTranslator.Add('ק', 'ق');
            simpleTranslator.Add('ר', 'ر');
            simpleTranslator.Add('ש', 'ش');
            simpleTranslator.Add('ת', 'ت');

            niquodTranslator.Add(POINT_SHEVA, Arabic.SUKUN);
            niquodTranslator.Add(POINT_HATAF_SEGOL, Arabic.SMALL_KASRA);
            niquodTranslator.Add(POINT_HATAF_PATAH, Arabic.SMALL_FATHA);
            niquodTranslator.Add(POINT_HATAF_QAMATS, Arabic.SMALL_FATHA);
            niquodTranslator.Add(POINT_HIRIQ, Arabic.KASRA);
            niquodTranslator.Add(POINT_TSERE, Arabic.KASRA);
            niquodTranslator.Add(POINT_SEGOL, Arabic.KASRA);
            niquodTranslator.Add(POINT_PATAH, Arabic.FATHA);
            niquodTranslator.Add(POINT_QAMATS, Arabic.FATHA);
            niquodTranslator.Add(POINT_HOLAM, Arabic.DAMMA);
            niquodTranslator.Add(POINT_HOLAM_HASER_FOR_VAV, Arabic.DAMMA);
            niquodTranslator.Add(POINT_QUBUTS, Arabic.SMALL_DAMMA);
            niquodTranslator.Add(POINT_QAMATS_QATAN, Arabic.DAMMA);
        }

        private void Trace(string text, Color color)
        {
            mainForm.Trace(text, color);
        }
        private void TraceError(string method, string text)
        {
            mainForm.TraceError(method, text);
        }

        public bool IsCantillationMark(char c)
        {
            // Hebrew Cantillation Marks        '\u0591' to '\u05AF'
            if (c >= '\u0591' && c <= '\u05AF')
                return true;
            return false;
        }
        public bool IsVowel(char c)
        {
            // Hebrew Points and punctuation    '\u05B0' to '\u05C3'
            if ((c >= '\u05B0' && c <= '\u05BC') ||
                (c >= '\u05C1' && c <= '\u05C2') ||
                c == POINT_QAMATS_QATAN)
                return true;
            return false;
        }

        public bool IsBegedKefetLetter(char c)
        {
            /*
             * If there's a vowel immediately before a beged-kefet letter, you 
             * should expect that it would not have a dagesh.
             * But if there's a consonant before a beged-kefet letter,
             * or nothing (as at the beginning of a word), that dagesh is not 
             * telling us anything significant.
             */
            if (c == 'ב' ||  // בּ
               c == 'ג' ||
               c == 'ד' ||
               c == 'כ' ||  // כּ
               c == 'פ' ||
               c == 'ת')
                return true;
            return false;
        }
        public bool IsPunctaExtraordinaria(char c)
        {
            // Hebrew Puncta extraordinaria     '\u05C4' to '\u05C5'
            if (c >= '\u05C4' && c <= '\u05C5')
                return true;
            return false;
        }

        public bool IsConsonant(char c)
        {
            // Hebrew consonanta                '\u05D0' to '\u05EA'
            if (c >= '\u05D0' && c <= '\u05EA')
                return true;
            return false;
        }

        public string RemoveAccents(string hebWord)
        {
            string result = string.Empty;
            char[] chars = hebWord.ToCharArray();
            foreach (char c in chars)
            {
                // Hebrew Cantillation Marks        '\u0591' to '\u05AF'
                if ((c >= '\u0591' && c <= '\u05AF') ||
                    c == POINT_METEG || c == PUNCTUATION_MAQAF || c == POINT_RAFE || c== PUNCTUATION_PASEQ)
                    continue;
                result += c.ToString();
            }

            return result;
        }

        public string TranslateConsonant(char consonant)
        {
            if (consonant == 'צ' || consonant == 'ץ')
                return "تص";
            return simpleTranslator[consonant].ToString();
        }

        public string TranslateNiquod(char niquod)
        {
            return niquodTranslator[niquod].ToString();
        }
    }
}
