using OpenBreed.Common;
using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Common.Logging;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Entities;
using System;
using OpenBreed.Wecs.Worlds;
using OpenBreed.Common.Interface.Logging;

namespace OpenBreed.Wecs.Systems.Core.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupCoreSystems(this ISystemFactory systemFactory, IServiceProvider serviceProvider)
        {
            systemFactory.RegisterSystem<FsmSystem>((world) => new FsmSystem(world, serviceProvider.GetService<IFsmMan>(),
                                                                  serviceProvider.GetService<ILogger>()));
            systemFactory.RegisterSystem<TextInputSystem>((world) => new TextInputSystem(world, serviceProvider.GetService<IEntityMan>()));
            systemFactory.RegisterSystem<TimerSystem>((world) => new TimerSystem(
                world,
                serviceProvider.GetService<IEntityMan>(),
                serviceProvider.GetService<IEventsMan>(),
                serviceProvider.GetService<ILogger>()));
            systemFactory.RegisterSystem<FrameSystem>((world) => new FrameSystem(
                world,
                serviceProvider.GetService<IEntityMan>(),
                serviceProvider.GetService<IEventsMan>(),
                serviceProvider.GetService<ILogger>()));

            systemFactory.RegisterSystem<PausingSystem>((world) => new PausingSystem(
                world,
                serviceProvider.GetService<IWorldMan>(),                                                    
                serviceProvider.GetService<IEventsMan>()));
        }

        #endregion Public Methods
    }
}