using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data;

namespace XenfbotDN
{


    public class GroupConfigurationObject
    {
        public long groupID;
        public string langCode;

        public int kickTime;

        public bool useNameFilter;
        public bool useBotScreen;
        public int botScreenSensitivity;

        public bool kickMediaBeforeCaptcha;
        public bool muteUntilVerified;
        public bool annouceKicks; 

        public string message;
        public string activationMessage;

        public bool invalidated = false;

        public GroupConfigurationObject(long id,DataRow data)
        {
            groupID = id;

            kickTime = (int)data["kickTime"];
            useNameFilter = ((int)data["useNameFilter"]) > 0;
            useBotScreen = ((int)data["useBotScreen"]) > 0;
            kickMediaBeforeCaptcha = ((int)data["kickMediaBeforeCaptcha"]) > 0;
            muteUntilVerified = ((int)data["muteUntilVerified"]) > 0;
            annouceKicks = ((int)data["announceKicks"]) > 0;
            message = (string)data["message"];
            activationMessage = (string)data["activationMessage"];
            langCode = (string)data["langCode"];
        }

        public GroupConfigurationObject()
        {

        }

        public async Task<bool> setString(string key, string val)
        {
            var qry = "UPDATE xenf_groupconfigs SET `{0}`='{1}' WHERE `group`={2}";
            var wescape = SQL.escape(val);
            qry = string.Format(qry, key, wescape,groupID);
            var ok = await SQL.NonQueryAsync(qry);
            if (!ok)
            {
                return false;
            }
            invalidated = true;
            return true;
        }


        public async Task<bool> setNumber(string key, int val)
        {
            var qry = "UPDATE xenf_groupconfigs SET `{0}`={1} WHERE `group`={2}";
            qry = string.Format(qry, key, val,groupID);
            var ok = await SQL.NonQueryAsync(qry);
            if (!ok)
            {
                return false;
            }
            invalidated = true;
            return true;
        }
    }


    public static class GroupConfiguration
    {
        const string tag = "xenfbot@grpconfig";
        static Dictionary<long, GroupConfigurationObject> configCache = new Dictionary<long, GroupConfigurationObject>();
        static Dictionary<long, int> groupConfig = new Dictionary<long, int>();

        public static string[] writeFields = new string[]
        {
                "kickTime",
                "useNameFilter",
                "useBotScreen",
                "kickMediaBeforeCaptcha",
                "muteUntilVerified",
                "announceKicks",
                "message",
                "activationMessage"
        };


        public static async Task<GroupConfigurationObject> loadObject(long groupID, bool force = false)
        {
            if (!force)
            {
                GroupConfigurationObject gco;
                var w = configCache.TryGetValue(groupID, out gco);
                if (w)
                {
                    if (!gco.invalidated)
                    {
                        return gco;
                    }
                }
            }
            var dr = await SQL.QueryAsync("SELECT * FROM xenf_groupconfigs WHERE `group`=" + groupID);
            if (dr==null)
            {
                Console.WriteLine(SQL.getLastError());
            }
            if (dr.Count > 0)
            {
                var my = new GroupConfigurationObject(groupID,dr[0]);
                configCache[groupID] = my;
                return my;
            } else
            {
                return null;
            } 

        }

        public static GroupConfigurationObject initializeObject(long groupID)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            SQL.NonQueryAsync(string.Format("INSERT INTO xenf_groupconfigs(`group`) VALUES ({0})", groupID)).Wait();

#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Helpers.writeOut(tag, "Pushing new group configuration index {0}", groupID);
            var w = new GroupConfigurationObject();
            w.groupID = groupID;
            return w;
        }
    }

}
