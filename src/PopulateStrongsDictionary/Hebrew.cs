using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopulateStrongsDictionary
{
    internal class Hebrew
    {
        // Hebrew Points and punctuation
        const char POINT_SHEVA = '\u05B0';
        const char POINT_HATAF_SEGOL = '\u05B1';     // segol sheva
        const char POINT_HATAF_PATAH = '\u05B2';     // oatah sheva
        const char POINT_HATAF_QAMATS = '\u05B3';    // qamats sheva
        const char POINT_HIRIQ = '\u05B4';
        const char POINT_TSERE = '\u05B5';
        const char POINT_SEGOL = '\u05B6';
        const char POINT_PATAH = '\u05B7';   // Short A vowel
        const char POINT_QAMATS = '\u05B8';  // Long A vowel or an O if closed un-acented syllable
        const char POINT_HOLAM = '\u05B9';
        const char POINT_HOLAM_HASER_FOR_VAV = '\u05BA';
        const char POINT_QUBUTS = '\u05BB';
        const char POINT_DAGESH_OR_MAPIQ = '\u05BC';
        const char POINT_METEG = '\u05BD';
        const char PUNCTUATION_MAQAF = '\u05BE';
        const char POINT_RAFE = '\u05BF';
        const char POINT_SHIN_DOT = '\u05C1';
        const char POINT_SIN_DOT = '\u05C2';
        const char PUNCTUATION_SOF_PASUQ = '\u05C3';
        const char POINT_QAMATS_QATAN = '\u05C7';

        public static string RemovePoints(string word)
        {
            string result = word.
                Replace("\u05B0", "").	//        POINT_SHEVA 	
                Replace("\u05B1", "").	//        POINT_HATAF_SEGOL 	     // segol sheva
                Replace("\u05B2", "").	//        POINT_HATAF_PATAH 	     // oatah sheva
                Replace("\u05B3", "").	//        POINT_HATAF_QAMATS 	    // qamats sheva
                Replace("\u05B4", "").	//        POINT_HIRIQ 	
                Replace("\u05B5", "").	//        POINT_TSERE 	
                Replace("\u05B6", "").	//        POINT_SEGOL 	
                Replace("\u05B7", "").	//        POINT_PATAH 	   // Short A vowel
                Replace("\u05B8", "").	//        POINT_QAMATS 	  // Long A vowel or an O if closed un-acented syllable
                Replace("\u05B9", "").	//        POINT_HOLAM 	
                Replace("\u05BA", "").	//        POINT_HOLAM_HASER_FOR_VAV 	
                Replace("\u05BB", "").	//        POINT_QUBUTS 	
               // Replace("\u05BC", "").	//        POINT_DAGESH_OR_MAPIQ 	
                Replace("\u05BD", "").	//        POINT_METEG 	
                Replace("\u05BE", "").	//        PUNCTUATION_MAQAF 	
                Replace("\u05BF", "").	//        POINT_RAFE 	
                Replace("\u05C1", "").	//        POINT_SHIN_DOT 	
                Replace("\u05C2", "").	//        POINT_SIN_DOT 	
                Replace("\u05C3", "").	//        PUNCTUATION_SOF_PASUQ 	
                Replace("\u05C7", "");	//        POINT_QAMATS_QATAN	

            return result;
        }
    }
}
