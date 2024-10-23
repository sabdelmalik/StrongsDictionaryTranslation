using SheetData = Google.Apis.Sheets.v4.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryEditorV2.GoogleAPI
{
    internal class UserEnteredFormat
    {
        public UserEnteredFormat(SheetData.CellFormat? cellFormat, string fields)
        {
            CellFormat = cellFormat;
            Fields = fields;
        }

        public SheetData.CellFormat? CellFormat { get; private set; }
        public string Fields { get; private set; }
    }
}
