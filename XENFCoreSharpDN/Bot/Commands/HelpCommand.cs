using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XENFCoreSharp.DoubleDecker;

namespace XENFCoreSharp.Bot.Commands
{
    static class HelpCommand
    {

        public static void sendHelp(TGMessage data,string[] arguments)
        {
            data.replySendMessage("Xen Enforce Bot V3.1.4.5 (Serg--- Circle?)\n\nDocumentation on commands is available at https://www.github.com/XAYRGA/XENFCoreSharp/");
          
        }


        public static void load()
        {
            XenforceCommandInterpreter.AddCommand("help", sendHelp);
        }
    }
}
