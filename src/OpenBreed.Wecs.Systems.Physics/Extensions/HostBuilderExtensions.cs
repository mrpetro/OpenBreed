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
            systemFactory.Register(() => new DirectionSystemVanilla(serviceProvider.GetService<IEntityMan>()));
            systemFactory.Register(() => new MovementSystem(serviceProvider.GetService<IEntityMan>()));
            systemFactory.Register(() => new MovementSystemVanilla(serviceProvider.GetService<IEntityMan>()));
            systemFactory.Register(() => new DynamicBodiesAabbUpdaterSystem(serviceProvider.GetService<IShapeMan>()));
            systemFactory.Register(() => new DynamicBodiesCollisionCheckSystem(serviceProvider.GetService<IEntityMan>(),
                                                                               serviceProvider.GetService<IShapeMan>(),
                                                                               serviceProvider.GetService<ICollisionMan<Entity>>()));
            systemFactory.Register(() => new StaticBodiesSystem(serviceProvider.GetService<IEntityMan>(),
                                                                serviceProvider.GetService<IShapeMan>(),
                                                                serviceProvider.GetService<IEventsMan>()));

            systemFactory.Register(() => new CollisionResponseSystem(serviceProvider.GetService<IEntityMan>(),
                                                                     serviceProvider.GetService<IWorldMan>(),
                                                                     serviceProvider.GetService<ICollisionMan<Entity>>()));
        }

    }
}
