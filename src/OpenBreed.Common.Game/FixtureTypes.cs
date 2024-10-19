using OpenBreed.Physics.Interface.Managers;

namespace OpenBreed.Common.Game
{
    public class FixtureTypes
    {
        #region Private Fields

        private readonly IShapeMan shapeMan;

        #endregion Private Fields

        #region Public Constructors

        public FixtureTypes(IShapeMan shapeMan)
        {
            this.shapeMan = shapeMan;
        }

        #endregion Public Constructors

        #region Public Properties

        public int HeroBody { get; private set; }
        public int HeroTrigger { get; private set; }
        public int ObstacleCellBody { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void Register()
        {
            //HeroBody = fixtureMan.Create("Fixtures/HeroBody", shapeMan.GetByTag("Shapes/Box_0_0_28_28")).Id;
            //HeroTrigger = fixtureMan.Create("Fixtures/HeroTrigger", shapeMan.GetByTag("Shapes/Point_14_14")).Id;
            //ObstacleCellBody = fixtureMan.Create("Fixtures/GridCell", shapeMan.GetByTag("Shapes/Box_0_0_16_16")).Id;

            //fixtureMan.Create("Fixtures/TeleportEntry", shapeMan.GetByTag("Shapes/Box_16_16_8_8"));
            //fixtureMan.Create("Fixtures/TeleportExit", shapeMan.GetByTag("Shapes/Box_16_16_8_8"));
            //fixtureMan.Create("Fixtures/Projectile", shapeMan.GetByTag("Shapes/Box_0_0_16_16"));
            //fixtureMan.Create("Fixtures/DoorVertical", shapeMan.GetByTag("Shapes/Box_0_0_16_32"));
            //fixtureMan.Create("Fixtures/DoorHorizontal", shapeMan.GetByTag("Shapes/Box_0_0_32_16"));
            //fixtureMan.Create("Fixtures/Arrow", shapeMan.GetByTag("Shapes/Box_0_0_32_32"));
            //fixtureMan.Create("Fixtures/Turret", shapeMan.GetByTag("Shapes/Box_0_0_32_32"));
        }

        #endregion Public Methods
    }
}