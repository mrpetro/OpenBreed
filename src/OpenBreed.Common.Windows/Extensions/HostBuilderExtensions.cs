using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Windows.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Windows.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void ConfigurePcmPlayer(this IHostBuilder hostBuilder)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                hostBuilder.ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IPcmPlayer, OpenBreed.Common.Windows.PcmPlayer>();
                });
            }
            else
            {
                throw new PlatformNotSupportedException(nameof(IPcmPlayer));
            }
        }

        public static void SetupWindowsDrawingContext(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IDrawingFactory, DrawingFactory>();
                services.AddSingleton<IDrawingContextProvider, DrawingContextProvider>();
                services.AddSingleton<IBitmapProvider, BitmapProvider>();
                services.AddSingleton<IImageProvider, ImageProvider>();
                services.AddSingleton<IPensProvider, PensProvider>();
            });
        }

        #endregion Public Methods
    }
}