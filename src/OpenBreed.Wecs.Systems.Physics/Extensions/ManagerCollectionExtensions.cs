using OpenBreed.Common;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Physics.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Physics.Extensions
{
    public static class ManagerCollectionExtensions
    {
        public static void SetupPhysicsSystems(this IManagerCollection manCollection)
        {
            var systemFactory = manCollection.GetManager<ISystemFactory>();
            systemFactory.Register(() => new DirectionSystem(manCollection.GetManager<IEntityMan>()));
            systemFactory.Register(() => new MovementSystem(manCollection.GetManager<IEntityMan>()));
            systemFactory.Register(() => new PhysicsSystem(manCollection.GetManager<IEntityMan>(),
                                                           manCollection.GetManager<IFixtureMan>(),
                                                           manCollection.GetManager<ICollisionMan>()));


            var entityCommandHandler = manCollection.GetManager<EntityCommandHandler>();
            entityCommandHandler.BindCommand<BodyOffCommand, PhysicsSystem>();
            entityCommandHandler.BindCommand<BodyOnCommand, PhysicsSystem>();
        }
    }
}
