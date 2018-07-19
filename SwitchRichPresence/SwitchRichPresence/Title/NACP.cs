using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Random0666_s_ToolBox.Modules.NintendoArchive
{
    public class NACP
    {
        public const int LANG_COUNT = 15;

        public enum NACPLangID
        {
            AmericanEnglish,
            BritishEnglish,
            Japanese,
            French,
            German,
            LatinAmericanSpanish,
            Spanish,
            Italian,
            Dutch,
            CanadianFrench,
            Portuguese,
            Russian,
            Korean,
            Taiwanese,
            Chinese
        }

        public class Language
        {
            public NACPLangID ID;
            public string ApplicationName;
            public string ApplicationDev;

            public Language(BinaryReader br, NACPLangID lang)
            {
                ID = lang;
                ApplicationName = Encoding.UTF8.GetString(br.ReadBytes(0x200)).Replace("\0", "");
                ApplicationDev = Encoding.UTF8.GetString(br.ReadBytes(0x100)).Replace("\0", "");
            }
        }

        public List<Language> Languages;
        public string BaseTitleId;
        public string TitleId;
        public string ProductCode;
        public string AppVersion;
        public string Passphrase;
        
        private bool tryLang(NACPLangID langID, out Language retLang)
        {
            foreach (var lang in Languages)
            {
                if (lang.ID == langID && lang.ApplicationName != "")
                {
                    retLang = lang;
                    return true;
                }
            }
            retLang = null;
            return false;
        }

        public Language GetLanguage()
        {
            Language lang;

            //most to less common alphabet
            if (tryLang(NACPLangID.AmericanEnglish, out lang))
                return lang;
            if (tryLang(NACPLangID.BritishEnglish, out lang))
                return lang;
            if (tryLang(NACPLangID.CanadianFrench, out lang))
                return lang;
            if (tryLang(NACPLangID.Dutch, out lang))
                return lang;
            if (tryLang(NACPLangID.French, out lang))
                return lang;
            if (tryLang(NACPLangID.German, out lang))
                return lang;
            if (tryLang(NACPLangID.Spanish, out lang))
                return lang;
            if (tryLang(NACPLangID.LatinAmericanSpanish, out lang))
                return lang;
            if (tryLang(NACPLangID.Italian, out lang))
                return lang;
            if (tryLang(NACPLangID.Portuguese, out lang))
                return lang;
            if (tryLang(NACPLangID.Taiwanese, out lang))
                return lang;
            if (tryLang(NACPLangID.Japanese, out lang))
                return lang;
            if (tryLang(NACPLangID.Korean, out lang))
                return lang;
            if (tryLang(NACPLangID.Chinese, out lang))
                return lang;
            if (tryLang(NACPLangID.Russian, out lang))
                return lang;

            throw new Exception("Can't find common title lang !");
        }

        private void ParseFile(BinaryReader br)
        {
            br.BaseStream.Position = 0;

            Languages = new List<Language>();
            for (int i = 0; i < LANG_COUNT; i++)
            {
                Languages.Add(new Language(br, (NACPLangID)i));
            }

            br.BaseStream.Seek(0x3038, SeekOrigin.Begin);
            BaseTitleId = br.ReadUInt64().ToString("X16");

            br.BaseStream.Seek(0x3078, SeekOrigin.Begin);
            TitleId = br.ReadUInt64().ToString("X16");

            br.BaseStream.Seek(0x3060, SeekOrigin.Begin);
            AppVersion = Encoding.ASCII.GetString(br.ReadBytes(0x10)).Replace("\0", "");

            br.BaseStream.Seek(0x30A8, SeekOrigin.Begin);
            ProductCode = Encoding.ASCII.GetString(br.ReadBytes(8)).Replace("\0", "");
            if (ProductCode == "") ProductCode = "null";

            br.BaseStream.Seek(0x3100, SeekOrigin.Begin);
            Passphrase = Encoding.ASCII.GetString(br.ReadBytes(0x40));
        }
        public NACP(string path)
        {
            using (FileStream fs = File.OpenRead(path))
            {
                ParseFile(new BinaryReader(fs));
            }
        }
        public NACP(Stream stream)
        {
            using (stream)
            {
                ParseFile(new BinaryReader(stream));
            }
        }
    }
}
