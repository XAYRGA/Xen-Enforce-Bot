using System;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace XenfbotDN
{
    public static class Root
    {

        private const string tag = "xenfbot@boot";
        public static string botUsername;
        public static string botName;
        static void Main(string[] args)
        {
            Console.WriteLine("XenfbotDN (C) XAYRGA 2020");
            if (args.Length > 0)
            {
                var ptg = "xenfbot@preboot";
                Console.WriteLine("parameter check.....");
                if (args[0] == "-mklang")
                {
                    Helpers.writeOut(ptg, "localizaton mode");
                    Helpers.writeOut(ptg, "Initialize localization engine.");
                    Localization.init();
                    Helpers.writeOut(ptg, "Developer/mklanguage.cs");
                    Developer.mklanguage.start();
                    return; // close
                }
                else if (args[0] == "-importql")
                {
                    Helpers.writeOut(ptg, "localizaton mode");
                    Helpers.writeOut(ptg, "Initialize localization engine.");
                    Localization.init();
                    Helpers.writeOut(ptg, "Developer/importql.cs");
                    Developer.importql.start();
                    return; // close
                }
            }

            Helpers.writeOut(tag, "Initializing configuration.");
            // Test Load configuration.
            Config.init("config.ini");
            // Load languages
            Helpers.writeOut(tag, "Starting localization engine");
            Localization.init();
            // Test Telegram API
            Helpers.writeOut(tag, "Initializing Telegram API");
            Telegram.SetAPIKey(Config.getValue("TGAPIKey"));
            {
                var tries = 0;
                var me = Telegram.getMe().Result; // Synchronous call for result.
                while (me == null)
                {
                    tries++;
                    Thread.Sleep(1200);
                    Helpers.warn("Failed. Trying again");
                    me = Telegram.getMe().Result;
                    if (tries > 3)
                    {
                        Helpers.warn("Invalid telegram API key or cannot connect to tgapi.");
                        Environment.Exit(-1);
                    }
                }
                botUsername = me.username;
                botName = me.first_name;
            }

            var initValue = SQL.Init(Config.getValue("MySQLHost"), Config.getValue("MySQLUser"), Config.getValue("MySQLPassword"), Config.getValue("MySQLDatabase"));
            Console.WriteLine(Config.getValue("MySQLHost"), Config.getValue("MySQLUser"), Config.getValue("MySQLPassword"), Config.getValue("MySQLDatabase"));
            Helpers.writeOut(tag, "Testing MySQL Interface");
            {
                var ok = SQL.Query("SELECT * FROM xenf_activations LIMIT 1");
                var tries = 0; 
                while (ok==null)
                {
                 
                    tries++;
                    Thread.Sleep(1200);
                    ok = SQL.Query("SELECT * FROM xenf_activations LIMIT 1");
                    if (tries > 3)
                    {
                        Helpers.warn("Cannot connect to MySQL server.");
                        Console.WriteLine(SQL.getLastError());
                        Environment.Exit(-1);
                    }
                }
            }
            Helpers.writeOut(tag, "Loading commands...");
            CommandSystem.loadCommands();
            Helpers.writeOut(tag,"Successfully initialized interfaces.");


            while (true)
            {
                processUpdates();
                Thread.Sleep(1000);
            }
        }

        static long lastUpdate = 0;
        public static void processUpdates()
        {
            var up = Telegram.getUpdates(lastUpdate).Result;
            Console.WriteLine("Updates: {0}", up.Length);
        
            for (int i=0; i < up.Length; i++)
            {
                var currentUpdate = up[i];
                if (currentUpdate.update_id >= lastUpdate)
                {
                    lastUpdate = currentUpdate.update_id + 1;
                }
                if (currentUpdate.edited_message!=null)
                {
                    currentUpdate.message = currentUpdate.edited_message; // ahax.
                }
                Console.WriteLine(JsonConvert.SerializeObject(currentUpdate));
                try
                {
                    if (currentUpdate.message != null) {
                        processIndividualUpdate(currentUpdate);
                    }
                    
                } catch
                {
                    Console.WriteLine("UPDATE FAIL");
                }
            }
        }



        public static async void processIndividualUpdate(TGUpdate update)
        {
            var msg = update.message;
            var langcode = "en"; // default language is englsh
            var gc = await GroupConfiguration.loadObject(update.message.chat.id);
            if (gc==null)
            {
                gc = GroupConfiguration.initializeObject(update.message.chat.id); // Init group config if it doesnt exist
            }
     
            //if (msg.from.language_code!=null)
            //{
            //   langcode = msg.from.language_code; // Check if the user has language settings
            //    Console.WriteLine(langcode);
            //} else
            {
                langcode = gc.langCode; // use group configuration if they don't.
            }
            Console.WriteLine(langcode);
            
            // Do captcha
            if (msg.new_chat_members!=null)
            {
                var ncm = msg.new_chat_members;
                for (int i=0; i < ncm.Length; i++)
                {
                    if (ncm[i].username==botUsername)
                    {
                        var cl1 = Localization.getLanguageFile(langcode);
                        var cl = Localization.getStringLocalized(langcode,"locale/currentLangName");
                        var smsg = string.Format(Localization.getStringLocalized(langcode, "basic/xenfbot"), "3.0.0.1", cl, cl1.author,cl1.version,"@xayrga");
                        smsg += "\n\n";
                        smsg += Localization.getStringLocalized(langcode, "basic/welcome");

                        msg.replySendMessage(smsg);
                    }


                    ///// ADD FILTERS HERE ////
                    bool ok = true;
                    if (ok)
                        ok = await Filters.botscreen.Execute(msg, ncm[i], gc, langcode);
                    if (ok)
                        ok = await Filters.captcha.Execute(msg,ncm[i], gc, langcode);
                }
            }

            if (msg.text!=null)
            {
                await CommandSystem.runCommand(msg, gc, langcode);
            }

        }
    }
}
