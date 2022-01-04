using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Rendering.Interface.Events;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenBreed.Sandbox.Entities.Viewport;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Wecs.Systems.Rendering.Events;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Worlds;
using System;

namespace OpenBreed.Sandbox.Worlds
{
    public class ScreenWorldHelper
    {
        #region Public Fields

        public const string GAME_VIEWPORT = "GameViewport";

        public const string DEBUG_HUD_VIEWPORT = "DebugHudViewport";
        public const string GAME_HUD_VIEWPORT = "GameHudViewport";

        public const string TEXT_VIEWPORT = "TextViewport";

        #endregion Public Fields

        #region Private Fields

        private readonly ISystemFactory systemFactory;
        private readonly IRenderableFactory renderableFactory;
        private readonly IRenderingMan renderingMan;
        private readonly IWorldMan worldMan;
        private readonly IEventsMan eventsMan;
        private readonly ViewportCreator viewportCreator;
        private readonly IViewClient viewClient;

        #endregion Private Fields

        #region Public Constructors

        public ScreenWorldHelper(ISystemFactory systemFactory,
                                 IRenderableFactory renderableFactory,
                                 IRenderingMan renderingMan,
                                 IWorldMan worldMan,
                                 IEventsMan eventsMan,
                                 ViewportCreator viewportCreator,
                                 IViewClient viewClient)
        {
            this.systemFactory = systemFactory;
            this.renderableFactory = renderableFactory;
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
            builder.AddModule(renderableFactory.CreateRenderableBatch());

            AddSystems(builder);

            var world = builder.Build();

            var gameViewport = viewportCreator.CreateViewportEntity(GAME_VIEWPORT, 0, 0, viewClient.ClientRectangle.Size.X, viewClient.ClientRectangle.Size.Y, GAME_VIEWPORT);
            var gameHudViewport = viewportCreator.CreateViewportEntity(GAME_HUD_VIEWPORT, 0, 0, viewClient.ClientRectangle.Size.X, viewClient.ClientRectangle.Size.Y, GAME_HUD_VIEWPORT);
            gameViewport.Get<ViewportComponent>().ScalingType = ViewportScalingType.FitBothPreserveAspectRatio;
            gameHudViewport.Get<ViewportComponent>().ScalingType = ViewportScalingType.FitBothPreserveAspectRatio;

            var debugHudViewport = viewportCreator.CreateViewportEntity(DEBUG_HUD_VIEWPORT, 0, 0, viewClient.ClientRectangle.Size.X, viewClient.ClientRectangle.Size.Y, DEBUG_HUD_VIEWPORT);

            var textViewport = viewportCreator.CreateViewportEntity(TEXT_VIEWPORT, 0, 0, viewClient.ClientRectangle.Size.X, viewClient.ClientRectangle.Size.Y, TEXT_VIEWPORT);

            renderingMan.ClientResized += (s, a) => ResizeGameViewport(gameViewport, a);
            renderingMan.ClientResized += (s, a) => ResizeHudViewport(gameHudViewport, a);
            renderingMan.ClientResized += (s, a) => ResizeHudViewport(debugHudViewport, a);
            renderingMan.ClientResized += (s, a) => ResizeTextViewport(debugHudViewport, a);

            gameViewport.EnterWorld(world.Id);
            gameHudViewport.EnterWorld(world.Id);
            debugHudViewport.EnterWorld(world.Id);
            textViewport.EnterWorld(world.Id);

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
            viewport.SetViewportSize(args.Width, args.Height);
        }

        private void ResizeHudViewport(Entity viewport, ClientResizedEventArgs args)
        {
            viewport.SetViewportSize(args.Width, args.Height);
        }

        private void ResizeTextViewport(Entity viewport, ClientResizedEventArgs args)
        {
            viewport.SetViewportSize(args.Width, args.Height);
        }

        #endregion Private Methods
    }
}