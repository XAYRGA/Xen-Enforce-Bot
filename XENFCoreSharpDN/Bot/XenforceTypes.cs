using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XENFCoreSharp.Bot
{
   public class XenforceGroupConfiguration
    {
        public long group;
        public int kicktime = 30;
        public string message = "!";
        public int autoban = 1;
        public int muteuntilverified = 0;
        public int announcekicks = 1;
        public int activationmode = 1;
    }
}
