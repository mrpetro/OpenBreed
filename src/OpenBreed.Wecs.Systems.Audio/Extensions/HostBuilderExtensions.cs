using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Audio.Interface.Managers;
using OpenBreed.Common;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Extensions;
using System;

namespace OpenBreed.Wecs.Systems.Audio.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupAudioSystems(this IHostBuilder hostBuilder)
        {
            hostBuilder.SetupWecsAssemblySystems();
        }

        #endregion Public Methods
    }
}