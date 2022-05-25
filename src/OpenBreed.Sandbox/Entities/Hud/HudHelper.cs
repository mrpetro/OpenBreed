using OpenBreed.Common;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Sandbox.Jobs;
using OpenBreed.Sandbox.Worlds;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Events;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Mathematics;
using System;
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
        private readonly ITriggerMan triggerMan;

        #endregion Private Fields

        #region Public Constructors

        public HudHelper(IEntityFactory entityFactory,
                         IEntityMan entityMan,
                         IViewClient viewClient,
                         IJobsMan jobsMan,
                         IRenderingMan renderingMan, 
                         ITriggerMan triggerMan)
        {
            this.entityFactory = entityFactory;
            this.entityMan = entityMan;
            this.viewClient = viewClient;
            this.jobsMan = jobsMan;
            this.renderingMan = renderingMan;
            this.triggerMan = triggerMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void AddFpsCounter(World world)
        {
            var fpsCounter = entityFactory.Create(@"Defaults\Templates\ABTA\Common\Hud\FpsCounter.xml")
                .SetParameter("posX", -viewClient.ClientRectangle.Size.X / 2.0f)
                .SetParameter("posY", -viewClient.ClientRectangle.Size.Y / 2.0f)
                .Build();

            triggerMan.OnWorldInitialized(world, () => fpsCounter.EnterWorld(world.Id), singleTime: true);

            var hudViewport = entityMan.GetByTag(ScreenWorldHelper.DEBUG_HUD_VIEWPORT).First();

            jobsMan.Execute(new FpsTextUpdateJob(renderingMan, fpsCounter));

            triggerMan.OnEntityViewportResized(hudViewport, (a) => UpdateFpsCounterPos(fpsCounter, a));
        }



        #endregion Public Methods

        #region Private Methods

        private static void UpdateFpsCounterPos(Entity fpsTextEntity, ViewportResizedEventArgs a)
        {
            fpsTextEntity.Get<PositionComponent>().Value = new Vector2(-a.Width / 2.0f, -a.Height / 2.0f);
        }

        public void AddPositionInfo(World world)
        {
            var positionInfo = entityFactory.Create(@"Defaults\Templates\ABTA\Common\Hud\PositionInfo.xml")
                .SetParameter("posX", viewClient.ClientRectangle.Size.X / 2.0f - 180.0f)
                .SetParameter("posY", -viewClient.ClientRectangle.Size.Y / 2.0f)
                .Build();


            triggerMan.OnWorldInitialized(world, () => positionInfo.EnterWorld(world.Id), singleTime: true);

            var hudViewport = entityMan.GetByTag(ScreenWorldHelper.DEBUG_HUD_VIEWPORT).First();

            jobsMan.Execute(new JohnPositionTextUpdateJob(entityMan, positionInfo));



            triggerMan.OnEntityViewportResized(hudViewport, (a) => UpdatePositionInfoPos(positionInfo, a));
        }

        private static void UpdatePositionInfoPos(Entity fpsTextEntity, ViewportResizedEventArgs a)
        {
            fpsTextEntity.Get<PositionComponent>().Value = new Vector2(a.Width / 2.0f - 180.0f, -a.Height / 2.0f);
        }

        #endregion Private Methods
    }
}