using System;
using System.Collections.Generic;
using System.Text;

namespace XenfbotDN
{
    public static class Removals
    {
        public static void addIncident(TGUser user, TGChat chat, string reason)
        {
            int ra = 0;
            SQL.NonQuery($"INSERT INTO `removals`(`user`,`group`,`cause`,`when`) VALUES({user.id},{chat.id},'{SQL.escape(reason)},{Helpers.getUnixTime()}", out ra);
        }
    }
}
