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

        private readonly IEntityFactory entityFactory;
        private readonly IEntityMan entityMan;
        private readonly IViewClient viewClient;
        private readonly IJobsMan jobsMan;
        private readonly IRenderingMan renderingMan;

        #endregion Private Fields

        #region Public Constructors

        public HudHelper(IEntityFactory entityFactory,
                         IEntityMan entityMan,
                         IViewClient viewClient,
                         IJobsMan jobsMan,
                         IRenderingMan renderingMan)
        {
            this.entityFactory = entityFactory;
            this.entityMan = entityMan;
            this.viewClient = viewClient;
            this.jobsMan = jobsMan;
            this.renderingMan = renderingMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void AddFpsCounter(World world)
        {
            var fpsCounter = entityFactory.Create(@"Entities\Common\Hud\FpsCounter.xml")
                .SetParameter("posX", -viewClient.ClientRectangle.Width / 2.0f)
                .SetParameter("posY", -viewClient.ClientRectangle.Height / 2.0f)
                .Build();

            fpsCounter.EnterWorld(world.Id);

            var hudViewport = entityMan.GetByTag(ScreenWorldHelper.HUD_VIEWPORT).First();

            jobsMan.Execute(new FpsTextUpdateJob(renderingMan, fpsCounter));
            hudViewport.Subscribe<ViewportResizedEventArgs>((s, a) => UpdateFpsCounterPos(fpsCounter, a));
        }

        #endregion Public Methods

        #region Private Methods

        private static void UpdateFpsCounterPos(Entity fpsTextEntity, ViewportResizedEventArgs a)
        {
            fpsTextEntity.Get<PositionComponent>().Value = new Vector2(-a.Width / 2.0f, -a.Height / 2.0f);
        }

        public void AddPositionInfo(World world)
        {
            var positionInfo = entityFactory.Create(@"Entities\Common\Hud\PositionInfo.xml")
                .SetParameter("posX", viewClient.ClientRectangle.Width / 2.0f - 180.0f)
                .SetParameter("posY", -viewClient.ClientRectangle.Height / 2.0f)
                .Build();

            positionInfo.EnterWorld(world.Id);

            var hudViewport = entityMan.GetByTag(ScreenWorldHelper.HUD_VIEWPORT).First();

            jobsMan.Execute(new JohnPositionTextUpdateJob(entityMan, positionInfo));
            hudViewport.Subscribe<ViewportResizedEventArgs>((s, a) => UpdatePositionInfoPos(positionInfo, a));
        }

        private static void UpdatePositionInfoPos(Entity fpsTextEntity, ViewportResizedEventArgs a)
        {
            fpsTextEntity.Get<PositionComponent>().Value = new Vector2(a.Width / 2.0f - 180.0f, -a.Height / 2.0f);
        }

        #endregion Private Methods
    }
}