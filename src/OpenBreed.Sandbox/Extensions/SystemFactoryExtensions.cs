using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Core.Managers;
using OpenBreed.Input.Interface;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Sandbox.Managers;
using OpenBreed.Sandbox.Wecs.Systems;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Worlds;
using System;

namespace OpenBreed.Sandbox.Extensions
{
    public static class SystemFactoryExtensions
    {
        #region Public Methods

        public static void SetupGameSystems(this ISystemFactory systemFactory, IServiceProvider sp)
        {
            systemFactory.RegisterSystem<ActorMovementByPlayerInputsSystem>(() => new ActorMovementByPlayerInputsSystem(
                sp.GetService<IInputsMan>(),
                sp.GetService<IEntityMan>(),
                sp.GetService<IEventsMan>()));

            systemFactory.RegisterSystem<UnknownMapCellDisplaySystem>(() => new UnknownMapCellDisplaySystem(
                sp.GetService<IPrimitiveRenderer>(),
                sp.GetService<IFontMan>()));
            systemFactory.RegisterSystem<GroupMapCellDisplaySystem>(() => new GroupMapCellDisplaySystem(
                sp.GetService<IPrimitiveRenderer>(),
                sp.GetService<IFontMan>()));

            systemFactory.RegisterSystem<DamageOnHealthDistributionSystem>(() => new DamageOnHealthDistributionSystem(
                sp.GetService<IEntityMan>(),
                sp.GetService<IEventsMan>(),
                sp.GetService<ILogger>()));

            systemFactory.RegisterSystem<DestroyOnZeroHealthSystem>(() => new DestroyOnZeroHealthSystem(
                sp.GetService<IWorldMan>(),
                sp.GetService<IEntityMan>(),
                sp.GetService<IEventsMan>(),
                sp.GetService<ILogger>()));

            systemFactory.RegisterSystem<LivesSystem>(() => new LivesSystem(
                sp.GetService<IEventsMan>(),
                sp.GetService<ILogger>()));

            systemFactory.RegisterSystem<TurretTargetingSystem>(() => new TurretTargetingSystem(
                sp.GetService<IEventsMan>(),
                sp.GetService<IEntityMan>(),
                sp.GetService<ILogger>()));

            systemFactory.RegisterSystem<ResurrectionSystem>(() => new ResurrectionSystem(
                sp.GetService<IWorldMan>()));

            systemFactory.RegisterSystem<ItemManagingSystem>(() => new ItemManagingSystem(
                sp.GetService<ItemsMan>(),
                sp.GetService<IEventsMan>(),
                sp.GetService<ILogger>()));

            systemFactory.RegisterSystem<ItemPickupSystem>(() => new ItemPickupSystem(
                sp.GetService<IEventsMan>(),
                sp.GetService<IEntityMan>()));
        }

        #endregion Public Methods
    }
}