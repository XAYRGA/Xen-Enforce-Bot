using System;
using System.Collections.Generic;
using System.Text;
using NLua;

namespace XenfbotDN.LStateLibaries
{
    public static class LuaUtil
    {
        public static LuaTable EmptyTable(Lua state)
        {
            return (LuaTable)state.DoString("return {}")[0];
        }

        public static LuaTable stringArrayToTable(string[] asd, Lua state)
        {
            LuaTable what = EmptyTable(state);
            for (int i = 1; i < asd.Length + 1; i++)
            {
                what[i] = asd[i - 1];
            }
            return what;
        }

    }
}
