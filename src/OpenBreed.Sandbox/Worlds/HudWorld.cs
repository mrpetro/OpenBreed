using OpenBreed.Core;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Systems.Control.Systems;
using OpenBreed.Core.Modules.Physics.Systems;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Entities.Builders;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenTK;
using System.Linq;

namespace OpenBreed.Sandbox.Worlds
{
    public class HudWorld : World
    {
        #region Private Fields

        //private IEntity worldTextEntity;
        private IEntity fpsTextEntity;
        private Viewport hudViewport;

        #endregion Private Fields

        #region Public Constructors

        public HudWorld(ICore core) : base(core)
        {
            //Input
            AddSystem(new WalkingControlSystem(core));
            AddSystem(new AiControlSystem(core));

            //Action
            AddSystem(new MovementSystem(core));
            AddSystem(Core.Animations.CreateAnimationSystem<int>());

            //Audio
            AddSystem(Core.Sounds.CreateSoundSystem());

            //Video
            AddSystem(Core.Rendering.CreateTileSystem(64, 64, 1, 16, false));
            AddSystem(Core.Rendering.CreateSpriteSystem());
            AddSystem(Core.Rendering.CreateTextSystem());
        }

        #endregion Public Constructors

        #region Public Methods

        public void Update()
        {
            fpsTextEntity.PostMsg(new TextSetMsg(fpsTextEntity, $"FPS: {Core.Rendering.Fps.ToString("0.00")}"));
        }

        public void Setup()
        {
            hudViewport = (Viewport)Core.Rendering.Viewports.Create(0.0f, 0.0f, 1.0f, 1.0f, 100.0f);
            Core.Rendering.Viewports.Add(hudViewport);
            hudViewport.DrawBorder = true;
            hudViewport.DrawBackgroud = false;
            hudViewport.Clipping = false;

            var cameraBuilder = new CameraBuilder(Core);
            cameraBuilder.SetPosition(new Vector2(0, 0));
            cameraBuilder.SetRotation(0.0f);
            cameraBuilder.SetZoom(1.0f);
            var hudCamera = cameraBuilder.Build();
            AddEntity(hudCamera);
            hudViewport.CameraEntity = hudCamera;

            var cameraPos = hudCamera.Components.OfType<IPosition>().FirstOrDefault();
            cameraPos.Value = hudViewport.ViewportToWorldPoint(new Vector2(1.0f, 1.0f));

            var arial12 = Core.Rendering.Fonts.Create("ARIAL", 12);

            //worldTextEntity = Core.Entities.Create();
            //worldTextEntity.Add(Position.Create(0, 15));
            //worldTextEntity.Add(TextComponent.Create(arial12.Id, Vector2.Zero, "World: none", 100.0f));
            //AddEntity(worldTextEntity);

            fpsTextEntity = Core.Entities.Create();

            fpsTextEntity.Add(Position.Create(0, 0));
            fpsTextEntity.Add(TextComponent.Create(arial12.Id, Vector2.Zero, "FPS: 0.0", 100.0f));
            AddEntity(fpsTextEntity);

            Core.Worlds.Add(this);
        }

        #endregion Public Methods
    }
}