using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Model;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Data;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Data;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.OpenGL.Extensions
{
    public static class HostBuilderExtensions
    {
        public static void SetupOpenGLManagers(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ITileGridFactory, TileGridFactory>();
                services.AddSingleton<IRenderingMan, RenderingMan>();
                services.AddSingleton<Func<IGraphicsContext, HostCoordinateSystemConverter, IRenderContext>>((sp)
                    => (graphicalContext, hostCoordinateSystemConverter)
                    => new OpenTKRenderContext(sp.GetRequiredService<ILogger>(), sp.GetRequiredService<IEventsMan>(), graphicalContext, hostCoordinateSystemConverter));
            });
        }


        public static void SetupGLWindow(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IWindow, OpenTKWindow>();
                services.AddSingleton((sp) => sp.GetRequiredService<IWindow>().Context);
            });
        }

        public static void SetupGLRenderContextComponents(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton((sp) => sp.GetRequiredService<IRenderContext>().Primitives);
                services.AddSingleton((sp) => sp.GetRequiredService<IRenderContext>().Sprites);
                services.AddSingleton((sp) => sp.GetRequiredService<IRenderContext>().Fonts);
                services.AddSingleton((sp) => sp.GetRequiredService<IRenderContext>().Textures);
                services.AddSingleton((sp) => sp.GetRequiredService<IRenderContext>().Pictures);
                services.AddSingleton((sp) => sp.GetRequiredService<IRenderContext>().PictureRenderer);
                services.AddSingleton((sp) => sp.GetRequiredService<IRenderContext>().Tiles);
                services.AddSingleton((sp) => sp.GetRequiredService<IRenderContext>().TileStamps);
                services.AddSingleton((sp) => sp.GetRequiredService<IRenderContext>().Palettes);
                services.AddSingleton((sp) => sp.GetRequiredService<IRenderContext>().SpriteRenderer);
            });
        }

        public static void ConfigureGraphicsDataLoaders(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ISpriteAtlasDataLoader, SpriteAtlasDataLoader>();
                services.AddSingleton<IPictureDataLoader, PictureDataLoader>();
                services.AddSingleton<ITileAtlasDataLoader, TileAtlasDataLoader>();
                services.AddSingleton<ITileStampDataLoader, TileStampDataLoader>();
            });
        }

        public static void RegisterGraphicsDataLoader(this DataLoaderFactory dataLoaderFactory, IServiceProvider sp)
        {
            dataLoaderFactory.Register<ISpriteAtlasDataLoader>(() => sp.GetRequiredService<ISpriteAtlasDataLoader>());
            dataLoaderFactory.Register<IPictureDataLoader>(() => sp.GetRequiredService<IPictureDataLoader>());
            dataLoaderFactory.Register<ITileAtlasDataLoader>(() => sp.GetRequiredService<ITileAtlasDataLoader>());
            dataLoaderFactory.Register<ITileStampDataLoader>(() => sp.GetRequiredService<ITileStampDataLoader>());
        }
    }
}
