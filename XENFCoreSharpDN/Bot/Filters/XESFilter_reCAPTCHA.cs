using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XENFCoreSharp.DoubleDecker;


namespace XENFCoreSharp.Bot.Filters
{
    public class CaptchaActivationIndex
    {
        public long index;
        public string activation_id;
        public int activated;
        public long forwho;
        public long group;
        public long whencreated;
        public int activation_checked;
        public string username;
        public long actmessage;

    }


    public partial class XESFilter
    {
        public static bool captcha_CheckExpired()
        {
            MySql.Data.MySqlClient.MySqlDataReader cur;
            SQLQueryInstance QueryInst; 

            var ss = SQL.Query("SELECT * FROM xen_activations WHERE activated=0 OR activation_checked=0",out QueryInst);
            if (!ss)
            {
                Console.WriteLine("Query for activation checks failed {0}", SQL.getLastError());
                if (QueryInst!=null)
                {
                    QueryInst.Finish();
                }
                return false;
            }
            Stack<CaptchaActivationIndex> captchaActivationIndices  = new Stack<CaptchaActivationIndex>(1024); // hax? 
                                                                                                               // totally hax, I have to pull group configuration to check and see if the group has specific features enabled.
                                                                                                               // But I can't do that if I already have an SQL cursor open. So i'll have to read all of the results of it

            cur = QueryInst.reader;
            // before I can make a call to get group configuration.

            var ib = 0;
           
            while (cur.Read())
            {
                ib++;
               // Console.WriteLine(ib);
                var b = new CaptchaActivationIndex
                {
                    index = (long)cur["index"],
                    activation_id = (string)cur["activation_id"],
                    activated = (int)cur["activated"],
                    forwho = (long)cur["forwho"],
                    group = (long)cur["group"],
                    whencreated = (long)cur["whencreated"],
                    activation_checked = (int)cur["activation_checked"],
                    username = (string)cur["username"],
                    actmessage = (long)cur["actmessage"]
                };

                captchaActivationIndices.Push(b);
               
            
            }
            QueryInst.Finish();  // close it up. 

            for (int i=0; i < captchaActivationIndices.Count; i++)
            {
                var CurrentActivation = captchaActivationIndices.Pop();
                var chat = new TGChat();
                var user = new TGUser();
                user.id = CurrentActivation.forwho;
                chat.id = CurrentActivation.group;
                var kicktime = XenforceRoot.getGroupConfigurationValue(chat, "kicktime", 30);
                var announce = XenforceRoot.getGroupConfigurationValue(chat, "announcekicks",1);
                var unmute = XenforceRoot.getGroupConfigurationValue(chat, "muteuntilverified", false);
                //Console.WriteLine("Wtf {0} {1}",CurrentActivation.activated,CurrentActivation.activation_checked);
                if (CurrentActivation.activated==0)
                {
                    if (CurrentActivation.whencreated < Helpers.getUnixTime() - (kicktime * 60))
                    {
                        Telegram.deleteMessage(chat, CurrentActivation.actmessage);
                        Telegram.kickChatMember(chat, user, 0); // kick them from the chat. 
                        Console.WriteLine("Remove user?");
                        var rar = 0;
                        var ok = SQL.NonQuery(string.Format("DELETE FROM xen_activations WHERE activation_id='{0}'", CurrentActivation.activation_id), out rar);
                        if (announce > 0)
                        {
                            var mymessage = Telegram.sendMessage(chat, CurrentActivation.username + " was removed from the chat for not completing the CAPTCHA.");
                            if (mymessage != null)
                            {
                                XenforceRoot.AddCleanupMessage(chat.id, mymessage.message_id, 30); // Clean up after 30 seconds.
                            }
                        }
    

                    }
                } else if (CurrentActivation.activated==1 && CurrentActivation.activation_checked==0)
                {
                    Telegram.deleteMessage(chat, CurrentActivation.actmessage);

                    var Oldmsg = CurrentActivation.username + ", thanks for verifying you're not a robot.";
                    var actiMsg = XenforceRoot.getGroupConfigurationValue(chat, "activationmessage", "%NAME, thanks for verifying you're not a robot.");
                    if (actiMsg==null || actiMsg.Length < 5)
                    {
                        actiMsg = Oldmsg;
                    }

                    actiMsg = actiMsg.Replace("%NAME", CurrentActivation.username);

                    var mymessage = Telegram.sendMessage(chat,actiMsg );
                    var ra = 0;
                    var ok = SQL.NonQuery("UPDATE xen_activations SET activation_checked=1 WHERE activation_id='" + SQL.escape(CurrentActivation.activation_id) + "'", out ra);
                    if (!ok)
                    {
                        Console.WriteLine("Updating activation message failed! Might spam!!!?");
                    }
                    if (mymessage != null)
                    {
                        XenforceRoot.AddCleanupMessage(chat.id, mymessage.message_id, 30); // Clean up after 30 seconds.
                    }

                    if (unmute)
                    {
                        Telegram.restrictChatMember(chat, user, 0, true, true, true, true);
                    }
                }
            }
            return true;
        }
        public static bool captcha(TGMessage msg, TGUser usr)
        {
            var UserID = usr.id;
            var GroupID = msg.chat.id;
            var ActivationID = Helpers.Base64Encode(UserID.ToString() + GroupID.ToString());
            var q = usr.username;
            var user_name_full = usr.first_name + " " + usr.last_name;
            var ko = 0;

            var ok = SQL.NonQuery(string.Format("DELETE FROM xen_activations WHERE activation_id='{0}'", ActivationID), out ko); // Remove the current activation ID. 


            if (q!=null)
            {
                user_name_full = "@" + q; // okay okay fine jeez.
            } 
          
            
            var muteUntilVerified = XenforceRoot.getGroupConfigurationValue(msg.chat, "muteuntilverified", false);
            var kicktime = XenforceRoot.getGroupConfigurationValue(msg.chat, "kicktime", 30);
            var instance_time = Helpers.getUnixTime();
        
            var FullMessageOld = string.Format(
                "Welcome, %NAME. \n" +
                "Please complete a quick captcha within %DURATION minutes to verify you're not a bot: \n\n" +
                "%ACTURL"
                );

            var FullMessage = XenforceRoot.getGroupConfigurationValue(msg.chat, "message", FullMessageOld);


            if (FullMessage==null || FullMessage.Length < 10)
            {
                FullMessage = FullMessageOld;
            }

            FullMessage = FullMessage.Replace("%ACTURL", "https://xenfbot.ga" +
                "/xenf/?actid=" + ActivationID);
            FullMessage = FullMessage.Replace("%DURATION", kicktime.ToString());
            FullMessage = FullMessage.Replace("%NAME", user_name_full);

            if (muteUntilVerified == true)
            {
                Telegram.restrictChatMember(msg.chat, usr, 0, false, false, false, false); // Restrict until they verify.
                FullMessage += "\n\nYou will not be able to send any messages until you've verified.";
            }

            var message =  msg.replySendMessage(FullMessage);

            // INSERT INTO xen_activations (activation_id,group,forwho,whencreated,actmessage,username) VALUES ('{0}',{1},{2},{3},{4},'{5}');

            if (message == null || GroupID == null || instance_time == null || user_name_full == null || ActivationID==null)
            {
                try
                {
                    Console.WriteLine("MESSAGE IS NULL OR SOMETHING");
                    Console.WriteLine("PROBLEM: {0} {1}", message, user_name_full);
                }
                catch { }
            } else 

            {
                var statement =
                    string.Format("INSERT INTO xen_activations (`activation_id`,`group`,`forwho`,`whencreated`,`actmessage`,`username`) VALUES ('{0}',{1},{2},{3},{4},'{5}')",
                    SQL.escape(ActivationID),
                    GroupID,
                    UserID,
                    instance_time,
                    message.message_id,
                    SQL.escape(user_name_full)
                    );
                int ra = 0;
                SQL.NonQuery(statement, out ra);
                if (ra < 1)
                {
                    Console.WriteLine("Creating activation ID failed. No SQL rows affected.");
                    var cmsg = msg.replySendMessage("CreateActivationID() FAILED:\n\n Info:\n\n" + SQL.getLastError());
                    if (cmsg != null)
                    {
                        XenforceRoot.AddCleanupMessage(message.chat.id, cmsg.message_id, 120);
                    }

                }

                return false;
            }
            return false;
        }
    
    }
}
