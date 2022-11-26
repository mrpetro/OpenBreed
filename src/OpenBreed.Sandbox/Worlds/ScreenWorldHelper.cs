using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Input.Interface;
using OpenBreed.Rendering.Interface.Events;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenBreed.Sandbox.Entities.Viewport;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Systems;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Audio;
using OpenBreed.Wecs.Systems.Control;
using OpenBreed.Wecs.Systems.Core;
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

        public const string GAME_VIEWPORT = "Viewport.Game";
        public const string DEBUG_HUD_VIEWPORT = "Viewport.DebugHud";
        public const string GAME_HUD_VIEWPORT = "Viewport.GameHud";
        public const string TEXT_VIEWPORT = "Viewport.Text";

        #endregion Public Fields

        #region Private Fields

        private readonly ISystemFactory systemFactory;
        private readonly IRenderableFactory renderableFactory;
        private readonly IRenderingMan renderingMan;
        private readonly IWorldMan worldMan;
        private readonly IEventsMan eventsMan;
        private readonly IEntityMan entityMan;
        private readonly IPlayersMan playersMan;
        private readonly ViewportCreator viewportCreator;
        private readonly IEntityFactory entityFactory;
        private readonly IViewClient viewClient;
        private readonly ITriggerMan triggerMan;
        private readonly IScriptMan scriptMan;

        #endregion Private Fields

        #region Public Constructors

        public ScreenWorldHelper(ISystemFactory systemFactory,
                                 IRenderableFactory renderableFactory,
                                 IRenderingMan renderingMan,
                                 IWorldMan worldMan,
                                 IEventsMan eventsMan,
                                 IEntityMan entityMan,
                                 IPlayersMan playersMan,
                                 ViewportCreator viewportCreator,
                                 IEntityFactory entityFactory,
                                 IViewClient viewClient,
                                 ITriggerMan triggerMan,
                                 IScriptMan scriptMan)
        {
            this.systemFactory = systemFactory;
            this.renderableFactory = renderableFactory;
            this.renderingMan = renderingMan;
            this.worldMan = worldMan;
            this.eventsMan = eventsMan;
            this.entityMan = entityMan;
            this.playersMan = playersMan;
            this.viewportCreator = viewportCreator;
            this.entityFactory = entityFactory;
            this.viewClient = viewClient;
            this.triggerMan = triggerMan;
            this.scriptMan = scriptMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void AddSystems(WorldBuilder builder)
        {
            //Input Stage
            builder.AddSystem(systemFactory.Create<ActorMovementByPlayerControlSystem>());
            builder.AddSystem(systemFactory.Create<ActorScriptByPlayerControlSystem>());
            builder.AddSystem(systemFactory.Create<AttackControllerSystem>());


            //Video

            builder.AddSystem(systemFactory.Create<ViewportSystem>());
            builder.AddSystem(systemFactory.Create<SoundSystem>());
            builder.AddSystem(systemFactory.Create<TimerSystem>());
            builder.AddSystem(systemFactory.Create<FrameSystem>());
            //builder.AddSystem(core.CreateSpriteSystem().Build());
            //builder.AddSystem(core.CreateWireframeSystem().Build());
            //builder.AddSystem(core.CreateTextSystem().Build());
        }

        public IEntity CreateController(string player)
        {
            var p1Controller = entityMan.Create($"Controllers.{player}");
            var p1 = playersMan.GetByName(player);

            p1Controller.Add(new WalkingInputComponent(p1.Id, 0));
            p1Controller.Add(new AttackInputComponent(p1.Id, 0));
            p1Controller.Add(new ControlComponent());
            p1Controller.Add(new AttackControlComponent());

            return p1Controller;
        }

        public IWorld CreateWorld()
        {
            var builder = worldMan.Create().SetName("ScreenWorld");
            builder.AddModule(renderableFactory.CreateRenderableBatch());

            AddSystems(builder);

            var world = builder.Build();

            var gameCommentatorBuilder = entityFactory.Create($@"Vanilla\ABTA\Templates\Common\GameCommentator.xml");
            var gameCommentator = gameCommentatorBuilder.Build();


            var p1Controller = CreateController("P1");


            scriptMan.Expose("Commentator", gameCommentator);

            var gameViewport = viewportCreator.CreateViewportEntity(GAME_VIEWPORT, 0, 0, viewClient.ClientRectangle.Size.X, viewClient.ClientRectangle.Size.Y, "GameViewport");
            var gameHudViewport = viewportCreator.CreateViewportEntity(GAME_HUD_VIEWPORT, 0, 0, viewClient.ClientRectangle.Size.X, viewClient.ClientRectangle.Size.Y, "GameHudViewport");
            gameViewport.Get<ViewportComponent>().ScalingType = ViewportScalingType.FitBothPreserveAspectRatio;
            gameHudViewport.Get<ViewportComponent>().ScalingType = ViewportScalingType.FitBothPreserveAspectRatio;

            var debugHudViewport = viewportCreator.CreateViewportEntity(DEBUG_HUD_VIEWPORT, 0, 0, viewClient.ClientRectangle.Size.X, viewClient.ClientRectangle.Size.Y, "DebugHudViewport");

            var textViewport = viewportCreator.CreateViewportEntity(TEXT_VIEWPORT, 0, 0, viewClient.ClientRectangle.Size.X, viewClient.ClientRectangle.Size.Y, "TextViewport");

            renderingMan.ClientResized += (s, a) => ResizeGameViewport(gameViewport, a);
            renderingMan.ClientResized += (s, a) => ResizeHudViewport(gameHudViewport, a);
            renderingMan.ClientResized += (s, a) => ResizeHudViewport(debugHudViewport, a);
            renderingMan.ClientResized += (s, a) => ResizeTextViewport(debugHudViewport, a);

            triggerMan.OnWorldInitialized(
                world, () =>
                {
                    gameCommentator.EnterWorld(world.Id);
                    gameViewport.EnterWorld(world.Id);
                    gameHudViewport.EnterWorld(world.Id);
                    debugHudViewport.EnterWorld(world.Id);
                    textViewport.EnterWorld(world.Id);
                    p1Controller.EnterWorld(world.Id);
                }, singleTime: true);




            //gameViewport.Subscribe<ViewportClickedEventArgs>(OnViewportClick);

            return world;
        }

        #endregion Public Methods

        #region Private Methods

        //private void OnViewportClick(object sender, ViewportClickedEventArgs args)
        //{
        //    throw new NotImplementedException();
        //}

        private void ResizeGameViewport(IEntity viewport, ClientResizedEventArgs args)
        {
            viewport.SetViewportSize(args.Width, args.Height);
        }

        private void ResizeHudViewport(IEntity viewport, ClientResizedEventArgs args)
        {
            viewport.SetViewportSize(args.Width, args.Height);
        }

        private void ResizeTextViewport(IEntity viewport, ClientResizedEventArgs args)
        {
            viewport.SetViewportSize(args.Width, args.Height);
        }

        #endregion Private Methods
    }
}