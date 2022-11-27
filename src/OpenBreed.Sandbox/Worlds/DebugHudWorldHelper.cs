using OpenBreed.Common;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
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
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Animation;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Wecs.Systems.Rendering.Events;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Systems.Scripting;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Mathematics;
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
        private readonly ITriggerMan triggerMan;

        #endregion Private Fields

        #region Public Constructors

        public DebugHudWorldHelper(ISystemFactory systemFactory, IRenderableFactory renderableFactory, IWorldMan worldMan, IEntityMan entityMan, HudHelper hudHelper, CameraHelper cameraHelper, IViewClient viewClient, ITriggerMan triggerMan)
        {
            this.systemFactory = systemFactory;
            this.renderableFactory = renderableFactory;
            this.worldMan = worldMan;
            this.entityMan = entityMan;
            this.hudHelper = hudHelper;
            this.cameraHelper = cameraHelper;
            this.viewClient = viewClient;
            this.triggerMan = triggerMan;
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
            builder.AddSystem<AnimatorSystem>();
            builder.AddSystem<TextSystem >();
            builder.AddSystem<ScriptRunningSystem>();
        }

        private void Setup(IWorld world)
        {
            var hudCamera = cameraHelper.CreateCamera(
                "Camera.DebugHud",
                0.0f,                                     
                0.0f,                                     
                viewClient.ClientRectangle.Size.X,                                        
                viewClient.ClientRectangle.Size.Y);

            triggerMan.OnWorldInitialized(world, () => hudCamera.EnterWorld(world.Id), singleTime: true);
 
            hudHelper.AddFpsCounter(world);
            hudHelper.AddPositionInfo(world);

            var hudViewport = entityMan.GetByTag(ScreenWorldHelper.DEBUG_HUD_VIEWPORT).First();
            hudViewport.SetViewportCamera(hudCamera.Id);

            triggerMan.OnEntityViewportResized(hudViewport, (e, a) => UpdateCameraFov(hudCamera, a));
        }

        private void UpdateCameraFov(IEntity cameraEntity, ViewportResizedEventArgs a)
        {
            cameraEntity.Get<CameraComponent>().Size = new Vector2(a.Width, a.Height);
        }

        #endregion Private Methods
    }
}