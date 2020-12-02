using OpenBreed.Core.Modules.Physics.Helpers;
using System;

namespace OpenBreed.Core.Modules.Physics
{
    public class PhysicsModule : BaseCoreModule
    {
        #region Public Constructors

        public PhysicsModule(ICore core) : base(core)
        {
            Shapes = new ShapeMan(this);
            Fixturs = new FixtureMan(this);
            Collisions = new CollisionMan(this);
       }

        #endregion Public Constructors

        #region Public Properties

        public FixtureMan Fixturs { get; }

        public ShapeMan Shapes { get; }

        public CollisionMan Collisions { get; }


        #endregion Public Properties
    }
}