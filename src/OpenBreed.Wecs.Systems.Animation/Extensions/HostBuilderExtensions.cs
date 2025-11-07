using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Interface;
using OpenBreed.Common;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Extensions;
using System;

namespace OpenBreed.Wecs.Systems.Animation.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupAnimationSystems(this IHostBuilder hostBuilder)
        {
            hostBuilder.SetupWecsAssemblySystems();
        }

        #endregion Public Methods
    }
}