using BibleTaggingUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.CompilerServices;
using Npgsql;

namespace PopulateStrongsDictionary.Transliteration
{
    internal class TransliterateAra
    {
        private enum SyllableFSM
        {
            IDLE,
            CONS,
            FIRST_CONS,
            CONS_VOWEL,
            CONS_VOWEL2,
            CONS_VOWEL_CONS
        }

        SyllableFSM m_FSM = SyllableFSM.IDLE;

        Hebrew hebrew;
        TransliterateHeb2Ara heb2Ara;
        MainForm mainForm;
        public TransliterateAra(MainForm mainForm)
        {
            this.mainForm = mainForm;
            hebrew = new Hebrew(mainForm);
            heb2Ara = new TransliterateHeb2Ara(mainForm);

        }

        private void Trace(string text, Color color)
        {
            mainForm.Trace(text, color);
        }
        private void TraceError(
                string text,
                [CallerLineNumber] int lineNumber = 0,
                [CallerMemberName] string caller = null)
        {
            mainForm.TraceError(string.Format("{0}#{1}", caller==null? "???" : caller, lineNumber), text);
        }

 /*
        public string Translitrate(string hebText, string engTranslit)
        {
            string result = string.Empty;

            string[] words = hebText.Split(new char[] {' ', '־' });
            string seperator = string.Empty;

            string finalHeb = string.Empty;
            string finalAra = string.Empty; 

            foreach (string word in words)
            {
                List<SyllableDef> syllables = TranslitrateWord(word.Trim().Trim(','), "");

                if(syllables == null)
                    continue;
                string outString = string.Empty;
                string outArabicString = string.Empty;

                for (int i = 0; i < syllables.Count; i++)
                {
                    SyllableDef s = syllables[i];
                    outString += s.Syllable + ".";
                    outArabicString += s.Arabic + ".";
                }
                finalHeb += outString.Trim('.') + " ";
                finalAra += outArabicString.Trim('.') + " ";
            }
               //Trace(string.Format("{0}\t\t{1}\t\t{2}", hebText, finalHeb, finalAra), Color.Blue);
            return finalAra;
        }
 */
        private List<SyllableDef> TranslitrateWord(string hebWord, string engTranslit)
        {
            string result = string.Empty;

            List<SyllableDef> syllables = new List<SyllableDef>();

            if (string.IsNullOrEmpty(hebWord))
                return null;

            char[] chars = hebrew.RemoveAccents(hebWord.Trim()).ToCharArray();
            char startConsonant = chars[0]; // words always start with a startConsonant
            List<char> vowels = new List<char>();

            m_FSM = SyllableFSM.IDLE;
            string diphthong = string.Empty;
            bool hasDiphthong = false;
            for (int i = 0; i < chars.Length; i++)
            {
                char current = chars[i];
                switch (m_FSM)
                {
                    case SyllableFSM.IDLE:
                        if (hebrew.IsConsonant(current))
                        {
                            startConsonant = current;
                            vowels.Clear();
                            m_FSM = SyllableFSM.FIRST_CONS;
                        }
                        break;

                    case SyllableFSM.FIRST_CONS:
                        if (hebrew.IsVowel(current))
                        {
                            #region Diphthongs
                            // check for Diphthong
                            // https://www.hebrew4christians.com/Grammar/Unit_Two/Hebrew_Dipthongs/hebrew_dipthongs.html
                            if ((i < chars.Length - 1) && (current == Hebrew.POINT_PATAH || current == Hebrew.POINT_QAMATS) &&
                                (chars[i + 1] == 'י' && ((i > chars.Length - 3) || hebrew.IsConsonant(chars[i + 2]))))
                            {
                                // אָי  אַי
                                hasDiphthong = true;
                                diphthong = current.ToString();
                                vowels.Add(current);
                                vowels.Add(chars[++i]);
                                diphthong += chars[i].ToString();
                                if ((i < chars.Length - 1) && chars[i + 1] == Hebrew.POINT_HIRIQ)
                                {
                                    // אַיִ
                                    vowels.Add(chars[++i]);
                                    diphthong += chars[i].ToString();
                                }
                                else if ((i < chars.Length - 1) && chars[i + 1] == 'ו' && 
                                    ((i > chars.Length - 3) || (chars[i + 2] != Hebrew.POINT_HOLAM && chars[i + 2] != Hebrew.POINT_DAGESH_OR_MAPIQ)))
                                {
                                    // אָיו
                                    vowels.Add(chars[++i]);
                                    diphthong += chars[i].ToString();
                                }
                            }
                            else if ((i < chars.Length - 1) && (current == Hebrew.POINT_TSERE || current == Hebrew.POINT_HIRIQ) && (chars[i + 1] == 'י' && ((i > chars.Length - 3) || hebrew.IsConsonant(chars[i + 2]))))
                            {
                                // אִי  אֵי
                                hasDiphthong = true;
                                diphthong = current.ToString();
                                vowels.Add(current);
                                vowels.Add(chars[++i]);
                                diphthong += chars[i].ToString();
                            }
                            else if ((i < chars.Length - 2) && current == 'ו' && chars[i + 2] == 'י' &&
                                (chars[i + 1] == Hebrew.POINT_HOLAM || chars[i + 1] == Hebrew.POINT_DAGESH_OR_MAPIQ))
                            {
                                // אוּי  אוֹי
                                hasDiphthong = true;
                                diphthong = current.ToString();
                                vowels.Add(current);
                                vowels.Add(chars[++i]);
                                diphthong += chars[i].ToString();
                                vowels.Add(chars[++i]);
                                diphthong += chars[i].ToString();
                            }
                            #endregion  Diphthong
                            else
                            {
                                vowels.Add(current);
                            }
                            if (current != Hebrew.POINT_SHIN_DOT && current != Hebrew.POINT_SIN_DOT && current != Hebrew.POINT_DAGESH_OR_MAPIQ)
                                m_FSM = SyllableFSM.CONS_VOWEL;
                        }
                        else if (current == 'ו')
                        {
                            if (i < chars.Length - 1)
                            {
                                if (chars[i + 1] == Hebrew.POINT_HOLAM || chars[i + 1] == Hebrew.POINT_DAGESH_OR_MAPIQ)
                                {
                                    vowels.Add(current);
                                    vowels.Add(chars[i + 1]);
                                    i++;
                                    m_FSM = SyllableFSM.CONS_VOWEL;
                                }
                            }
                        }
                        else if (startConsonant == 'א' && hebrew.IsConsonant(current))
                        {
                            // Quiescent Alef
                            startConsonant = current;
                        }
                        break;

                    case SyllableFSM.CONS_VOWEL:
                        if (hebrew.IsVowel(current))
                        {
                            if (vowels.Contains(current))
                            {
                                TraceError(string.Format("Vowel repeated: {0}", hebWord));
                                return null;
                            }
                            if ((new char[] { Hebrew.POINT_DAGESH_OR_MAPIQ, Hebrew.POINT_SHIN_DOT, Hebrew.POINT_SIN_DOT }).Contains(current) ||
                                (new char[] { Hebrew.POINT_DAGESH_OR_MAPIQ, Hebrew.POINT_SHIN_DOT, Hebrew.POINT_SIN_DOT }).Contains(vowels[0]))
                            {
                                vowels.Add(current);
                                m_FSM = SyllableFSM.CONS_VOWEL2;
                            }
                            else
                            {
                                // check for the case of יְרוּשָׁלִַ֫ם
                                if (current == Hebrew.POINT_HIRIQ && chars[i - 1] == Hebrew.POINT_PATAH && (i < chars.Length - 1) && chars[i + 1] == 'ם')
                                {
                                    // simulate a batah youd diphthong
                                    vowels.Add('י');
                                    hasDiphthong = true;
                                    diphthong = Hebrew.POINT_PATAH.ToString() + 'י'.ToString();
                                    m_FSM = SyllableFSM.CONS_VOWEL2;
                                }
                                else
                                {
                                    TraceError(string.Format("More than two Points: {0}", hebWord));
                                    return null;
                                }
                            }
                        }
                        else if ((i < chars.Length - 1) && 
                            current == 'ו' && 
                            chars[i - 1] != 'י' &&
                            (chars[i + 1] == Hebrew.POINT_HOLAM || chars[i + 1] == Hebrew.POINT_DAGESH_OR_MAPIQ)
                            && ((i > chars.Length - 3) || hebrew.IsConsonant(chars[i + 2])))
                        {
                            vowels.Add(current);
                            vowels.Add(chars[++i]);
                            m_FSM = SyllableFSM.CONS_VOWEL2;
                        }
                        else
                        {
                            try
                            {
                                if ((i < chars.Length - 1) && current == 'א' && hebrew.IsConsonant(chars[i + 1]))
                                {
                                    // Quiescent Alef
                                    vowels.Add(current);
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                int x = 0;
                            }
                            // a second consonant, , may be a closed syllable
                            string syllable = startConsonant.ToString();
                            foreach (char c in vowels)
                            {
                                syllable += c.ToString();
                            }
                            // fix Furtive Pathach
                            //if (i == chars.Length - 2 && chars[i + 1] == Hebrew.POINT_PATAH)
                            //{
                            //    // Furtive Pathach
                            //    syllable += chars[i++].ToString();
                            //    syllable += chars[i++].ToString();
                            //    syllables.Add(new SyllableDef(syllable, true, hasDiphthong, diphthong));
                            //    m_FSM = SyllableFSM.IDLE;
                            //    hasDiphthong = false;
                            //    diphthong = string.Empty;
                            //}
                            //else 
                            if ((i == chars.Length - 2) &&
                                    (chars[i + 1] == Hebrew.POINT_SHIN_DOT ||
                                     chars[i + 1] == Hebrew.POINT_SIN_DOT ||
                                     chars[i + 1] == Hebrew.POINT_SHEVA ||
                                     chars[i + 1] == Hebrew.POINT_DAGESH_OR_MAPIQ))
                            {
                                syllable += chars[i].ToString() + chars[i + 1].ToString();
                                syllables.Add(new SyllableDef(syllable, true, hasDiphthong, diphthong));
                                m_FSM = SyllableFSM.IDLE;
                                hasDiphthong = false;
                                diphthong = string.Empty;
                            }
                            else if (i == chars.Length - 1)
                            {
                                // last character of the word
                                syllable += chars[i++].ToString();
                                syllables.Add(new SyllableDef(syllable, true, hasDiphthong, diphthong));
                                m_FSM = SyllableFSM.IDLE;
                                hasDiphthong = false;
                                diphthong = string.Empty;
                            }
                            else if (chars[i + 1] == Hebrew.POINT_SHEVA)
                            {
                                // A closed syllable
                                syllable += current.ToString() + chars[i + 1].ToString();
                                syllables.Add(new SyllableDef(syllable, true, hasDiphthong, diphthong));
                                hasDiphthong = false;
                                diphthong = string.Empty;

                                i += 2;
                                if (i == chars.Length)
                                {
                                    m_FSM = SyllableFSM.IDLE;
                                }
                                else
                                {
                                    try
                                    {
                                        startConsonant = chars[i];
                                        vowels.Clear();
                                        m_FSM = SyllableFSM.FIRST_CONS;
                                    }
                                    catch(Exception e)
                                    {
                                        int x = 0;
                                    }
                                }
                            }
                            else
                            {
                                // open syllable
                                syllables.Add(new SyllableDef(syllable, false, hasDiphthong, diphthong));
                                startConsonant = current;
                                vowels.Clear();
                                m_FSM = SyllableFSM.FIRST_CONS;
                                hasDiphthong = false;
                                diphthong = string.Empty;

                            }
                        }
                        break;

                    case SyllableFSM.CONS_VOWEL2:
                        if (hebrew.IsVowel(current))
                        {
                            TraceError(string.Format("More than two Points: {0}", hebWord));
                            return null;
                        }
                        else
                        {
                            try
                            {
                                if ((i < chars.Length - 1) && current == 'א' && hebrew.IsConsonant(chars[i + 1]))
                                {
                                    // Quiescent Alef
                                    vowels.Add(current);
                                    break;
                                }
                            }
                            catch(Exception ex)
                            {
                                TraceError(string.Format("[{0}] {1}",hebWord, ex.Message));
                            }
                            // a second consonant, , may be a closed syllable
                            string syllable = startConsonant.ToString();
                            foreach (char c in vowels)
                            {
                                syllable += c.ToString();
                            }

                            if (i == chars.Length - 1)
                            {
                                // last character of the word
                                syllable += chars[i++].ToString();
                                syllables.Add(new SyllableDef(syllable, true, hasDiphthong,diphthong));
                                m_FSM = SyllableFSM.IDLE;
                                hasDiphthong = false;
                                diphthong = string.Empty;
                                vowels.Clear();
                            }
                            else if (chars[i + 1] == Hebrew.POINT_SHEVA)
                            {
                                // A closed syllable
                                syllable += current.ToString() + chars[i + 1].ToString();
                                syllables.Add(new SyllableDef(syllable, true, hasDiphthong, diphthong));
                                hasDiphthong = false;
                                diphthong = string.Empty;
                                vowels.Clear();
                                i += 2;
                                if (i == chars.Length)
                                {
                                    m_FSM = SyllableFSM.IDLE;
                                }
                                else
                                {
                                    try
                                    {
                                        startConsonant = chars[i];
                                    }
                                    catch(Exception ex)
                                    {
                                        TraceError(string.Format("[{0}] {1}", hebWord, ex.Message));
                                    }
                                    m_FSM = SyllableFSM.FIRST_CONS;
                                }
                            }
                            else
                            {
                                // not a closed syllable
                                syllables.Add(new SyllableDef(syllable, false, hasDiphthong, diphthong));
                                startConsonant = current;
                                vowels.Clear();
                                m_FSM = SyllableFSM.FIRST_CONS;
                                hasDiphthong = false;
                                diphthong = string.Empty;
                            }
                        }
                        break;

                }
            }
            if(m_FSM != SyllableFSM.CONS && m_FSM != SyllableFSM.FIRST_CONS && m_FSM != SyllableFSM.IDLE)
            {
                string syllable = startConsonant.ToString();
                foreach (char c in vowels)
                {
                    syllable += c.ToString();
                }
                syllables.Add(new SyllableDef(syllable, false, hasDiphthong, diphthong));
            }
            string outString = string.Empty;
            string outArabicString = string.Empty;
            char prevVowel = ' ';

            // Get the Arabic Translitration
            for (int i = 0; i < syllables.Count; i++)
            {
                SyllableDef s = syllables[i];
                syllables[i].Arabic = GetArabicSyllable(s, prevVowel, i == 0, i == syllables.Count - 1, hebWord);
                prevVowel = s.Syllable[s.Syllable.Length - 1];
            }

            //for (int i = 0; i < syllables.Count; i++)
            //{
            //    SyllableDef s = syllables[i];
            //    outString += s.Syllable + ".";
            //    outArabicString += GetArabicSyllable(s, prevVowel, i == 0, i == syllables.Count - 1) + ".";
            //    prevVowel = s.Syllable[s.Syllable.Length - 1];
            //}
            //outString = outString.Trim('.');
            //outArabicString = outArabicString.Trim('.');
            //Trace(string.Format("{0}\t\t{1}\t\t{2}", hebWord, outString, outArabicString), Color.Blue);

            // handle last character
            // result = AddArabic(result, startConsonant, vowels, false, ' ', true);
            return syllables;
        }

        public string GetArabicSyllable(SyllableDef syllableDef, char previousVowel, bool first, bool last, string hebWord)
        {
            string result = string.Empty;
            int i = 0;
            string syl = syllableDef.Syllable;
            char[] sylChars = syl.ToCharArray();
            bool firstVowelDone = false;

            if (sylChars[i] == 'א')
            {
                // handle Alef
                switch (sylChars[++i])
                {
                    case Hebrew.POINT_HATAF_PATAH:
                    case Hebrew.POINT_PATAH:
                    case Hebrew.POINT_HATAF_QAMATS:
                    case Hebrew.POINT_QAMATS:
                        if (i < sylChars.Length - 1 && sylChars[i + 1] == 'י' && ((i > sylChars.Length - 3) || hebrew.IsConsonant(sylChars[i + 2])))
                        {
                            result += "أي";
                            i++;
                        }
                        else
                        {
                            result += Arabic.LETTER_ALEF_WITH_HAMZA_ABOVE;
                            result += Arabic.FATHA;
                        }
                        break;
                    case Hebrew.POINT_QUBUTS:
                        result += "أُو";
                        break;
                    case Hebrew.POINT_QAMATS_QATAN:
                        result += "أُ";
                        break;
                    case Hebrew.POINT_HOLAM:
                    case Hebrew.POINT_HOLAM_HASER_FOR_VAV:
                        result += Arabic.LETTER_U_WITH_HAMZA_ABOVE;
                        break;

                    case Hebrew.POINT_HATAF_SEGOL:
                    case Hebrew.POINT_SEGOL:
                        result += Arabic.LETTER_ALEF_WITH_HAMZA_BELOW;
                        result += Arabic.KASRA;
                        break;

                    case Hebrew.POINT_HIRIQ:
                    case Hebrew.POINT_TSERE:
                        if (i < sylChars.Length - 1 && sylChars[i + 1] == 'י' && ((i > sylChars.Length - 3) || hebrew.IsConsonant(sylChars[i + 2])))
                        {
                            result += "إي";
                            i++;
                        }
                        else
                        {
                            result += Arabic.LETTER_ALEF_WITH_HAMZA_BELOW;
                            result += Arabic.KASRA;
                        }
                        break;
                }
                if (result == string.Empty)
                {
                    // the Alph may be followed by an oh or oo 
                    if (sylChars.Length >= 3 && sylChars[i] == 'ו')
                    {
                        if (sylChars[i + 1] == Hebrew.POINT_HOLAM)
                            result += "أُ";
                        else if (sylChars[i + 1] == Hebrew.POINT_DAGESH_OR_MAPIQ)
                            result += "أُوُ";
                        i += 2;
                    }
                }
                else
                    i++;
                if (i >= sylChars.Length)
                {
                    // this a complete open syllable
                    return result;
                }
                if (result != string.Empty)
                    firstVowelDone = true;
            }
            else if (hebrew.IsBegedKefetLetter(sylChars[i]))
            {
                // handle Beged Kefet
                // 
                if (!hebrew.IsVowel(previousVowel) && sylChars[i + 1] == Hebrew.POINT_DAGESH_OR_MAPIQ)
                {
                    // Daghesh Lene (because not preceded by a vowel)
                    switch (sylChars[i])
                    {
                        case 'ב':  // בּ
                            result += 'ب'.ToString();
                            break;
                        case 'ג':
                            result += 'ج'.ToString();  //'غ'.ToString();
                            break;
                        case 'ד':
                            result += 'د'.ToString(); //'ذ'.ToString();
                            break;
                        case 'כ':  // כּ
                            result += 'ك'.ToString();
                            break;
                        case 'פ':
                            result += 'پ'.ToString();
                            break;
                        case 'ת':
                            result += 'ت'.ToString(); // 'ث'.ToString();
                            break;

                    }
                    i += 2;
                }
                else
                    result += hebrew.TranslateConsonant(sylChars[i++]);

            }
            else if (sylChars[i] == 'ש')
            {
                // Handle Seen & sheen
                i++;
                if (hebrew.IsConsonant(sylChars[i]))
                {
                    result += 'س'.ToString();
                }
                else
                {
                    if (sylChars[i] == Hebrew.POINT_SHIN_DOT)
                        result += 'ش'.ToString();
                    else if (sylChars[i] == Hebrew.POINT_SIN_DOT)
                        result += 'س'.ToString();
                    else if (i < sylChars.Length - 1 && hebrew.IsVowel(sylChars[i]))
                    {
                        if (sylChars[i + 1] == Hebrew.POINT_SHIN_DOT)
                        {
                            result += 'ش'.ToString();
                            sylChars[i + 1] = sylChars[i];
                        }
                        else if (sylChars[i + 1] == Hebrew.POINT_SIN_DOT)
                        {
                            result += 'س'.ToString();
                            sylChars[i + 1] = sylChars[i];
                        }
                    }
                    i++;
                }
            }
            else if (sylChars[i] == 'ץ')
            {
                result += "تص";
                i++;
            }
            else
            {
                try
                {
                    if (sylChars[i] == 'ו' && sylChars[i + 1] == Hebrew.POINT_DAGESH_OR_MAPIQ)
                    {
                        if (i == 0)
                        {
                            result += "ڤُو";
                        }
                        else
                        {
                            result += "أُو";
                            firstVowelDone = true;
                        }
                        i += 2;

                    }
                    else
                        result += hebrew.TranslateConsonant(sylChars[i++]);
                }
                catch (Exception)
                {
                    int x = 0;
                }

            }
            //======================================
            // done the first consonant
            // Now, the vowel
            // https://bnaimitzvahacademy.com/hebrew-vowels-chart/
            if (!firstVowelDone)
            {
                try
                {
                    if (sylChars[i] == Hebrew.POINT_DAGESH_OR_MAPIQ)
                    {
                        result += Arabic.SHADDA.ToString();
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    int x = 0;
                }

                if (syllableDef.HasDiphthong)
                {
                    switch (syllableDef.Diphthong)
                    {
                        case "ָיו":
                            result += "اڤ";
                            i += 3;
                            break;
                        case "וּי":
                            result += "وُي";
                            i += 3;
                            break;
                        case "וֹי":
                            result += "ُي";
                            i += 3;
                            break;
                        case "ַיִ":
                            result += "آي";
                            i += 3;
                            break;
                        case "ָי":
                            result += "اي";
                            i += 2;
                            break;
                        case "ִי":
                            result += "ِي";
                            i += 2;
                            break;
                        case "ַי":
                            result += "َيْ";
                            i += 2;
                            break;
                        case "ֵי":
                            result += "ِيْ";
                            i += 2;
                            break;
                    }
                }
                else
                    try
                    {
                        if (sylChars[i] == 'ו')
                        {
                            if (sylChars[i + 1] == Hebrew.POINT_HOLAM)
                                result += "ُ";
                            else if (sylChars[i + 1] == Hebrew.POINT_DAGESH_OR_MAPIQ)
                                result += "وُ";

                            i += 2;
                        }
                        else
                        {
                            if (last && !first & 
                                (i == sylChars.Length - 1) && (sylChars[i] == Hebrew.POINT_PATAH) &&
                                (sylChars[i-1] == 'ח' || sylChars[i-1] == 'ע' || (sylChars[i-1] == Hebrew.POINT_DAGESH_OR_MAPIQ && sylChars[i-2] == 'ה')))
                            {
                                // Furtive Pathach
                                if (result.Length > 1 && (sylChars[i - 1] == 'ח' || sylChars[i - 1] == 'ע'))
                                    result = result.Substring(0, result.Length - 2) + 'أ' + result.Substring(result.Length - 1);
                                else if (result.Length > 2 && (sylChars[i - 1] == Hebrew.POINT_DAGESH_OR_MAPIQ && sylChars[i - 2] == 'ה'))
                                    result = result.Substring(0, result.Length - 3) + 'أ' + result.Substring(result.Length - 2);
                                else
                                    result = 'أ' + result;
                            }
                            else
                                result += hebrew.TranslateNiquod(sylChars[i++]);
                        }
                    }
                    catch (Exception ex)
                    {
                        TraceError(string.Format("[{0}] {1}", hebWord, ex.Message));
                    }
            }
            // done with the vowel

            if (syllableDef.IsClosed)
            {
                if (last)
                {
                    if (sylChars[i] == 'א' && (i == syl.Length - 1 || (i < syl.Length - 1 && !hebrew.IsVowel(sylChars[i + 1]))))
                    {
                        // Quiescent Alef
                        if (sylChars[i - 1] == Hebrew.POINT_QAMATS)
                            result += "ا";
                        i++;
                    }
                    else if (sylChars[i - 1] == Hebrew.POINT_QAMATS || sylChars[i - 1] == Hebrew.POINT_PATAH)
                    {
                        result += "ا";
                    }
                    else if ((i < sylChars.Length - 1) && sylChars[i - 1] == Hebrew.POINT_HIRIQ && sylChars[i] == 'י' && hebrew.IsConsonant(sylChars[i]))
                    {
                        result += "ي";
                        i++;
                    }

                    // this must be the last consonant
                    // check if it has a PATAH
                    if (i < sylChars.Length - 1 && sylChars[i + 1] == Hebrew.POINT_PATAH)
                        result += "أ";
                    try
                    {
                        if (i <= sylChars.Length - 2)
                        {
                            if (sylChars[i] == 'ש' && sylChars[i + 1] == Hebrew.POINT_SHIN_DOT)
                                result += "ش";
                            else if (sylChars[i] == 'ש' && sylChars[i + 1] == Hebrew.POINT_SIN_DOT)
                                result += "س";
                            else if (sylChars[i] == 'ך' && sylChars[i + 1] == Hebrew.POINT_SHEVA)
                                result += "خْ";
                        }
                        else if (sylChars[i] == 'ה')
                        {
                            if (i == sylChars.Length - 1)
                            {
                                if (result[result.Length - 1] == Arabic.DAMMA)
                                    result += "و";
                                else if (result[result.Length - 1] == Arabic.FATHA)
                                    result += "ا";
                            }
                            else
                                result += "ه";
                        }
                        else
                            result += hebrew.TranslateConsonant(sylChars[i++]);
                    }
                    catch (Exception e)
                    {
                        int x = 0;
                    }
                }
                else
                {

                    try
                    {
                        result += hebrew.TranslateConsonant(sylChars[i++]);
                        if (i <= sylChars.Length - 1 && sylChars[i] == Hebrew.POINT_SHEVA)
                            result += Arabic.SUKUN;
                    }
                    catch (Exception e)
                    {
                        int y = 0;
                    }
                }

            }
            else
            {
                if (i < syl.Length)
                {
                    if ((i < syl.Length - 1) && sylChars[i] == 'ו' && (sylChars[i + 1] == Hebrew.POINT_HOLAM || sylChars[i + 1] == Hebrew.POINT_DAGESH_OR_MAPIQ))
                    {
                        result += "و";
                    }
                    else if (i != syl.Length - 1 && sylChars[i] != 'א') // Quiescent Alef
                        TraceError(string.Format("[{0}] {1}", hebWord, "open syllable error."));
                }
            }

            return result;
        }

        public string GetArabicSyllable_Old(SyllableDef syllableDef, char previousVowel, bool first, bool last, string hebWord)
        {
            string result = string.Empty;
            int i = 0;
            string syl = syllableDef.Syllable;
            char[] sylChars = syl.ToCharArray();
            bool firstVowelDone = false;

            if (sylChars[i] == 'א')
            {
                // handle Alef
                switch (sylChars[++i])
                {
                    case Hebrew.POINT_HATAF_PATAH:
                    case Hebrew.POINT_PATAH:
                    case Hebrew.POINT_HATAF_QAMATS:
                    case Hebrew.POINT_QAMATS:
                        if (i < sylChars.Length - 1 && sylChars[i + 1] == 'י' && ((i > sylChars.Length-3) || hebrew.IsConsonant(sylChars[i + 2])))
                        {
                            result += "أي";
                            i++;
                        }
                        else
                        {
                            result += Arabic.LETTER_ALEF_WITH_HAMZA_ABOVE;
                            result += Arabic.FATHA;
                        }
                        break;
                    case Hebrew.POINT_QUBUTS:
                        result += "أُو";
                        break;
                    case Hebrew.POINT_QAMATS_QATAN:
                        result += "أُ";
                        break;
                    case Hebrew.POINT_HOLAM:
                    case Hebrew.POINT_HOLAM_HASER_FOR_VAV:
                        result += Arabic.LETTER_U_WITH_HAMZA_ABOVE;
                        break;

                    case Hebrew.POINT_HATAF_SEGOL:
                    case Hebrew.POINT_SEGOL:
                        result += Arabic.LETTER_ALEF_WITH_HAMZA_BELOW;
                        result += Arabic.KASRA;
                        break;

                    case Hebrew.POINT_HIRIQ:
                    case Hebrew.POINT_TSERE:
                        if (i < sylChars.Length - 1 && sylChars[i + 1] == 'י' && ((i > sylChars.Length - 3) || hebrew.IsConsonant(sylChars[i + 2])))
                        {
                            result += "إي";
                            i++;
                        }
                        else
                        {
                            result += Arabic.LETTER_ALEF_WITH_HAMZA_BELOW;
                            result += Arabic.KASRA;
                        }
                        break;
                }
                if (result == string.Empty)
                {
                    // the Alph may be followed by an oh or oo 
                    if(sylChars.Length >= 3 && sylChars[i] == 'ו')
                    {
                        if (sylChars[i + 1] == Hebrew.POINT_HOLAM)
                            result += "أُ";
                        else if (sylChars[i + 1] == Hebrew.POINT_DAGESH_OR_MAPIQ)
                            result += "أُوُ";
                        i += 2;
                    }
                }
                else
                    i++;
                if (i >= sylChars.Length)
                {
                    // this a complete open syllable
                    return result;
                }
                if (result != string.Empty)
                    firstVowelDone = true;
            }
            else if (hebrew.IsBegedKefetLetter(sylChars[i]))
            {
                // handle Beged Kefet
                // 
                if (!hebrew.IsVowel(previousVowel) && sylChars[i + 1] == Hebrew.POINT_DAGESH_OR_MAPIQ)
                {
                    // Daghesh Lene (because not preceded by a vowel)
                    switch (sylChars[i])
                    {
                        case 'ב':  // בּ
                            result += 'ب'.ToString();
                            break;
                        case 'ג':
                            result += 'ج'.ToString();  //'غ'.ToString();
                            break;
                        case 'ד':
                            result += 'د'.ToString(); //'ذ'.ToString();
                            break;
                        case 'כ':  // כּ
                            result += 'ك'.ToString();
                            break;
                        case 'פ':
                            result += 'پ'.ToString();
                            break;
                        case 'ת':
                            result += 'ت'.ToString(); // 'ث'.ToString();
                            break;

                    }
                    i += 2;
                }
                else
                    result += hebrew.TranslateConsonant(sylChars[i++]);

            }
            else if (sylChars[i] == 'ש')
            {
                // Handle Seen & sheen
                i++;
                if (hebrew.IsConsonant(sylChars[i]))
                {
                    result += 'س'.ToString();
                }
                else
                {
                    if (sylChars[i] == Hebrew.POINT_SHIN_DOT)
                        result += 'ش'.ToString();
                    else if (sylChars[i] == Hebrew.POINT_SIN_DOT)
                        result += 'س'.ToString();
                    else if (i < sylChars.Length - 1 && hebrew.IsVowel(sylChars[i]))
                    {
                        if (sylChars[i + 1] == Hebrew.POINT_SHIN_DOT)
                        {
                            result += 'ش'.ToString();
                            sylChars[i + 1] = sylChars[i];
                        }
                        else if (sylChars[i + 1] == Hebrew.POINT_SIN_DOT)
                        {
                            result += 'س'.ToString();
                            sylChars[i + 1] = sylChars[i];
                        }
                    }
                    i++;
                }
            }
            else if (sylChars[i] == 'ץ')
            {
                result += "تص";
                i++;
            }
            else
            {
                try
                {
                    if(sylChars[i] == 'ו' && sylChars[i + 1] == Hebrew.POINT_DAGESH_OR_MAPIQ)
                    {
                        if(i == 0)
                        {
                            result += "ڤُو";
                        }
                        else
                        {
                            result += "أُو";
                            firstVowelDone = true;
                        }
                        i += 2;
                        
                    }
                    else
                        result += hebrew.TranslateConsonant(sylChars[i++]);
                }
                catch (Exception)
                {
                    int x = 0;
                }

            }
            //======================================
            // done the first consonant
            // Now, the vowel
            // https://bnaimitzvahacademy.com/hebrew-vowels-chart/
            if (!firstVowelDone)
            {
                try
                {
                    if (sylChars[i] == Hebrew.POINT_DAGESH_OR_MAPIQ)
                    {
                        result += Arabic.SHADDA.ToString();
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    int x = 0;
                }

                if (syllableDef.HasDiphthong)
                {
                    switch (syllableDef.Diphthong)
                    {
                        case "ָיו":
                            result += "اڤ";
                            i += 3;
                            break;
                        case "וּי":
                            result += "وُي";
                            i += 3;
                            break;
                        case "וֹי":
                            result += "ُي";
                            i += 3;
                            break;
                        case "ַיִ":
                            result += "آي";
                            i += 3;
                            break;
                        case "ָי":
                            result += "اي";
                            i += 2;
                            break;
                        case "ִי":
                            result += "ِي";
                            i += 2;
                            break;
                        case "ַי":
                            result += "َيْ";
                            i += 2;
                            break;
                        case "ֵי":
                            result += "ِيْ";
                            i += 2;
                            break;
                    }
                }
                else
                    try
                    {
                        if (sylChars[i] == 'ו')
                        {
                            if (sylChars[i + 1] == Hebrew.POINT_HOLAM)
                                result += "ُ";
                            else if (sylChars[i + 1] == Hebrew.POINT_DAGESH_OR_MAPIQ)
                                result += "وُ";

                            i += 2;
                        }
                        else
                        {
                            if ((i == sylChars.Length - 1) && (sylChars[i] == Hebrew.POINT_PATAH))
                            {
                                // Furtive Pathach
                                if (result.Length > 1)
                                    result = result.Substring(0, result.Length - 2) + 'أ' + result.Substring(result.Length - 1);
                                else
                                    result = 'أ' + result;
                            }
                            else
                                result += hebrew.TranslateNiquod(sylChars[i++]);
                        }
                    }
                    catch(Exception ex)
                    {
                        TraceError(string.Format("[{0}] {1}", hebWord, ex.Message));
                    }
            }
            // done with the vowel

            if (syllableDef.IsClosed)
            {
                if (last)
                {
                    if (i < syl.Length - 1 && sylChars[i] == 'א' && !hebrew.IsVowel(sylChars[i + 1]))
                    {
                        // Quiescent Alef
                        i++;
                    }
                    else if (sylChars[i - 1] == Hebrew.POINT_QAMATS || sylChars[i - 1] == Hebrew.POINT_PATAH)
                    {
                        result += "ا";
                    }
                    else if ((i < sylChars.Length - 1) && sylChars[i - 1] == Hebrew.POINT_HIRIQ && sylChars[i] == 'י' && hebrew.IsConsonant(sylChars[i]))
                    {
                        result += "ي";
                        i++;
                    }

                    // this must be the last consonant
                    // check if it has a PATAH
                    if (i < sylChars.Length - 1 && sylChars[i + 1] == Hebrew.POINT_PATAH)
                        result += "أ";
                    try
                    {
                        if (i <= sylChars.Length - 2)
                        {
                            if (sylChars[i] == 'ש' && sylChars[i + 1] == Hebrew.POINT_SHIN_DOT)
                                result += "ش";
                            else if (sylChars[i] == 'ש' && sylChars[i + 1] == Hebrew.POINT_SIN_DOT)
                                result += "س";
                            else if (sylChars[i] == 'ך' && sylChars[i + 1] == Hebrew.POINT_SHEVA)
                                result += "خْ";
                        }
                        else if (sylChars[i] == 'ה')
                        {
                            if (i == sylChars.Length - 1)
                            {
                                if (result[result.Length - 1] == Arabic.DAMMA)
                                    result += "و";
                                else if (result[result.Length - 1] == Arabic.FATHA)
                                    result += "ا";
                            }
                            else
                                result += "ه";
                        }
                        else
                            result += hebrew.TranslateConsonant(sylChars[i++]);
                    }
                    catch (Exception e)
                    {
                        int x = 0;
                    }
                }
                else
                {

                    try
                    {
                        result += hebrew.TranslateConsonant(sylChars[i++]);
                        if (i <= sylChars.Length - 1 && sylChars[i] == Hebrew.POINT_SHEVA)
                            result += Arabic.SUKUN;
                    }
                    catch(Exception e)
                    {
                        int y = 0;
                    }
                }

            }
            else
            {
                if (i < syl.Length)
                {
                    if((i < syl.Length - 1) && sylChars[i] == 'ו' && (sylChars[i+1] == Hebrew.POINT_HOLAM || sylChars[i+1] == Hebrew.POINT_DAGESH_OR_MAPIQ))
                    {
                        result += "و";
                    }
                    else if(i != syl.Length -1 && sylChars[i] != 'א') // Quiescent Alef
                        TraceError(string.Format("[{0}] open syllable error.", hebWord));

                }
            }

            return result;
        }

        internal void Transliterate(NpgsqlDataSource dataSource)
        {
            mainForm.Trace("Transliteration Started ...", Color.Green);

            string cmdText = string.Empty;

            Dictionary<string, string> transliterations = new Dictionary<string, string>();

            try
            {
                var command = dataSource.CreateCommand();

                cmdText = "SELECT strongs_number,d_strong,original_word FROM public.\"strongs_numbers\";";

                command.CommandText = cmdText;
                NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string strong = (string)reader[0] + (string)reader[1];
                    transliterations.Add(strong, (string)reader[2]);
                }
                reader.Close();
                reader.DisposeAsync();

                string tableName = "dictionary_translation";
                int language = 7;
                //TransliterateHeb2Ara trns = new TransliterateHeb2Ara(mainForm);
                foreach (string strong in transliterations.Keys)
                {
                    string trx = transliterations[strong];
                    string strongs = strong.Trim();
                    string dStrong = string.Empty;
                    if (strongs.Length > 5)
                    {
                        dStrong = strongs[5].ToString();
                        strongs = strongs.Substring(0, 5);
                    }
                    if (strongs[0] == 'H')
                    {

                        string ara = string.Empty;

                        if (strongs == "H3485")
                            ara = "يِسّ.سَ.خَار";
                        else if (strongs == "H4881")
                            ara = "مْ.سؙ.خَّا";
                        else
                            ara = heb2Ara.Translitrate(trx, "");

                        if (!string.IsNullOrEmpty(ara))
                            trx = ara;
                        else
                            TraceError(string.Format("Translitrate returned null for '{0}", trx));
                    }
                    if(strongs == "H3492")
                    {
                        int x = 0;
                    }
                    cmdText = "UPDATE public.\"" + tableName + "\" " +
                            string.Format("SET transliteration='{0}' ",
                            trx) +
                            string.Format("WHERE strongs_number='{0}' AND d_strong='{1}' AND language_id={2};",
                            strongs, dStrong,language);
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
