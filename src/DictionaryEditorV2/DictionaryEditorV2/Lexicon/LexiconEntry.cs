using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryEditorV2.Lexicon
{
    public class LexiconEntry
    {

        public LexiconEntry()
        {
            Clear();
        }
        public void Clear()
        {
            UnicodeAccented = string.Empty;
            Transliteration = string.Empty;
            Gloss = string.Empty;
            Definition = string.Empty;
            English = string.Empty;
        }

        public string UnicodeAccented { get; set; }
        public string Transliteration { get; set; }
        public string English { get; set; }
        public string Gloss { get; set; }
        public string Definition { get; set; }

        public string StrongsNum { get; set; }
        public string dStrong { get; set; }
        public string References { get; set; }

    }
}
