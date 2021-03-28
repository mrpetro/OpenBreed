﻿using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenBreed.Game.Entities;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Rendering.Interface;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs.Systems.Rendering.Events;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenBreed.Wecs.Commands;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Core.Managers;

namespace OpenBreed.Game
{
    internal static class ScreenWorldHelper
    {
        public const string GAME_VIEWPORT = "GameViewport";
        public const string HUD_VIEWPORT = "HUDViewport";
        public const string TEXT_VIEWPORT = "TextViewport";

        public static void AddSystems(Game game, WorldBuilder builder)
        {
            var systemFactory = game.GetManager<ISystemFactory>();

            //Video
            builder.AddSystem(systemFactory.Create<ViewportSystem>());
            //builder.AddSystem(core.CreateSpriteSystem().Build());
            //builder.AddSystem(core.CreateWireframeSystem().Build());
            builder.AddSystem(systemFactory.Create<TextSystem>());
        }

        public static World CreateWorld(Game game)
        {
            var windowClient = game.GetManager<IViewClient>();

            var builder = game.GetManager<IWorldMan>().Create().SetName("ScreenWorld");
            AddSystems(game, builder);

            var world = builder.Build(game);

            var viewportCreator = game.GetManager<ViewportCreator>();

            var gameViewport = viewportCreator.CreateViewportEntity(GAME_VIEWPORT, 32, 32, windowClient.ClientRectangle.Width - 64, windowClient.ClientRectangle.Height - 64, true, true);
            //gameViewport.GetComponent<ViewportComponent>().ScalingType = ViewportScalingType.FitBothPreserveAspectRatio;
            //gameViewport.GetComponent<ViewportComponent>().ScalingType = ViewportScalingType.FitHeightPreserveAspectRatio;
            gameViewport.Get<ViewportComponent>().ScalingType = ViewportScalingType.FitBothPreserveAspectRatio;

            var renderingMan = game.GetManager<IRenderingMan>();
            var eventsMan = game.GetManager<IEventsMan>();
            eventsMan.Subscribe<ClientResizedEventArgs>(renderingMan, (s, a) => ResizeGameViewport(gameViewport, a));

            FpsCounterHelper.AddToWorld(game, world);

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
