using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace XenfbotDN
{

    public class VerifyData
    {
        private DataRow data;

        public VerifyData(DataRow qData)
        {
            data = qData;
        }

        public object getObject(string name)
        {
            if (data == null)
                return null;
            try
            {
                if (data[name] != null)
                {
                    return data[name];
                }
            }
            catch { return null; }
            return null;
        }
        public string getString(string name)
        {
            return (string)(getObject(name));
        }

        public bool getBool(string name)
        {
            var odata = getObject(name);
            if (odata == null)
                return false;
            return (bool)(odata);
        }
        public int getInt(string name)
        {
            var odata = getObject(name);
            if (odata == null)
                return 0;
            return (int)(odata);
        }
        public long getLong(string name)
        {
            var odata = getObject(name);
            if (odata == null)
                return 0;
            return (long)(odata);
        }
    }
    public static class Verify
    {
        private static int delay = 15;
        private static int last = 0;
        public static void runTask()
        {
            if (Helpers.getUnixTime() < last + delay)
                return;
            last = Helpers.getUnixTime();
            var data = SQL.Query("SELECT * FROM `verify` WHERE `verified`=false OR `notified`=false"); 
            
            foreach (DataRow row in data)
            {
                var groupID = (long)row["group"];
                var GCO = GroupConfiguration.getConfig(groupID);
                var verified = (bool)row["verified"];
                var notified = (bool)row["notified"];
                var whenexpire = (int)row["texpire"];
                var user = (long)row["user"];
                var mid = (long)row["message"];
                var jmid = (long)row["joinmessage"];

                if (verified==false)
                {
                    //Console.WriteLine($"{whenexpire} -- {Helpers.getUnixTime()}");
                    if (whenexpire < Helpers.getUnixTime())
                    {
                        doRemoval(user, mid, groupID, GCO,jmid);
                    }
                } else if (notified==false & verified==true) {
                    doNotify(user, mid, groupID, GCO,jmid);
                }
            }           
        }

        public static bool checkDoubt(TGUser user, TGChat chat)
        {
            var dr0 = SQL.Query($"SELECT * FROM `verify_doubt` WHERE `user`={user.id} AND `group`={chat.id}");
            if (dr0.Count == 0)
            {
                return false;
            }
            return true; 
        }

        public static void addInstance(TGUser user, TGChat chat,TGMessage assoc_message, GroupConfigurationObject GCO,string challenge_data, int minutes, TGMessage joinM)
        {
            int ra = 0;
            int messageID = 0;
            SQL.NonQuery($"DELETE FROM `verify` WHERE `user`={user.id} AND `group`={chat.id}", out ra);
            SQL.NonQuery($"INSERT INTO `verify` (`user`,`group`,`challenge`,`tcreated`,`texpire`,`joinmessage`,`message`) VALUES({user.id},{chat.id},'{SQL.escape(challenge_data)}',{Helpers.getUnixTime()}, {Helpers.getUnixTime() + (minutes * 60)},{joinM.message_id},{assoc_message.message_id})", out ra);
        }

        public static VerifyData getVerifyData(TGUser user, TGChat chat, TGMessage assoc_message)
        {
            int ra = 0;
            int messageID = 0;
            var dr0 = SQL.Query($"SELECT * FROM `verify` WHERE `user`={user.id} AND `group`={chat.id} ORDER BY `uid` DESC");
            if (dr0.Count==0)
            {
                return null; 
            }
            return new VerifyData(dr0[0]);   
        }

        public static void doNotify(long user, long mid, long groupID, GroupConfigurationObject GCO, long jmid)
        {
            int ra = 0;
            SQL.NonQuery($"UPDATE `verify` SET notified=TRUE WHERE `user`={user} AND `group`={groupID}", out ra);
            var thc = new TGChat() { id = groupID };
            var thu = new TGUser() { id = user };
            var thm = Telegram.getChatMember(thc, thu);

            if (mid!=0)
            {
                Telegram.deleteMessage(thc, mid);
            }
            if (jmid!=0)
            {
                Telegram.deleteMessage(thc, jmid);
            }
            root.callHook.Call("verUserVerifiedNotify", thc, thu, groupID, GCO);
            

            if (GCO.getBool("verifyannounce"))
            {
                var name = Helpers.getMentionName(thm);
                var sendMsg = Localization.getStringLocalized(GCO.getString("language"), "verify/userVerified", name);
                var vermsg = GCO.getString("verifymessage");

                if (vermsg != null & vermsg.Length > 2)
                {
                    sendMsg = vermsg;
                    sendMsg = sendMsg.Replace("%NAME", name);
                }
                var msg = Telegram.sendMessage(new TGChat() { id = groupID }, sendMsg);
                Cleanup.addMessage(msg);
            }
        }

        public static void doVerify(long user, long mid, long groupID, GroupConfigurationObject GCO)
        {
            int ra = 0;
            SQL.NonQuery($"UPDATE `verify` SET `verified`=TRUE, `tverified`={Helpers.getUnixTime()} WHERE `user`={user} AND `group`={groupID}", out ra);
            root.callHook.Call("verUserVerified",user, mid, groupID,GCO);
        }

        public static void doRemoval(long user, long mid, long groupID, GroupConfigurationObject GCO, long jmid)
        {
            int ra = 0;
            Removals.addIncident(new TGUser() { id = user }, new TGChat { id = groupID }, "VERIFYEXPIRE");
            SQL.NonQuery($"DELETE FROM `verify` WHERE `user`={user} AND `group`={groupID}", out ra);
            root.callHook.Call("verUserRemoved", user, mid, groupID, GCO,false);
            var thc = new TGChat() { id = groupID };
            var thu = new TGUser() { id = user };
            var thm = Telegram.getChatMember(thc,thu);
            if (mid != 0)
            {
                Telegram.deleteMessage(thc, mid);
            }
            if (jmid != 0)
            {
                Telegram.deleteMessage(thc, jmid);
            }
            Telegram.kickChatMember(thc, thu, 0);
            //Telegram.sendMessage(thc, "welcome to kicked, nobody will ever know you existed."); 
            if (GCO.getBool("verifyannounce"))
            {
                var name = Helpers.getMentionName(thm);
                var sendMsg = Localization.getStringLocalized(GCO.getString("language"), "verify/userKicked", name);
                var msg = Telegram.sendMessage(new TGChat() { id = groupID }, sendMsg);
                Cleanup.addMessage(msg);
            }
        }

        public static void doRemovalDoubt(long user, long mid, long groupID, GroupConfigurationObject GCO,long jmid)
        {
            int ra = 0;
            SQL.NonQuery($"DELETE FROM `verify` WHERE `user`={user} AND `group`={groupID}", out ra);
            SQL.NonQuery($"INSERT INTO `verify_doubt` (`user`,`group`) VALUES({user},{groupID})", out ra);
            Removals.addIncident(new TGUser() { id = user }, new TGChat { id = groupID }, "VERIFYEXPIRE");
            root.callHook.Call("verUserRemoved", user, mid, groupID, GCO,true);
            var thc = new TGChat() { id = groupID };
            var thu = new TGUser() { id = user };
            var thm = Telegram.getChatMember(thc, thu);
            if (mid != 0)
            {
                Telegram.deleteMessage(thc, mid);
            }
            if (jmid != 0)
            {
                Telegram.deleteMessage(thc, jmid);
            }
            Telegram.kickChatMember(thc, thu, 120);
            if (GCO.getBool("verifyannounce"))
            {
                var name = Helpers.getMentionName(thm);
                var sendMsg = Localization.getStringLocalized(GCO.getString("language"), "verify/userKickedDoubt", name);
                var msg = Telegram.sendMessage(new TGChat() { id = groupID }, sendMsg);
                Cleanup.addMessage(msg);
            }
        }

    }
}
