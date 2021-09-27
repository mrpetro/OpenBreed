using OpenBreed.Core;
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
using OpenBreed.Core.Managers;

namespace OpenBreed.Sandbox.Worlds
{
    public  class HudWorldHelper
    {
        private readonly ICore core;
        private readonly ICommandsMan commandsMan;
        private readonly ISystemFactory systemFactory;
        private readonly IWorldMan worldMan;
        private readonly IViewClient viewClient;
        private readonly IEntityMan entityMan;
        #region Public Methods

        public HudWorldHelper(ICore core, ICommandsMan commandsMan, ISystemFactory systemFactory, IWorldMan worldMan, IViewClient viewClient, IEntityMan entityMan)
        {
            this.core = core;
            this.commandsMan = commandsMan;
            this.systemFactory = systemFactory;
            this.worldMan = worldMan;
            this.viewClient = viewClient;
            this.entityMan = entityMan;
        }

        private void AddSystems(WorldBuilder builder)
        {
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

        public void Create()
        {
            var builder = worldMan.Create().SetName("HUD");

            AddSystems(builder);

            Setup(builder.Build());
        }

        #endregion Public Methods

        #region Private Methods

        private void Setup(World world)
        {
            var cameraBuilder = core.GetManager<CameraBuilder>();
            cameraBuilder.SetPosition(new Vector2(0, 0));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetFov(viewClient.ClientRectangle.Width, viewClient.ClientRectangle.Height);
            var hudCamera = cameraBuilder.Build();
            hudCamera.Tag = "HudCamera";
            commandsMan.Post(new AddEntityCommand(world.Id, hudCamera.Id));
            //world.AddEntity(hudCamera);

            FpsCounterHelper.AddToWorld(core, world);
            CursorCoordsHelper.AddToWorld(core, world);

            var hudViewport = entityMan.GetByTag(ScreenWorldHelper.HUD_VIEWPORT).First();
            hudViewport.Get<ViewportComponent>().CameraEntityId = hudCamera.Id;

            //world.Core.Rendering.Subscribe(GfxEventTypes.CLIENT_RESIZED, (s, a) => UpdateFpsPos(fpsTextEntity, (ClientResizedEventArgs)a));
            //world.Core.Rendering.Subscribe(GfxEventTypes.CLIENT_RESIZED, (s, a) => UpdateCameraFov(hudCamera, (ClientResizedEventArgs)a));

            hudViewport.Subscribe<ViewportResizedEventArgs>((s, a) => UpdateCameraFov(hudCamera, a));
        }

        private void UpdateCameraFov(Entity cameraEntity, ViewportResizedEventArgs a)
        {
            cameraEntity.Get<CameraComponent>().Width = a.Width;
            cameraEntity.Get<CameraComponent>().Height = a.Height;
        }

        #endregion Private Methods
    }
}