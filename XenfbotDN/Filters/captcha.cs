using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XenfbotDN.Filters
{
    public static class captcha
    {
        public static string FilterName;
        public static string FilterAuthor;
        public static string FilterVersion;


        public static async Task<bool> Execute(TGMessage message,TGUser ncm, GroupConfigurationObject gco, string langcode)
        {
            var query = "SELECT * FROM xenf_activations WHERE `group`={0} AND `forwho`={1}";
            return false;
        }
    }
}
