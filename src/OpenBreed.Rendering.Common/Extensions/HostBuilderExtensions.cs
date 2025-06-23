using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.Common.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.Common.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void SetupCommonRenderingServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IPaletteMan, PaletteMan>();
                services.AddSingleton<IStampMan, StampMan>();
            });
        }

        #endregion Public Methods
    }
}