using OpenBreed.Common;
using OpenBreed.Core.Managers;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Physics.Helpers;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Physics.Extensions
{
    public static class ManagerCollectionExtensions
    {
        public static void SetupDynamicResolver(this IManagerCollection manCollection)
        {
            manCollection.AddSingleton<DynamicResolver>(() => new DynamicResolver(manCollection.GetManager<IEntityMan>()));
        }

        public static void SetupPhysicsSystems(this IManagerCollection manCollection)
        {
            var systemFactory = manCollection.GetManager<ISystemFactory>();
            systemFactory.Register(() => new DirectionSystem(manCollection.GetManager<IEntityMan>()));
            systemFactory.Register(() => new MovementSystem(manCollection.GetManager<IEntityMan>()));
            systemFactory.Register(() => new DynamicBodiesAabbUpdaterSystem(manCollection.GetManager<IShapeMan>()));
            systemFactory.Register(() => new DynamicBodiesCollisionCheckSystem(manCollection.GetManager<IEntityMan>(),
                                                                               manCollection.GetManager<IShapeMan>(),
                                                                               manCollection.GetManager<ICollisionMan>()));
            systemFactory.Register(() => new StaticBodiesSystem(manCollection.GetManager<IEntityMan>(),
                                                                manCollection.GetManager<IShapeMan>(),
                                                                manCollection.GetManager<IEventsMan>()));

            systemFactory.Register(() => new CollisionResponseSystem(manCollection.GetManager<IEntityMan>(),
                                                                     manCollection.GetManager<IWorldMan>(),
                                                                     manCollection.GetManager<ICollisionMan>()));
        }
    }
}
