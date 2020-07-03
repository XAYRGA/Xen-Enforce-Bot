using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Win32.SafeHandles;
using System.Text.RegularExpressions;

namespace XenfbotDN
{
    public static class GroupConfiguration
    {
        private static Dictionary<long, GroupConfigurationObject> cache = new Dictionary<long, GroupConfigurationObject>();

        public static GroupConfigurationObject queryObject(long group)
        {
            var data = SQL.Query($"SELECT * FROM `configs` WHERE groupid={group}");
            if (data.Count == 0)
                return null;
            return new GroupConfigurationObject(group,data[0]); 
        }

        public static GroupConfigurationObject getConfig(long group)
        {
            GroupConfigurationObject ret = null; 
            var ok = cache.TryGetValue(group, out ret); 
            if (ok)
            {
                if (ret.invalidated)
                {
                    ret = queryObject(group);
                    cache[group] = ret;
                    return ret;
                }
                return ret;
            }
            ret = queryObject(group); 
            if (ret!=null)
            {
                cache[group] = ret;
                return ret;
            }
            var ra = 0;
            ok = SQL.NonQuery($"INSERT INTO `configs` (groupid,language) VALUES({group},'en')", out ra);

            return new GroupConfigurationObject(group);
        }
    }

    public class GroupConfigurationObject
    {
        public bool invalidated = false;
        private DataRow data;
        public long groupID;

        public GroupConfigurationObject(long Gid,DataRow qData)
        {
            groupID = Gid; 
            data = qData;
        }
        public GroupConfigurationObject(long Gid) 
        {
            groupID = Gid;
        } // ugh god. 

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
            } catch { return null; }
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
        public bool modify(string col, string data)
        {
            invalidated = true;
            var qry = $"UPDATE `configs` SET `{SQL.escape(col)}`='{SQL.escape(data)}' WHERE `groupid`={groupID}"; // spooky ' 
            int ra = 0;
            return SQL.NonQuery(qry, out ra);
        }

        public bool modify(string col, int data)
        {
            invalidated = true;
            var qry = $"UPDATE `configs` SET `{SQL.escape(col)}`={SQL.escape(data.ToString())} WHERE `groupid`={groupID}";
            int ra = 0;
            return SQL.NonQuery(qry, out ra);
          
        }

        public bool modify(string col, bool data)
        {
            invalidated = true;
            var qry = $"UPDATE `configs` SET `{SQL.escape(col)}`={SQL.escape(data.ToString())} WHERE `groupid`={groupID}";
            int ra = 0;
            return SQL.NonQuery(qry, out ra);
        }
    }
}
