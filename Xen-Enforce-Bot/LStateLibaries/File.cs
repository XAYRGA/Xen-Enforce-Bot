using System;
using System.Collections.Generic;
using System.Text;

using NLua;
namespace XenfbotDN.LStateLibaries
{
    public static class File
    {
        private static Lua State;

        public static string Read(string path)
        {
            string ret = null;
            try
            {
                ret = System.IO.File.ReadAllText(path);
            }
            catch
            {

            }
            return ret;
        }

        public static bool Exists(string path)
        {
            return System.IO.File.Exists(path);
        }

        public static bool DirectoryExists(string path)
        {
            return System.IO.Directory.Exists(path);
        }

        public static void Write(string path, string content)
        {
            System.IO.File.WriteAllText(path, content);
        }

        public static LuaTable Find(string path, string pattern)
        {
            try
            {
                return LuaUtil.stringArrayToTable(System.IO.Directory.GetFiles(path, pattern), State);
            }
            catch
            {
                return LuaUtil.EmptyTable(State);
            }
        }


        public static void Setup(Lua state)
        {
            State = state;
            state.DoString(" file = {}");
            state.RegisterFunction("file.Read", null, typeof(File).GetMethod("Read"));
            state.RegisterFunction("file.Exists", null, typeof(File).GetMethod("Exists"));
            state.RegisterFunction("file.Write", null, typeof(File).GetMethod("Write"));
            state.RegisterFunction("file.DirectoryExists", null, typeof(File).GetMethod("DirectoryExists"));
            state.RegisterFunction("file.Find", null, typeof(File).GetMethod("Find"));
        }
    }
}
