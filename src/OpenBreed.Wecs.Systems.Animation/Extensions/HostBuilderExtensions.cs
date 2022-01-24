using OpenBreed.Animation.Interface;
using OpenBreed.Common;
using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Common.Logging;
using OpenBreed.Wecs.Entities;
using System;

namespace OpenBreed.Wecs.Systems.Animation.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupAnimationSystems(this ISystemFactory systemFactory, IServiceProvider serviceProvider)
        {
            systemFactory.Register<AnimatorSystem>(() => new AnimatorSystem(serviceProvider.GetService<IEntityMan>(),
                                                             serviceProvider.GetService<IClipMan<Entity>>(),
                                                             serviceProvider.GetService<ILogger>()));
        }

        #endregion Public Methods
    }
}