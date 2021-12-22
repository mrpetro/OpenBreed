using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Extensions
{
    public static class HostBuilderExtensions
    {
        public static void SetupWecsManagers(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IEntityMan, EntityMan>();
                services.AddSingleton<IEntityFactory, EntityFactory>();
                services.AddSingleton<IWorldMan, WorldMan>();
                services.AddSingleton<ISystemFinder, SystemFinder>();
                services.AddSingleton<WorldBuilder, WorldBuilder>();
            });
        }

        public static void SetupSystemFactory(this IHostBuilder hostBuilder, Action<ISystemFactory, IServiceProvider> action)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ISystemFactory>((sp) =>
                {
                    var systemFactory = new DefaultSystemFactory();
                    action.Invoke(systemFactory, sp);
                    return systemFactory;
                });
            });
        }
    }
}
