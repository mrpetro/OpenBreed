using OpenBreed.Animation.Generic;
using OpenBreed.Animation.Interface;
using OpenBreed.Audio.OpenAL.Extensions;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Extensions;
using OpenBreed.Common.Formats;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules.Audio;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Xml;
using OpenBreed.Fsm;
using OpenBreed.Input.Generic;
using OpenBreed.Input.Interface;
using OpenBreed.Physics.Generic;
using OpenBreed.Physics.Generic.Extensions;
using OpenBreed.Rendering.OpenGL;
using OpenBreed.Rendering.OpenGL.Extensions;
using OpenBreed.Scripting.Interface;
using OpenBreed.Scripting.Lua;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
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

            manCollection.AddSingleton<IVariableMan>(() => new VariableMan(manCollection.GetManager<ILogger>()));

            manCollection.AddSingleton<IScriptMan>(() => new LuaScriptMan(manCollection.GetManager<ILogger>()));

            manCollection.AddSingleton<IFsmMan>(() => new FsmMan());

            manCollection.AddSingleton<IAnimMan>(() => new AnimMan(manCollection.GetManager<ILogger>()));

            manCollection.AddSingleton<IInputsMan>(() => new InputsMan(manCollection.GetManager<ICore>()));

            manCollection.AddSingleton<IPlayersMan>(() => new PlayersMan(manCollection.GetManager<ILogger>(),
                                                                         manCollection.GetManager<IInputsMan>()));

            manCollection.AddSingleton<IEntityMan>(() => new EntityMan(manCollection.GetManager<ICore>(),
                                                              manCollection.GetManager<ICommandsMan>()));


            manCollection.AddSingleton<IEntityFactory>(() => new EntityFactory(manCollection.GetManager<IEntityMan>()));

            manCollection.AddSingleton<IWorldMan>(() => new WorldMan(manCollection.GetManager<ICore>()));

            manCollection.AddSingleton<ISystemFinder>(() => new SystemFinder(manCollection.GetManager<IEntityMan>(),
                                                                             manCollection.GetManager<IWorldMan>()));

            manCollection.AddSingleton<IUnitOfWork>(() => manCollection.GetManager<IDatabase>().CreateUnitOfWork());

            manCollection.SetupABFormats();

            manCollection.AddSingleton<IDataProvider>(() => new DataProvider(manCollection.GetManager<IUnitOfWork>(), 
                                                                             manCollection.GetManager<ILogger>(),
                                                                             manCollection.GetManager<IVariableMan>(),
                                                                             manCollection.GetManager<DataFormatMan>()));

            manCollection.AddGenericPhysicsManagers();
            manCollection.AddOpenALManagers();
            manCollection.AddOpenGLManagers();
        }

        public ICore CreateGame(string gameDbFilePath)
        {
            manCollection.AddSingleton<IDatabase>(() => new XmlDatabaseMan(manCollection.GetManager<IVariableMan>(), gameDbFilePath));
            return new Game(manCollection);
        }
    }
}
