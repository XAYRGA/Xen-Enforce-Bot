using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

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

        public static void doNotify(long user, long mid, long groupID, GroupConfigurationObject GCO)
        {
            SQL.Query($"UPDATE `verify` SET notified=TRUE WHERE `user`={user} AND `group`={groupID}");
        
        }

        public static void doVerify(long user, long mid, long groupID, GroupConfigurationObject GCO)
        {
            SQL.Query($"UPDATE `verify` SET verified=TRUE WHERE `user`={user} AND `group`={groupID}");
            root.callHook.Call("UserVerified",user, mid, groupID,GCO);
        }

        public static void doRemoval(long user, long mid, long groupID, GroupConfigurationObject GCO)
        {

        }

    }
}
