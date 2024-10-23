using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CopyDbToV2
{
    internal class Sheets
    {
        /// <summary>
        /// key Strong's Number
        /// value: sheet_id
        /// </summary>
        Dictionary<int, int>  hebrewMap = new Dictionary<int, int>();
        Dictionary<int, int>  greekMap = new Dictionary<int, int>();

        string StrongsPpattern = @"([GH])(\d\d\d\d)([A-Z ]{0,1})";

        MainForm mainForm;

        public Sheets(MainForm form) 
        {
            mainForm = form;
        }

        #region Trace
        private void Trace(string text, System.Drawing.Color color,
        [CallerLineNumber] int lineNumber = 0,
        [CallerMemberName] string caller = null)
        {
            mainForm.Trace(text, color);
        }
        private void TraceError(
                string text,
                [CallerLineNumber] int lineNumber = 0,
                [CallerMemberName] string caller = null)
        {
            mainForm.TraceError(string.Format("{0}#{1}", caller == null ? "???" : caller, lineNumber), text);
        }
        #endregion Trace

        public bool Add(
            int sheet_id, 
            string id, 
            string name, 
            string first_strong,
            string last_strong, 
            int translation_language, 
            DateTime last_updated)
        {
            string Lang = string.Empty;
            string strongStr = string.Empty;
            int firstStrong = 0;
            int lastStrong = 0;

            try
            {
                Match match = Regex.Match(first_strong, StrongsPpattern);
                if (match != null)
                {
                    Lang = match.Groups[1].Value;
                    strongStr = match.Groups[2].Value;
                    if(!int.TryParse(strongStr, out firstStrong))
                    {
                        TraceError("Failed to parse Strong's " + firstStrong);
                        return false;
                    }
                }
                else
                {
                    TraceError("Match failed on " + firstStrong);
                    return false;
                }
                
                match = Regex.Match(last_strong, StrongsPpattern);
                if (match != null)
                {
                    Lang = match.Groups[1].Value;
                    strongStr = match.Groups[2].Value;
                    if (!int.TryParse(strongStr, out lastStrong))
                    {
                        TraceError("Failed to parse Strong's " + lastStrong);
                        return false;
                    }
                }
                else
                {
                    TraceError("Match failed on " + lastStrong);
                    return false;
                }
                Dictionary<int, int> dict = greekMap;
                if (Lang == "H")
                    dict = hebrewMap;

                if (firstStrong >= 9000)
                {
                    int x=  0;
                }
                for(int i = firstStrong; i <= lastStrong; i++)
                {
                    dict.Add(i, sheet_id);
                }
            }
            catch (Exception e)
            {
                TraceError(e.Message);
                return false;
            }

            return true;
        }

        public int GetSheet(string strongs)
        {
            string Lang = string.Empty;
            string strongStr = string.Empty;
            int strong = 0;
            int sheet = -1;
            try
            {
                Match match = Regex.Match(strongs, StrongsPpattern);
                if (match != null)
                {
                    Lang = match.Groups[1].Value;
                    strongStr = match.Groups[2].Value;
                    if (!int.TryParse(strongStr, out strong))
                    {
                        TraceError("Failed to parse Strong's " + strong);
                        return sheet;
                    }
                }
                else
                {
                    TraceError("Match failed on " + strong);
                    return sheet;
                }

                 sheet = (Lang == "H") ? hebrewMap[strong] : greekMap[strong];
            }
            catch (Exception e)
            {
                TraceError(e.Message);
                return sheet;
            }

            return sheet;
        }
    }

}
