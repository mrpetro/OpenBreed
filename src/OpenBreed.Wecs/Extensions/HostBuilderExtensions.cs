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
using OpenBreed.Scripting.Abstractions;
using OpenBreed.Wecs.Components;
using OpenBreed.Wecs.Components.Xml;
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
        #region Public Methods

        public static void SetupWecsBase(this IHostBuilder hostBuilder)
        {
            hostBuilder.SetupWecsManagers();
            hostBuilder.SetupWecsSystemFactory();
            hostBuilder.SetupWecsEntityFactory();
            hostBuilder.SetupWecsXmlEntityTemplateLoader();
            hostBuilder.SetupWecsComponents();
        }

        public static void SetupWecsAssemblySystems(this IHostBuilder hostBuilder)
        {
            var callingAssembly = Assembly.GetCallingAssembly();

            var systemTypes = callingAssembly.GetInterfaces<ISystem>().ToArray();

            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                foreach (var systemType in systemTypes)
                {
                    services.AddScoped(systemType, (sp) =>
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

        #endregion Public Methods

        #region Internal Methods

        internal static void SetupWecsComponents(this IHostBuilder hostBuilder)
        {
            XmlComponentsList.RegisterAllAssemblyComponentTypes();
        }

        internal static void SetupWecsXmlEntityTemplateLoader(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<IEntityTemplateLoader>((sp) => new XmlEntityTemplateLoader(sp.GetService<IOptions<XmlEntityTemplateLoaderSettings>>()));
            });
        }

        internal static void SetupWecsEntityFactory(this IHostBuilder hostBuilder, Action<IEntityFactory, IServiceProvider> action = null)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<IEntityFactory, EntityFactory>()
                .AddScoped((sp) => new Lazy<IEntityFactory>(() => sp.GetRequiredService<IEntityFactory>()));
            });
        }

        internal static void SetupWecsSystemFactory(this IHostBuilder hostBuilder)
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

        internal static void SetupEntityClasses(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<IEntityClassMan>((sp) =>
                {
                    var classesLoader = sp.GetRequiredService<IEntityClassesLoader>();

                    var classMan = new EntityClassMan();

                    classesLoader.Load((name, parentName) => classMan.CreateClass(name, parentName));

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

        internal static void SetupWecsManagers(this IHostBuilder hostBuilder)
        {
            hostBuilder.SetupEntityClasses();
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<IWecsCore, WecsCore>();
                services.AddScoped<EntityMan>();
                services.AddScoped<IEntityMan>((sp) => sp.GetRequiredService<EntityMan>());

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
                services.AddScoped(sp => new Lazy<IWorldMan>(() => sp.GetRequiredService<IWorldMan>()));
                services.AddScoped<IComponentsMan, ComponentsMan>();
                services.AddScoped<ISystemFinder, SystemFinder>();
                services.AddTransient<WorldBuilder>();
                services.AddSingleton<ISystemInitializer, DefaultSystemInitializer>();
                services.AddScoped<IEntityTriggerMan, EntityTriggerMan>();
                services.AddSingleton<ISystemInitializer, ActionOnTriggerSystemInitializer>();
            });
        }

        #endregion Internal Methods
    }
}