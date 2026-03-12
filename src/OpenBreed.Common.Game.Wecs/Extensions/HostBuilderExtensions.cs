using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Generic;
using OpenBreed.Animation.Generic.Extensions;
using OpenBreed.Animation.Interface;
using OpenBreed.Common.Extensions;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Interface.Extensions;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Animation.Components.Extensions;
using OpenBreed.Wecs.Audio.Components.Extensions;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Gui.Components.Extensions;
using OpenBreed.Wecs.Physics.Components.Extensions;
using OpenBreed.Wecs.Rendering.Components.Extensions;
using OpenBreed.Wecs.Scripting.Components.Extensions;
using OpenBreed.Wecs.Components.Xml;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Services;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Animation.Systems.Extensions;
using OpenBreed.Wecs.Audio.Systems.Extensions;
using OpenBreed.Wecs.Control.Systems.Extensions;
using OpenBreed.Wecs.Core.Systems.Extensions;
using OpenBreed.Wecs.Gui.Systems.Extensions;
using OpenBreed.Wecs.Physics.Systems;
using OpenBreed.Wecs.Physics.Systems.Abstractions;
using OpenBreed.Wecs.Physics.Systems.Extensions;
using OpenBreed.Wecs.Rendering.Systems.Extensions;
using OpenBreed.Wecs.Scripting.Systems.Extensions;
using OpenBreed.Wecs.Worlds;
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

            hostBuilder.SetupWecsComponents();
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
        }

        public static void SetupGameSystems(this IHostBuilder hostBuilder)
        {
            hostBuilder.SetupWecsAssemblySystems();
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