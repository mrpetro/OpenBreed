using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Common;
using OpenBreed.Core.Managers;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems.Physics.Helpers;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Physics.Extensions
{
    public static class HostBuilderExtensions
    {
        public static void SetupDynamicResolver(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<DynamicResolver>();
            });
        }

        public static void SetupPhysicsSystems(this ISystemFactory systemFactory, IServiceProvider serviceProvider)
        {
            systemFactory.RegisterSystem<DirectionSystemVanilla>((world) => new DirectionSystemVanilla(
                world,
                serviceProvider.GetService<IEntityMan>()));
            systemFactory.RegisterSystem<MovementSystem>((world) => new MovementSystem(
                world,
                serviceProvider.GetService<IEntityMan>()));
            systemFactory.RegisterSystem<VelocityChangedSystem>((world) => new VelocityChangedSystem(
                world,
                serviceProvider.GetService<IEntityMan>()));
            systemFactory.RegisterSystem<MovementSystemVanilla>((world) => new MovementSystemVanilla(
                world,
                serviceProvider.GetService<IEntityMan>()));
            systemFactory.RegisterSystem<DynamicBodiesCollisionCheckSystem>((world) => new DynamicBodiesCollisionCheckSystem(
                world,
                serviceProvider.GetService<IEntityMan>(),
                serviceProvider.GetService<IShapeMan>(),
                serviceProvider.GetService<ICollisionMan<IEntity>>()));

            systemFactory.RegisterSystem<AddDynamicBodySystem>((world) => new AddDynamicBodySystem(
                serviceProvider.GetService<IEventsMan>(),
                serviceProvider.GetService<IEntityMan>(),
                serviceProvider.GetService<IWorldMan>()));

            systemFactory.RegisterSystem<RemoveDynamicBodySystem>((world) => new RemoveDynamicBodySystem(
                serviceProvider.GetService<IEventsMan>(),
                serviceProvider.GetService<IEntityMan>(),
                serviceProvider.GetService<IWorldMan>()));

            systemFactory.RegisterSystem<UpdateDynamicBodySystem>((world) => new UpdateDynamicBodySystem(
                serviceProvider.GetService<IEventsMan>(),
                serviceProvider.GetService<IEntityMan>(),
                serviceProvider.GetService<IWorldMan>()));

            systemFactory.RegisterSystem<AddStaticBodySystem>((world) => new AddStaticBodySystem(
                serviceProvider.GetService<IEventsMan>(),
                serviceProvider.GetService<IEntityMan>(),
                serviceProvider.GetService<IWorldMan>()));

            systemFactory.RegisterSystem<RemoveStaticBodySystem>((world) => new RemoveStaticBodySystem(
                serviceProvider.GetService<IEventsMan>(),
                serviceProvider.GetService<IEntityMan>(),
                serviceProvider.GetService<IWorldMan>()));

            //systemFactory.RegisterSystem<StaticBodiesSystem>((world) => new StaticBodiesSystem(
            //    world,
            //    serviceProvider.GetService<IEntityMan>(),
            //    serviceProvider.GetService<IShapeMan>(),
            //    serviceProvider.GetService<IEventsMan>()));

            //systemFactory.Register<CollisionResponseSystem>(() => new CollisionResponseSystem(serviceProvider.GetService<IEntityMan>(),
            //                                                         serviceProvider.GetService<IWorldMan>(),
            //                                                         serviceProvider.GetService<ICollisionMan<Entity>>()));
        }

    }
}
