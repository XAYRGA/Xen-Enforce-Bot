using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XenfbotDN.Filters
{
    public static class botscreen
    {
        public static string FilterName;
        public static string FilterAuthor;
        public static string FilterVersion;


        public static async Task<bool> Execute(TGMessage message, TGUser ncm, GroupConfigurationObject gco, string langcode)
        {
            if (gco.useBotScreen!=true)   // Skipping because disabled.
                return true;           

            var query = "SELECT * FROM xenf_autokick WHERE `group`={0} AND `user`={1}";
            var fQry = string.Format(query, message.chat.id, ncm.id);
            var w = await SQL.QueryAsync(fQry);
            if (w.Count > 0)
            {
                return true;
            }

            var UserScore = 500;
            if (ncm.username != null)
                UserScore -= 50;

            



            return true;
        }
    }
}
