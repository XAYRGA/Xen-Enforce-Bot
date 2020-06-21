using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO; 

namespace XenfbotDN
{

    

    public class Language {
        public string code;
        public string authors;
        public string version;
        public Dictionary<string, string> data;
    }
    


    public static class Localization
    {
        public static Dictionary<string, Language> languages = new Dictionary<string, Language>();

        public static void init()
        {
            var w  = Directory.GetFiles("languages/ql", "*.ql");
            for (int i = 0; i < w.Length; i++)
            {
                var lng = loadQL(w[i]);
                languages[lng.code] = lng;
            }
        }

        public static Language getLanguageInfo(string langcode)
        {
            Language dl = languages["en"];
            languages.TryGetValue(langcode, out dl);
            return dl;
        }
        public static string getStringLocalized(string langcode, string path, params object[] fmtdata)
        {
            Language dl = languages["en"];
            Language dlEnglish = dl; 
            languages.TryGetValue(langcode, out dl);

            string LocString = null;
            dl.data.TryGetValue(path, out LocString);
            if (LocString==null)
            {
                dlEnglish.data.TryGetValue(path, out LocString);
            }
            if (LocString==null)
            {
                return $"[!] {path} not found in language [{langcode}] or in [en]. Please notify the developer!";
            }

            return string.Format(LocString, fmtdata);
        }

        public static Language loadQL(string data)
        {
            var rom_contents = File.ReadAllLines(data);
            var Author = "unknown";
            var Code = "unknown";
            var Version = "unknown";
            var meta = false;
            var cline = 0;

            while (meta != true)
            {
                if (cline >= rom_contents.Length)
                {
                    Console.WriteLine("Hit EOF no meta indicator");
                    return null;
                }
                var line = rom_contents[cline];
                if (line.Length > 0)
                {
                    if (line[0] == '*')
                    {
                        meta = true;
                        continue;
                    }
                    var gx = rom_contents[cline].Split("\t", 2, StringSplitOptions.None);
                    if (gx.Length == 2) // check to see if the length is == 2, a bit redundant but whatever
                    {
                        switch (gx[0])
                        {
                            case "CODE":
                                Code = gx[1];
                                break;
                            case "AUTHOR":
                                Author = gx[1];
                                break;
                            case "VERSION":
                                Version = gx[1];
                                break;
                        }
                    }
                }
                cline++;
            }

            Language newLang = new Language()
            {
                code = Code,
                authors = Author,
                version = Version,
               
            };
            var langDict = new Dictionary<string, string>();

            while (cline < rom_contents.Length)
            {
                var line = rom_contents[cline];
                var gx = rom_contents[cline].Split("|", 2, StringSplitOptions.None);
                if (gx.Length == 2)
                {
                    gx[1] = gx[1].Replace("\\n", "\n");
                    langDict[gx[0]] = gx[1];
                }
                cline++;
            }
            newLang.data = langDict;
            Console.WriteLine("Loaded language QL for {0} version {1} by {2}", Code, Version, Author);
            return newLang;

        }


    }




}
