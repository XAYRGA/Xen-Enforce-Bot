using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XENFCoreSharp.DoubleDecker;
using MySql.Data;
using MySql;
using MySql.Data.MySqlClient;
using System.Threading;


namespace XENFCoreSharp.Bot
{
    public static partial class XenforceRoot
    {
        public static int getGroupConfigurationValue(TGChat chat, string value, int def)
        {
            SQLQueryInstance QueryInst;
            MySqlDataReader cur;
            var vname = SQL.escape(value);
            var ok = SQL.Query(string.Format("SELECT `{1}` FROM xenf_groupconfigs WHERE `group`={0}", chat.id,vname), out QueryInst);
            if (!ok)
            {
                Console.WriteLine("[!] Error reading chat information for {0} -- {1}", chat.id, SQL.getLastError());
                Console.WriteLine(Environment.StackTrace);
                if (QueryInst!=null )
                {
                    QueryInst.Finish();
                }
                return def;
            }
            else
            {
                cur = QueryInst.reader;
                if (cur.HasRows)
                {
                    cur.Read();
                    var b = (int)cur[value];
                    QueryInst.Finish();
                    return b;
                }
                else
                {
                    QueryInst.Finish();
                    InitializeGroupConfiguration(chat);
                    return def;
                }
            }
        }

        public static bool getGroupConfigurationValue(TGChat chat, string value, bool def)
        {
            SQLQueryInstance QueryInst;
            MySqlDataReader cur;
            var vname = SQL.escape(value);
            var ok = SQL.Query(string.Format("SELECT `{1}` FROM xenf_groupconfigs WHERE `group`={0}", chat.id, vname), out QueryInst);
            if (!ok)
            {
                Console.WriteLine("[!] Error reading chat information for {0} -- {1}", chat.id, SQL.getLastError());
                Console.WriteLine(Environment.StackTrace);
                if (QueryInst != null)
                {
                    QueryInst.Finish();
                }
                return def;
            }
            else
            {
                cur = QueryInst.reader;
                if (cur.HasRows)
                {
                    cur.Read();
                    var b = (int)cur[value] > 0;
                    QueryInst.Finish();
                    return b;
                }
                else
                {
                    QueryInst.Finish();
                    InitializeGroupConfiguration(chat);
                    return def;
                }
            }
        }

        public static long getGroupConfigurationValue(TGChat chat, string value, long def)
        {
            SQLQueryInstance QueryInst; 
            MySqlDataReader cur;
            var vname = SQL.escape(value);
            var ok = SQL.Query(string.Format("SELECT `{1}` FROM xenf_groupconfigs WHERE `group`={0}", chat.id, vname), out QueryInst);
            if (!ok)
            {
                Console.WriteLine("[!] Error reading chat information for {0} -- {1}", chat.id, SQL.getLastError());
                Console.WriteLine(Environment.StackTrace);
                if (QueryInst != null)
                {
                    QueryInst.Finish();
                }
                return def;
            }
            else
            {
                cur = QueryInst.reader;
                if (cur.HasRows)
                {
                    cur.Read();
                    var b = (long)cur[value];
                    QueryInst.Finish();
                    return b;
                }
                else
                {

                    QueryInst.Finish();
                    
                    InitializeGroupConfiguration(chat);
                    return def;
                }
            }
        }

        public static string getGroupConfigurationValue(TGChat chat, string value, string def)
        {
            SQLQueryInstance QueryInst;
            MySqlDataReader cur;
            var vname = SQL.escape(value);
            var ok = SQL.Query(string.Format("SELECT `{1}` FROM xenf_groupconfigs WHERE `group`={0}", chat.id, vname), out QueryInst);
            if (!ok)
            {
                Console.WriteLine("[!] Error reading chat information for {0} -- {1}", chat.id, SQL.getLastError());
                Console.WriteLine(Environment.StackTrace);
                if (QueryInst != null)
                {
                    QueryInst.Finish();
                }
                return def;
            }
            else
            {
                cur = QueryInst.reader;
                if (cur.HasRows)
                {
                    cur.Read();
                    var b = (string)cur[value];
                    QueryInst.Finish();                    
                    return b;
                }
                else
                {
                    QueryInst.Finish();
                    InitializeGroupConfiguration(chat);
                    return def;
                }
            }
        }

        public static bool InitializeGroupConfiguration(TGChat chat)
        {
            int rowsAff = 0;
            var success = SQL.NonQuery(string.Format("INSERT INTO `xenf_groupconfigs` (`group`) VALUES({0})", chat.id), out rowsAff);
            if (!success)
            {
                Console.WriteLine("[!] Can't write configuration for {0} -- {1}", chat.id, SQL.getLastError());
                Console.WriteLine(Environment.StackTrace);
                return false;
            }
            Console.WriteLine("Wrote new configuration index for group {0}", chat.id);
            return true;
        }


        public static XenforceGroupConfiguration getGroupConfiguration(TGChat chat)
        {
            SQLQueryInstance QueryInst;
            MySqlDataReader cur;
            var ok = SQL.Query(string.Format("SELECT * FROM xenf_groupconfigs WHERE `group`={0}", chat.id), out QueryInst);
            if (!ok)
            {
                Console.WriteLine("[!] Error reading chat information for {0} -- {1}", chat.id, SQL.getLastError());
                Console.WriteLine(Environment.StackTrace);
                return null;
            }
            else
            {
                cur = QueryInst.reader;
                if (cur.HasRows)
                {
                    cur.Read();
                    XenforceGroupConfiguration b = new XenforceGroupConfiguration()
                    {
                        group = (long)cur["group"],
                        kicktime = (int)cur["kicktime"],
                        message = (string)cur["message"],
                        autoban = (int)cur["autoban"],
                        muteuntilverified = (int)cur["muteuntilverified"],
                        announcekicks = (int)cur["announcekicks"],
                        activationmode = ((int)cur["activationmode"])
                    };

                    QueryInst.Finish();
                    return b;
                }
                else
                {
                    QueryInst.Finish();
                    InitializeGroupConfiguration(chat);
                    return new XenforceGroupConfiguration();
                }

            }
        }

   
        public static bool writeGroupConfiguration(TGChat chat,string index, bool value)
        {
            var query = "UPDATE xenf_groupconfigs SET `{0}`={1} WHERE `group`={2}";
            var ivar = 0;
            if (value != false)
                ivar = 1;

            int ra = 0;
            var success = SQL.NonQuery(string.Format(query, SQL.escape(index), ivar, chat.id),out ra );
            if (!success || ra==0)
            {
                return false;
            }
            return true;
        }

        public static bool writeGroupConfiguration(TGChat chat, string index, int value)
        {
            var query = "UPDATE xenf_groupconfigs SET `{0}`={1} WHERE `group`={2}";
  
            int ra = 0;

            var success = SQL.NonQuery(string.Format(query, SQL.escape(index), value, chat.id), out ra);

            if (!success || ra == 0)
            {
                return false;
            }
           
            return true;
        }

        public static bool writeGroupConfiguration(TGChat chat, string index, string value)
        {
            var query = "UPDATE xenf_groupconfigs SET `{0}`='{1}' WHERE `group`={2}";

            int ra = 0;

            var success = SQL.NonQuery(string.Format(query, SQL.escape(index), SQL.escape(value), chat.id), out ra);

            if (!success || ra == 0)
            {
                return false;
            }
            return true;
        }
    }
}
