using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Rendering.Systems.Extensions;
using OpenBreed.Wecs.Physics.Systems.Extensions;
using OpenBreed.Wecs.Control.Systems.Extensions;
using OpenBreed.Wecs.Core.Systems.Extensions;
using OpenBreed.Wecs.Gui.Systems.Extensions;
using OpenBreed.Wecs.Scripting.Systems.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs.Audio.Systems.Extensions;
using OpenBreed.Sandbox.App.Systems;

namespace OpenBreed.Sandbox.App.Extensions
{
    public static  class WorldManExtensions
    {
        public static IWorld CreateSandboxWorld(this IWorldMan worldMan)
        {
            return worldMan.Create()
                .AddPhysicsSystems()
                .AddControlSystems()
                .AddCoreSystems()
                .AddSoundSystems()
                .AddGuiSystems(isEditor: false)
                .AddRenderingSystems()
                .AddScriptingSystems()
                .AddSystem<MapRenderSystem>()
                .SetName("Sandbox").Build();
        }
    }
}
