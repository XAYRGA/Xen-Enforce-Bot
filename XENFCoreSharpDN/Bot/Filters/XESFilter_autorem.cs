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
        public static bool autorem(TGMessage msg, TGUser usr)
        {
            var chat = msg.chat;
            var enable = XenforceRoot.getGroupConfigurationValue(chat, "autoban", true);
            if (!enable) { return false; } // Not enabled. 
            var qsc = "SELECT * FROM xenf_autokick WHERE `group`={0} AND `user`={1}";
            var rqry = string.Format(qsc, chat.id, usr.id);
            MySql.Data.MySqlClient.MySqlDataReader datar;
            SQLQueryInstance QueryInst; 
            var queryok = SQL.Query(rqry, out QueryInst);
            if (QueryInst!=null && QueryInst.reader.HasRows) // They've already been kicked before. 
            {
                QueryInst.Finish();
                return false; 
            }
            if (QueryInst != null)
            {
                QueryInst.Finish();
            }

            var UserID = usr.id;
            var GroupID = msg.chat.id;
            var user_name_full = usr.first_name + " " + usr.last_name;
            var userscore = 500;
            for (int i=0; i < user_name_full.Length; i++)
            {
                var wtf = user_name_full[i];
                if (wtf> 0xAF)
                {
                    userscore += 5;
                } else if (wtf < 0x80)
                {
                    userscore -= 3;
                }
            }
            var picons = Telegram.getNumProfilePhotos(usr);
            userscore -= picons * 45;
            if (picons==0)
            {
                userscore += 30;
            }
            if (userscore > 488)
            {
                Telegram.kickChatMember(chat, usr, 120); 
                if (XenforceRoot.getGroupConfigurationValue(chat,"announcekicks",1) > 0)
                {
                    msg.delete();
                    var msgr = msg.replySendMessage(user_name_full + " was automatically removed from the chat -- I think they're a bot.");
                    XenforceRoot.AddCleanupMessage(chat.id,msgr.message_id,30);
                    var statement =
                        string.Format("INSERT INTO xenf_autokick (`group`,`user`,`when`,`why`) VALUES ({0},{1},{2},'{3}')",
                        GroupID,
                        UserID,
                        Helpers.getUnixTime(),
                        "Bot Score too high"            
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
                return true;
            }
            return false; 
        }
    
    }
}
