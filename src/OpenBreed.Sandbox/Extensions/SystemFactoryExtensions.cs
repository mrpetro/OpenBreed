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
            systemFactory.RegisterSystem<ActorMovementByPlayerInputsSystem>((world) => new ActorMovementByPlayerInputsSystem(
                sp.GetService<IInputsMan>(),
                sp.GetService<IEntityMan>(),
                sp.GetService<IEventsMan>()));

            systemFactory.RegisterSystem<UnknownMapCellDisplaySystem>((world) => new UnknownMapCellDisplaySystem(
                world,
                sp.GetService<IPrimitiveRenderer>(),
                sp.GetService<IFontMan>()));
            systemFactory.RegisterSystem<GroupMapCellDisplaySystem>((world) => new GroupMapCellDisplaySystem(
                world,
                sp.GetService<IPrimitiveRenderer>(),
                sp.GetService<IFontMan>()));

            systemFactory.RegisterSystem<DamageOnHealthDistributionSystem>((world) => new DamageOnHealthDistributionSystem(
                world,
                sp.GetService<IEntityMan>(),
                sp.GetService<IEventsMan>(),
                sp.GetService<ILogger>()));

            systemFactory.RegisterSystem<DestroyOnZeroHealthSystem>((world) => new DestroyOnZeroHealthSystem(
                world,
                sp.GetService<IWorldMan>(),
                sp.GetService<IEntityMan>(),
                sp.GetService<IEventsMan>(),
                sp.GetService<ILogger>()));

            systemFactory.RegisterSystem<LivesSystem>((world) => new LivesSystem(
                world,
                sp.GetService<IEventsMan>(),
                sp.GetService<ILogger>()));

            systemFactory.RegisterSystem<ResurrectionSystem>((world) => new ResurrectionSystem(sp.GetService<IWorldMan>()));

            systemFactory.RegisterSystem<ItemManagingSystem>((world) => new ItemManagingSystem(
                world,
                sp.GetService<ItemsMan>(),
                sp.GetService<IEventsMan>(),
                sp.GetService<ILogger>()));

            systemFactory.RegisterSystem<ItemPickupSystem>((world) => new ItemPickupSystem(
                sp.GetService<IEventsMan>(),
                sp.GetService<IEntityMan>()));
        }

        #endregion Public Methods
    }
}