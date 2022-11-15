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
            systemFactory.Register<ActorMovementByPlayerControlSystem>(() => new ActorMovementByPlayerControlSystem(
                sp.GetService<IEntityMan>(),
                sp.GetService<IPlayersMan>()));
            systemFactory.Register<ActorScriptByPlayerControlSystem>(() => new ActorScriptByPlayerControlSystem(
                sp.GetService<IEntityMan>(),
                sp.GetService<IPlayersMan>(),
                sp.GetService<IScriptMan>(),
                sp.GetService<ILogger>()));
            systemFactory.Register<UnknownMapCellDisplaySystem>(() => new UnknownMapCellDisplaySystem(sp.GetService<IPrimitiveRenderer>(),
                                                                  sp.GetService<IFontMan>()));
            systemFactory.Register<GroupMapCellDisplaySystem>(() => new GroupMapCellDisplaySystem(sp.GetService<IPrimitiveRenderer>(),
                                                                       sp.GetService<IFontMan>()));
        }

        #endregion Public Methods
    }
}