using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryEditorV2.Lexicon
{
    public delegate void StrongEventHandler(object sender, StrongEventArgs e);

    public class StrongEventArgs : EventArgs
    {
        public StrongEventArgs(long id, string dStrong, string strong)
        {
            StrongsID = id;
            DStrongs = dStrong;
            Strongs = strong;
        }
        public long StrongsID { get; private set; }
        public string DStrongs { get; private set; }
        public string Strongs { get; private set; }

    }
}
