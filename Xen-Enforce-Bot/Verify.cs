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
    }
    public static class Verify
    {
        private static int delay = 30;
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
                var user = (long)row["user"];
                var mid = (long)row["message"];
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

        public static void addInstance(TGUser user, TGChat chat,TGMessage assoc_message, GroupConfigurationObject GCO,string challenge_data)
        {
            int ra = 0;
            int messageID = 0;
            SQL.NonQuery($"INSERT INTO `verify` (`user`,`group`,`challenge`,`tcreated`) VALUES({user.id},{chat.id},'{SQL.escape(challenge_data)}',{Helpers.getUnixTime()}", out ra);
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

        public static void doNotify(long user, long mid, long groupID, GroupConfigurationObject GCO)
        {
            int ra = 0;
            SQL.NonQuery($"UPDATE `verify` SET notified=TRUE WHERE `user`={user} AND `group`={groupID}", out ra);
            if (GCO.getBool("verifyannounce"))
            {
                var thc = new TGChat() { id = groupID };
                var thm = Telegram.getChatMember(thc, new TGUser() { id = user });
                var name = thm.User.username;
                if (name == null)
                {
                    name = thm.User.first_name;
                    if (thm.User.last_name != null)
                    {
                        name += " " + thm.User.last_name;
                    }
                }
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

        public static void doRemoval(long user, long mid, long groupID, GroupConfigurationObject GCO)
        {
            int ra = 0;
            Removals.addIncident(new TGUser() { id = user }, new TGChat { id = groupID }, "VERIFYEXPIRE");
            SQL.NonQuery($"DELETE FROM `verify` WHERE `user`={user} AND `group`={groupID}", out ra);
            root.callHook.Call("verUserRemoved", user, mid, groupID, GCO,false);
            var thc = new TGChat() { id = groupID };
            var thu = new TGUser() { id = user };
            var thm = Telegram.getChatMember(thc,thu);
            Telegram.kickChatMember(thc, thu, 0);
            if (GCO.getBool("verifyannounce"))
            {
                var name = thm.User.username;
                if (name == null)
                {
                    name = thm.User.first_name;
                    if (thm.User.last_name != null)
                    {
                        name += " " + thm.User.last_name;
                    }
                }
                var sendMsg = Localization.getStringLocalized(GCO.getString("language"), "verify/userKicked", name);
                var msg = Telegram.sendMessage(new TGChat() { id = groupID }, sendMsg);
                Cleanup.addMessage(msg);
            }
        }

        public static void doRemovalDoubt(long user, long mid, long groupID, GroupConfigurationObject GCO)
        {
            int ra = 0;
            Removals.addIncident(new TGUser() { id = user }, new TGChat { id = groupID }, "VERIFYEXPIRE");
            root.callHook.Call("verUserRemoved", user, mid, groupID, GCO,true);
            var thc = new TGChat() { id = groupID };
            var thu = new TGUser() { id = user };
            var thm = Telegram.getChatMember(thc, thu);
            Telegram.kickChatMember(thc, thu, 0);
            if (GCO.getBool("verifyannounce"))
            {
                var name = thm.User.username;
                if (name == null)
                {
                    name = thm.User.first_name;
                    if (thm.User.last_name != null)
                    {
                        name += " " + thm.User.last_name;
                    }
                }
                var sendMsg = Localization.getStringLocalized(GCO.getString("language"), "verify/userKickedDoubt", name);
                var msg = Telegram.sendMessage(new TGChat() { id = groupID }, sendMsg);
                Cleanup.addMessage(msg);
            }
        }

    }
}
