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
            systemFactory.Register<FsmSystem>((world) => new FsmSystem(world, serviceProvider.GetService<IFsmMan>(),
                                                                  serviceProvider.GetService<ILogger>()));
            systemFactory.Register<TextInputSystem>((world) => new TextInputSystem(world, serviceProvider.GetService<IEntityMan>()));
            systemFactory.Register<TimerSystem>((world) => new TimerSystem(
                world,
                serviceProvider.GetService<IEntityMan>(),
                serviceProvider.GetService<IEventsMan>(),
                serviceProvider.GetService<ILogger>()));
            systemFactory.Register<FrameSystem>((world) => new FrameSystem(
                world,
                serviceProvider.GetService<IEntityMan>(),
                serviceProvider.GetService<IEventsMan>(),
                serviceProvider.GetService<ILogger>()));

            systemFactory.Register<PausingSystem>((world) => new PausingSystem(
                world,
                serviceProvider.GetService<IWorldMan>(),                                                    
                serviceProvider.GetService<IEventsMan>()));
        }

        #endregion Public Methods
    }
}