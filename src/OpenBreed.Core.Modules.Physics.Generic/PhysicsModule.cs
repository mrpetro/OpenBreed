using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules.Physics.Helpers;
using System;

namespace OpenBreed.Core.Modules.Physics
{
    public class PhysicsModule : BaseCoreModule
    {
        #region Public Constructors

        public PhysicsModule(ICore core) : base(core)
        {
            Shapes = core.GetManager<IShapeMan>();
            Fixturs = core.GetManager<IFixtureMan>();
            Collisions = core.GetManager<ICollisionMan>();
        }

        public static void AddManagers(IManagerCollection manCollection)
        {
            manCollection.AddSingleton<IShapeMan>(() => new ShapeMan(manCollection.GetManager<ILogger>()));
            manCollection.AddSingleton<IFixtureMan>(() => new FixtureMan(manCollection.GetManager<ILogger>()));
            manCollection.AddSingleton<ICollisionMan>(() => new CollisionMan(manCollection.GetManager<ILogger>()));
        }

        #endregion Public Constructors

        #region Public Properties

        public IFixtureMan Fixturs { get; }

        public IShapeMan Shapes { get; }

        public ICollisionMan Collisions { get; }


        #endregion Public Properties
    }
}