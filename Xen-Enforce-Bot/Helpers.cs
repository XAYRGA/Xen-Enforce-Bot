using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace XenfbotDN
{
    static class Helpers
    {
        public static int getUnixTime()
        {
            return (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public static void writeOut(string tag, string msg, params object[] data)
        {
            var w = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write(tag);
            Console.ForegroundColor = w;
            Console.Write(": ");
            Console.WriteLine(msg, data);
        }

        public static string getMentionName(TGUser usr)
        {
            var name = usr.username;
            if (name == null)
            {
                name = usr.first_name;
                if (usr.last_name != null)
                {
                    name += " " + usr.last_name;
                }
            }
            else { name = "@" + name; }

            return name;
        }
        public static string getMentionName(TGChatMember usr)
        {
            if (usr==null)
            {
                return "null";
            }
            var name = usr.user.username;
            if (name == null)
            {
                name = usr.user.first_name;
                if (usr.user.last_name != null)
                {
                    name += " " + usr.user.last_name;
                }
            }
            else { name = "@" + name; }

            return name;
        }
        public static string quickFormat(ref string text, string what, string with)
        {
            return text.Replace(what, with);
        }


        public static void warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static string getStack()
        {
            return Environment.StackTrace;
        }
        public static string writeStack(string data)
        {
            var q = Guid.NewGuid();
            var gid = q.ToString();

            if (!Directory.Exists("exfs"))
            {
                Directory.CreateDirectory("exfs");
            }
            File.WriteAllText("exfs/" + gid + ".stk", data);

            return gid;
        }
        //https://stackoverflow.com/questions/11743160/how-do-i-encode-and-decode-a-base64-string
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }

}