using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;

namespace XenfbotDN
{
    public static class botRoot
    {
        public static void Enter()
        {
            while (true)
            {
                processUpdates();
                Thread.Sleep(200);
                Verify.runTask();
                Cleanup.runTask();
            }
        }

        static long lastUpdate = 0;
        static Dictionary<long, bool> groupError = new Dictionary<long, bool>();

        public static void processUpdates()
        {
            var up = Telegram.getUpdates(lastUpdate);
            if (up == null)
            {
                Console.WriteLine("TGAPI Response failure update==null");
                return;
            }
            Console.WriteLine("Updates: {0}", up.Length);

            for (int i = 0; i < up.Length; i++)
            {
                var currentUpdate = up[i];
                if (currentUpdate.update_id >= lastUpdate)
                {
                    lastUpdate = currentUpdate.update_id + 1;
                }
                if (currentUpdate.edited_message != null)
                {
                    currentUpdate.message = currentUpdate.edited_message; // ahax.
                }
                Console.WriteLine(JsonConvert.SerializeObject(currentUpdate));
           
                {
                    if (currentUpdate.message != null)
                    {
                        try
                        {
                            processIndividualUpdate(currentUpdate);
                        } catch (Exception E)
                        {
                            bool error = false;
                            groupError.TryGetValue(currentUpdate.message.chat.id, out error);
                            if (!error)
                            {
                                currentUpdate.message.replySendMessage("A serious exception has occured:\n\nException bleeding into root, there's blood everywhere.\n\nLua State Stack: UNAVAILABLE.\n\nC# State Stack:\n------XEN-ENFORCE-BOT\n" + E.ToString() + "\n\nThis is not supposed to happen, nor are you supposed to see this message if the bot is running in production mode. Please contact the bot's administrator!");
                                currentUpdate.message.replySendMessage("Due to this error condition, it is possible that Xenfbot may stop working for this chat until it is restarted.");
                                groupError[currentUpdate.message.chat.id] = true; 
                            }
                        }
                    }
                }
            }
        }

        public static void processIndividualUpdate(TGUpdate update)
        {
            
            var msg = update.message;
            if (msg.from.is_bot == true) // Don't process updates from other bots
                return;
            var langcode = "en"; // default language is englsh
            var gc = GroupConfiguration.getConfig(update.message.chat.id);
            var VFD = Verify.getVerifyData(update.message.from, update.message.chat, update.message);
            var doubt = Verify.checkDoubt(update.message.from, update.message.chat);
            // Do captcha
            if (msg.new_chat_members != null)
            {
                var ncm = msg.new_chat_members;
                for (int i = 0; i < ncm.Length; i++)
                {

                    if (ncm[i].username == root.botUsername)
                    {
                        var cl1 = Localization.getLanguageInfo(langcode);
                        var cl = Localization.getStringLocalized(langcode, "locale/currentLangName");
                        var smsg = Localization.getStringLocalized(langcode, "basic/xenfbot", " BRN 'Bone-Hurting-Juice' 4.0.1 (Cheetah) ", cl, cl1.authors, cl1.version,"@xayrga");
                        smsg += "\n\n";
                        smsg += Localization.getStringLocalized(langcode, "basic/welcome");

                        msg.replySendMessage(smsg);
                    }
                    if (!ncm[i].is_bot)
                    {
                        root.callHook.Call("NewChatMember", gc, msg, VFD, doubt);
                    }
                }
            }

            if (msg.text != null)
            {
                root.callHook.Call("OnTextMessage",gc,msg, VFD, doubt);
            }
            root.callHook.Call("OnRawMessage", gc, msg, VFD, doubt);
        }


    }
}
