using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace XenfbotDN
{
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

        public static void addInstance(TGUser user, TGChat chat,TGMessage assoc_message, GroupConfigurationObject GCO,string challenge_data)
        {
            int ra = 0;
            int messageID = 0;
            
            SQL.NonQuery($"INSERT INTO `verify` (`user`,`group`,`challenge`", out ra);
        }

        public static void isVerified(TGUser user, TGChat chat, TGMessage assoc_message, GroupConfigurationObject GCO, string challenge_data)
        {
            int ra = 0;
            int messageID = 0;
            SQL.NonQuery($"INSERT INTO `verify` (`user`,`group`,`challenge`", out ra);
        }

        public static void doNotify(long user, long mid, long groupID, GroupConfigurationObject GCO)
        {
            int ra = 0;
            SQL.NonQuery($"UPDATE `verify` SET notified=TRUE WHERE `user`={user} AND `group`={groupID}", out ra);
        
        }

        public static void doVerify(long user, long mid, long groupID, GroupConfigurationObject GCO)
        {
            int ra = 0;
            SQL.NonQuery($"UPDATE `verify` SET verified=TRUE WHERE `user`={user} AND `group`={groupID}", out ra);
            root.callHook.Call("UserVerified",user, mid, groupID,GCO);
        }

        public static void doRemoval(long user, long mid, long groupID, GroupConfigurationObject GCO)
        {

            Removals.addIncident(new TGUser() { id = user }, new TGChat { id = groupID }, "VERIFYEXPIRE");
        }

    }
}
