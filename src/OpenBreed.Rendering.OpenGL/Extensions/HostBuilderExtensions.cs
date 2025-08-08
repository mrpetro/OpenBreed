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
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Data;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.Abstractions.Renderers;
using OpenBreed.Rendering.OpenGL.Data;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenBreed.Rendering.OpenGL.Renderers;
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
                services.AddOpenGLServices();
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
                services.AddSingleton<SpriteMan>();
                services.AddSingleton<ISpriteMan>((sp) => sp.GetRequiredService<SpriteMan>());
                services.AddSingleton<FontMan>();
                services.AddSingleton<IFontMan>((sp) => sp.GetRequiredService<FontMan>());
                services.AddSingleton<ITextureMan, TextureMan>();
                services.AddSingleton<PictureMan>();
                services.AddSingleton<IPictureMan>((sp) => sp.GetRequiredService<PictureMan>());
                services.AddSingleton<TileMan>();
                services.AddSingleton<ITileMan>((sp) => sp.GetRequiredService<TileMan>());

                services.AddScoped<IPrimitiveRenderer, PrimitiveRenderer>();
                services.AddScoped<IPictureRenderer, PictureRenderer>();
                services.AddScoped<ISpriteRenderer, SpriteRenderer>();
                services.AddScoped<ITileRenderer, TileRenderer>();
                services.AddScoped<IFontRenderer, FontRenderer>();
            });
        }

        public static void ConfigureGraphicsDataLoaders(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<ISpriteAtlasDataLoader, SpriteAtlasDataLoader>();
                services.AddScoped<IPictureDataLoader, PictureDataLoader>();
                services.AddScoped<ITileAtlasDataLoader, TileAtlasDataLoader>();
                services.AddScoped<ITileStampDataLoader, TileStampDataLoader>();
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
