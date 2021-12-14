using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Sandbox.Worlds;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Rendering.Events;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Entities.Hud
{
    public class VanillaStatusBarHelper
    {
        #region Private Fields

        private readonly IEntityFactory entityFactory;
        private readonly IEntityMan entityMan;
        private readonly IViewClient viewClient;
        private readonly IJobsMan jobsMan;
        private readonly IRenderingMan renderingMan;

        #endregion Private Fields

        #region Public Constructors

        public VanillaStatusBarHelper(IEntityFactory entityFactory,
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

        public void AddP1StatusBar(World world)
        {
            var p1StatusBarEntity = entityFactory.Create(@"Defaults\Templates\ABTA\Common\Hud\StatusBarP1.xml")
                .SetParameter("posX", -viewClient.ClientRectangle.Width / 2.0f)
                .SetParameter("posY", -viewClient.ClientRectangle.Height / 2.0f)
                .Build();

            p1StatusBarEntity.EnterWorld(world.Id);
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
                .SetParameter("posX", viewClient.ClientRectangle.Width / 2.0f - 180.0f)
                .SetParameter("posY", -viewClient.ClientRectangle.Height / 2.0f)
                .Build();

            positionInfo.EnterWorld(world.Id);

            var hudViewport = entityMan.GetByTag(ScreenWorldHelper.DEBUG_HUD_VIEWPORT).First();

            //jobsMan.Execute(new JohnPositionTextUpdateJob(entityMan, positionInfo));
            hudViewport.Subscribe<ViewportResizedEventArgs>((s, a) => UpdatePositionInfoPos(positionInfo, a));
        }

        private static void UpdatePositionInfoPos(Entity fpsTextEntity, ViewportResizedEventArgs a)
        {
            fpsTextEntity.Get<PositionComponent>().Value = new Vector2(a.Width / 2.0f - 180.0f, -a.Height / 2.0f);
        }

        #endregion Private Methods
    }
}
