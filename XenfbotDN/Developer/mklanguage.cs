using System;
using System.Collections.Generic;
using System.Text;

namespace XenfbotDN.Developer
{
    public class mklanguage
    {
        public static void start()
        {

            Console.WriteLine("Enter language code:");
            var LangCode = Console.ReadLine();
            Console.WriteLine("language set to {0}",LangCode);
            Console.Write("Enter author: ");
            var Author = Console.ReadLine();
            Console.Write("Enter version: ");
            var Version = Console.ReadLine();

            while (true)
            {
                Console.WriteLine("Enter LString path:");
                var pth = Console.ReadLine();
                if (pth=="write")
                {
                    break;
                }
                Console.WriteLine("Enter LString text:");
                var pfw = Console.ReadLine();

                Localization.addLanguageString(LangCode + "/" + pth, pfw);
            }
            Localization.saveLanguage(LangCode, LangCode + ".json", Author, Version);
            Environment.Exit(0);
        }
    }
}
