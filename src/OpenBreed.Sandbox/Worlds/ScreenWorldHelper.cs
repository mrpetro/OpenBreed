using OpenBreed.Core;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Builders;
using OpenBreed.Core.Modules.Rendering.Commands;
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
        public const string GAME_VIEWPORT = "GameViewport";
        public const string HUD_VIEWPORT = "HUDViewport";

        public static void AddSystems(Program core, WorldBuilder builder)
        {
            //Video
            builder.AddSystem(new ViewportSystem(core));
            //builder.AddSystem(core.CreateSpriteSystem().Build());
            //builder.AddSystem(core.CreateWireframeSystem().Build());
            //builder.AddSystem(core.CreateTextSystem().Build());
        }

        public static IEntity CreateViewportEntity(ICore core, string name, float x, float y, float width, float height, bool drawBackground, bool clipping = true)
        {
            var viewport = core.Entities.Create();
            viewport.Tag = name;

            var vpcBuilder = ViewportComponentBuilder.New(core);
            vpcBuilder.SetProperty("Width", width);
            vpcBuilder.SetProperty("Height", height);
            vpcBuilder.SetProperty("DrawBorder", true);
            vpcBuilder.SetProperty("DrawBackground", drawBackground);
            vpcBuilder.SetProperty("Clipping", clipping);
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

            var gameViewport = CreateViewportEntity(core, GAME_VIEWPORT, 32, 32, core.ClientRectangle.Width - 64, core.ClientRectangle.Height - 64, true, true);
            gameViewport.GetComponent<ViewportComponent>().ScalingType = ViewportScalingType.FitBothPreserveAspectRatio;

            var hudViewport = CreateViewportEntity(core, HUD_VIEWPORT, 0, 0, core.ClientRectangle.Width, core.ClientRectangle.Height, false, true);

            core.Rendering.Subscribe(GfxEventTypes.CLIENT_RESIZED, (s, a) => ResizeGameViewport(gameViewport, (ClientResizedEventArgs)a));
            core.Rendering.Subscribe(GfxEventTypes.CLIENT_RESIZED, (s, a) => ResizeHudViewport(hudViewport, (ClientResizedEventArgs)a));

            world.AddEntity(gameViewport);
            world.AddEntity(hudViewport);

            return world;
        }

        private static void ResizeGameViewport(IEntity viewport, ClientResizedEventArgs args)
        {
            viewport.PostCommand(new ViewportResizeCommand(viewport.Id, args.Width - 64, args.Height - 64));
        }

        private static void ResizeHudViewport(IEntity viewport, ClientResizedEventArgs args)
        {
            viewport.PostCommand(new ViewportResizeCommand(viewport.Id, args.Width, args.Height));
        }

    }
}
