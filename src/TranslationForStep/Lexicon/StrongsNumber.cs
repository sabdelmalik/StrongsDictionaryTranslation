using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryEditorV2.Lexicon
{
    internal class StrongsNumber
    {
        /// <summary>
        /// key dStrong
        /// Value strong_id
        /// </summary>

        private SortedDictionary<string, long> dStrongs = new SortedDictionary<string, long>();

        private long strong_id = -1;
        public StrongsNumber(string strongs) 
        {
            Strongs = strongs;
        }
        public string Strongs { get; set; }
        public void AddDStrong(string dS, long strongID)
        {
            if(string.IsNullOrEmpty(dS.Trim()))
            {
                if (strong_id < 0)
                {
                    this.strong_id = strongID;
                }
                else
                //throw new Exception("strong_id aslrady set!");
                {
                    int x = 0;
                }
            }
            else
            {
                if (strong_id < 0)
                    dStrongs.Add(dS[0].ToString(), strongID);
                else
                    throw new Exception("strong_id aslrady set!"); 
            }
        }

        public List<string> DStrongs
        {
            get
            {
                return dStrongs.Keys.ToList();
            }
        }

        public long GetStrongID(string dstrong) 
        {
            long result = strong_id;
            if(!string.IsNullOrEmpty(dstrong))
            {
                result = dStrongs[dstrong[0].ToString()];
            }

            return result;
        }

        public bool HasDStrongs
        {
            get
            {
                return strong_id <0;
            }
        }


    }

}
