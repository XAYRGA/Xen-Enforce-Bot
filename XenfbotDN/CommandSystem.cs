using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XenfbotDN.Commands;

namespace XenfbotDN
{
    public static class CommandSystem
    {
        public const string tag = "xenfbot@commands";
        public static Dictionary<string,Commands.BaseCommand> AllCommands = new Dictionary<string, Commands.BaseCommand>();
        public static Type[] CommandLoadList =
        {
            typeof(Commands.configure),
        };
        public static void loadCommands()
        {
            for (int i=0; i < CommandLoadList.Length; i++)
            {
                var w = (Commands.BaseCommand)Activator.CreateInstance(CommandLoadList[i]);
                w.init();
                Helpers.writeOut(tag, "Loaded command {0} v {1} by {2}", w.command, w.CommandVersion, w.CommandAuthor);
                AllCommands[w.command] = w;
            }
        }
        public static async Task runCommand(TGMessage message, GroupConfigurationObject gco, string langcode)
        {
            
            var varg_1 = message.text.Split((char)0x20);
            if (varg_1.Length > 0)
            {
                if (varg_1[0]=="/xen")
                {
                    if (varg_1.Length > 1)
                    {
                        var VARGUMENTS = new string[varg_1.Length - 2];
                        for (int ix = 2; ix < varg_1.Length; ix++)
                        {
                            VARGUMENTS[ix - 2] = varg_1[ix]; // Move only string argument into argument table. 
                        }
                        var cmdText = varg_1[1];
                        BaseCommand cmd;
                        var ok = AllCommands.TryGetValue(cmdText, out cmd);
                        if (!ok)
                        {
                            var nokText = Localization.getStringLocalized(langcode, "basic/commands/notFound");
                            Helpers.quickFormat(ref nokText, "%s", cmdText);
                            await message.replySendMessage(nokText);
                        } else
                        {
                            await cmd.Execute(message, VARGUMENTS, gco, langcode);
                        }

                    }     
                                  
                }
            }
                        
        }
    }
}
