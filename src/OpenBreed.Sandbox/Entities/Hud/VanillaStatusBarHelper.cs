using OpenBreed.Core;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Rendering.Interface;
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
        private readonly IWorldMan worldMan;
        private readonly IWindow viewClient;
        private readonly IJobsMan jobsMan;

        #endregion Private Fields

        #region Public Constructors

        public VanillaStatusBarHelper(IEntityFactory entityFactory,
                         IEntityMan entityMan,
                         IWorldMan worldMan,
                         IWindow viewClient,
                         IJobsMan jobsMan)
        {
            this.entityFactory = entityFactory;
            this.entityMan = entityMan;
            this.worldMan = worldMan;
            this.viewClient = viewClient;
            this.jobsMan = jobsMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void AddDestructTimer(IWorld world, int x, int y)
        {
            var timer = entityFactory.Create(@"ABTA\Templates\Common\Hud\DestructTimer")
                .SetParameter("posX", x)
                .SetParameter("posY", y)
                .Build();

            worldMan.RequestAddEntity(timer, world.Id);
        }

        public IEntity CreateHudElement(
            string elementName,
            string entityTag,
            int x,
            int y)
        {
            return entityFactory.Create(@$"ABTA\Templates\Common\Hud\{elementName}")
                .SetParameter("posX", x)
                .SetParameter("posY", y)
                .SetTag(entityTag)
                .Build();
        }

        #endregion Public Methods
    }
}