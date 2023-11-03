using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;

namespace CleanTranslatedText
{
    public partial class MainForm : Form
    {

        Dictionary<string, string> strongMap = new Dictionary<string, string>();
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        #region Trace
        delegate void TraceDelegate(string text, Color color);
        delegate void ClearTraceDelegate();

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
        public void Trace(string text, Color color)
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
            Trace(string.Format("Error: {0}::{1}", method, text), Color.Red);
        }


        #endregion Trace

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();//openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderPath = folderBrowserDialog1.SelectedPath;
                if (folderPath != null)
                {
                    string greekPath = Path.Combine(folderPath, "ParsedTBESGArabic.txt");
                    string hebrewPath = Path.Combine(folderPath, "ParsedTBESHArabic.txt");

                    string cleanGreekPath = string.Empty;
                    string cleanHebrewPath = string.Empty;

                    CleanFile(greekPath, out cleanGreekPath);
                    CleanFile(hebrewPath, out cleanHebrewPath);

                    string outPath = Path.Combine(folderPath, "CleanTBES_G_H_Arabic.txt");
                    string lexicon = File.ReadAllText(cleanGreekPath);
                    lexicon += File.ReadAllText(cleanHebrewPath);

                    File.WriteAllText(outPath, lexicon);
                    
                    Trace(string.Format("CleanTBES_G_H_Arabic.txt Generated"), Color.Green);

                    Trace(string.Format("All Done!"), Color.Green);
                }
            }
        }

        private void CleanFile(string filePath, out string cleanPath)
        {

            bool greek = false;
            if (filePath.Contains("TBESG"))
                greek = true;

            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string folder = Path.GetDirectoryName(filePath);
            string outPath = Path.Combine(folder, fileName + "Clean.txt");

            Trace("Cleaning " + fileName, Color.Blue);

            cleanPath = outPath;

            string englishPath = filePath.Replace("Arabic", "");

            try
            {
                BuildStrongMap(filePath, englishPath);

                string text = File.ReadAllText(filePath);


                Trace(string.Format("Fixing Strong Numbers"), Color.Blue);
                foreach (string arabicStrong in strongMap.Keys)
                {
                    text = text.Replace(arabicStrong, strongMap[arabicStrong]);
                }


                Trace(string.Format("Removing double quotes"), Color.Blue);
                text = Regex.Replace(text, @"""[ ]*", "");


                if (greek)
                {
                    text = text.Replace("( सेग्यlem 26:18 , ặ)* ;]", "(סְגֻלָּה, עַם)* ;]");
                    text = text.Replace("( аëlon 2: 17 ( )", "(לִמּוּד)");

                    text = Regex.Replace(text, @"IV[ ]*", "4");
                    text = Regex.Replace(text, @"III[ ]*", "3");
                    text = Regex.Replace(text, @"II[ ]*", "2");

                    text = RegExReplace(text, @"indecl\.\;", "");
                    text = RegExReplace(text, @"indecl\.", "");
                    text = RegExReplace(text, @"indecl\,", "");
                    text = RegExReplace(text, @"indecl", "");

                    text = Regex.Replace(text, @"[ ]*[†]{0,1}[ ]*\(مثل\)", "");
                    text = Regex.Replace(text, @"[ ]*[†]{0,1}[ ]*\(AS\)", "");
                    text = Regex.Replace(text, @"[ ]*[†]{0,1}[ ]*\(ع\)", "");
                    text = text.Replace("3 مل", "3مل");
                    text = text.Replace("2 مل", "2مل");
                    text = text.Replace("1 مل", "1مل");

                    text = Regex.Replace(text, @"\(عبرانيين", "(بالعبرية");

                    text = text.Replace("Dan LXX Bel", "Dan70B");
                    text = text.Replace("Dan LXX", "Dan70");
                    text = text.Replace("دا LXX", "Dan70");
                    text = text.Replace("دان LXX", "Dan70");

                    Trace(string.Format("Replacing LXX"), Color.Blue);
                    text = text.Replace("السبعينية", "الترجمة السبعينية");
                    text = text.Replace("LXX", "الترجمة السبعينية");
                }

                text = text.Replace("المرجع", "راجع");
                text = text.Replace("Ref:", "راجع");
                text = text.Replace("مرجع", "راجع");

                File.WriteAllText(outPath, text);


                //++++++++++++++++++++++
                string[] arabicLines = File.ReadAllLines(outPath);
                VerseReferences vref = new VerseReferences(this);


                vref.CorrectAbbreviations(arabicLines, greek);
                if (greek)
                {
                    vref.RemoveApocrypha(arabicLines);
                }

                FixHebrew(arabicLines, englishPath, outPath);

                File.WriteAllLines(outPath, arabicLines);

                Trace(string.Format("Clean Done!"), Color.Green);

            }
            catch (Exception ex)
            {
                TraceError(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }


        /// <summary>
        /// Hebrew is sometimes translated by Google into Russion or some other language
        /// Restore the Hebrew from the English text the Hebrew from 
        /// </summary>
        /// <param name="arabicPath"></param>
        /// <param name="englishPath"></param>
        /// <param name="outPath"></param>
        private void FixHebrew(string[] arabicLines, string englishPath, string outPath)
        {
            int lineNo = 0;
            try
            {
                ///key arabic book name in the translation
                ///value dictionary
                /// key line number
                /// value list of english names in the original
                ///

                SortedDictionary<string, Dictionary<int, List<string>>> arabicBookNames = new SortedDictionary<string, Dictionary<int, List<string>>>();
                //                using (StreamReader arabicSR = new StreamReader(arabicPath))
                {
                    string pattern = @"([\d]{0,1}[ ]*[a-zA-Z\u0620-\u064A]{2,12})[.]{0,1}[ ]*([\d]{1,2}[ ]*\:[ ]*[\d]{1,3})";
                    using (StreamReader englishSR = new StreamReader(englishPath))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(outPath))
                        {
                            while (lineNo < arabicLines.Length && !englishSR.EndOfStream)
                            {
                                string englishLine = englishSR.ReadLine();
                                string arabicLine = arabicLines[lineNo++];

                                if (lineNo == 977)
                                {
                                    int x = 0;
                                }

                                string heb = string.Empty;
                                string ara = string.Empty;

                                Match engMatch = Regex.Match(englishLine, @"(\(Heb\.)[,]*[ ]([ ,\u0590-\u05FF]*)");
                                if (engMatch.Success)
                                {
                                    heb = string.Format("(بالعبرية: {0}", engMatch.Groups[2].Value);
                                }

                                Match araMatch = Regex.Match(arabicLine, @"(\(بالعبرية[: ,\u0100-\uFFFF]*)");
                                if (araMatch.Success)
                                {
                                    ara = araMatch.Groups[1].Value;
                                }
                                else
                                {
                                    araMatch = Regex.Match(arabicLine, @"(\(Heb\.)[,]*[ ]([ .,\u0590-\u05FF]*)");
                                    if (araMatch.Success)
                                    {
                                        ara = string.Format("{0} {1}", araMatch.Groups[1].Value, araMatch.Groups[2].Value);
                                    }
                                }

                                if (heb != string.Empty && ara != string.Empty)
                                {
                                    arabicLine = arabicLine.Replace(ara, heb);
                                }

                                /// get Bible book names
                                List<string> arabic = new List<string>();
                                List<string> english = new List<string>();

                                MatchCollection matches = Regex.Matches(arabicLine, pattern);
                                if (matches.Count > 0)
                                {
                                    foreach (Match match in matches)
                                    {
                                        if (match.Success)
                                        {
                                            string reference = match.Groups[1].Value;
                                            if (!arabic.Contains(match.Groups[1].Value.Replace(" ", "")))
                                                arabic.Add(match.Groups[1].Value.Replace(" ", ""));
                                            //Trace(string.Format("L{0}\t{1}\t{2} {3}",
                                            //    lineNo,
                                            //    match.Groups[0].Value,
                                            //    match.Groups[1].Value.Replace(" ", ""),
                                            //    match.Groups[2].Value.Replace(" ", "")), Color.Black); ;
                                        }
                                    }
                                }

                                matches = Regex.Matches(englishLine, pattern);
                                if (matches.Count > 0)
                                {
                                    foreach (Match match in matches)
                                    {
                                        if (match.Success)
                                        {
                                            string reference = match.Groups[1].Value;
                                            if (!english.Contains(match.Groups[1].Value.Replace(" ", "")))
                                                english.Add(match.Groups[1].Value.Replace(" ", ""));
                                        }
                                    }
                                }

                                foreach (string a in arabic)
                                {
                                    if (arabicBookNames.ContainsKey(a))
                                    {
                                        Dictionary<int, List<string>> entry = arabicBookNames[a];
                                        if (entry.ContainsKey(lineNo))
                                        {
                                            entry[lineNo].AddRange(english);
                                        }
                                        else
                                        {
                                            entry.Add(lineNo, english);
                                        }
                                    }
                                    else
                                    {
                                        // create a new entry
                                        Dictionary<int, List<string>> entry = new Dictionary<int, List<string>>();
                                        entry.Add(lineNo, english);
                                        arabicBookNames.Add(a, entry);
                                    }
                                }
                                //arabicLine = ConvertVersesToArabic(arabicLine);
                                streamWriter.WriteLine(arabicLine);
                            }
                        }
                    }
                }
                foreach (string name in arabicBookNames.Keys)
                {
                    if (arabicBookAbbreviations.Contains(name))
                        continue;

                    string line = name + "\t\t";
                    var entry = arabicBookNames[name];
                    foreach (int num in entry.Keys)
                    {
                        line += string.Format("[{0}: {1}]\r\n\t\t", num, String.Join(",", entry[num]));
                    }
                    //Trace(line.Trim(), Color.Black);
                }
            }
            catch (Exception ex)
            {
                TraceError(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private string ConvertVersesToArabic(string line)
        {
            string result = line;
            // convert verse references Arabic numerals to Hindu numerals 
            while (true)
            {
                // Arabic \u0620-\u064A
                // Hebrew \u0590-\u05FF
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

        private string FixMisc(string text)
        {
            string result = text.
                Replace("الاسم الصحيح", "إسم علم").
                Replace("المرجع", "انظر").
                Replace("مرجع", "انظر").
                Replace("روم ", "رو ").
                Replace("1كو", "١كو").
                Replace("1 كو", "١كو").
                Replace("2 كو", "٢كو").
                Replace("2كو", "٢كو").
                Replace("غلا ", "غل ").
                 Replace("1 تي", "١تي").
                Replace("2 تي", "٢تي").
            Replace("١ بي", "١بط").
            Replace("2 بط", "٢بط").
            Replace("1يو", "١يو").
            Replace("2ي", "٢يو").
            Replace("†", "").Trim();


            result = RegExReplace(result, @"2ti", "٢تي");
            result = RegExReplace(result, @"2pe", "٢بط");
            result = RegExReplace(result, @"1co", "١كو");
            result = RegExReplace(result, @"2co", "٢كو");
            result = RegExReplace(result, @"php", "في");
            result = RegExReplace(result, @"\(AS\)", "");
            result = RegExReplace(result, @"mk", "مر");
            result = RegExReplace(result, @"mk", "مر");
            result = RegExReplace(result, @"col", "كو");
            result = RegExReplace(result, @"1th", "١تس");
            result = RegExReplace(result, @"2th", "٢تس");
            result = RegExReplace(result, @"1Ti", "١تي");
            result = RegExReplace(result, @"phlm\.", "فل");
            result = RegExReplace(result, @"act", "أع");

            return result;

        }

        private string RegExReplace(string text, string oldTxt, string newTxt)
        {
            Regex regex = new Regex(oldTxt, RegexOptions.IgnoreCase);
            return regex.Replace(text, newTxt);
        }

        private void BuildStrongMap(string arabicPath, string englishPath)
        {
            try
            {
                using (StreamReader arabicSR = new StreamReader(arabicPath))
                {
                    using (StreamReader englishSR = new StreamReader(englishPath))
                    {
                        while (!arabicSR.EndOfStream && !englishSR.EndOfStream)
                        {
                            string arabicLine = arabicSR.ReadLine();
                            string englishLine = englishSR.ReadLine();

                            string strongReg = @"^([GH\u0620-\u064A]\d\d\d\d[a-zA-Z\u0620-\u064A]*)\t";
                            string strongNoPrefix = @"^([GH\u0620-\u064A]*\d\d\d\d[a-zA-Z\u0620-\u064A]*)\t";
                            string strongGPrefix = @"^(جي \d\d\d\d[a-zA-Z\u0620-\u064A]*)\t";
                            string strongMagPrefix = @"^(مجموعة \d\d\d\d[a-zA-Z\u0620-\u064A]*)\t";

                            string englishStrong = string.Empty;
                            string arabicStrong = string.Empty;

                            Match match = Regex.Match(englishLine, strongReg);
                            if (match.Success)
                            {
                                englishStrong = match.Groups[1].Value;
                            }
                            else
                            {
                                TraceError(MethodBase.GetCurrentMethod().Name,
                                    string.Format("No Strong in line: {0}", englishLine));
                            }

                            match = Regex.Match(arabicLine, strongNoPrefix);
                            if (match.Success)
                            {
                                arabicStrong = match.Groups[1].Value;
                            }
                            else
                            {
                                match = Regex.Match(arabicLine, strongGPrefix);
                                if (match.Success)
                                {
                                    arabicStrong = match.Groups[1].Value;
                                }
                                else
                                {
                                    match = Regex.Match(arabicLine, strongMagPrefix);
                                    if (match.Success)
                                    {
                                        arabicStrong = match.Groups[1].Value;
                                    }
                                    else
                                    {
                                        TraceError(MethodBase.GetCurrentMethod().Name,
                                            string.Format("No Strong in line: {0}", arabicLine));
                                    }
                                }

                            }

                            if (englishStrong != arabicStrong)
                            {
                                if (strongMap.ContainsKey(arabicStrong))
                                {
                                    if (strongMap[arabicStrong] != englishStrong)
                                    {
                                        TraceError(MethodBase.GetCurrentMethod().Name,
                                           string.Format("Arabic [{0}] cannot be mapped to [{1}] because it is already mapped to [{2}]",
                                           arabicStrong, englishStrong, strongMap[arabicStrong]));
                                    }
                                }
                                else
                                {
                                    strongMap.Add(arabicStrong, englishStrong);
                                }
                            }
                        }

                    }
                }


            }
            catch (Exception ex)
            {
                TraceError(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private string[] arabicBookAbbreviations =
        {
            "تك",
            "خر",
            "لا",
            "عد",
            "تث",
            "يش",
            "قض",
            "را",
            "١صم",
            "٢صم",
            "١مل",
            "٢مل",
            "١أخ",
            "٢أخ",
            "عز",
            "نح",
            "أس",
            "أي",
            "مز",
            "أم",
            "جا",
            "نش",
            "إش",
            "إر",
            "مرا",
            "حز",
            "دا",
            "هو",
            "يوئ",
            "عا",
            "عو",
            "يون",
            "مي",
            "نا",
            "حب",
            "صف",
            "حج",
            "زك",
            "مل",
            "مت",
            "مر",
            "لو",
            "يو",
            "أع",
            "رو",
            "١كو",
            "٢كو",
            "غل",
            "أف",
            "في",
            "كو",
            "١تس",
            "٢تس",
            "١تي",
            "٢تي",
            "تي",
            "فل",
            "عب",
            "يع",
            "١بط",
            "٢بط",
            "١يو",
            "٢يو",
            "٣يو",
            "يه",
            "رؤ",
        };
    }


}