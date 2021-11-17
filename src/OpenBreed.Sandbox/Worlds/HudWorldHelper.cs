using OpenBreed.Core;
using OpenBreed.Sandbox.Entities.Hud;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Animation;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Wecs.Systems.Rendering.Events;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using System.Linq;

namespace OpenBreed.Sandbox.Worlds
{
    public class HudWorldHelper
    {
        #region Private Fields

        private readonly ISystemFactory systemFactory;
        private readonly IWorldMan worldMan;
        private readonly IViewClient viewClient;
        private readonly IEntityMan entityMan;
        private readonly IEntityFactory entityFactory;
        private readonly HudHelper hudHelper;

        #endregion Private Fields

        #region Public Constructors

        public HudWorldHelper(ISystemFactory systemFactory, IWorldMan worldMan, IViewClient viewClient, IEntityMan entityMan, IEntityFactory entityFactory, HudHelper hudHelper)
        {
            this.systemFactory = systemFactory;
            this.worldMan = worldMan;
            this.viewClient = viewClient;
            this.entityMan = entityMan;
            this.entityFactory = entityFactory;
            this.hudHelper = hudHelper;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Create()
        {
            var builder = worldMan.Create().SetName("HUD");

            AddSystems(builder);

            Setup(builder.Build());
        }

        #endregion Public Methods

        #region Private Methods

        private void AddSystems(WorldBuilder builder)
        {
            //Input
            //builder.AddSystem(core.CreateWalkingControlSystem().Build());
            //builder.AddSystem(core.CreateAiControlSystem().Build());

            //Action
            //builder.AddSystem(core.CreateMovementSystem().Build());
            builder.AddSystem(systemFactory.Create<AnimatorSystem>());

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

        private void Setup(World world)
        {
            var hudCamera = entityFactory.Create(@"Entities\Common\Camera.xml")
                .SetParameter("posX", 0.0)
                .SetParameter("posY", 0.0)
                .SetParameter("width", viewClient.ClientRectangle.Width)
                .SetParameter("height", viewClient.ClientRectangle.Height)
                .Build();

            hudCamera.Tag = "HudCamera";

            hudCamera.EnterWorld(world.Id);

            hudHelper.AddFpsCounter(world);
            hudHelper.AddPositionInfo(world);

            var hudViewport = entityMan.GetByTag(ScreenWorldHelper.HUD_VIEWPORT).First();
            hudViewport.SetViewportCamera(hudCamera.Id);

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