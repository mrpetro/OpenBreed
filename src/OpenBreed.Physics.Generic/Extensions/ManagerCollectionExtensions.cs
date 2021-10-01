using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Physics.Generic.Managers;
using OpenBreed.Physics.Interface.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Physics.Generic.Extensions
{
    public static class ManagerCollectionExtensions
    {
        public static void SetupGenericPhysicsManagers(this IManagerCollection manCollection)
        {
            manCollection.AddSingleton<IShapeMan>(() => new ShapeMan(manCollection.GetManager<ILogger>()));
            manCollection.AddSingleton<IFixtureMan>(() => new FixtureMan(manCollection.GetManager<ILogger>()));
            manCollection.AddSingleton<ICollisionMan>(() => new CollisionMan(manCollection.GetManager<ILogger>()));
            manCollection.AddSingleton<IBroadphaseFactory>(() => new BroadphaseFactory(manCollection.GetManager<ILogger>()));
        }
    }
}
