using OpenBreed.Common;
using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Common.Logging;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Entities;
using System;

namespace OpenBreed.Wecs.Systems.Core.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupCoreSystems(this ISystemFactory systemFactory, IServiceProvider serviceProvider)
        {
            systemFactory.Register(() => new FsmSystem(serviceProvider.GetService<IEntityMan>(),
                                                       serviceProvider.GetService<IFsmMan>(),
                                                       serviceProvider.GetService<ILogger>()));
            systemFactory.Register(() => new TextInputSystem(serviceProvider.GetService<IEntityMan>()));
            systemFactory.Register(() => new TimerSystem(serviceProvider.GetService<IEntityMan>(),
                                                         serviceProvider.GetService<ILogger>()));
        }

        #endregion Public Methods
    }
}