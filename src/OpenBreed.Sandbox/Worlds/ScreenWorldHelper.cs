using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Wecs.Commands;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenBreed.Wecs.Systems.Rendering.Events;
using OpenBreed.Wecs.Worlds;
using System;

namespace OpenBreed.Sandbox.Worlds
{
    public class ScreenWorldHelper
    {
        #region Public Fields

        public const string GAME_VIEWPORT = "GameViewport";

        public const string HUD_VIEWPORT = "HUDViewport";

        public const string TEXT_VIEWPORT = "TextViewport";

        #endregion Public Fields

        #region Private Fields

        private readonly ICore core;
        private readonly ISystemFactory systemFactory;
        private readonly ICommandsMan commandsMan;
        private readonly IRenderingMan renderingMan;
        private readonly IWorldMan worldMan;
        private readonly IEventsMan eventsMan;
        private readonly ViewportCreator viewportCreator;
        private readonly IViewClient viewClient;

        #endregion Private Fields

        #region Public Constructors

        public ScreenWorldHelper(ICore core, ISystemFactory systemFactory, ICommandsMan commandsMan, IRenderingMan renderingMan, IWorldMan worldMan, IEventsMan eventsMan, ViewportCreator viewportCreator, IViewClient viewClient)
        {
            this.core = core;
            this.systemFactory = systemFactory;
            this.commandsMan = commandsMan;
            this.renderingMan = renderingMan;
            this.worldMan = worldMan;
            this.eventsMan = eventsMan;
            this.viewportCreator = viewportCreator;
            this.viewClient = viewClient;
        }

        #endregion Public Constructors

        #region Public Methods

        public void AddSystems(WorldBuilder builder)
        {
            //Video
            builder.AddSystem(systemFactory.Create<ViewportSystem>());
            //builder.AddSystem(core.CreateSpriteSystem().Build());
            //builder.AddSystem(core.CreateWireframeSystem().Build());
            //builder.AddSystem(core.CreateTextSystem().Build());
        }

        public World CreateWorld()
        {
            var builder = worldMan.Create().SetName("ScreenWorld");
            AddSystems(builder);

            var world = builder.Build(core);

            var gameViewport = viewportCreator.CreateViewportEntity(GAME_VIEWPORT, 32, 32, viewClient.ClientRectangle.Width - 64, viewClient.ClientRectangle.Height - 64, true, true);
            //gameViewport.GetComponent<ViewportComponent>().ScalingType = ViewportScalingType.FitBothPreserveAspectRatio;
            //gameViewport.GetComponent<ViewportComponent>().ScalingType = ViewportScalingType.FitHeightPreserveAspectRatio;
            gameViewport.Get<ViewportComponent>().ScalingType = ViewportScalingType.FitBothPreserveAspectRatio;
            var hudViewport = viewportCreator.CreateViewportEntity(HUD_VIEWPORT, 0, 0, viewClient.ClientRectangle.Width, viewClient.ClientRectangle.Height, false, true);
            var textViewport = viewportCreator.CreateViewportEntity(TEXT_VIEWPORT, 0, 0, viewClient.ClientRectangle.Width, viewClient.ClientRectangle.Height, false, true);

            eventsMan.Subscribe<ClientResizedEventArgs>(renderingMan, (s, a) => ResizeGameViewport(gameViewport, a));
            eventsMan.Subscribe<ClientResizedEventArgs>(renderingMan, (s, a) => ResizeHudViewport(hudViewport, a));
            eventsMan.Subscribe<ClientResizedEventArgs>(renderingMan, (s, a) => ResizeTextViewport(hudViewport, a));

            commandsMan.Post(new AddEntityCommand(world.Id, gameViewport.Id));
            commandsMan.Post(new AddEntityCommand(world.Id, hudViewport.Id));
            commandsMan.Post(new AddEntityCommand(world.Id, textViewport.Id));
            //world.AddEntity(gameViewport);
            //world.AddEntity(hudViewport);

            gameViewport.Subscribe<ViewportClickedEventArgs>(OnViewportClick);

            return world;
        }

        #endregion Public Methods

        #region Private Methods

        private void OnViewportClick(object sender, ViewportClickedEventArgs args)
        {
            throw new NotImplementedException();
        }

        private void ResizeGameViewport(Entity viewport, ClientResizedEventArgs args)
        {
            commandsMan.Post(new ViewportResizeCommand(viewport.Id, args.Width - 64, args.Height - 64));
        }

        private void ResizeHudViewport(Entity viewport, ClientResizedEventArgs args)
        {
            commandsMan.Post(new ViewportResizeCommand(viewport.Id, args.Width, args.Height));
        }

        private void ResizeTextViewport(Entity viewport, ClientResizedEventArgs args)
        {
            commandsMan.Post(new ViewportResizeCommand(viewport.Id, args.Width, args.Height));
        }

        #endregion Private Methods
    }
}