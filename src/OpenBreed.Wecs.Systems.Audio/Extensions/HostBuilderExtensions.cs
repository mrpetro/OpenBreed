using OpenBreed.Audio.Interface.Managers;
using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Common;
using System;
using OpenBreed.Core.Managers;

namespace OpenBreed.Wecs.Systems.Audio.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupAudioSystems(this ISystemFactory systemFactory, IServiceProvider serviceProvider)
        {
            systemFactory.RegisterSystem<SoundSystem>(
                (world) => new SoundSystem(
                    world,
                    serviceProvider.GetService<ISoundMan>(),
                    serviceProvider.GetService<IEventsMan>()));
        }

        #endregion Public Methods
    }
}