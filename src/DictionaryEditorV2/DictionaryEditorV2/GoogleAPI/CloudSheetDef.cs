using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryEditorV2.GoogleAPI
{
    internal class CloudSheetDef
    {
        public CloudSheetDef(string sheetId, string name,
            string lastModifyingUserName,
            string lastModifyingUserEmail,
            string modifiedUtcTimestamp,
            string mimeType,
            bool hebrew, int startStrong, int lastStrong)
        {
            SheetId = sheetId;
            Name = name;
            LastModifyingUserName = lastModifyingUserName;
            LastModifyingUserEmail = lastModifyingUserEmail;
            ModifiedTimestamp = modifiedUtcTimestamp;
            MimeType = mimeType;
            Hebrew = hebrew;
            StartStrong = startStrong;
            LastStrong = lastStrong;
            MissingFromDB = false;
            IdChanged = false;
        }

        public string SheetId { get; set; }
        public string Name { get; set; }
        public string MimeType { get; set; }
        public bool Hebrew { get; set; }
        public int StartStrong { get; set; }
        public int LastStrong { get; set; }
        public string LastModifyingUserName { get; set; }
        public string LastModifyingUserEmail { get; set; }
        public string ModifiedTimestamp { get; set; }
        public bool MissingFromDB { get; set; }
        public bool IdChanged { get; set; } 

        public override string ToString()
        {
            var sb = new StringBuilder(Name);
            if (MissingFromDB) sb.Append(" * Missing fro DB");
            if (IdChanged) sb.Append(" * ID changed");
            return sb.ToString();
        }

    }

}
