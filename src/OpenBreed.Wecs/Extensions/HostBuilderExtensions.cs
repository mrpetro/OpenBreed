using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenBreed.Common;
using OpenBreed.Common.Interface.Extensions;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Components;
using OpenBreed.Wecs.Physics.Systems;
using OpenBreed.Wecs.Services;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Extensions
{
    public static class HostBuilderExtensions
    {
        public static void SetupWecsManagers(this IHostBuilder hostBuilder)
        {
            hostBuilder.SetupEntityClasses();
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<IEntityMan, EntityMan>();
                services.AddScoped<IWorldMan, WorldMan>((sp) =>
                {
                    var entityMan = sp.GetRequiredService<IEntityMan>();

                    var worldMan = new WorldMan(
                        sp.GetRequiredService<IEventsMan>(),
                        sp.GetRequiredService<ISystemFactory>(),
                        sp.GetRequiredService<IEntityToSystemMatcher>(),
                        sp.GetRequiredService<IEventSystemManager>(),
                        sp.GetRequiredService<ILogger>());

                    entityMan.ComponentAdded += (entity, componentType) => worldMan.RequestUpdateEntity(entity);
                    entityMan.ComponentRemoved += (entity, componentType) => worldMan.RequestUpdateEntity(entity);

                    return worldMan;
                });

                services.AddScoped<ISystemFinder, SystemFinder>();
                services.AddTransient<WorldBuilder>();
                services.AddSingleton<ISystemInitializer, DefaultSystemInitializer>();
                services.AddScoped<IEntityTriggerMan, EntityTriggerMan>();
                services.AddSingleton<ISystemInitializer, ActionOnTriggerSystemInitializer>();
            });
        }


        public static void SetupEntityClasses(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<IEntityClassMan>((sp) =>
                {
                    var classMan = new EntityClassMan();

                    classMan.CreateClass("Entity");
                    classMan.CreateClass("Actor", "Entity");

                    return classMan;
                });
                services.AddScoped<IEntityToSystemMatcher, DefaultEntityToSystemMatcher>();
                services.AddScoped<IEventSystemManager, EventSystemManager>();

                services.AddScoped<ISystemFactory>((sp) =>
                {
                    var systemFactory = new DefaultSystemFactory(
                        sp,
                        sp.GetRequiredService<ISystemRequirementsProvider>());
                    return systemFactory;
                });
            });
        }

        public static void SetupWecsSystemFactory(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<ISystemRequirementsProvider, DefaultSystemRequirementsProvider>();
                services.AddScoped<IEntityToSystemMatcher, DefaultEntityToSystemMatcher>();
                services.AddScoped<IEventSystemManager, EventSystemManager>();

                services.AddScoped<ISystemFactory>((sp) =>
                {
                    var systemFactory = new DefaultSystemFactory(
                        sp,
                        sp.GetRequiredService<ISystemRequirementsProvider>());
                    return systemFactory;
                });
            });
        }

        public static void SetupWecsComponentFactoryProvider(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<IComponentFactoryProvider>((sp) => new ComponentFactoryProvider(services, sp));
            });
        }

        public static void SetupWecsAssemblySystems(this IHostBuilder hostBuilder)
        {
            var callingAssembly = Assembly.GetCallingAssembly();

            var systemTypes = callingAssembly.GetInterfaces<ISystem>().ToArray();

            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                foreach (var systemType in systemTypes)
                {
                    services.AddScoped(systemType,(sp) => 
                    {
                        var systemInitializers = sp.GetServices<ISystemInitializer>();

                        var system = (ISystem)ActivatorUtilities.CreateInstance(sp, systemType);

                        foreach (var systemInitializer in systemInitializers)
                        {
                            systemInitializer.Initialize(sp, system);
                        }

                        return system;
                    });
                }
            });
        }

        public static void SetupWecsAssemblyComponentFactories(this IHostBuilder hostBuilder)
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
                    services.AddScoped(type);
                }
            });
        }

        public static void SetupWecsEntityFactory(this IHostBuilder hostBuilder, Action<IEntityFactory, IServiceProvider> action = null)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<IEntityFactory>((sp) =>
                {
                    var entityFactory = new EntityFactory(
                        sp.GetService<IEntityMan>(),
                        sp.GetService<IComponentFactoryProvider>(),
                        sp.GetService<IEntityTemplateLoader>());

                    if (action is not null)
                    {
                        action.Invoke(entityFactory, sp);
                    }

                    return entityFactory;
                })
                .AddScoped((sp) => new Lazy<IEntityFactory>(() => sp.GetRequiredService<IEntityFactory>()));
            });
        }

        public static void SetupWecsXmlEntityTemplateLoader(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<IEntityTemplateLoader>((sp) => new XmlEntityTemplateLoader(sp.GetService<IOptions<XmlEntityTemplateLoaderSettings>>()));
            });
        }
    }
}
