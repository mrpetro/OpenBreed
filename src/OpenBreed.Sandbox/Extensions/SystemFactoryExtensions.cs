using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Input.Interface;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Sandbox.Systems;
using OpenBreed.Sandbox.Worlds.Wecs.Systems;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using System;

namespace OpenBreed.Sandbox.Extensions
{
    public static class SystemFactoryExtensions
    {
        #region Public Methods

        public static void SetupGameSystems(this ISystemFactory systemFactory, IServiceProvider serviceProvider)
        {
            systemFactory.Register<ActorMovementByPlayerControlSystem>(() => new ActorMovementByPlayerControlSystem(
                serviceProvider.GetService<IEntityMan>(),
                serviceProvider.GetService<IPlayersMan>()));
            systemFactory.Register<UnknownMapCellDisplaySystem>(() => new UnknownMapCellDisplaySystem(serviceProvider.GetService<IPrimitiveRenderer>(),
                                                                  serviceProvider.GetService<IFontMan>()));
            systemFactory.Register<GroupMapCellDisplaySystem>(() => new GroupMapCellDisplaySystem(serviceProvider.GetService<IPrimitiveRenderer>(),
                                                                       serviceProvider.GetService<IFontMan>()));
        }

        #endregion Public Methods
    }
}