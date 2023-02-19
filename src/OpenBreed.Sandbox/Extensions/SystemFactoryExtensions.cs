using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Input.Interface;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Sandbox.Systems;
using OpenBreed.Sandbox.Worlds.Wecs.Systems;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using System;

namespace OpenBreed.Sandbox.Extensions
{
    public static class SystemFactoryExtensions
    {
        #region Public Methods

        public static void SetupGameSystems(this ISystemFactory systemFactory, IServiceProvider sp)
        {
            systemFactory.RegisterSystem<ActorMovementByPlayerControlSystem>((world) => new ActorMovementByPlayerControlSystem(
                world,
                sp.GetService<IEntityMan>(),
                sp.GetService<IInputsMan>()));
            systemFactory.RegisterSystem<UnknownMapCellDisplaySystem>((world) => new UnknownMapCellDisplaySystem(
                world,
                sp.GetService<IPrimitiveRenderer>(),
                sp.GetService<IFontMan>()));
            systemFactory.RegisterSystem<GroupMapCellDisplaySystem>((world) => new GroupMapCellDisplaySystem(
                world,
                sp.GetService<IPrimitiveRenderer>(),
                sp.GetService<IFontMan>()));
        }

        #endregion Public Methods
    }
}