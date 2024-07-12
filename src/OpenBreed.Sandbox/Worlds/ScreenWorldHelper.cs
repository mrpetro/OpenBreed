using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Input.Interface;
using OpenBreed.Rendering.Interface.Events;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Viewport;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Wecs.Components;
using OpenBreed.Sandbox.Wecs.Systems;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Audio;
using OpenBreed.Wecs.Systems.Control;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems.Core.Extensions;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Wecs.Systems.Rendering.Events;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Numerics;

namespace OpenBreed.Sandbox.Worlds
{
    public class ScreenWorldHelper
    {
        #region Public Fields

        public const string GAME_VIEWPORT = "Viewport.Game";
        public const string DEBUG_HUD_VIEWPORT = $"Viewport.{WorldNames.DEBUG_HUD}";
        public const string GAME_HUD_VIEWPORT = $"Viewport.{WorldNames.GAME_HUD}";
        public const string TEXT_VIEWPORT = "Viewport.Text";

        #endregion Public Fields

        #region Private Fields

        private readonly ISystemFactory systemFactory;
        private readonly IRenderableFactory renderableFactory;
        private readonly IRenderingMan renderingMan;
        private readonly IWorldMan worldMan;
        private readonly IEventsMan eventsMan;
        private readonly IEntityMan entityMan;
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
            this.viewportCreator = viewportCreator;
            this.entityFactory = entityFactory;
            this.viewClient = viewClient;
            this.triggerMan = triggerMan;
            this.scriptMan = scriptMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void AddSystems(IWorldBuilder builder)
        {
            //Input Stage
            builder.AddSystem<ActorMovementByPlayerInputsSystem>();

            //Video

            builder.AddSystem<ViewportSystem>();
            //builder.AddSystem<SoundSystem>();
            builder.AddSystem<SoundSystem>();
            builder.AddSystem<TimerSystem>();
            builder.AddSystem<FrameSystem>();
            //builder.AddSystem(core.CreateSpriteSystem().Build());
            //builder.AddSystem(core.CreateWireframeSystem().Build());
            //builder.AddSystem(core.CreateTextSystem().Build());
        }

        public IEntity CreatePlayer(string player)
        {
            var player1Entity = entityMan.Create($"Players/{player}");

            var playerInputs = new PlayerInputsComponent();
            playerInputs.Up = Keys.Up;
            playerInputs.Down =Keys.Down;
            playerInputs.Left = Keys.Left;
            playerInputs.Right = Keys.Right;
            playerInputs.Fire = Keys.RightControl;
            playerInputs.SwitchWeapon = Keys.PageDown;

            player1Entity.Add(playerInputs);
            player1Entity.Add(new ControllerComponent());

            return player1Entity;
        }

        public IWorld CreateWorld()
        {
            var builder = worldMan.Create().SetName("ScreenWorld");

            AddSystems(builder);

            var world = builder.Build();

            var gameCommentatorBuilder = entityFactory.Create($@"ABTA\Templates\Common\GameCommentator");

            var gameCommentator = gameCommentatorBuilder.Build();
            gameCommentator.CreateTimer("SpeechDelay");
            gameCommentator.CreateTimer("MissionDelay");



            var player1Entity = CreatePlayer("P1");




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
                    worldMan.RequestAddEntity(gameCommentator, world.Id);
                    worldMan.RequestAddEntity(gameViewport, world.Id);
                    worldMan.RequestAddEntity(gameHudViewport, world.Id);
                    worldMan.RequestAddEntity(debugHudViewport, world.Id);
                    worldMan.RequestAddEntity(textViewport, world.Id);
                    worldMan.RequestAddEntity(player1Entity, world.Id);
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
            viewport.SetViewportSize(eventsMan, args.Width, args.Height);
        }

        private void ResizeHudViewport(IEntity viewport, ClientResizedEventArgs args)
        {
            viewport.SetViewportSize(eventsMan, args.Width, args.Height);
        }

        private void ResizeTextViewport(IEntity viewport, ClientResizedEventArgs args)
        {
            viewport.SetViewportSize(eventsMan, args.Width, args.Height);
        }

        #endregion Private Methods
    }
}