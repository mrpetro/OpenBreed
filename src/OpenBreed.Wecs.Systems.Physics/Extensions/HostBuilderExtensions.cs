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

        public static void SetupPhysicsSystems(this ISystemFactory systemFactory, IServiceProvider sp)
        {
            systemFactory.RegisterSystem<DirectionSystemVanilla>(() => new DirectionSystemVanilla(
                sp.GetService<IEntityMan>(),
                sp.GetService<IEventsMan>()));
            systemFactory.RegisterSystem<MovementSystem>(() => new MovementSystem(
                sp.GetService<IEntityMan>(),
                sp.GetService<IEventsMan>()));
            systemFactory.RegisterSystem<VelocityChangedSystem>(() => new VelocityChangedSystem(
                sp.GetService<IEntityMan>(),
                sp.GetService<IEventsMan>()));
            systemFactory.RegisterSystem<MovementSystemVanilla>(() => new MovementSystemVanilla(
                sp.GetService<IEntityMan>(),
                sp.GetService<IEventsMan>()));
            systemFactory.RegisterSystem<DynamicBodiesCollisionCheckSystem>(() => new DynamicBodiesCollisionCheckSystem(
                sp.GetService<IEntityMan>(),
                sp.GetService<IShapeMan>(),
                sp.GetService<ICollisionMan<IEntity>>(),
                sp.GetService<ICollisionChecker>()));

            systemFactory.RegisterSystem<AddDynamicBodySystem>(() => new AddDynamicBodySystem(
                sp.GetService<IEventsMan>(),
                sp.GetService<IEntityMan>(),
                sp.GetService<IWorldMan>()));

            systemFactory.RegisterSystem<RemoveDynamicBodySystem>(() => new RemoveDynamicBodySystem(
                sp.GetService<IEventsMan>(),
                sp.GetService<IEntityMan>(),
                sp.GetService<IWorldMan>()));

            systemFactory.RegisterSystem<UpdateDynamicBodySystem>(() => new UpdateDynamicBodySystem(
                sp.GetService<IEventsMan>(),
                sp.GetService<IEntityMan>(),
                sp.GetService<IWorldMan>()));

            systemFactory.RegisterSystem<AddStaticBodySystem>(() => new AddStaticBodySystem(
                sp.GetService<IEventsMan>(),
                sp.GetService<IEntityMan>(),
                sp.GetService<IWorldMan>()));

            systemFactory.RegisterSystem<RemoveStaticBodySystem>(() => new RemoveStaticBodySystem(
                sp.GetService<IEventsMan>(),
                sp.GetService<IEntityMan>(),
                sp.GetService<IWorldMan>()));

        }

    }
}
