using OpenBreed.Core;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Modules.Animation.Systems.Control.Systems;
using OpenBreed.Core.Modules.Physics.Systems;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Entities.Builders;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Sandbox.Jobs;
using OpenTK;
using System.Linq;

namespace OpenBreed.Sandbox.Worlds
{
    public static class HudWorldHelper
    {
        #region Public Methods

        public static void AddSystems(Program core, WorldBuilder builder)
        {
            //Input
            builder.AddSystem(core.CreateWalkingControlSystem().Build());
            builder.AddSystem(core.CreateAiControlSystem().Build());

            //Action
            builder.AddSystem(core.CreateMovementSystem().Build());
            builder.AddSystem(core.CreateAnimationSystem().Build());

            //Audio
            builder.AddSystem(core.CreateSoundSystem().Build());

            //Video
            builder.AddSystem(core.CreateTileSystem().SetGridSize(64, 64)
                                           .SetLayersNo(1)
                                           .SetTileSize(16)
                                           .SetGridVisible(false)
                                           .Build());

            builder.AddSystem(core.CreateSpriteSystem().Build());
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
            var hudViewport = (Viewport)world.Core.Rendering.Viewports.Create(0.0f, 0.0f, 1.0f, 1.0f, 100.0f);
            world.Core.Rendering.Viewports.Add(hudViewport);
            hudViewport.DrawBorder = true;
            hudViewport.DrawBackgroud = false;
            hudViewport.Clipping = false;

            var cameraBuilder = new CameraBuilder(world.Core);
            cameraBuilder.SetPosition(new Vector2(0, 0));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetZoom(1.0f);
            var hudCamera = cameraBuilder.Build();
            world.AddEntity(hudCamera);
            hudViewport.CameraEntity = hudCamera;

            var cameraPos = hudCamera.Components.OfType<Position>().FirstOrDefault();
            cameraPos.Value = hudViewport.ViewportToWorldPoint(new Vector2(1.0f, 1.0f));

            var arial12 = world.Core.Rendering.Fonts.Create("ARIAL", 9);

            var fpsTextEntity = world.Core.Entities.Create();

            fpsTextEntity.Add(Position.Create(0, 0));
            fpsTextEntity.Add(TextComponent.Create(arial12.Id, Vector2.Zero, "FPS: 0.0", 100.0f));
            world.AddEntity(fpsTextEntity);

            world.Core.Jobs.Execute(new FpsTextUpdateJob(fpsTextEntity));
        }

        #endregion Private Methods
    }
}