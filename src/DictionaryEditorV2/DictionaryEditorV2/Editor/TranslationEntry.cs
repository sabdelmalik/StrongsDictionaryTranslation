using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryEditorV2.Editor
{
    internal class TranslationEntry
    {
        public TranslationEntry(long strong_id, string sheet_name, DateTime update_date,
            string updater_name, string updater_email, string status, string translated_gloss,
            string translated_text, string transliteration)
        {
            Strong_id = strong_id;
            Sheet_name = sheet_name;
            Update_date = update_date;
            Updater_name = updater_name;
            Updater_email = updater_email;
            Status = status;
            Translated_gloss = translated_gloss;
            Translated_text = translated_text;
            Transliteration = transliteration;
        }
            
        public long Strong_id { get; private set; }
        public string Sheet_name { get; private set; }
        public DateTime Update_date { get; private set; }
        public string Updater_name { get; private set; }
        public string Updater_email { get; private set; }
        public string Status { get; private set; }
        public string Translated_gloss { get; private set; }
        public string Translated_text { get; private set; }
        public string Transliteration { get; private set; }

    }
}
