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
                try
                {
                    processUpdates();
                    Thread.Sleep(200);
                    Verify.runTask();
                    Cleanup.runTask();
                } catch (Exception E)
                {
                    Helpers.writeStack(E.ToString());
                }
            }
        }

        static long lastUpdate = 0;
        static Dictionary<long, bool> groupError = new Dictionary<long, bool>();
        static bool allowProcessUpdates = false;
        public static void processUpdates()
        {
            var up = Telegram.getUpdates(lastUpdate);
            if (up == null)
            {
                Console.WriteLine("TGAPI Response failure update==null");
                return;
            }
            Console.WriteLine("Updates: {0}", up.Length);
            if (up.Length == 0)
                allowProcessUpdates = true;

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
                if (allowProcessUpdates)
                {
#if DEBUG
                    //Console.WriteLine(JsonConvert.SerializeObject(currentUpdate));
#endif

                    {
                        if (currentUpdate.message != null)
                        {
                            try
                            {
                                processIndividualUpdate(currentUpdate);
                            }
                            catch (Exception E)
                            {

                                Helpers.writeStack(E.ToString());
                            }
                        }
                    }
                } else
                {
                    Console.WriteLine("Skipping update due to startup condition....");
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
                        var smsg = Localization.getStringLocalized(langcode, "basic/xenfbot", " BRN 'MASTER' 4.0.8 (Noodle Dragon) ", cl, cl1.authors, cl1.version,"@xayrga");
                        smsg += "\n\n";
                        smsg += Localization.getStringLocalized(langcode, "basic/welcome");

                        msg.replySendMessage(smsg);
                    }
                    if (!ncm[i].is_bot)
                    {
                        root.callHook.Call("NewChatMember", gc, msg, VFD, doubt,ncm[i]);
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
