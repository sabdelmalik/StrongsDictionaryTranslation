using BibleTaggingUtil;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryEditorV1
{
    internal class DictionaryDump
    {
        MainForm mainForm;
        public DictionaryDump(MainForm mainForm) 
        { 
            this.mainForm = mainForm;
        }

        internal void DumpJSON(int targetLanguage, string outFile, bool outputDStrong, bool supressReferences, SortedDictionary<int, List<char>> hebrewStrongs, SortedDictionary<int, List<char>> greekStrongs)
        {
            using (StreamWriter jsonList = new StreamWriter(outFile))
            {
                List<LexiconJsonObject> strings = new List<LexiconJsonObject>()
                    ;
                int ot = OutputJsonObjects(targetLanguage, strings, hebrewStrongs, outputDStrong, supressReferences, "H", true);
                //int nt = OutputJsonObjects(targetLanguage, strings, greekStrongs, outputDStrong, supressReferences, "G", false);

                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                using (JsonWriter writer = new JsonTextWriter(jsonList))
                {
                    serializer.Serialize(writer, strings);
                    // {"ExpiryDate":new Date(1230375600000),"Price":0}
                }

            }
        

        }

        internal void DumpCSV(int targetLanguage, string outFile, bool outputDStrong, bool supressReferences, SortedDictionary<int, List<char>> hebrewStrongs, SortedDictionary<int, List<char>> greekStrongs)
        {
            using (StreamWriter sw = new StreamWriter(outFile))
            {
                List<LexiconJsonObject> strings = new List<LexiconJsonObject>()
                    ;
                int ot = OutputJsonObjects(targetLanguage, strings, hebrewStrongs, outputDStrong, supressReferences, "H", true);
                //                int nt = OutputJsonObjects(targetLanguage, strings, greekStrongs, outputDStrong, supressReferences, "G", false);

                // write the header
                string line = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}",
                        "Strongs",
                        "Hebrew",
                        "English",
                        "Transliteration",
                        "Arabic",
                        "Definition",
                        "References"
                        );

                sw.WriteLine(line);

                foreach ( LexiconJsonObject lexiconJsonObject in strings )
                {
                    line = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}",
                        lexiconJsonObject.Strongs,
                        lexiconJsonObject.Hebrew,
                        lexiconJsonObject.English,
                        lexiconJsonObject.Transliteration,
                        lexiconJsonObject.Arabic,
                        lexiconJsonObject.Definition,
                        lexiconJsonObject.References
                        );

                    sw.WriteLine(line);
                }
            }
        }

        internal void DumpJSON_Old(int targetLanguage, string outFile, SortedDictionary<int, List<char>> hebrewStrongs, SortedDictionary<int, List<char>> greekStrongs)
        {
            using (StreamWriter jsonList = new StreamWriter(outFile))
            {
                // open array
                jsonList.WriteLine("[");

                int ot = OutputJsonObjects_Old(targetLanguage, jsonList, hebrewStrongs, "H", true);
                int nt = OutputJsonObjects_Old(targetLanguage, jsonList, greekStrongs, "G", false);

                //GetBibleVerses("Gen 27:21, Jdg 12:5, Jdg 13:11, 2Sa 2:20, 2Sa 7:5, 2Sa 9:2, 2Sa 20:17, 1Ki 1:24, 1Ki 13:14, 1Ki 18:7, 1Ki 18:17, 1Ki 21:7, Isa 14:10,");


                // close array
                jsonList.WriteLine("");
                jsonList.WriteLine("]");
            }
        }

        private int OutputJsonObjects(int targetLanguage, List<LexiconJsonObject> jsonList, SortedDictionary<int, List<char>> strongsDict, bool outputDStrong, bool supressReferences, string prefix, bool first)
        {
            int result = 0;


            List<LexiconEntry> listForStrong = new List<LexiconEntry>();

            foreach (int strongs in strongsDict.Keys)
            {
                List<char> chars = strongsDict[strongs];
                bool firstLine = true;

                listForStrong.Clear();

                foreach (char c in chars)
                {
                    string strongsNumber = string.Format("{0}{1:D4}", prefix, strongs);
                    string dStrong = c.ToString().Trim();

                    LexiconEntry entry = new LexiconEntry();

                    bool res = mainForm.GetStrongsEntry(strongsNumber, dStrong, false, entry);
                    if (res)
                    {
                        res = mainForm.GetDictionaryTranslation(targetLanguage, strongsNumber, dStrong, false, entry);
                    }
                    if (res)
                    {
                        // Now "entry" contains all the information for StrongNum + d
                        // Add it to this strong's list
                        entry.StrongsNum = strongsNumber;
                        entry.dStrong = dStrong;

                        listForStrong.Add(entry);
                    }
                }

                // Now process the Strong list of entries
                if (listForStrong.Count == 0)
                    continue;

                if (!supressReferences)
                {
                    // populate reference 
                    for (int i = 0; i < listForStrong.Count; i++)
                    {
                        LexiconEntry entry = listForStrong[i];
                        string strongsNumber = entry.StrongsNum;
                        string dStrong = entry.dStrong;

                        entry.References = string.Empty;
                        Dictionary<string, string> associatedWords = mainForm.GetAssociatedWords(targetLanguage, strongsNumber, dStrong);
                        if (associatedWords != null && associatedWords.Count > 0)
                        {
                            List<string> refsSum = new List<string>();
                            foreach (string refs in associatedWords.Values)
                            {
                                string[] refParts = refs.Split(new char[] { ',' });
                                foreach (string refPart in refParts)
                                {
                                    int idx = Utils.GetBookIndex(refPart.Trim());
                                    string litedRefs = string.Format("{0:d2}-{1}", idx, refPart.Trim());
                                    if (refsSum.Contains(litedRefs))
                                        continue;

                                    refsSum.Add(litedRefs);
                                }
                            }
                            refsSum.Sort();
                            for (int j = 0; j < refsSum.Count; j++)
                            {
                                refsSum[j] = refsSum[j].Substring(3);
                            }
                            entry.References = string.Join(",", refsSum);
                        }
                    }
                }

                if (!outputDStrong)
                {
                    // output uStrong
                    for (int i = 0; i < listForStrong.Count; i++)
                    {
                        LexiconEntry entry = listForStrong[i];
                        string def = entry.Definition.Trim();
                        while (def.StartsWith("\"")) def = def.Substring(1);
                        while (def.EndsWith("\"")) def = def.Substring(0, def.Length - 1);

                        string gloss = entry.Gloss;
                        while (gloss.StartsWith("\"")) gloss = gloss.Substring(1);
                        while (gloss.EndsWith("\"")) gloss = gloss.Substring(0, gloss.Length - 1);
                        if (i == 0)
                        {
                            entry.Definition = def.Replace("\"\"", "\"");
                            entry.Gloss = gloss;
                        }
                        else
                        {
                            listForStrong[0].Definition += "  **  " + listForStrong[i].Definition.Replace("\"\"", "\"");
                        }
                    }
                    LexiconJsonObject lexiconObject = new LexiconJsonObject
                    {

                        Strongs = listForStrong[0].StrongsNum,
                        Hebrew = listForStrong[0].UnicodeAccented,
                        English = listForStrong[0].English,
                        Transliteration = listForStrong[0].Transliteration,
                        Arabic = listForStrong[0].Gloss,
                        Definition = listForStrong[0].Definition,
                        References = listForStrong[0].References
                    };

                    jsonList.Add(lexiconObject);
                    result++;
                }
                else
                {
                    if (listForStrong.Count > 1 && outputDStrong)
                    {
                        LexiconEntry entry = listForStrong[listForStrong.Count - 1];
                        string last = entry.Definition;
                        int idx = last.IndexOf('§');
                        if (idx == -1)
                        {
                            mainForm.TraceError(MethodBase.GetCurrentMethod().Name,
                                string.Format("{0} does not contain §", entry.StrongsNum + entry.dStrong));
                        }
                        else
                        {
                            string trailerText = last.Substring(idx);
                            for (int i = 0; i < listForStrong.Count - 1; i++)
                                listForStrong[i].Definition += "  " + trailerText;
                        }
                    }

                    // now all entries are ready
                    for (int i = 0; i < listForStrong.Count; i++)
                    {
                        LexiconEntry entry = listForStrong[i];

                        string def = entry.Definition.Trim();
                        while (def.StartsWith("\"")) def = def.Substring(1);
                        while (def.EndsWith("\"")) def = def.Substring(0, def.Length - 1);

                        string gloss = entry.Gloss;
                        while (gloss.StartsWith("\"")) gloss = gloss.Substring(1);
                        while (gloss.EndsWith("\"")) gloss = gloss.Substring(0, gloss.Length - 1);

                        LexiconJsonObject lexiconObject = new LexiconJsonObject
                        {

                            Strongs = entry.StrongsNum + entry.dStrong,
                            Hebrew = entry.UnicodeAccented,
                            English = entry.English,
                            Transliteration = entry.Transliteration,
                            Arabic = gloss,
                            Definition = def.Replace("\"\"", "\""),
                            References = entry.References
                        };

                        jsonList.Add(lexiconObject);
                        result++;
                    }
                }
            }
            return result;
        }

        private int OutputJsonObjects_Old2(int targetLanguage, List<LexiconJsonObject> jsonList, SortedDictionary<int, List<char>> strongsDict, string prefix, bool first)
        {
            int result = 0;
            LexiconEntry entry = new LexiconEntry();
 
            LexiconJsonObject lexiconObject = null; 

            foreach (int strongs in strongsDict.Keys)
            {
                List<char> chars = strongsDict[strongs];
                bool firstLine = true;

                foreach (char c in chars)
                {
                    string strongsNumber = string.Format("{0}{1:D4}", prefix, strongs);
                    string dStrong = c.ToString().Trim();
                    entry.Clear();
                    bool res = mainForm.GetStrongsEntry(strongsNumber, dStrong, false, entry);
                    if (res)
                    {
                        res = mainForm.GetDictionaryTranslation(targetLanguage, strongsNumber, dStrong, false, entry);
                    }
                    if (res)
                    {
                        // Now "entry" contains all the information for StrongNum + d
                        string references = string.Empty;
                        Dictionary<string, string> associatedWords = mainForm.GetAssociatedWords(targetLanguage, strongsNumber, dStrong);
                        if (associatedWords != null && associatedWords.Count > 0)
                        {
                            List<string> refsSum = new List<string>();
                            foreach (string refs in associatedWords.Values)
                            {
                                string[] refParts = refs.Split(new char[] { ',' });
                                foreach (string refPart in refParts)
                                {
                                    int idx = Utils.GetBookIndex(refPart.Trim());
                                    string litedRefs = string.Format("{0:d2}-{1}", idx, refPart.Trim());
                                    if (refsSum.Contains(litedRefs))
                                        continue;

                                    refsSum.Add(litedRefs);
                                }
                            }
                            refsSum.Sort();
                            for (int i = 0; i < refsSum.Count; i++)
                            {
                                refsSum[i] = refsSum[i].Substring(3);
                            }
                            references = string.Join(",", refsSum);
                        }

          
                        string def = entry.Definition.Trim();
                        while (def.StartsWith("\"")) def = def.Substring(1);
                        while (def.EndsWith("\"")) def = def.Substring(0, def.Length - 1);

                        string gloss = entry.Gloss;
                        while (gloss.StartsWith("\"")) gloss = gloss.Substring(1);
                        while (gloss.EndsWith("\"")) gloss = gloss.Substring(0, gloss.Length - 1);

                        if (firstLine)
                        {
                            lexiconObject = new LexiconJsonObject
                            {
                                Strongs = strongsNumber,
                                Hebrew = entry.UnicodeAccented,
                                English = entry.English,
                                Transliteration = entry.Transliteration,
                                Arabic = gloss,
                                Definition = def.Replace("\"\"", "\""),
                                References = references
                            };
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(def))
                            {
                                lexiconObject.Definition += "  **  " + def.Replace("\"\"", "\"");
                            }
                        }

                        firstLine = false;
                        //string jsonString = JsonConvert.SerializeObject(lexiconObject, Formatting.Indented);
                        //break; // limit output to one strong number
                    }

                }
                if (lexiconObject != null)
                {
                    jsonList.Add(lexiconObject);
                    result++;
                }
            }
            return result;
        }

        private int OutputJsonObjects_Old(int targetLanguage, StreamWriter jsonList, SortedDictionary<int, List<char>> strongsDict, string prefix, bool first)
        {
            int result = 0;
            LexiconEntry entry = new LexiconEntry();
            bool firstLine = first;
            foreach (int strongs in strongsDict.Keys)
            {
                List<char> chars = strongsDict[strongs];
                foreach (char c in chars)
                {
                    string strongsNumber = string.Format("{0}{1:D4}", prefix, strongs);
                    string dStrong = c.ToString().Trim();
                    entry.Clear();
                    bool res = mainForm.GetStrongsEntry(strongsNumber, dStrong, false, entry);
                    if (res)
                    {
                        res = mainForm.GetDictionaryTranslation(targetLanguage, strongsNumber, dStrong, false, entry);
                    }
                    if (res)
                    {
                        string references = string.Empty;
                        Dictionary<string, string> associatedWords = mainForm.GetAssociatedWords(targetLanguage, strongsNumber, dStrong);
                        if (associatedWords != null && associatedWords.Count > 0)
                        {
                            List<string> refsSum = new List<string>();
                            foreach (string refs in associatedWords.Values)
                            {
                                string[] refParts = refs.Split(new char[] { ',' });
                                foreach (string refPart in refParts)
                                {
                                    int idx = Utils.GetBookIndex(refPart.Trim());
                                    string litedRefs = string.Format("{0:d2}-{1}", idx, refPart.Trim());
                                    if (refsSum.Contains(litedRefs))
                                        continue;

                                    refsSum.Add(litedRefs);
                                }
                            }
                            refsSum.Sort();
                            for (int i = 0; i < refsSum.Count; i++)
                            {
                                refsSum[i] = refsSum[i].Substring(3);
                            }
                            references = string.Join(",", refsSum);
                        }

                        if (firstLine)
                        {
                            firstLine = false;
                        }
                        else
                        {
                            jsonList.WriteLine(",");
                        }

                        string def = entry.Definition.Trim();
                        while (def.StartsWith("\"")) def = def.Substring(1);
                        while (def.EndsWith("\"")) def = def.Substring(0, def.Length - 1);

                        string gloss = entry.Gloss;
                        while (gloss.StartsWith("\"")) gloss = gloss.Substring(1);
                        while (gloss.EndsWith("\"")) gloss = gloss.Substring(0, gloss.Length - 1);

                        var lexiconObject = new LexiconJsonObject
                        {
                            Strongs = strongsNumber,
                            Hebrew = entry.UnicodeAccented,
                            English = entry.English,
                            Transliteration = entry.Transliteration,
                            Arabic = gloss,
                            Definition = def.Replace("\"\"", "\""),
                            References = references
                        };

                        string jsonString = JsonConvert.SerializeObject(lexiconObject, Formatting.Indented);
                        jsonList.Write(jsonString);
                        result++;
                        break; // limit output to one strong number
                    }

                }
            }
            return result;
        }
    }
}
