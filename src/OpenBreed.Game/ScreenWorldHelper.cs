using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Builders;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Builders;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Systems;
using OpenBreed.Game.Entities;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game
{
    internal static class ScreenWorldHelper
    {
        public const string GAME_VIEWPORT = "GameViewport";
        public const string HUD_VIEWPORT = "HUDViewport";
        public const string TEXT_VIEWPORT = "TextViewport";

        public static void AddSystems(Game game, WorldBuilder builder)
        {
            //Video
            builder.AddSystem(game.VideoSystemsFactory.CreateViewportSystem());
            //builder.AddSystem(core.CreateSpriteSystem().Build());
            //builder.AddSystem(core.CreateWireframeSystem().Build());
            builder.AddSystem(game.VideoSystemsFactory.CreateTextSystem());
        }

        public static Entity CreateViewportEntity(ICore core, string name, float x, float y, float width, float height, bool drawBackground, bool clipping = true)
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

        public static World CreateWorld(Game game)
        {
            var builder = game.Worlds.Create().SetName("ScreenWorld");
            AddSystems(game, builder);

            var world = builder.Build();

            var gameViewport = CreateViewportEntity(game, GAME_VIEWPORT, 32, 32, game.ClientRectangle.Width - 64, game.ClientRectangle.Height - 64, true, true);
            //gameViewport.GetComponent<ViewportComponent>().ScalingType = ViewportScalingType.FitBothPreserveAspectRatio;
            //gameViewport.GetComponent<ViewportComponent>().ScalingType = ViewportScalingType.FitHeightPreserveAspectRatio;
            gameViewport.Get<ViewportComponent>().ScalingType = ViewportScalingType.FitBothPreserveAspectRatio;

            game.Rendering.Subscribe<ClientResizedEventArgs>((s, a) => ResizeGameViewport(gameViewport, a));

            FpsCounterHelper.AddToWorld(world);

            game.Commands.Post(new AddEntityCommand(world.Id, gameViewport.Id));
            //world.AddEntity(gameViewport);
            //world.AddEntity(hudViewport);

            gameViewport.Subscribe<ViewportClickedEventArgs>(OnViewportClick);

            return world;
        }

        private static void OnViewportClick(object sender, ViewportClickedEventArgs args)
        {
            throw new NotImplementedException();
        }

        private static void ResizeGameViewport(Entity viewport, ClientResizedEventArgs args)
        {
            viewport.Core.Commands.Post(new ViewportResizeCommand(viewport.Id, args.Width - 64, args.Height - 64));
        }

        private static void ResizeHudViewport(Entity viewport, ClientResizedEventArgs args)
        {
            viewport.Core.Commands.Post(new ViewportResizeCommand(viewport.Id, args.Width, args.Height));
        }

        private static void ResizeTextViewport(Entity viewport, ClientResizedEventArgs args)
        {
            viewport.Core.Commands.Post(new ViewportResizeCommand(viewport.Id, args.Width, args.Height));
        }
    }
}
