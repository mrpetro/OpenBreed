using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Factories;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.OpenGL.Factories;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.OpenGL.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddOpenGLServices(this IServiceCollection services)
        {
            services.AddScoped<ITileGridFactory, TileGridFactory>();
            services.AddSingleton<IRenderingMan, RenderingMan>();

            services.AddScoped<IRenderViewFactory, RenderViewFactory>();

            services.AddSingleton<IRenderContextProvider, OpenTKRenderContextProvider>();

            services.AddSingleton<Func<IGraphicsContext, HostCoordinateSystemConverter, Action<IGraphicsContext>, IRenderContext>>((sp)
                => (graphicalContext, hostCoordinateSystemConverter, deinitializeCallback)
                => new OpenTKRenderContext(
                    sp.GetRequiredService<ILogger>(),
                    sp.GetRequiredService<IEventsMan>(),
                    sp.GetRequiredService<IPaletteMan>(),
                    sp.GetRequiredService<IStampMan>(),
                    sp.GetRequiredService<IServiceScopeFactory>(),
                    graphicalContext, deinitializeCallback, hostCoordinateSystemConverter));
        }
    }
}
