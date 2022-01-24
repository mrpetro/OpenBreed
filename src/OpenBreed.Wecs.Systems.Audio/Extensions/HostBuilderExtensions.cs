using OpenBreed.Audio.Interface.Managers;
using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Common;
using System;

namespace OpenBreed.Wecs.Systems.Audio.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupAudioSystems(this ISystemFactory systemFactory, IServiceProvider serviceProvider)
        {
            systemFactory.Register<SoundSystem>(() => new SoundSystem(serviceProvider.GetService<ISoundMan>()));
        }

        #endregion Public Methods
    }
}