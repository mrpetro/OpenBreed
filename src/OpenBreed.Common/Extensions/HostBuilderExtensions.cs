using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenBreed.Common.Data;
using OpenBreed.Common.Formats;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Common.Services;
using System;

namespace OpenBreed.Common.Extensions
{
    public static class HostBuilderExtensions
    {
        #region Public Methods

        public static void ConfigureLogConsolePrinter(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<LogConsolePrinter>();
                services.AddHostedService<LogConsolePrinter>();
            });
        }

        public static void SetupDataHandlers(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IAssetDataHandler, PcmSoundDataHandler>();
                services.AddSingleton<IAssetDataHandler, IffImageDataHandler>();
                services.AddSingleton<IAssetDataHandler, AcbmImageDataHandler>();
                services.AddSingleton<IAssetDataHandler, MapDataHandler>();
                services.AddSingleton<IAssetDataHandler, MapPaletteDataHandler>();
                services.AddSingleton<IAssetDataHandler, BinaryPaletteDataHandler>();
                services.AddSingleton<IAssetDataHandler, LbmPaletteDataHandler>();
                services.AddSingleton<IAssetDataHandler, BlkTileAtlasDataHandler>();
                services.AddSingleton<IAssetDataHandler, AcbmTileAtlasDataHandler>();
                services.AddSingleton<IAssetDataHandler, SprSpriteAtlasDataHandler>();
                services.AddSingleton<IAssetDataHandler, FileTextDataHandler>();
                services.AddSingleton<IAssetDataHandler, MapTextDataHandler>();
                services.AddSingleton<IAssetDataHandler, FileScriptDataHandler>();
            });
        }

        public static void SetupDefaultLogger(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ILoggerClient, DefaultLoggerClient>();
                services.AddSingleton<ILogger, DefaultLogger>();
            });
        }

        public static void SetupBuilderFactory(this IHostBuilder hostBuilder, Action<IBuilderFactory, IServiceProvider> action)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<IBuilderFactory>((sp) =>
                {
                    var entityFactory = new BuilderFactory();
                    action.Invoke(entityFactory, sp);
                    return entityFactory;
                });
            });
        }

        public static void SetupDefaultTypeAttributesProvider(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ITypeAttributesProvider, DefaultTypeAttributesProvider>();
            });
        }

        public static void SetupModelProvider(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IModelsProvider, ModelsProvider>();
            });
        }

        public static void SetupDataLoaderFactory(this IHostBuilder hostBuilder, Action<DataLoaderFactory, IServiceProvider> action)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<IDataLoaderFactory>((sp) =>
                {
                    var dataLoaderFactory = new DataLoaderFactory();
                    action.Invoke(dataLoaderFactory, sp);
                    return dataLoaderFactory;
                });
            });
        }

        public static void SetupDataProviders(this IHostBuilder hostBuilder, bool isEditor)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<DataSourceProvider>((sp) => ActivatorUtilities.CreateInstance<DataSourceProvider>(sp, !isEditor));
                services.AddSingleton<AssetsDataProvider>();
                services.AddSingleton<ActionSetsDataProvider>();
                services.AddSingleton<SpriteAtlasDataProvider>();
                services.AddSingleton<TileAtlasDataProvider>();
                services.AddSingleton<ScriptsDataProvider>();
                services.AddSingleton<EntityTemplatesDataProvider>();
                services.AddSingleton<PalettesDataProvider>();
                services.AddSingleton<TextsDataProvider>();
                services.AddSingleton<MapsDataProvider>();
                services.AddSingleton<Lazy<MapsDataProvider>>((sp) => new Lazy<MapsDataProvider>(() => sp.GetService<MapsDataProvider>()));
                services.AddSingleton<ImagesDataProvider>();
                services.AddSingleton<Lazy<ImagesDataProvider>>((sp) => new Lazy<ImagesDataProvider>(() => sp.GetService<ImagesDataProvider>()));
                services.AddSingleton<SoundsDataProvider>();
            });
        }

        public static void AddCommonServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IUpdaterFactory, DefaultUpdaterFactory>();
            });
        }

        public static void SetupVariableManager(this IHostBuilder hostBuilder, Action<IVariableMan, IServiceProvider> action)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IVariableMan>((sp) =>
                {
                    var variableMan = new VariableMan(sp.GetService<ILogger>());
                    action.Invoke(variableMan, sp);
                    return variableMan;
                });
            });
        }

        #endregion Public Methods
    }
}