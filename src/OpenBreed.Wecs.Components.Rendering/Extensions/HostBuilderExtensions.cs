using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Rendering.Extensions
{
    public static class HostBuilderExtensions
    {
        public static void SetupRenderingComponentFactories(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<SpriteComponentFactory>();
                services.AddSingleton<ViewportComponentFactory>();
                services.AddSingleton<TextComponentFactory>();
                services.AddSingleton<CameraComponentFactory>();
                services.AddSingleton<TilePutterComponentFactory>();
            });
        }
    }
}
