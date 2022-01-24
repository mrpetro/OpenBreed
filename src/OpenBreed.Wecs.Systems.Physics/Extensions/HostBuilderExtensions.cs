using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            systemFactory.Register<DirectionSystemVanilla>(() => new DirectionSystemVanilla(serviceProvider.GetService<IEntityMan>()));
            systemFactory.Register<MovementSystem>(() => new MovementSystem(serviceProvider.GetService<IEntityMan>()));
            systemFactory.Register<MovementSystemVanilla>(() => new MovementSystemVanilla(serviceProvider.GetService<IEntityMan>()));
            systemFactory.Register<DynamicBodiesAabbUpdaterSystem>(() => new DynamicBodiesAabbUpdaterSystem(serviceProvider.GetService<IShapeMan>()));
            systemFactory.Register<DynamicBodiesCollisionCheckSystem>(() => new DynamicBodiesCollisionCheckSystem(serviceProvider.GetService<IEntityMan>(),
                                                                               serviceProvider.GetService<IShapeMan>(),
                                                                               serviceProvider.GetService<ICollisionMan<Entity>>()));
            systemFactory.Register<StaticBodiesSystem>(() => new StaticBodiesSystem(serviceProvider.GetService<IEntityMan>(),
                                                                serviceProvider.GetService<IShapeMan>(),
                                                                serviceProvider.GetService<IEventsMan>()));

            systemFactory.Register<CollisionResponseSystem>(() => new CollisionResponseSystem(serviceProvider.GetService<IEntityMan>(),
                                                                     serviceProvider.GetService<IWorldMan>(),
                                                                     serviceProvider.GetService<ICollisionMan<Entity>>()));
        }

    }
}
