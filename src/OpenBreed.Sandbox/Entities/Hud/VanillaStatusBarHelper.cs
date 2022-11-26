using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;

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

        public void AddDestructTimer(IWorld world, int x, int y)
        {
            var timer = entityFactory.Create(@"Vanilla\ABTA\Templates\Common\Hud\DestructTimer.xml")
                .SetParameter("posX", x)
                .SetParameter("posY", y)
                .Build();

            timer.EnterWorld(world.Id);
        }

        public IEntity CreateHudElement(
            string elementName,
            string entityTag,
            int x,
            int y)
        {
            return entityFactory.Create(@$"Vanilla\ABTA\Templates\Common\Hud\{elementName}.xml")
                .SetParameter("posX", x)
                .SetParameter("posY", y)
                .SetTag(entityTag)
                .Build();
        }

        #endregion Public Methods
    }
}