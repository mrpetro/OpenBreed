using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Extensions;
using System;

namespace OpenBreed.Wecs.Systems.Scripting.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupScriptingSystems(this IHostBuilder hostBuilder)
        {
            hostBuilder.SetupWecsAssemblySystems();
        }

        #endregion Public Methods
    }
}