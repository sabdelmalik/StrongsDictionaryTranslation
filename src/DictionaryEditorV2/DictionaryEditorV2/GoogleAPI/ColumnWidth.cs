using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryEditorV2.GoogleAPI
{
    internal class ColumnWidth
    {
        public ColumnWidth(int sheetId, int startIndex, int endIndex, int pixelSize)
        {
            SheetId = sheetId;
            StartIndex = startIndex;
            EndIndex = endIndex;
            PixelSize = pixelSize;
        }

        public int SheetId { get; private set; }
        public int StartIndex { get; private set; }
        public int EndIndex { get; private set; }
        public int PixelSize { get; private set; }
    }
}
