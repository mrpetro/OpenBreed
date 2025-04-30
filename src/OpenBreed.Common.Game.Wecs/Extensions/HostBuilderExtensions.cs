using Microsoft.Extensions.Hosting;
using OpenBreed.Wecs.Components.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Wecs.Components.Animation.Extensions;
using OpenBreed.Wecs.Components.Audio.Extensions;
using OpenBreed.Wecs.Components.Common.Extensions;
using OpenBreed.Wecs.Components.Gui.Extensions;
using OpenBreed.Wecs.Components.Physics.Extensions;
using OpenBreed.Wecs.Components.Rendering.Extensions;
using OpenBreed.Wecs.Components.Scripting.Extensions;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Wecs.Systems.Audio.Extensions;
using OpenBreed.Wecs.Systems.Control.Extensions;
using OpenBreed.Wecs.Systems.Core.Extensions;
using OpenBreed.Wecs.Systems.Gui.Extensions;
using OpenBreed.Wecs.Systems.Physics.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Systems.Scripting.Extensions;
using OpenBreed.Common.Extensions;
using OpenBreed.Wecs.Entities;
using OpenBreed.Animation.Generic.Extensions;
using OpenBreed.Rendering.Abstractions.Managers;
using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Common.Interface;

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

            hostBuilder.SetupWecsSystemFactory((systemFactory, sp) =>
            {
                systemFactory.SetupRenderingSystems(sp);
                systemFactory.SetupScriptingSystems(sp);
                systemFactory.SetupAudioSystems(sp);
                systemFactory.SetupPhysicsSystems(sp);
                systemFactory.SetupCoreSystems(sp);
                systemFactory.SetupControlSystems(sp);
                systemFactory.SetupAnimationSystems(sp);
                systemFactory.ConfigureGuiSystems(sp, isEditor);
                systemFactory.SetupGameSystems(sp);
            });

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