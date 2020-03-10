using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XenfbotDN.Commands
{
    class configure : BaseCommand
    {



        public override void init()
        {
            command = "config";
            CommandAuthor = "XAYRGA";
            CommandVersion = "1.0";
            adminOnly = true;
        }

        public static string[] configureCommands =
        {
                "kickTime",
                "useNameFilter",
                "useBotScreen",
                "kickMediaBeforeCaptcha",
                "muteUntilVerified",
                "announceKicks",
                "message",
                "activationMessage"
        };        

        public override async Task Execute(TGMessage message, string[] args, GroupConfigurationObject gco, string langcode)
        {
            Console.WriteLine(args.Length);
            if (args.Length==0)
            {
                var msgs = Localization.getStringLocalized(langcode, "basic/commands/config/availableCommands") + "\n\n";
                for (int i=0; i < configureCommands.Length;i++)
                {
                    
                    msgs += "[" + configureCommands[i] +"] "+ Localization.getStringLocalized(langcode, "basic/commands/config/" + configureCommands[i]) + "\n\n";
                }
                await message.replySendMessage(msgs);
                return;
            }

            var val = args[0];
            
            switch (val)
            {
                case "kickTime":
                    {
                        int num = 0;
                        var w = Int32.TryParse(args[1], out num);
                        if (w != true)
                        {
                            var mems = await message.replySendMessage("I couldn't understand the value " + args[1] + ", it should be a number.");
                            return;
                        }
                        var q = await gco.setNumber(args[0], num);
                        if (q != true)
                        {
                            var mems = await message.replySendMessage(Localization.getStringLocalized(langcode, "basic/config/somethingWrong"));
                            return;
                        }
                        break;
                    }
                case "useNameFilter":
                case "useBotScreen":
                case "kickMediaBeforeCaptcha":
                case "muteUntilVerified":
                case "announceKicks":
                    {
                        bool resv = args[1] == "true";

                        if (resv == true)
                        {
                            var q = await gco.setNumber(args[0], 1);
                            if (q != true)
                            {
                                var mems = await message.replySendMessage(Localization.getStringLocalized(langcode, "basic/config/somethingWrong"));
                                return;
                            }
                        } else
                        {
                            var q = await gco.setNumber(args[0], 0);
                            if (q != true)
                            {
                                var mems = await message.replySendMessage(Localization.getStringLocalized(langcode, "basic/config/somethingWrong"));
                                return;
                            }
                        }

                        break;
                    }


                case "message":
                case "activationmessage":
                    break;
            }
            var fmt = Localization.getStringLocalized(langcode, "basic/config/success");
            var vl1 = string.Format(fmt,args[0],args[1]);
            await message.replySendMessage(vl1);
        }
    }
}
