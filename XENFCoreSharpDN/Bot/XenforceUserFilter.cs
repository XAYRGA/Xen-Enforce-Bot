using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XENFCoreSharp.DoubleDecker;

namespace XENFCoreSharp.Bot
{
    static class XenforceUserFilter
    {

        
        public static void doFilterUser(TGMessage msg, TGUser user)
        {
            if (!user.is_bot)
            {
                try
                {
                    bool b = false;
                    b = Filters.XESFilter.autorem(msg, user);
                    if (b == true) { return; }
                    b = Filters.XESFilter.namefilter(msg, user);
                    if (b == true) { return; }
                    b = Filters.XESFilter.captcha(msg, user);
                    if (b == true) { return; }
                } catch (Exception E)
                {
                    Console.WriteLine("Fucking seriously?\n{0}", E.ToString());
                }
            }
        }        
    }
}
