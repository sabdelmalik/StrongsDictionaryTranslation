using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryEditorV2.GoogleAPI
{
    internal class DbSheetDef
    {
        public DbSheetDef(string sheetId, string name,
            string startStrong, string lastStrong, DateTime lastUpdated)
        {
            SheetId = sheetId;
            Name = name;
            StartStrong = startStrong;
            LastStrong = lastStrong;
            LastUpdated = lastUpdated;
        }

        public string SheetId { get; set; }
        public string Name { get; set; }
        public string StartStrong { get; set; }
        public string LastStrong { get; set; }
        public DateTime LastUpdated { get; set; }

    }

}
