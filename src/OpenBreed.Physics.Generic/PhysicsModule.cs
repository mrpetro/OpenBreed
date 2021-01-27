using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules;
using OpenBreed.Physics.Generic.Helpers;
using OpenBreed.Physics.Generic.Managers;
using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using System;

namespace OpenBreed.Physics.Generic
{
    public class PhysicsModule : BaseCoreModule, IPhysicsModule
    {
        #region Public Constructors

        public PhysicsModule(ICore core) : base(core)
        {
            Shapes = core.GetManager<IShapeMan>();
            Fixturs = core.GetManager<IFixtureMan>();
            Collisions = core.GetManager<ICollisionMan>();
        }

        public IPhysicalWorld CreateWorld()
        {
            throw new NotImplementedException();
        }

        #endregion Public Constructors

        #region Public Properties

        public IFixtureMan Fixturs { get; }

        public IShapeMan Shapes { get; }

        public ICollisionMan Collisions { get; }


        #endregion Public Properties
    }
}