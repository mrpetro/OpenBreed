using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Scripting.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Scripting.Lua.Extensions
{
    public static class ManagerCollectionExtensions
    {
        public static void SetupLuaScripting(this IManagerCollection manCollection)
        {
            manCollection.AddSingleton<IScriptMan>(() => new LuaScriptMan(manCollection.GetManager<ILogger>()));
        }
    }
}
