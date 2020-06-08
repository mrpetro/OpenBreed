﻿using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Entities.Builders;
using OpenBreed.Sandbox.Entities.FpsCounter;
using OpenTK;
using System.Linq;

namespace OpenBreed.Sandbox.Worlds
{
    public static class HudWorldHelper
    {
        #region Public Methods

        private static void AddSystems(Program core, WorldBuilder builder)
        {
            //Input
            //builder.AddSystem(core.CreateWalkingControlSystem().Build());
            //builder.AddSystem(core.CreateAiControlSystem().Build());

            //Action
            //builder.AddSystem(core.CreateMovementSystem().Build());
            builder.AddSystem(core.CreateAnimationSystem().Build());

            ////Audio
            //builder.AddSystem(core.CreateSoundSystem().Build());

            //Video
            //builder.AddSystem(core.CreateTileSystem().SetGridSize(64, 64)
            //                               .SetLayersNo(1)
            //                               .SetTileSize(16)
            //                               .SetGridVisible(false)
            //                               .Build());

            //builder.AddSystem(core.CreateSpriteSystem().Build());
            builder.AddSystem(core.CreateTextSystem().Build());
        }

        public static void Create(Program core)
        {
            var builder = core.Worlds.Create().SetName("HUD");

            AddSystems(core, builder);

            Setup(builder.Build());
        }

        #endregion Public Methods

        #region Private Methods

        private static void Setup(World world)
        {
            var cameraBuilder = new CameraBuilder(world.Core);
            cameraBuilder.SetPosition(new Vector2(0, 0));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetFov(world.Core.ClientRectangle.Width, world.Core.ClientRectangle.Height);
            var hudCamera = cameraBuilder.Build();
            hudCamera.Tag = "HudCamera";
            world.PostCommand(new AddEntityCommand(world.Id, hudCamera.Id));
            //world.AddEntity(hudCamera);

            FpsCounterHelper.AddToWorld(world);

            var hudViewport = world.Core.Entities.GetByTag(ScreenWorldHelper.HUD_VIEWPORT).First();
            hudViewport.GetComponent<ViewportComponent>().CameraEntityId = hudCamera.Id;

            //world.Core.Rendering.Subscribe(GfxEventTypes.CLIENT_RESIZED, (s, a) => UpdateFpsPos(fpsTextEntity, (ClientResizedEventArgs)a));
            //world.Core.Rendering.Subscribe(GfxEventTypes.CLIENT_RESIZED, (s, a) => UpdateCameraFov(hudCamera, (ClientResizedEventArgs)a));

            hudViewport.Subscribe<ViewportResizedEventArgs>((s, a) => UpdateCameraFov(hudCamera, a));
        }

        private static void UpdateCameraFov(IEntity cameraEntity, ViewportResizedEventArgs a)
        {
            cameraEntity.GetComponent<CameraComponent>().Width = a.Width;
            cameraEntity.GetComponent<CameraComponent>().Height = a.Height;
        }

        #endregion Private Methods
    }
}