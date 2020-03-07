using OpenBreed.Core;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Systems.Control.Systems;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Physics.Systems;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Entities.Builders;
using OpenBreed.Core.Modules.Rendering.Events;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Sandbox.Jobs;
using OpenTK;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Worlds
{
    public static class HudWorldHelper
    {
        #region Public Methods

        public static void AddSystems(Program core, WorldBuilder builder)
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

        public static void CreateHudWorld(Program core)
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
            cameraBuilder.SetPosition(new Vector2(0,0));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetZoom(1.0f / (float)world.Core.ClientRectangle.Width, 1.0f / (float)world.Core.ClientRectangle.Height);
            var hudCamera = cameraBuilder.Build();
            hudCamera.Tag = "HudCamera";
            world.AddEntity(hudCamera);

            var arial12 = world.Core.Rendering.Fonts.Create("ARIAL", 9);

            var fpsTextEntity = world.Core.Entities.Create();

            fpsTextEntity.Add(PositionComponent.Create(new Vector2(-fpsTextEntity.Core.ClientRectangle.Width / 2.0f, -fpsTextEntity.Core.ClientRectangle.Height / 2.0f + 74)));
            fpsTextEntity.Add(TextComponent.Create(arial12.Id, Vector2.Zero, "FPS: 0.0", 100.0f));
            world.AddEntity(fpsTextEntity);

            world.Core.Jobs.Execute(new FpsTextUpdateJob(fpsTextEntity));

            world.Core.Entities.GetByTag(ScreenWorldHelper.HUD_VIEWPORT).First().GetComponent<ViewportComponent>().CameraEntityId = hudCamera.Id;


            world.Core.Rendering.Subscribe(GfxEventTypes.CLIENT_RESIZED, (s, a) => OnClientResized(fpsTextEntity, (ClientResizedEventArgs)a));
        }

        private static void OnClientResized(IEntity fpsTextEntity, ClientResizedEventArgs a)
        {
            fpsTextEntity.GetComponent<PositionComponent>().Value = new Vector2(-fpsTextEntity.Core.ClientRectangle.Width / 2.0f, -fpsTextEntity.Core.ClientRectangle.Height / 2.0f + 74);

        }

        #endregion Private Methods
    }
}