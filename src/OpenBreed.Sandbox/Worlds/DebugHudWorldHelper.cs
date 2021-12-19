using OpenBreed.Common;
using OpenBreed.Core;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Model.Palettes;
using OpenBreed.Rendering.Interface.Data;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenBreed.Sandbox.Entities;
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
    public class DebugHudWorldHelper
    {
        #region Private Fields

        private readonly ISystemFactory systemFactory;
        private readonly IRenderableFactory renderableFactory;
        private readonly IWorldMan worldMan;
        private readonly IEntityMan entityMan;
        private readonly HudHelper hudHelper;
        private readonly CameraHelper cameraHelper;
        private readonly IViewClient viewClient;

        #endregion Private Fields

        #region Public Constructors

        public DebugHudWorldHelper(ISystemFactory systemFactory, IRenderableFactory renderableFactory, IWorldMan worldMan, IEntityMan entityMan, HudHelper hudHelper, CameraHelper cameraHelper, IViewClient viewClient)
        {
            this.systemFactory = systemFactory;
            this.renderableFactory = renderableFactory;
            this.worldMan = worldMan;
            this.entityMan = entityMan;
            this.hudHelper = hudHelper;
            this.cameraHelper = cameraHelper;
            this.viewClient = viewClient;
        }

        #endregion Public Constructors

        #region Public Methods



        public void Create()
        {
            var builder = worldMan.Create().SetName("DebugHUD");
            builder.AddModule<IRenderableBatch>(renderableFactory.CreateRenderableBatch());

            AddSystems(builder);

            Setup(builder.Build());
        }

        #endregion Public Methods

        #region Private Methods

        private void AddSystems(WorldBuilder builder)
        {
            builder.AddSystem(systemFactory.Create<AnimatorSystem>());
            builder.AddSystem(systemFactory.Create<TextSystem>());
        }

        private void Setup(World world)
        {
            var hudCamera = cameraHelper.CreateCamera(0.0f,
                                                      0.0f,
                                                      viewClient.ClientRectangle.Width,
                                                      viewClient.ClientRectangle.Height);

            hudCamera.Tag = "DebugHudCamera";

            hudCamera.EnterWorld(world.Id);

            hudHelper.AddFpsCounter(world);
            hudHelper.AddPositionInfo(world);

            var hudViewport = entityMan.GetByTag(ScreenWorldHelper.DEBUG_HUD_VIEWPORT).First();
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