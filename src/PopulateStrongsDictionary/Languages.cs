using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopulateStrongsDictionary
{
    public class Languages
    {
        private Dictionary<int, Language> languagesDict = new Dictionary<int, Language>();

        private int[] supportedLanguags = new int[]
        {
            7,  // Arabic
            30, // Chinese
            63  // Hindi
        };   

        private MainForm mainForm;
        public Languages(MainForm mainForm)
        {
            this.mainForm = mainForm;

            languagesDict.Add(1, new Language(1, "Abkhazian", "ab", "abk"));
            languagesDict.Add(2, new Language(2, "Afar", "aa", "aar"));
            languagesDict.Add(3, new Language(3, "Afrikaans", "af", "afr"));
            languagesDict.Add(4, new Language(4, "Akan", "ak", "aka"));
            languagesDict.Add(5, new Language(5, "Albanian", "sq", "sqi"));
            languagesDict.Add(6, new Language(6, "Amharic", "am", "amh"));
            languagesDict.Add(7, new Language(7, "Arabic", "ar", "ara"));
            languagesDict.Add(8, new Language(8, "Aragonese", "an", "arg"));
            languagesDict.Add(9, new Language(9, "Armenian", "hy", "hye"));
            languagesDict.Add(10, new Language(10, "Assamese", "as", "asm"));
            languagesDict.Add(11, new Language(11, "Avaric", "av", "ava"));
            languagesDict.Add(12, new Language(12, "Avestan", "ae", "ave"));
            languagesDict.Add(13, new Language(13, "Aymara", "ay", "aym"));
            languagesDict.Add(14, new Language(14, "Azerbaijani", "az", "aze"));
            languagesDict.Add(15, new Language(15, "Bambara", "bm", "bam"));
            languagesDict.Add(16, new Language(16, "Bashkir", "ba", "bak"));
            languagesDict.Add(17, new Language(17, "Basque", "eu", "eus"));
            languagesDict.Add(18, new Language(18, "Belarusian", "be", "bel"));
            languagesDict.Add(19, new Language(19, "Bengali", "bn", "ben"));
            languagesDict.Add(20, new Language(20, "Bislama", "bi", "bis"));
            languagesDict.Add(21, new Language(21, "Bosnian", "bs", "bos"));
            languagesDict.Add(22, new Language(22, "Breton", "br", "bre"));
            languagesDict.Add(23, new Language(23, "Bulgarian", "bg", "bul"));
            languagesDict.Add(24, new Language(24, "Burmese", "my", "mya"));
            languagesDict.Add(25, new Language(25, "Catalan, Valencian", "ca", "cat"));
            languagesDict.Add(26, new Language(26, "Central Khmer", "km", "khm"));
            languagesDict.Add(27, new Language(27, "Chamorro", "ch", "cha"));
            languagesDict.Add(28, new Language(28, "Chechen", "ce", "che"));
            languagesDict.Add(29, new Language(29, "Chichewa, Chewa, Nyanja", "ny", "nya"));
            languagesDict.Add(30, new Language(30, "Chinese", "zh", "zho"));
            languagesDict.Add(31, new Language(31, "Church Slavonic, Old Slavonic, Old Church Slavonic", "cu", "chu"));
            languagesDict.Add(32, new Language(32, "Chuvash", "cv", "chv"));
            languagesDict.Add(33, new Language(33, "Cornish", "kw", "cor"));
            languagesDict.Add(34, new Language(34, "Corsican", "co", "cos"));
            languagesDict.Add(35, new Language(35, "Cree", "cr", "cre"));
            languagesDict.Add(36, new Language(36, "Croatian", "hr", "hrv"));
            languagesDict.Add(37, new Language(37, "Czech", "cs", "ces"));
            languagesDict.Add(38, new Language(38, "Danish", "da", "dan"));
            languagesDict.Add(39, new Language(39, "Divehi, Dhivehi, Maldivian", "dv", "div"));
            languagesDict.Add(40, new Language(40, "Dutch, Flemish", "nl", "nld"));
            languagesDict.Add(41, new Language(41, "Dzongkha", "dz", "dzo"));
            languagesDict.Add(42, new Language(42, "English", "en", "eng"));
            languagesDict.Add(43, new Language(43, "Esperanto", "eo", "epo"));
            languagesDict.Add(44, new Language(44, "Estonian", "et", "est"));
            languagesDict.Add(45, new Language(45, "Ewe", "ee", "ewe"));
            languagesDict.Add(46, new Language(46, "Faroese", "fo", "fao"));
            languagesDict.Add(47, new Language(47, "Fijian", "fj", "fij"));
            languagesDict.Add(48, new Language(48, "Finnish", "fi", "fin"));
            languagesDict.Add(49, new Language(49, "French", "fr", "fra"));
            languagesDict.Add(50, new Language(50, "Fulah", "ff", "ful"));
            languagesDict.Add(51, new Language(51, "Gaelic, Scottish Gaelic", "gd", "gla"));
            languagesDict.Add(52, new Language(52, "Galician", "gl", "glg"));
            languagesDict.Add(53, new Language(53, "Ganda", "lg", "lug"));
            languagesDict.Add(54, new Language(54, "Georgian", "ka", "kat"));
            languagesDict.Add(55, new Language(55, "German", "de", "deu"));
            languagesDict.Add(56, new Language(56, "Greek, Modern (1453–)", "el", "ell"));
            languagesDict.Add(57, new Language(57, "Guarani", "gn", "grn"));
            languagesDict.Add(58, new Language(58, "Gujarati", "gu", "guj"));
            languagesDict.Add(59, new Language(59, "Haitian, Haitian Creole", "ht", "hat"));
            languagesDict.Add(60, new Language(60, "Hausa", "ha", "hau"));
            languagesDict.Add(61, new Language(61, "Hebrew", "he", "heb"));
            languagesDict.Add(62, new Language(62, "Herero", "hz", "her"));
            languagesDict.Add(63, new Language(63, "Hindi", "hi", "hin"));
            languagesDict.Add(64, new Language(64, "Hiri Motu", "ho", "hmo"));
            languagesDict.Add(65, new Language(65, "Hungarian", "hu", "hun"));
            languagesDict.Add(66, new Language(66, "Icelandic", "is", "isl"));
            languagesDict.Add(67, new Language(67, "Ido", "io", "ido"));
            languagesDict.Add(68, new Language(68, "Igbo", "ig", "ibo"));
            languagesDict.Add(69, new Language(69, "Indonesian", "id", "ind"));
            languagesDict.Add(70, new Language(70, "Interlingua (International Auxiliary Language Association)", "ia", "ina"));
            languagesDict.Add(71, new Language(71, "Interlingue, Occidental", "ie", "ile"));
            languagesDict.Add(72, new Language(72, "Inuktitut", "iu", "iku"));
            languagesDict.Add(73, new Language(73, "Inupiaq", "ik", "ipk"));
            languagesDict.Add(74, new Language(74, "Irish", "ga", "gle"));
            languagesDict.Add(75, new Language(75, "Italian", "it", "ita"));
            languagesDict.Add(76, new Language(76, "Japanese", "ja", "jpn"));
            languagesDict.Add(77, new Language(77, "Javanese", "jv", "jav"));
            languagesDict.Add(78, new Language(78, "Kalaallisut, Greenlandic", "kl", "kal"));
            languagesDict.Add(79, new Language(79, "Kannada", "kn", "kan"));
            languagesDict.Add(80, new Language(80, "Kanuri", "kr", "kau"));
            languagesDict.Add(81, new Language(81, "Kashmiri", "ks", "kas"));
            languagesDict.Add(82, new Language(82, "Kazakh", "kk", "kaz"));
            languagesDict.Add(83, new Language(83, "Kikuyu, Gikuyu", "ki", "kik"));
            languagesDict.Add(84, new Language(84, "Kinyarwanda", "rw", "kin"));
            languagesDict.Add(85, new Language(85, "Kirghiz, Kyrgyz", "ky", "kir"));
            languagesDict.Add(86, new Language(86, "Komi", "kv", "kom"));
            languagesDict.Add(87, new Language(87, "Kongo", "kg", "kon"));
            languagesDict.Add(88, new Language(88, "Korean", "ko", "kor"));
            languagesDict.Add(89, new Language(89, "Kuanyama, Kwanyama", "kj", "kua"));
            languagesDict.Add(90, new Language(90, "Kurdish", "ku", "kur"));
            languagesDict.Add(91, new Language(91, "Lao", "lo", "lao"));
            languagesDict.Add(92, new Language(92, "Latin", "la", "lat"));
            languagesDict.Add(93, new Language(93, "Latvian", "lv", "lav"));
            languagesDict.Add(94, new Language(94, "Limburgan, Limburger, Limburgish", "li", "lim"));
            languagesDict.Add(95, new Language(95, "Lingala", "ln", "lin"));
            languagesDict.Add(96, new Language(96, "Lithuanian", "lt", "lit"));
            languagesDict.Add(97, new Language(97, "Luba-Katanga", "lu", "lub"));
            languagesDict.Add(98, new Language(98, "Luxembourgish, Letzeburgesch", "lb", "ltz"));
            languagesDict.Add(99, new Language(99, "Macedonian", "mk", "mkd"));
            languagesDict.Add(100, new Language(100, "Malagasy", "mg", "mlg"));
            languagesDict.Add(101, new Language(101, "Malay", "ms", "msa"));
            languagesDict.Add(102, new Language(102, "Malayalam", "ml", "mal"));
            languagesDict.Add(103, new Language(103, "Maltese", "mt", "mlt"));
            languagesDict.Add(104, new Language(104, "Manx", "gv", "glv"));
            languagesDict.Add(105, new Language(105, "Maori", "mi", "mri"));
            languagesDict.Add(106, new Language(106, "Marathi", "mr", "mar"));
            languagesDict.Add(107, new Language(107, "Marshallese", "mh", "mah"));
            languagesDict.Add(108, new Language(108, "Mongolian", "mn", "mon"));
            languagesDict.Add(109, new Language(109, "Nauru", "na", "nau"));
            languagesDict.Add(110, new Language(110, "Navajo, Navaho", "nv", "nav"));
            languagesDict.Add(111, new Language(111, "Ndonga", "ng", "ndo"));
            languagesDict.Add(112, new Language(112, "Nepali", "ne", "nep"));
            languagesDict.Add(113, new Language(113, "North Ndebele", "nd", "nde"));
            languagesDict.Add(114, new Language(114, "Northern Sami", "se", "sme"));
            languagesDict.Add(115, new Language(115, "Norwegian", "no", "nor"));
            languagesDict.Add(116, new Language(116, "Norwegian Bokmål", "nb", "nob"));
            languagesDict.Add(117, new Language(117, "Norwegian Nynorsk", "nn", "nno"));
            languagesDict.Add(118, new Language(118, "Occitan", "oc", "oci"));
            languagesDict.Add(119, new Language(119, "Ojibwa", "oj", "oji"));
            languagesDict.Add(120, new Language(120, "Oriya", "or", "ori"));
            languagesDict.Add(121, new Language(121, "Oromo", "om", "orm"));
            languagesDict.Add(122, new Language(122, "Ossetian, Ossetic", "os", "oss"));
            languagesDict.Add(123, new Language(123, "Pali", "pi", "pli"));
            languagesDict.Add(124, new Language(124, "Pashto, Pushto", "ps", "pus"));
            languagesDict.Add(125, new Language(125, "Persian", "fa", "fas"));
            languagesDict.Add(126, new Language(126, "Polish", "pl", "pol"));
            languagesDict.Add(127, new Language(127, "Portuguese", "pt", "por"));
            languagesDict.Add(128, new Language(128, "Punjabi, Panjabi", "pa", "pan"));
            languagesDict.Add(129, new Language(129, "Quechua", "qu", "que"));
            languagesDict.Add(130, new Language(130, "Romanian, Moldavian, Moldovan", "ro", "ron"));
            languagesDict.Add(131, new Language(131, "Romansh", "rm", "roh"));
            languagesDict.Add(132, new Language(132, "Rundi", "rn", "run"));
            languagesDict.Add(133, new Language(133, "Russian", "ru", "rus"));
            languagesDict.Add(134, new Language(134, "Samoan", "sm", "smo"));
            languagesDict.Add(135, new Language(135, "Sango", "sg", "sag"));
            languagesDict.Add(136, new Language(136, "Sanskrit", "sa", "san"));
            languagesDict.Add(137, new Language(137, "Sardinian", "sc", "srd"));
            languagesDict.Add(138, new Language(138, "Serbian", "sr", "srp"));
            languagesDict.Add(139, new Language(139, "Shona", "sn", "sna"));
            languagesDict.Add(140, new Language(140, "Sichuan Yi, Nuosu", "ii", "iii"));
            languagesDict.Add(141, new Language(141, "Sindhi", "sd", "snd"));
            languagesDict.Add(142, new Language(142, "Sinhala, Sinhalese", "si", "sin"));
            languagesDict.Add(143, new Language(143, "Slovak", "sk", "slk"));
            languagesDict.Add(144, new Language(144, "Slovenian", "sl", "slv"));
            languagesDict.Add(145, new Language(145, "Somali", "so", "som"));
            languagesDict.Add(146, new Language(146, "South Ndebele", "nr", "nbl"));
            languagesDict.Add(147, new Language(147, "Southern Sotho", "st", "sot"));
            languagesDict.Add(148, new Language(148, "Spanish, Castilian", "es", "spa"));
            languagesDict.Add(149, new Language(149, "Sundanese", "su", "sun"));
            languagesDict.Add(150, new Language(150, "Swahili", "sw", "swa"));
            languagesDict.Add(151, new Language(151, "Swati", "ss", "ssw"));
            languagesDict.Add(152, new Language(152, "Swedish", "sv", "swe"));
            languagesDict.Add(153, new Language(153, "Tagalog", "tl", "tgl"));
            languagesDict.Add(154, new Language(154, "Tahitian", "ty", "tah"));
            languagesDict.Add(155, new Language(155, "Tajik", "tg", "tgk"));
            languagesDict.Add(156, new Language(156, "Tamil", "ta", "tam"));
            languagesDict.Add(157, new Language(157, "Tatar", "tt", "tat"));
            languagesDict.Add(158, new Language(158, "Telugu", "te", "tel"));
            languagesDict.Add(159, new Language(159, "Thai", "th", "tha"));
            languagesDict.Add(160, new Language(160, "Tibetan", "bo", "bod"));
            languagesDict.Add(161, new Language(161, "Tigrinya", "ti", "tir"));
            languagesDict.Add(162, new Language(162, "Tonga (Tonga Islands)", "to", "ton"));
            languagesDict.Add(163, new Language(163, "Tsonga", "ts", "tso"));
            languagesDict.Add(164, new Language(164, "Tswana", "tn", "tsn"));
            languagesDict.Add(165, new Language(165, "Turkish", "tr", "tur"));
            languagesDict.Add(166, new Language(166, "Turkmen", "tk", "tuk"));
            languagesDict.Add(167, new Language(167, "Twi", "tw", "twi"));
            languagesDict.Add(168, new Language(168, "Uighur, Uyghur", "ug", "uig"));
            languagesDict.Add(169, new Language(169, "Ukrainian", "uk", "ukr"));
            languagesDict.Add(170, new Language(170, "Urdu", "ur", "urd"));
            languagesDict.Add(171, new Language(171, "Uzbek", "uz", "uzb"));
            languagesDict.Add(172, new Language(172, "Venda", "ve", "ven"));
            languagesDict.Add(173, new Language(173, "Vietnamese", "vi", "vie"));
            languagesDict.Add(174, new Language(174, "Volapük", "vo", "vol"));
            languagesDict.Add(175, new Language(175, "Walloon", "wa", "wln"));
            languagesDict.Add(176, new Language(176, "Welsh", "cy", "cym"));
            languagesDict.Add(177, new Language(177, "Western Frisian", "fy", "fry"));
            languagesDict.Add(178, new Language(178, "Wolof", "wo", "wol"));
            languagesDict.Add(179, new Language(179, "Xhosa", "xh", "xho"));
            languagesDict.Add(180, new Language(180, "Yiddish", "yi", "yid"));
            languagesDict.Add(181, new Language(181, "Yoruba", "yo", "yor"));
            languagesDict.Add(182, new Language(182, "Zhuang, Chuang", "za", "zha"));
            languagesDict.Add(183, new Language(183, "Zulu", "zu", "zul"));
        }

        public int[] SupportedLanguags
        {
            get
            {
                return supportedLanguags;
            }
        }

        public Language GetLanguage(int langid)
        {
            return languagesDict[langid];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSource"></param>
        public void PopulateLanguageTable(NpgsqlDataSource dataSource)
        {
            string cmdText = string.Empty;
            mainForm.Trace("\r\nPolpulating Language Table!", Color.Green);

            try
            {
                var command = dataSource.CreateCommand();

                bool exists = false;
                command.CommandText = "SELECT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'language') AS table_existence;";
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    exists = (bool)reader[0];
                }
                reader.Close();

                if (!exists)
                {
                    mainForm.Trace("Table 'language' does not exist!", Color.Red);
                    return;
                }

                command.CommandText = "DELETE FROM public.\"language\";";
                command.ExecuteNonQuery();

                foreach (int langId in languagesDict.Keys)
                {
                    Language lang = languagesDict[langId];

                    cmdText = "INSERT INTO public.\"language\" " +
                       "(id, name, iso_639_1, iso_639_2) " +
                       "VALUES " +
                       string.Format("({0}, '{1}', '{2}', '{3}');",
                       lang.ID,
                       lang.Name.Replace("'", "''"),
                       lang.ISO639_1.Replace("'", "''"),
                       lang.ISO639_2.Replace("'", "''"));
                    command.CommandText = cmdText;
                    command.ExecuteNonQuery();
                }

                mainForm.Trace("Language population Done!", Color.Green);


            }
            catch (Exception ex)
            {
                mainForm.Trace("PopulateBookNames Exception\r\n" + ex.ToString(), Color.Red);
                return;
            }

        }
    }

    public class Language
{

    public Language(string name, string iso639_1, string iso639_2)
    {
        this.Name = name;
        this.ISO639_1 = iso639_1;
        this.ISO639_2 = iso639_2;
    }

    public Language(int id, string name, string iso639_1, string iso639_2)
    {
        this.ID = id;
        this.Name = name;
        this.ISO639_1 = iso639_1;
        this.ISO639_2 = iso639_2;
    }

    public int ID { get; private set; }
    public string Name { get; private set; }
    public string ISO639_1 { get; private set; }
    public string ISO639_2 { get; private set; }

    public override string ToString()
    {
        return this.Name;
    }
}
}
