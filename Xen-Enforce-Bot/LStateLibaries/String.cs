using System;
using System.Collections.Generic;
using System.Text;

using NLua;
namespace XenfbotDN.LStateLibaries
{
    public static class LuaString
    {
        private static Lua State;

        public static string[] Split(string data,string separator)
        {
            return data.Split(separator);
        }

 
        public static void Setup(Lua state)
        {
            State = state;
            
            state.RegisterFunction("string.split", null, typeof(LuaString).GetMethod("Split"));

        }
    }
}
