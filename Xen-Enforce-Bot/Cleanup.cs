using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace XenfbotDN
{
    public static class Cleanup
    {
        private static int delay = 30;
        private static int last = 0; 
        public static void runTask()
        {
            if (Helpers.getUnixTime() < last + delay)
                return;
            last = Helpers.getUnixTime();

            var data = SQL.Query("SELECT * FROM `cleanup`");
            if (data!=null)
            {
                foreach (DataRow dr in data)
                {
                    var GCO = GroupConfiguration.getConfig((long)dr["group"]);

                    if ((int)dr["when"] + (int)dr["life"] < Helpers.getUnixTime())
                    {
                        Telegram.deleteMessage(new TGChat() { id = (long)dr["id"] }, (long)dr["mid"]);
                    }
                }
            }
        }


        public static void addMessage(TGMessage msg, int lifetime = 30)
        {
            var qry = $"INSERT INTO `cleanup`(`group`,`mid`,`when`,`life`) VALUES({msg.chat.id},{msg.message_id},{Helpers.getUnixTime()},{lifetime})";
            int ra = 0;
            SQL.NonQuery(qry, out ra);
        }
    }
}
