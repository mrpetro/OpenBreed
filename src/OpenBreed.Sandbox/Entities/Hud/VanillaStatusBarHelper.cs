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

        public void AddLivesCounter(World world, int x, int y)
        {
            var timer = entityFactory.Create(@"Defaults\Templates\ABTA\Common\Hud\LivesCounter.xml")
                .SetParameter("posX", x)
                .SetParameter("posY", y)
                .Build();

            timer.EnterWorld(world.Id);
        }

        public void AddDestructTimer(World world, int x, int y)
        {
            var timer = entityFactory.Create(@"Defaults\Templates\ABTA\Common\Hud\DestructTimer.xml")
                .SetParameter("posX", x)
                .SetParameter("posY", y)
                .Build();

            timer.EnterWorld(world.Id);
        }

        public void AddKeysCounter(World world, int x, int y)
        {
            var timer = entityFactory.Create(@"Defaults\Templates\ABTA\Common\Hud\KeysCounter.xml")
                .SetParameter("posX", x)
                .SetParameter("posY", y)
                .Build();

            timer.EnterWorld(world.Id);
        }

        public void AddAmmoCounter(World world, int x, int y)
        {
            var timer = entityFactory.Create(@"Defaults\Templates\ABTA\Common\Hud\AmmoCounter.xml")
                .SetParameter("posX", x)
                .SetParameter("posY", y)
                .Build();

            timer.EnterWorld(world.Id);
        }

        public void AddAmmoBar(World world, int x, int y)
        {
            var timer = entityFactory.Create(@"Defaults\Templates\ABTA\Common\Hud\AmmoBar.xml")
                .SetParameter("posX", x)
                .SetParameter("posY", y)
                .Build();

            timer.EnterWorld(world.Id);
        }

        public void AddHealthBar(World world, int x, int y)
        {
            var timer = entityFactory.Create(@"Defaults\Templates\ABTA\Common\Hud\HealthBar.xml")
                .SetParameter("posX", x)
                .SetParameter("posY", y)
                .Build();

            timer.EnterWorld(world.Id);
        }

        public void AddP1StatusBar(World world)
        {
            var p1StatusBar = entityFactory.Create(@"Defaults\Templates\ABTA\Common\Hud\StatusBarP1.xml")
                .SetParameter("posX", -160)
                .SetParameter("posY", 109)
                .Build();

            p1StatusBar.EnterWorld(world.Id);
        }

        public void AddP2StatusBar(World world)
        {
            var p1StatusBar = entityFactory.Create(@"Defaults\Templates\ABTA\Common\Hud\StatusBarP2.xml")
                .SetParameter("posX", -160)
                .SetParameter("posY", -120)
                .Build();

            p1StatusBar.EnterWorld(world.Id);
        }

        #endregion Public Methods
    }
}