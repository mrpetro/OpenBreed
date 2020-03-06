using OpenBreed.Core;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Builders;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Events;
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
            //builder.AddSystem(core.CreateSpriteSystem().Build());
            //builder.AddSystem(core.CreateWireframeSystem().Build());
            //builder.AddSystem(core.CreateTextSystem().Build());
        }

        public static IEntity CreateViewportEntity(ICore core, string name, float x, float y, float width, float height)
        {
            var viewport = core.Entities.Create();
            viewport.Tag = name;

            var vpcBuilder = ViewportComponentBuilder.New(core);
            vpcBuilder.SetProperty("Width", width);
            vpcBuilder.SetProperty("Height", height);
            vpcBuilder.SetProperty("DrawBorder", true);
            vpcBuilder.SetProperty("DrawBackground", true);
            vpcBuilder.SetProperty("Clipping", true);
            vpcBuilder.SetProperty("BackgroundColor", Color4.Black);

            viewport.Add(vpcBuilder.Build());
            viewport.Add(PositionComponent.Create(x, y));

            return viewport;
        }

        public static World CreateWorld(Program core)
        {
            var builder = core.Worlds.Create().SetName("ScreenWorld");
            AddSystems(core, builder);

            var world = builder.Build();

            //var viewport = CreateViewportEntity(core, "ScreenViewport", 0.05f, 0.05f, 0.9f, 0.9f);
            var viewport = CreateViewportEntity(core, "ScreenViewport", 32, 32, core.ClientRectangle.Width - 64, core.ClientRectangle.Height - 64);

            core.Rendering.Subscribe(GfxEventTypes.CLIENT_RESIZED, (s,a) => OnClientResized(viewport, (ClientResizedEventArgs)a));

            world.AddEntity(viewport);

            return world;
        }

        private static void OnClientResized(IEntity viewport, ClientResizedEventArgs args)
        {
            var vpc = viewport.GetComponent<ViewportComponent>();
            vpc.Width = args.Width - 64;
            vpc.Height = args.Height - 64;
        }

    }
}
