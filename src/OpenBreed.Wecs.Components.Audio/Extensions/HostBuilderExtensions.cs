using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Audio.Extensions
{
    public static class HostBuilderExtensions
    {
        public static void SetupAudioComponentFactories(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<SoundPlayerComponentFactory>();
            });
        }
    }
}
