using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.UI.WinForms.Extensions
{
    public static class HostBuilderExtensions
    {
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
    }
}
