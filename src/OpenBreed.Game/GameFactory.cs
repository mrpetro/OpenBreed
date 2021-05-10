using OpenBreed.Animation.Generic;
using OpenBreed.Animation.Generic.Extensions;
using OpenBreed.Animation.Interface;
using OpenBreed.Audio.OpenAL.Extensions;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Database.Xml.Extensions;
using OpenBreed.Common.Extensions;
using OpenBreed.Common.Formats;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Xml;
using OpenBreed.Fsm;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Game.Extensions;
using OpenBreed.Input.Generic;
using OpenBreed.Input.Generic.Extensions;
using OpenBreed.Input.Interface;
using OpenBreed.Physics.Generic;
using OpenBreed.Physics.Generic.Extensions;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL;
using OpenBreed.Rendering.OpenGL.Extensions;
using OpenBreed.Scripting.Interface;
using OpenBreed.Scripting.Lua;
using OpenBreed.Scripting.Lua.Extensions;
using OpenBreed.Wecs.Components.Animation.Extensions;
using OpenBreed.Wecs.Components.Common.Extensions;
using OpenBreed.Wecs.Components.Physics.Extensions;
using OpenBreed.Wecs.Components.Rendering.Extensions;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Wecs.Systems.Control.Extensions;
using OpenBreed.Wecs.Systems.Core.Extensions;
using OpenBreed.Wecs.Systems.Gui.Extensions;
using OpenBreed.Wecs.Systems.Physics.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game
{
    public class GameFactory : CoreFactory
    {
        public GameFactory()
        {

            manCollection.AddSingleton<IViewClient>(() => new OpenTKWindowClient(800, 600, "OpenBreed"));

            manCollection.SetupVariableManager();
            manCollection.SetupABFormats();
            manCollection.SetupModelProvider();
            manCollection.SetupAnimationManagers();
            manCollection.SetupLuaScripting();
            manCollection.SetupDataProviders();
            manCollection.SetupGenericInputManagers();
            manCollection.SetupGenericPhysicsManagers();
            manCollection.SetupOpenALManagers();
            manCollection.SetupOpenGLManagers();
            manCollection.SetupWecsManagers();
            manCollection.SetupRenderingSystems();
            manCollection.SetupPhysicsSystems();
            manCollection.SetupCoreSystems();
            manCollection.SetupControlSystems();
            manCollection.SetupAnimationSystems();
            manCollection.SetupGuiSystems();
            manCollection.SetupXmlReadonlyDatabase();
            //manCollection.SetupAudioSystems();


            manCollection.SetupCommonComponents();
            manCollection.SetupPhysicsComponents();
            manCollection.SetupRenderingComponents();
            manCollection.SetupAnimationComponents();
            manCollection.SetupFsmComponents();


            //manCollection.AddSingleton<ScreenWorldHelper>(() => new ScreenWorldHelper(core, manCollection.GetManager<ICommandsMan>(),
            //                                                                                manCollection.GetManager<IRenderingMan>(),
            //                                                                                manCollection.GetManager<IWorldMan>(),
            //                                                                                manCollection.GetManager<IEventsMan>(),
            //                                                                                manCollection.GetManager<ViewportCreator>(),
            //                                                                                manCollection.GetManager<IViewClient>()));
        }

        public ICore CreateGame(string gameDbFilePath, string gameFolderPath)
        {
            var variables = manCollection.GetManager<IVariableMan>();

            variables.RegisterVariable(typeof(string), gameFolderPath, "Cfg.Options.ABTA.GameFolderPath");

            manCollection.AddSingleton<IRepositoryProvider>(() => new XmlReadonlyDatabaseMan(variables, gameDbFilePath));

            manCollection.SetupGameScriptingApi();

            return new Game(manCollection);
        }
    }
}
