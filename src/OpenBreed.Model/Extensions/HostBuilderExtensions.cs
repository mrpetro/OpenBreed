
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace OpenBreed.Model.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupModelTools(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ISpriteMerger, SpriteMerger>();
            });
        }

        #endregion Public Methods
    }
}