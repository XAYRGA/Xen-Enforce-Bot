using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace XenfbotDN.Developer
{
    public class importql
    {
        public static void start()
        {
            Console.WriteLine("Enter QL File:");
            var LangCode = Console.ReadLine();
            var rom_contents = File.ReadAllLines(LangCode);
            var Author = "unknown";
            var Code = "unknown";
            var Version = "unknown";
            var meta=false;
            var cline = 0;
            while (meta!=true)
            {
                if (cline >= rom_contents.Length)
                {
                    Console.WriteLine("Hit EOF no meta indicator");
                    return;
                }
                var line = rom_contents[cline];
                if (line.Length > 0)
                {
                    if (line[0]=='*')
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
                    Localization.addLanguageString(Code + "/" + gx[0], gx[1]);
                }
                cline++;
            }
       
            Localization.saveLanguage(Code, Code + ".json", Author, Version);
            Environment.Exit(0);
        }
    }
}
