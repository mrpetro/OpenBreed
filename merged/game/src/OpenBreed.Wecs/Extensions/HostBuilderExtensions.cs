using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenBreed.Common;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Components;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                services.AddSingleton<IWorldMan, WorldMan>((sp) =>
                {
                    var entityMan = sp.GetRequiredService<IEntityMan>();

                    var worldMan = new WorldMan(
                        sp.GetRequiredService<IEventsMan>(),
                        sp.GetRequiredService<ISystemFactory>(),
                        sp.GetRequiredService<IEntityToSystemMatcher>(),
                        sp.GetRequiredService<ILogger>());

                    entityMan.ComponentAdded += (entity, componentType) => worldMan.RequestUpdateEntity(entity);
                    entityMan.ComponentRemoved += (entity, componentType) => worldMan.RequestUpdateEntity(entity);

                    return worldMan;
                });

                services.AddSingleton<ISystemFinder, SystemFinder>();
                services.AddTransient<WorldBuilder>();
            });
        }

        public static void SetupSystemFactory(this IHostBuilder hostBuilder, Action<ISystemFactory, IServiceProvider> action)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ISystemFactory>((sp) =>
                {
                    var systemFactory = new DefaultSystemFactory(
                        sp.GetRequiredService<ISystemRequirementsProvider>());
                    action.Invoke(systemFactory, sp);
                    return systemFactory;
                });
            });
        }

        public static void SetupDefaultSystemRequirementsProvider(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ISystemRequirementsProvider, DefaultSystemRequirementsProvider>();
            });
        }

        public static void SetupDefaultEntityToSystemMatcher(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IEntityToSystemMatcher, DefaultEntityToSystemMatcher>();
            });
        }

        public static void SetupComponentFactoryProvider(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IComponentFactoryProvider>((sp) => new ComponentFactoryProvider(services, sp));
            });
        }

        public static void SetupAssemblyComponentFactories(this IHostBuilder hostBuilder)
        {
            var callingAssembly = Assembly.GetCallingAssembly();

            var componentFactoryServiceTypes = new List<Type>();

            foreach (var type in callingAssembly
                .DefinedTypes
                .Where(type => type.ImplementedInterfaces.Any(item => item == typeof(IComponentFactory))))
            {
                componentFactoryServiceTypes.Add(type);
            }

            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                foreach (var type in componentFactoryServiceTypes)
                {
                    services.AddSingleton(type);
                }
            });
        }

        public static void SetupEntityFactory(this IHostBuilder hostBuilder, Action<IEntityFactory, IServiceProvider> action)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IEntityFactory>((sp) =>
                {
                    var entityFactory = new EntityFactory(
                        sp.GetService<IEntityMan>(),
                        sp.GetService<IComponentFactoryProvider>(),
                        sp.GetService<IEntityTemplateLoader>());
                    action.Invoke(entityFactory, sp);
                    return entityFactory;
                });
            });
        }

        public static void SetupXmlEntityTemplateLoader(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IEntityTemplateLoader>((sp) => new XmlEntityTemplateLoader(sp.GetService<IOptions<XmlEntityTemplateLoaderSettings>>()));
            });
        }
    }
}
