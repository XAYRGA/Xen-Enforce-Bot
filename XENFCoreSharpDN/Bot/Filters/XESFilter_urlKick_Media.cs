using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XENFCoreSharp.DoubleDecker;

namespace XENFCoreSharp.Bot.Filters
{
    public partial class XESFilter
    {
        public static void doURLMediaFilter(TGMessage msg, TGUser usr)
        {
             
            var chat = msg.chat; // grab chat.

            var enabled = XenforceRoot.getGroupConfigurationValue(chat, "kickurlunactivated", false); // Check configuration value.

            if (!enabled) // return if not enabled.
                return;

            var qsc = "SELECT * FROM xen_activations WHERE activated=0 AND `group`={0} AND `forwho`={1}"; // 

            var rqry = string.Format(qsc, chat.id, usr.id);

            SQLQueryInstance QueryInst;
            var queryok = SQL.Query(rqry, out QueryInst);
            bool onerow = false;

            if (QueryInst!= null && QueryInst.reader.HasRows) // They've already been kicked before. If we return at least one row, then its valid to assume they havent activated 
            { // There can only be one activation index per user per group.
                onerow = true;
            }

            if (QueryInst != null)
            {
                QueryInst.Finish();
            }

            if (!onerow)
                return; // There was no activation 

            var wtf = msg.replySendMessage(usr.first_name + " was removed from the chat for sending URL/Media before activating!");
            XenforceRoot.AddCleanupMessage(msg.chat.id, wtf.message_id, 30);
            Telegram.kickChatMember(msg.chat, msg.from, 30);
            msg.delete();

            var statement =
              string.Format("INSERT INTO xenf_autokick (`group`,`user`,`when`,`why`) VALUES ({0},{1},{2},'{3}')",
              msg.chat.id,
              msg.from.id,
              Helpers.getUnixTime(),
              "URLMedia_Picture"
            );
            int ra = 0;
            SQL.NonQuery(statement, out ra);
            if (ra < 1)
            {
                Console.WriteLine("Creating autorem incident failed failed. No SQL rows affected.");
                var cmsg = msg.replySendMessage("AutoremAddIncident() FAILED:\n\n Info:\n\n" + SQL.getLastError());
                XenforceRoot.AddCleanupMessage(chat.id, cmsg.message_id, 120);
            }
        }
    }
}
