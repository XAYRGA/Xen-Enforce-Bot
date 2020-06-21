using System;
using System.Threading.Tasks;
using System.Threading;
using NLua;
using System.Data;

namespace XenfbotDN
{


    class root
    {
        public static Lua LuaState;
        public static LuaFunction callHook; 
        private const string tag = "xenfbot@boot";
        public static string botUsername;
        public static string botName;

       
        static void Main(string[] args)
        {

            Console.WriteLine("XenfbotDN (C) XAYRGA 2020");
    
            // Param check 
            if (args.Length > 0)
            {
                var ptg = "xenfbot@preboot";
                Console.WriteLine("parameter check.....");
            }

            /// Load Config File
            Helpers.writeOut(tag, "Initializing configuration.");
            Config.init("config.ini");

            /// Load telegram API
            Telegram.SetAPIKey(Config.getValue("TGAPIKey"));
            {
                var tries = 0;
                var me = Telegram.getMe(); // Synchronous call for result.
                while (me == null)
                {
                    tries++;
                    Thread.Sleep(1200);
                    Helpers.warn("Failed. Trying again");
                    me = Telegram.getMe();
                    if (tries > 3)
                    {
                        Helpers.warn("Invalid telegram API key or cannot connect to tgapi.");
                        Environment.Exit(-1);
                    }
                }
                botUsername = me.username; // set bot username
                botName = me.first_name; // set bot name
                Console.WriteLine($"Hello, I'm {botName} under the handle {botUsername}");
            }

            /// Load SQL 
            var initValue = SQL.Init(Config.getValue("MySQLHost"), Config.getValue("MySQLUser"), Config.getValue("MySQLPassword"), Config.getValue("MySQLDatabase"));
            Console.WriteLine(Config.getValue("MySQLHost"), Config.getValue("MySQLUser"), Config.getValue("MySQLPassword"), Config.getValue("MySQLDatabase"));
            Helpers.writeOut(tag, "Testing MySQL Interface");
            {
                var ok = SQL.Query($"SHOW FUNCTION STATUS");
                var tries = 0;
                while (ok == null)
                {

                    tries++;
                    Thread.Sleep(1200);
                    ok = SQL.Query("SHOW FUNCTION STATUS");
                    if (tries > 3)
                    {
                        Helpers.warn("Cannot connect to MySQL server.");
                        Console.WriteLine(SQL.getLastError());
                        Environment.Exit(-1);
                    }
                }
            }
            Console.WriteLine("OK!");

            /// Setup Lua State 
            LuaState = new Lua();
            LuaState.LoadCLRPackage(); // Initialize CLR for lua state 
            LuaState.DoString("dofile('xen/preinit.lua')");
            LuaState.DoString("import('XenfbotDN','XenfbotDN')"); // Import xenfbot namespace
            LuaState.DoString("print(Telegram)");
            LuaState.DoString("print(GroupConfiguration)");
            LuaState.DoString("dofile('xen/init.lua')");
     
            callHook = (LuaFunction)LuaState["modhook.Call"]; 

            
        }
    }
}
