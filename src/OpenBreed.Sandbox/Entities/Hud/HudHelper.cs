using OpenBreed.Common;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Sandbox.Jobs;
using OpenBreed.Sandbox.Worlds;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Rendering.Events;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Graphics;
using System.Linq;

namespace OpenBreed.Sandbox.Entities.Hud
{
    public class HudHelper
    {
        #region Private Fields

        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly IEntityFactory entityFactory;
        private readonly IEntityMan entityMan;
        private readonly IViewClient viewClient;
        private readonly IBuilderFactory builderFactory;
        private readonly IFontMan fontMan;
        private readonly IJobsMan jobsMan;
        private readonly IRenderingMan renderingMan;

        #endregion Private Fields

        #region Public Constructors

        public HudHelper(IDataLoaderFactory dataLoaderFactory,
                         IEntityFactory entityFactory,
                         IEntityMan entityMan,
                         IViewClient viewClient,
                         IBuilderFactory builderFactory,
                         IFontMan fontMan,
                         IJobsMan jobsMan,
                         IRenderingMan renderingMan)
        {
            this.dataLoaderFactory = dataLoaderFactory;
            this.entityFactory = entityFactory;
            this.entityMan = entityMan;
            this.viewClient = viewClient;
            this.builderFactory = builderFactory;
            this.fontMan = fontMan;
            this.jobsMan = jobsMan;
            this.renderingMan = renderingMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void AddFpsCounter(World world)
        {
            var arial12 = fontMan.Create("ARIAL", 10);

            var fpsTextEntity = entityMan.Create();

            fpsTextEntity.Add(PositionComponent.Create(new Vector2(-viewClient.ClientRectangle.Width / 2.0f, -viewClient.ClientRectangle.Height / 2.0f)));

            var textBuilder = builderFactory.GetBuilder<TextComponentBuilder>();
            textBuilder.SetFontById(arial12.Id);
            textBuilder.SetOffset(Vector2.Zero);
            textBuilder.SetColor(Color4.White);
            textBuilder.SetText("FPS: 0.0");
            textBuilder.SetOrder(100);

            fpsTextEntity.Add(textBuilder.Build());
            fpsTextEntity.EnterWorld(world.Id);

            var hudViewport = entityMan.GetByTag(ScreenWorldHelper.HUD_VIEWPORT).First();

            jobsMan.Execute(new FpsTextUpdateJob(renderingMan, fpsTextEntity));
            hudViewport.Subscribe<ViewportResizedEventArgs>((s, a) => UpdateFpsPos(fpsTextEntity, a));
        }

        #endregion Public Methods

        #region Private Methods

        private static void UpdateFpsPos(Entity fpsTextEntity, ViewportResizedEventArgs a)
        {
            fpsTextEntity.Get<PositionComponent>().Value = new Vector2(-a.Width / 2.0f, -a.Height / 2.0f);
        }

        public void AddPositionInfo(World world)
        {
            var arial12 = fontMan.Create("ARIAL", 10);

            var entity = entityMan.Create();

            entity.Add(PositionComponent.Create(new Vector2(viewClient.ClientRectangle.Width / 2.0f - 180.0f, -viewClient.ClientRectangle.Height / 2.0f)));

            var textBuilder = builderFactory.GetBuilder<TextComponentBuilder>();
            textBuilder.SetFontById(arial12.Id);
            textBuilder.SetOffset(Vector2.Zero);
            textBuilder.SetColor(Color4.White);
            textBuilder.SetText("Coords: (0.0, 0.0)");
            textBuilder.SetOrder(100);

            entity.Add(textBuilder.Build());
            entity.EnterWorld(world.Id);

            var hudViewport = entityMan.GetByTag(ScreenWorldHelper.HUD_VIEWPORT).First();

            jobsMan.Execute(new JohnPositionTextUpdateJob(entityMan, entity));
            hudViewport.Subscribe<ViewportResizedEventArgs>((s, a) => UpdatePos(entity, a));
        }

        private static void UpdatePos(Entity fpsTextEntity, ViewportResizedEventArgs a)
        {
            fpsTextEntity.Get<PositionComponent>().Value = new Vector2(a.Width / 2.0f - 180.0f, -a.Height / 2.0f);
        }

        #endregion Private Methods
    }
}