using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XENFCoreSharp.DoubleDecker;
using System.Threading.Tasks;
using System.Threading;

namespace XENFCoreSharp
{
    public class Root
    {
        public static CXayrgaConfRom Configuration; 

        static void Main(string[] args)
        {
            Console.WriteLine("Opening config from config.ini");
            Configuration = new CXayrgaConfRom("config.ini");
            

            Console.WriteLine("Opening SQL connection");
            var initValue = SQL.Init(Configuration.getValue("MySQLHost"), Configuration.getValue("MySQLUser"), Configuration.getValue("MySQLPassword"), Configuration.getValue("MySQLDatabase"));

            if (!initValue)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Initialization stopped: Failed to connect to MySQL server.");
                Console.ReadLine();
                return;
            }
            initValue = SQL.Init(Configuration.getValue("MySQLHost"), Configuration.getValue("MySQLUser"), Configuration.getValue("MySQLPassword"), Configuration.getValue("MySQLDatabase"));

            if (!initValue)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Initialization stopped: Failed to connect to MySQL server.");
                Console.ReadLine();
                return;
            }

            initValue = SQL.Init(Configuration.getValue("MySQLHost"), Configuration.getValue("MySQLUser"), Configuration.getValue("MySQLPassword"), Configuration.getValue("MySQLDatabase"));

            if (!initValue)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Initialization stopped: Failed to connect to MySQL server.");
                Console.ReadLine();
                return;
            }




            Console.WriteLine("Initializing Telegram API");

            Telegram.SetAPIKey(Configuration.getValue("TGAPIKey"));
            var q = Telegram.getMe(); // Test API. 
            if (q==null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Initialization stopped: TGAPIKey is invalid.");
                Console.ReadLine();
                return;
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("OK. Successfully initialized all subsystems. Starting bot.");
            Console.ForegroundColor = ConsoleColor.White;
            long last_update = 0;

            XENFCoreSharp.Bot.XenforceRoot.Start();
        }
    }
}
