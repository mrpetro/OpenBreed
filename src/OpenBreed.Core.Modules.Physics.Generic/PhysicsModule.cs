using OpenBreed.Core.Modules.Physics.Helpers;
using System;

namespace OpenBreed.Core.Modules.Physics
{
    public class PhysicsModule : BaseCoreModule
    {
        #region Public Constructors

        public PhysicsModule(ICore core) : base(core)
        {
            Fixturs = new FixtureMan(this);
            Collisions = new CollisionMan(this);
       }

        #endregion Public Constructors

        #region Public Properties

        public FixtureMan Fixturs { get; }

        public CollisionMan Collisions { get; }


        #endregion Public Properties
    }
}