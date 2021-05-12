using OpenBreed.Core;
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
using OpenBreed.Common;
using OpenBreed.Rendering.Interface.Events;

namespace OpenBreed.Game
{
    internal class ScreenWorldHelper
    {
        public const string GAME_VIEWPORT = "GameViewport";
        public const string HUD_VIEWPORT = "HUDViewport";
        public const string TEXT_VIEWPORT = "TextViewport";
        private readonly ICore core;
        private readonly IWorldMan worldMan;
        private readonly ICommandsMan commandsMan;
        private readonly ViewportCreator viewportCreator;
        private readonly ISystemFactory systemFactory;
        private readonly IViewClient viewClient;

        public ScreenWorldHelper(ICore core, IWorldMan worldMan, ICommandsMan commandsMan, ViewportCreator viewportCreator, ISystemFactory systemFactory, IViewClient viewClient)
        {
            this.core = core;
            this.worldMan = worldMan;
            this.commandsMan = commandsMan;
            this.viewportCreator = viewportCreator;
            this.systemFactory = systemFactory;
            this.viewClient = viewClient;
        }

        public World CreateWorld()
        {
            var builder = worldMan.Create().SetName("ScreenWorld");

            builder.AddSystem(systemFactory.Create<ViewportSystem>());
            builder.AddSystem(systemFactory.Create<TextSystem>());

            var world = builder.Build(core);

            var gameViewport = viewportCreator.CreateViewportEntity(GAME_VIEWPORT, 32, 32, viewClient.ClientRectangle.Width - 64, viewClient.ClientRectangle.Height - 64, true, true);
            //gameViewport.GetComponent<ViewportComponent>().ScalingType = ViewportScalingType.FitBothPreserveAspectRatio;
            //gameViewport.GetComponent<ViewportComponent>().ScalingType = ViewportScalingType.FitHeightPreserveAspectRatio;
            gameViewport.Get<ViewportComponent>().ScalingType = ViewportScalingType.FitBothPreserveAspectRatio;

            var renderingMan = core.GetManager<IRenderingMan>();
            var eventsMan = core.GetManager<IEventsMan>();

            renderingMan.ClientResized += (s, a) => ResizeGameViewport(gameViewport, a);

            FpsCounterHelper.AddToWorld(core, world);

            commandsMan.Post(new AddEntityCommand(world.Id, gameViewport.Id));
            gameViewport.Subscribe<ViewportClickedEventArgs>(OnViewportClick);

            return world;
        }

        private void OnViewportClick(object sender, ViewportClickedEventArgs args)
        {
            throw new NotImplementedException();
        }

        private void ResizeGameViewport(Entity viewport, ClientResizedEventArgs args)
        {
            commandsMan.Post(new ViewportResizeCommand(viewport.Id, args.Width - 64, args.Height - 64));
        }
    }
}
