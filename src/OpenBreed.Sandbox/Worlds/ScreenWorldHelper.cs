using OpenBreed.Core;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Systems;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Worlds
{
    public static class ScreenWorldHelper
    {
        public static void AddSystems(Program core, WorldBuilder builder)
        {
            //Video
            builder.AddSystem(new ViewportSystem(core));
            builder.AddSystem(core.CreateSpriteSystem().Build());
            //builder.AddSystem(core.CreateWireframeSystem().Build());
            builder.AddSystem(core.CreateTextSystem().Build());
        }

        public static World CreateWorld(Program core)
        {
            var builder = core.Worlds.Create().SetName("ScreenWorld");
            AddSystems(core, builder);

            var world = builder.Build();

            var viewport = core.Entities.Create();
            viewport.Tag = "ScreenViewport";
            viewport.Add(new ViewportComponent(0.9f, 0.9f) { DrawBorder = true, DrawBackgroud = false, Clipping = true, BackgroundColor = Color4.Blue });
            viewport.Add(Position.Create(0.05f,0.05f));

            world.AddEntity(viewport);

            return world;
        }
    }
}
