using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Animation.Generic;
using OpenBreed.Animation.Generic.Extensions;
using OpenBreed.Animation.Interface;
using OpenBreed.Common.Extensions;
using OpenBreed.Common.Game.Wecs.Services;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Interface.Extensions;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Wecs.Components.Animation.Extensions;
using OpenBreed.Wecs.Components.Audio.Extensions;
using OpenBreed.Wecs.Components.Common.Extensions;
using OpenBreed.Wecs.Components.Gui.Extensions;
using OpenBreed.Wecs.Components.Physics.Extensions;
using OpenBreed.Wecs.Components.Rendering.Extensions;
using OpenBreed.Wecs.Components.Scripting.Extensions;
using OpenBreed.Wecs.Components.Xml;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Wecs.Systems.Audio.Extensions;
using OpenBreed.Wecs.Systems.Control.Extensions;
using OpenBreed.Wecs.Systems.Core.Extensions;
using OpenBreed.Wecs.Systems.Gui.Extensions;
using OpenBreed.Wecs.Systems.Physics.Abstractions;
using OpenBreed.Wecs.Systems.Physics.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Systems.Scripting.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game.Wecs.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupCommonGameWecsServices(this IHostBuilder hostBuilder, bool isEditor)
        {
            hostBuilder.SetupClipMan<IEntity>();

            hostBuilder.SetupFrameUpdaterMan<IEntity>((frameUpdaterMan, sp) =>
            {
                new SpriteComponentAnimator(frameUpdaterMan, sp.GetService<ISpriteMan>(), sp.GetRequiredService<IDataLoaderFactory>());
            });

            hostBuilder.SetupCollisionVisualizingOptions();

            hostBuilder.SetupRenderingSystems();
            hostBuilder.SetupScriptingSystems();
            hostBuilder.SetupAudioSystems();
            hostBuilder.SetupPhysicsSystems();
            hostBuilder.SetupCoreSystems();
            hostBuilder.SetupControlSystems();
            hostBuilder.SetupAnimationSystems();
            hostBuilder.ConfigureGuiSystems(isEditor);
            hostBuilder.SetupGameSystems();
            hostBuilder.SetupWecsSystemFactory();

            hostBuilder.SetupWecsCommonComponents();
            hostBuilder.SetupWecsPhysicsComponents();
            hostBuilder.SetupWecsRenderingComponents();
            hostBuilder.SetupWecsAnimationComponents();
            hostBuilder.SetupWecsAudioComponents();
            hostBuilder.SetupWecsFsmComponents();
            hostBuilder.SetupWecsScriptingComponents();
            hostBuilder.SetupWecsGuiComponents();
            hostBuilder.SetupWecsGameCommonComponents();
            hostBuilder.SetupWecsComponentFactoryProvider();

            hostBuilder.SetupWecsXmlEntityTemplateLoader();

            hostBuilder.SetupWecsEntityFactory();

            hostBuilder.SetupWecsManagers();

            hostBuilder.SetupBuilderFactory((builderFactory, sp) =>
            {
                builderFactory.SetupWecsPhysicsBuilders(sp);
                builderFactory.SetupWecsRenderingBuilders(sp);
                builderFactory.SetupWecsAnimationBuilders(sp);
                builderFactory.SetupWecsCommonBuilders(sp);
            });

            hostBuilder.SetupGameServices();
        }


        public static void SetupGameSystems(this IHostBuilder hostBuilder)
        {
            hostBuilder.SetupWecsAssemblySystems();
        }

        public static void SetupGameServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<IGameServices, GameServices>();
            });
        }

        #endregion Public Methods

        #region Private Methods

        private static void SetupWecsGameCommonComponents(this IHostBuilder hostBuilder)
        {
            XmlComponentsList.RegisterAllAssemblyComponentTypes();
            hostBuilder.SetupWecsAssemblyComponentFactories();
        }

        #endregion Private Methods
    }
}