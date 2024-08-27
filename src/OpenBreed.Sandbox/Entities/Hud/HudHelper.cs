using OpenBreed.Common;
using OpenBreed.Core;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Rendering.Interface;
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
        private readonly IWorldMan worldMan;
        private readonly IWindow viewClient;
        private readonly IJobsMan jobsMan;
        private readonly ITriggerMan triggerMan;

        #endregion Private Fields

        #region Public Constructors

        public HudHelper(IEntityFactory entityFactory,
                         IEntityMan entityMan,
                         IWorldMan worldMan,
                         IWindow viewClient,
                         IJobsMan jobsMan,
                         ITriggerMan triggerMan)
        {
            this.entityFactory = entityFactory;
            this.entityMan = entityMan;
            this.worldMan = worldMan;
            this.viewClient = viewClient;
            this.jobsMan = jobsMan;
            this.triggerMan = triggerMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void AddFpsCounter(IWorld world)
        {
            var fpsCounter = entityFactory.Create(@"ABTA\Templates\Common\Hud\FpsCounter")
                .SetParameter("posX", -viewClient.ClientRectangle.Size.X / 2.0f)
                .SetParameter("posY", -viewClient.ClientRectangle.Size.Y / 2.0f)
                .Build();

            triggerMan.OnWorldInitialized(world, () => worldMan.RequestAddEntity(fpsCounter, world.Id), singleTime: true);
        }

        public void AddCursor(IWorld world)
        {
            var fpsCounter = entityFactory.Create(@"ABTA\Templates\Common\Hud\Cursor")
                .SetParameter("posX", 0.0f)
                .SetParameter("posY", 0.0f)
                .Build();

            triggerMan.OnWorldInitialized(world, () => worldMan.RequestAddEntity(fpsCounter, world.Id), singleTime: true);
        }

        #endregion Public Methods

        #region Private Methods

        public void AddPositionInfo(IWorld world)
        {
            var positionInfo = entityFactory.Create(@"ABTA\Templates\Common\Hud\PositionInfo")
                .SetParameter("posX", viewClient.ClientRectangle.Size.X / 2.0f - 180.0f)
                .SetParameter("posY", -viewClient.ClientRectangle.Size.Y / 2.0f)
                .Build();


            triggerMan.OnWorldInitialized(world, () => worldMan.RequestAddEntity(positionInfo, world.Id), singleTime: true);

            var hudViewport = entityMan.GetByTag(ScreenWorldHelper.DEBUG_HUD_VIEWPORT).First();

            jobsMan.Execute(new JohnPositionTextUpdateJob(entityMan, positionInfo));



            triggerMan.OnEntityViewportResized(hudViewport, (e, a) => UpdatePositionInfoPos(positionInfo, a));
        }

        private static void UpdatePositionInfoPos(IEntity fpsTextEntity, ViewportResizedEvent a)
        {
            fpsTextEntity.Get<PositionComponent>().Value = new Vector2(a.Width / 2.0f - 180.0f, -a.Height / 2.0f);
        }

        #endregion Private Methods
    }
}