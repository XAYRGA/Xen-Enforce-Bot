using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Threading;

namespace XenfbotDN
{
    public static class botRoot
    {
        public static void Enter()
        {
            while (true)
            {
                processUpdates();
                Thread.Sleep(1000);
                Verify.runTask();
                Cleanup.runTask();
            }
        }

        static long lastUpdate = 0;
        public static void processUpdates()
        {
            var up = Telegram.getUpdates(lastUpdate);
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
                        processIndividualUpdate(currentUpdate);
                    }
                }
            
            }
        }

        public static void processIndividualUpdate(TGUpdate update)
        {
            var msg = update.message;
            var langcode = "en"; // default language is englsh
            var gc = GroupConfiguration.getConfig(update.message.chat.id);

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
                        var smsg = Localization.getStringLocalized(langcode, "basic/xenfbot", "4.0.0.1", cl, cl1.authors, cl1.version,"@xayrga");
                        smsg += "\n\n";
                        smsg += Localization.getStringLocalized(langcode, "basic/welcome");

                        msg.replySendMessage(smsg);
                    }
                    root.callHook.Call("NewChatMember", gc, msg);
                }
            }

            if (msg.text != null)
            {
                root.callHook.Call("OnMessage",gc,msg);
            }

        }


    }
}
