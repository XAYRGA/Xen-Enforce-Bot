using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XENFCoreSharp.DoubleDecker;

namespace XENFCoreSharp.Bot
{
    public static class XenforceCommandInterpreter
    {

        public static bool BRESULT;
        public static string TRESULT;
        public static Dictionary<string, Action<TGMessage,string[]>> AllCommands = new Dictionary<string, Action<TGMessage, string[]>>();
        
        public static void LoadCommands()
        {
            Commands.HelpCommand.load();
            Commands.ConfigCommand.load();
        }

        public static bool AddCommand(string commandName , Action<TGMessage, string[]> method)
        {
            Console.WriteLine("CXenforceCommandInterpreter: ADDING COMMAND {0}", commandName);
            AllCommands[commandName] = method;
            return true;
        }
        public static bool ExecuteCommand(string command, TGMessage metadata, string[] arguments)
        {
            Action<TGMessage, string[]> currentCommand;
            var Success = AllCommands.TryGetValue(command, out currentCommand);
       
            if (currentCommand==null || Success == false)
            {
                TRESULT = "No such command.";
                BRESULT = false; 
                return false;
            }
            try
            {   if (metadata.isSenderAdmin())
                {
                    currentCommand.Invoke(metadata, arguments);
                }
            } catch (Exception E)
            {

                var file = Helpers.writeStack(E.ToString());
                BRESULT = false;
                TRESULT = "Something terrible happened while running this. If you need support with this, please reference this XEN_STACK_" + file;
                return false;
            }
           
            return true;
        }
    }
}
