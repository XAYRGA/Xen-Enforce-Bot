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
        public Dictionary<string, string> langLookup;
    }
    
    public enum LanguageNodeType
    {
        NULL,
        BRANCH,
        DATA
    }

    public class LanguageNode
    {
        public LanguageNodeType type;
        public object data;
    }

    public class LanguageFile
    {
        public string code;
        public string author;
        public string version;
        public LanguageNode data;

    }

    public static class Localization
    {

        private static int languageIndex = 0; 
        private static LanguageFile[] languages = new LanguageFile[128];
        private static Dictionary<string, LanguageFile> languageData = new Dictionary<string, LanguageFile>();

        public const string defaultLanguage = "en";

        public static LanguageNode map = new LanguageNode()
        {
            type = LanguageNodeType.BRANCH,
            data = new Dictionary<string, LanguageNode>()
        };

        public static void addLanguageString(string path, string obj)
        {
            var absPath = path.Split('/');
            addLanguageString_I(absPath, 0, obj, map);
        }

        public static string getString(string path)
        {
            var absPath = path.Split('/');
            return getString_I(absPath, 0, map);
        }

        public static string getStringLocalized(string langCode, string path)
        {
            var rtl = getString(langCode + "/" + path);
            if (rtl != null)
                return rtl;
            rtl = getString(defaultLanguage + "/" + path);
            if (rtl != null)
                return rtl;
            return "[!] Localization for " + path + " not found under language code " + langCode + " and has no fallback.";
        }

        private static void addLanguageString_I(string[] path, int depth, string act, LanguageNode current)
        {
            var myPath = path[depth];
            
            //Console.WriteLine(myPath);
            if (current.type == LanguageNodeType.DATA && depth!= path.Length-1)
            {
                throw new InvalidOperationException("The path for " + myPath + " is not a bramch");
            } else if (current.type == LanguageNodeType.DATA && depth == path.Length - 1)
            {
                Console.WriteLine("OVERWRITE EXISTING VALUE.");
            }
            if (depth == path.Length - 1)
            {
                ((Dictionary<string, LanguageNode>)current.data)[myPath] = new LanguageNode()
                {
                    type = LanguageNodeType.DATA,
                    data = act
                };
                return;
            }
          
            var cdt = (Dictionary<string, LanguageNode>)current.data;
            LanguageNode outH;
            var w = cdt.TryGetValue(myPath, out outH);

            if (w == false || outH == null)
            {
                outH = new LanguageNode()
                {
                    data = new Dictionary<string, LanguageNode>(),
                    type = LanguageNodeType.BRANCH
                };

                cdt[myPath] = outH;
            }
            depth++;
            addLanguageString_I(path, depth, act, outH);
        }


        private static string getString_I(string[] path, int depth, LanguageNode current)
        {
            var myPath = path[depth];
            LanguageNode outH;
            var next = ((Dictionary<string, LanguageNode>)current.data).TryGetValue(myPath, out outH);

            if (next == false)
            {
                return null;
            }
            if (outH.type == LanguageNodeType.DATA)
            {
                return (string)(outH.data);
            }
            depth++;
            if (depth == path.Length)
            {
                return null;
            }
            return getString_I(path, depth, outH);
        }




        public static void loadQL(string data)
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
                    return;
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
            while (cline < rom_contents.Length)
            {
                var line = rom_contents[cline];
                var gx = rom_contents[cline].Split("|", 2, StringSplitOptions.None);
                if (gx.Length == 2)
                {
                    gx[1] = gx[1].Replace("\\n", "\n");
                    Localization.addLanguageString(Code + "/" + gx[0], gx[1]);
                }
                cline++;
            }
            languageData[Code] = new LanguageFile
            {
                author = Author,
                version = Version,
             };
            Console.WriteLine("Loaded language QL for {0} version {1} by {2}", Code,Version,Author);

        }

        public static void init()
        {
            var w = Directory.GetFiles("languages", "*.json");
            for (int i=0; i < w.Length;i++)
            {
                loadLanguage(w[i]);
            }

            w = Directory.GetFiles("languages/ql", "*.ql");
            for (int i = 0; i < w.Length; i++)
            {
                loadQL(w[i]);
            }
        }



        private static LanguageNode unpackLanguageTree(LanguageNode my)
        {
            if (my.type == LanguageNodeType.BRANCH)
            {
                var newData = JsonConvert.DeserializeObject<Dictionary<string,LanguageNode>>(my.data.ToString());
                var keys = new List<string>(newData.Keys);
                for (int i = 0; i <  keys.Count; i++)
                {
                    newData[keys[i]] = unpackLanguageTree(newData[keys[i]]  );
                }
                my.data = newData;
            }
            else if (my.type == LanguageNodeType.DATA)
            {
                var newData = my.data.ToString();
                my.data = newData;
            }
            return my;
        }

        public static bool loadLanguage(string data)
        {
            var wdata = File.ReadAllText(data);
            //var w = JObject.Parse(wdata);
            // var rede = w.CreateReader();
            //var q = JsonSerializer.Create();
            var myMap = JsonConvert.DeserializeObject<LanguageFile>(wdata);
            Console.WriteLine("Loaded language {0} version {1} by {2}", myMap.code, myMap.version, myMap.author);
            languageData[myMap.code] = myMap;
            (((Dictionary<string, LanguageNode>)map.data))[myMap.code] = unpackLanguageTree(myMap.data);
            return true; 
        }

        public static LanguageFile getLanguageFile(string code)
        {
            LanguageFile ou;
            var wh = languageData.TryGetValue(code, out ou);
            if (!wh)
            {
                return languageData[defaultLanguage];
            }
            return ou;
        }

        public static void saveLanguage(string code, string filename, string Author, string Version)
        {
            var q = JsonSerializer.Create();
            var cMap = ((Dictionary<string, LanguageNode>)map.data)[code];
            var w = new LanguageFile()
            {
                code = code,
                data = cMap,
                author = Author,
                version = Version
            };

            var wJ = JsonConvert.SerializeObject(w,Formatting.Indented);
            File.WriteAllText(filename, wJ.ToString());
        }

    }




}
