﻿using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Systems.Rendering.Events;
using OpenBreed.Sandbox.Entities.Camera;
using OpenBreed.Sandbox.Entities.CursorCoords;
using OpenBreed.Sandbox.Entities.FpsCounter;
using OpenTK;
using System.Linq;
using OpenBreed.Wecs;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenBreed.Wecs.Commands;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Wecs.Systems.Animation;

namespace OpenBreed.Sandbox.Worlds
{
    public static class HudWorldHelper
    {
        #region Public Methods

        private static void AddSystems(Program core, WorldBuilder builder)
        {
            var systemFactory = core.GetManager<ISystemFactory>();

            //Input
            //builder.AddSystem(core.CreateWalkingControlSystem().Build());
            //builder.AddSystem(core.CreateAiControlSystem().Build());

            //Action
            //builder.AddSystem(core.CreateMovementSystem().Build());
            builder.AddSystem(systemFactory.Create<AnimationSystem>());

            ////Audio
            //builder.AddSystem(core.CreateSoundSystem().Build());

            //Video
            //builder.AddSystem(core.CreateTileSystem().SetGridSize(64, 64)
            //                               .SetLayersNo(1)
            //                               .SetTileSize(16)
            //                               .SetGridVisible(false)
            //                               .Build());

            //builder.AddSystem(core.CreateSpriteSystem().Build());
            builder.AddSystem(systemFactory.Create<TextSystem>());
        }

        public static void Create(Program core)
        {
            var builder = core.GetManager<IWorldMan>().Create().SetName("HUD");

            AddSystems(core, builder);

            Setup(core, builder.Build(core));
        }

        #endregion Public Methods

        #region Private Methods

        private static void Setup(ICore core, World world)
        {
            var windowClient = core.GetManager<IViewClient>();
            var cameraBuilder = new CameraBuilder(core);
            cameraBuilder.SetPosition(new Vector2(0, 0));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetFov(windowClient.ClientRectangle.Width, windowClient.ClientRectangle.Height);
            var hudCamera = cameraBuilder.Build();
            hudCamera.Tag = "HudCamera";
            core.Commands.Post(new AddEntityCommand(world.Id, hudCamera.Id));
            //world.AddEntity(hudCamera);

            FpsCounterHelper.AddToWorld(core, world);
            CursorCoordsHelper.AddToWorld(core, world);

            var hudViewport = core.GetManager<IEntityMan>().GetByTag(ScreenWorldHelper.HUD_VIEWPORT).First();
            hudViewport.Get<ViewportComponent>().CameraEntityId = hudCamera.Id;

            //world.Core.Rendering.Subscribe(GfxEventTypes.CLIENT_RESIZED, (s, a) => UpdateFpsPos(fpsTextEntity, (ClientResizedEventArgs)a));
            //world.Core.Rendering.Subscribe(GfxEventTypes.CLIENT_RESIZED, (s, a) => UpdateCameraFov(hudCamera, (ClientResizedEventArgs)a));

            hudViewport.Subscribe<ViewportResizedEventArgs>((s, a) => UpdateCameraFov(hudCamera, a));
        }

        private static void UpdateCameraFov(Entity cameraEntity, ViewportResizedEventArgs a)
        {
            cameraEntity.Get<CameraComponent>().Width = a.Width;
            cameraEntity.Get<CameraComponent>().Height = a.Height;
        }

        #endregion Private Methods
    }
}