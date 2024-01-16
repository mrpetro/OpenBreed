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
            systemFactory.RegisterSystem<DirectionSystemVanilla>(() => new DirectionSystemVanilla(
                serviceProvider.GetService<IEntityMan>(),
                serviceProvider.GetService<IEventsMan>()));
            systemFactory.RegisterSystem<MovementSystem>(() => new MovementSystem(
                serviceProvider.GetService<IEntityMan>(),
                serviceProvider.GetService<IEventsMan>()));
            systemFactory.RegisterSystem<VelocityChangedSystem>(() => new VelocityChangedSystem(
                serviceProvider.GetService<IEntityMan>(),
                serviceProvider.GetService<IEventsMan>()));
            systemFactory.RegisterSystem<MovementSystemVanilla>(() => new MovementSystemVanilla(
                serviceProvider.GetService<IEntityMan>(),
                serviceProvider.GetService<IEventsMan>()));
            systemFactory.RegisterSystem<DynamicBodiesCollisionCheckSystem>(() => new DynamicBodiesCollisionCheckSystem(
                serviceProvider.GetService<IEntityMan>(),
                serviceProvider.GetService<IShapeMan>(),
                serviceProvider.GetService<ICollisionMan<IEntity>>()));

            systemFactory.RegisterSystem<AddDynamicBodySystem>(() => new AddDynamicBodySystem(
                serviceProvider.GetService<IEventsMan>(),
                serviceProvider.GetService<IEntityMan>(),
                serviceProvider.GetService<IWorldMan>()));

            systemFactory.RegisterSystem<RemoveDynamicBodySystem>(() => new RemoveDynamicBodySystem(
                serviceProvider.GetService<IEventsMan>(),
                serviceProvider.GetService<IEntityMan>(),
                serviceProvider.GetService<IWorldMan>()));

            systemFactory.RegisterSystem<UpdateDynamicBodySystem>(() => new UpdateDynamicBodySystem(
                serviceProvider.GetService<IEventsMan>(),
                serviceProvider.GetService<IEntityMan>(),
                serviceProvider.GetService<IWorldMan>()));

            systemFactory.RegisterSystem<AddStaticBodySystem>(() => new AddStaticBodySystem(
                serviceProvider.GetService<IEventsMan>(),
                serviceProvider.GetService<IEntityMan>(),
                serviceProvider.GetService<IWorldMan>()));

            systemFactory.RegisterSystem<RemoveStaticBodySystem>(() => new RemoveStaticBodySystem(
                serviceProvider.GetService<IEventsMan>(),
                serviceProvider.GetService<IEntityMan>(),
                serviceProvider.GetService<IWorldMan>()));

        }

    }
}
