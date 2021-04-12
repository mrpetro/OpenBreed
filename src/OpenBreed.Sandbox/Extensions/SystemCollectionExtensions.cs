using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Core.Managers;
using OpenBreed.Input.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Entities.Camera;
using OpenBreed.Scripting.Interface;
using OpenBreed.Scripting.Lua.Extensions;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Extensions
{
    public static class SystemCollectionExtensions
    {
        public static void SetupGameScriptingApi(this IManagerCollection manCollection)
        {
            var scriptMan = manCollection.GetManager<IScriptMan>();

            scriptMan.Expose("Worlds", manCollection.GetManager<IWorldMan>());
            scriptMan.Expose("Entities", manCollection.GetManager<IEntityMan>());
            scriptMan.Expose("Commands", manCollection.GetManager<ICommandsMan>());
            scriptMan.Expose("Inputs", manCollection.GetManager<IInputsMan>());
            scriptMan.Expose("Logging", manCollection.GetManager<ILogger>());
            scriptMan.Expose("Players", manCollection.GetManager<IPlayersMan>());
        }

        public static void SetupSandboxBuilders(this IManagerCollection manCollection)
        {
            manCollection.AddTransient<CameraBuilder>(() => new CameraBuilder(manCollection.GetManager<IEntityMan>(),
                                                                              manCollection.GetManager<CameraComponentBuilder>()));

            manCollection.AddTransient<WorldBlockBuilder>(() => new WorldBlockBuilder(manCollection.GetManager<ITileMan>(),
                                                                                      manCollection.GetManager<IFixtureMan>(),
                                                                                      manCollection.GetManager<IEntityMan>(),
                                                                                      manCollection.GetManager<BodyComponentBuilder>()));
            
        }
    }
}
