using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Rendering.Interface.Factories;
using OpenBreed.Rendering.Interface.Managers;
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
            services.AddSingleton<ITileGridFactory, TileGridFactory>();
            services.AddSingleton<IRenderingMan, RenderingMan>();

            services.AddScoped<IRenderViewFactory, RenderViewFactory>();

            services.AddScoped<IRenderContextFactory, OpenTKRenderContextFactory>();
            services.AddScoped((sp) => sp.GetRequiredService<IRenderContextFactory>().CreateContext());

            services.AddSingleton<Func<IGraphicsContext, HostCoordinateSystemConverter, IRenderContext>>((sp)
                => (graphicalContext, hostCoordinateSystemConverter)
                => new OpenTKRenderContext(sp.GetRequiredService<ILogger>(), sp.GetRequiredService<IEventsMan>(), graphicalContext, hostCoordinateSystemConverter));
        }
    }
}
