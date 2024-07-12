using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Database.Interface;
using OpenBreed.Scripting.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Scripting.Lua.Extensions
{
    public static class HostBuilderExtensions
    {
        public static void SetupLuaScripting(this IHostBuilder hostBuilder, Action<LuaScriptMan, IServiceProvider> action)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IScriptMan, LuaScriptMan>((sp) =>
                {
                    var scriptMan = new LuaScriptMan(sp.GetService<ILogger>());
                    action.Invoke(scriptMan, sp);
                    return scriptMan;
                });
            });
        }

        public static void SetupScriptDataLoader(this DataLoaderFactory dataLoaderFactory, IServiceProvider managerCollection)
        {
            dataLoaderFactory.Register<IScriptDataLoader>(() => new ScriptDataLoader(managerCollection.GetService<IRepositoryProvider>(),
                                                                                     managerCollection.GetService<ScriptsDataProvider>(),
                                                                                     managerCollection.GetService<IScriptMan>()));
        }
    }
}
