using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryEditorV2.GoogleAPI
{
    internal enum SheetVerticalAlignment
    {
        VERTICAL_ALIGN_UNSPECIFIED,
        TOP,
        MIDDLE,
        BOTTOM
    }
    internal enum SheetHorizontalAlignment
    {
        HORIZONTAL_ALIGN_UNSPECIFIED,
        LEFT,
        CENTER,
        RIGHT
    }

    internal enum SheetWrapStrategy
    {
        WRAP_STRATEGY_UNSPECIFIED,
        OVERFLOW_CELL,
        LEGACY_WRAP,
        CLIP,
        WRAP
    }

    internal enum SheetTextDirection
    {
        TEXT_DIRECTION_UNSPECIFIED,
        LEFT_TO_RIGHT,
        RIGHT_TO_LEFT
    }

    internal enum NumberFormatType
    {
        NUMBER_FORMAT_UNSPECIFIED,
        TEXT,
        NUMBER,
        PERCENT,
        CURRENCY,
        DATE,
        TIME,
        DATE_TIME,
        SCIENTIFIC
    }

    internal enum BorderStyle
    {
        STYLE_UNSPECIFIED,
        DOTTED,
        DASHED,
        SOLID,
        SOLID_MEDIUM,
        SOLID_THICK,
        NONE,
        DOUBLE
    }
}
