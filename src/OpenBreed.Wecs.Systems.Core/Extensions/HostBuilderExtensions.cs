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
            systemFactory.Register<FsmSystem>(() => new FsmSystem(serviceProvider.GetService<IFsmMan>(),
                                                                  serviceProvider.GetService<ILogger>()));
            systemFactory.Register<TextInputSystem>(() => new TextInputSystem(serviceProvider.GetService<IEntityMan>()));
            systemFactory.Register<TimerSystem>(() => new TimerSystem(
                serviceProvider.GetService<IEntityMan>(),
                serviceProvider.GetService<IEventsMan>(),
                serviceProvider.GetService<ILogger>()));

            systemFactory.Register<PausingSystem>(() => new PausingSystem(serviceProvider.GetService<IWorldMan>(),
                                                                          serviceProvider.GetService<IEventsMan>()));
        }

        #endregion Public Methods
    }
}