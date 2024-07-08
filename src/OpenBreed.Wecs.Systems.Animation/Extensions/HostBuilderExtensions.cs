using OpenBreed.Animation.Interface;
using OpenBreed.Common;
using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Common.Logging;
using OpenBreed.Wecs.Entities;
using System;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Core.Managers;
using Microsoft.Extensions.Logging;

namespace OpenBreed.Wecs.Systems.Animation.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupAnimationSystems(this ISystemFactory systemFactory, IServiceProvider sp)
        {
            systemFactory.RegisterSystem<AnimatorSystem>(() => new AnimatorSystem(
                sp.GetService<IEntityMan>(),
                sp.GetService<IEventsMan>(),
                sp.GetService<IClipMan<IEntity>>(),
                sp.GetService<ILogger>()));
        }

        #endregion Public Methods
    }
}