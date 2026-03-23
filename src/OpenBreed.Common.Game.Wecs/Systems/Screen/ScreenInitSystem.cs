using OpenBreed.Common.Data;
using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Components;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.OpenGL;
using OpenBreed.Wecs.Abstractions.Events;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Control.Components;
using OpenBreed.Wecs.Control.Systems.Extensions;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Rendering.Components;
using OpenBreed.Wecs.Rendering.Systems.Extensions;
using OpenTK.Mathematics;
using System;
using System.Linq;

namespace OpenBreed.Common.Game.Wecs.Systems.Screen
{
    public class ScreenInitSystem :
        IEventSystem<WorldInitializedEventArgs>
    {
        #region Private Fields

        private readonly IGameServices services;
        private readonly IWindow viewClient;

        #endregion Private Fields

        #region Public Constructors

        public ScreenInitSystem(IGameServices services,
            IWindow viewClient)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
            this.viewClient = viewClient ?? throw new ArgumentNullException(nameof(viewClient));
        }

        #endregion Public Constructors

        #region Public Methods

        public void OnEvent(WorldInitializedEventArgs e)
        {
            var world = services.Worlds.GetById(e.WorldId);

            if (world.Name != WorldNames.ScreenWorld)
            {
                return;
            }

            var renderView = viewClient.Context.ActiveViews.FirstOrDefault();

            if (renderView is null)
            {
                throw new InvalidOperationException("Expected at least one active render view");
            }

            var player1Entity = CreatePlayer("P1");
            var gameCommentator = services.Factory.CreateCommentator();
            var playerCamera = services.Factory.CreateCamera("Camera.Player", 0, 0, 320, 240);
            var johnPlayerEntity = services.Factory.CreatePlayerActor("John", new Vector2(0, 0));
            var gameViewport = services.Factory.CreateViewport(EntityNames.GameViewport, 0, 0, viewClient.ClientRectangle.Size.X, viewClient.ClientRectangle.Size.Y, "GameViewport");
            var gameHudViewport = services.Factory.CreateViewport(EntityNames.GameHudViewport, 0, 0, viewClient.ClientRectangle.Size.X, viewClient.ClientRectangle.Size.Y, "GameHudViewport");
            var debugHudViewport = services.Factory.CreateViewport(EntityNames.DebugHudViewport, 0, 0, viewClient.ClientRectangle.Size.X, viewClient.ClientRectangle.Size.Y, "DebugHudViewport");
            var textViewport = services.Factory.CreateViewport(EntityNames.TextViewport, 0, 0, viewClient.ClientRectangle.Size.X, viewClient.ClientRectangle.Size.Y, "TextViewport");

            playerCamera.Add(new PauseImmuneComponent());
            gameViewport.SetViewportCamera(playerCamera.Id);

            player1Entity.SetControlledEntity(johnPlayerEntity.Id);

            johnPlayerEntity.AddFollower(playerCamera);

            gameViewport.Get<ViewportComponent>().ScalingType = ViewportScalingType.FitBothPreserveAspectRatio;
            gameHudViewport.Get<ViewportComponent>().ScalingType = ViewportScalingType.FitBothPreserveAspectRatio;


            renderView.Resized += (s, w, h) => ResizeViewport(gameViewport, w, h);
            renderView.Resized += (s, w, h) => ResizeViewport(gameHudViewport, w, h);
            renderView.Resized += (s, w, h) => ResizeViewport(debugHudViewport, w, h);
            renderView.Resized += (s, w, h) => ResizeViewport(debugHudViewport, w, h);

            services.Worlds.RequestAddEntity(gameCommentator, world.Id);
            services.Worlds.RequestAddEntity(gameViewport, world.Id);
            services.Worlds.RequestAddEntity(gameHudViewport, world.Id);
            services.Worlds.RequestAddEntity(debugHudViewport, world.Id);
            services.Worlds.RequestAddEntity(textViewport, world.Id);
            services.Worlds.RequestAddEntity(player1Entity, world.Id);
        }

        #endregion Public Methods

        #region Private Methods

        private IEntity CreatePlayer(string player)
        {
            var player1Entity = services.Entities.Create($"Players/{player}");

            var playerInputs = new PlayerInputsComponent();
            playerInputs.Up = OpenTK.Windowing.GraphicsLibraryFramework.Keys.Up;
            playerInputs.Down = OpenTK.Windowing.GraphicsLibraryFramework.Keys.Down;
            playerInputs.Left = OpenTK.Windowing.GraphicsLibraryFramework.Keys.Left;
            playerInputs.Right = OpenTK.Windowing.GraphicsLibraryFramework.Keys.Right;
            playerInputs.Fire = OpenTK.Windowing.GraphicsLibraryFramework.Keys.RightControl;
            playerInputs.SwitchWeapon = OpenTK.Windowing.GraphicsLibraryFramework.Keys.PageDown;

            player1Entity.Add(playerInputs);
            player1Entity.Add(new ControllerComponent());

            return player1Entity;
        }

        private void ResizeViewport(IEntity viewport, float width, float height)
        {
            viewport.SetViewportSize(services.Events, width, height);
        }

        #endregion Private Methods
    }
}