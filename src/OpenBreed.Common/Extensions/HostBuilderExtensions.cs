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
using System;
using System.Collections.Generic;
using System.Text;
using static System.Collections.Specialized.BitVector32;

namespace OpenBreed.Common.Extensions
{
    public static class HostBuilderExtensions
    {
        public static void SetupDataHandlers(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IAssetDataHandler, PcmSoundDataHandler>();
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
                services.AddSingleton<IBuilderFactory>((sp) =>
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

        public static void SetupDataProviders(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<DataSourceProvider>();
                services.AddSingleton<AssetsDataProvider>();
                services.AddSingleton<ActionSetsDataProvider>();
                services.AddSingleton<SpriteAtlasDataProvider>();
                services.AddSingleton<TileAtlasDataProvider>();
                services.AddSingleton<ScriptsDataProvider>();
                services.AddSingleton<EntityTemplatesDataProvider>();
                services.AddSingleton<PalettesDataProvider>();
                services.AddSingleton<TextsDataProvider>();
                services.AddSingleton<MapsDataProvider>();
                services.AddSingleton<ImagesDataProvider>();
                services.AddSingleton<SoundsDataProvider>();
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

        public static void SetupABFormats(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton((s) =>
                {
                    var formatMan = new DataFormatMan();
                    formatMan.RegisterFormat("ABSE_MAP", new ABSEMAPFormat(s.GetRequiredService<IDrawingFactory>()));
                    formatMan.RegisterFormat("ABHC_MAP", new ABHCMAPFormat(s.GetRequiredService<IDrawingFactory>()));
                    formatMan.RegisterFormat("ABTA_MAP", new ABTAMAPFormat(s.GetRequiredService<IDrawingFactory>()));
                    formatMan.RegisterFormat("ABTABLK", new ABTABLKFormat());
                    formatMan.RegisterFormat("ABTASPR", new ABTASPRFormat());
                    formatMan.RegisterFormat("ABTAODDSPR", new ABTAODDSPRFormat());
                    formatMan.RegisterFormat("ACBM_TILE_SET", new ACBMTileSetFormat());
                    formatMan.RegisterFormat("ACBM_IMAGE", new ACBMImageFormat(s.GetRequiredService<IBitmapProvider>()));
                    formatMan.RegisterFormat("IFF_IMAGE", new IFFImageFormat(s.GetRequiredService<IBitmapProvider>()));
                    formatMan.RegisterFormat("BINARY", new BinaryFormat());
                    formatMan.RegisterFormat("TEXT", new TextFormat());
                    return formatMan;
                });
            });
        }

    }
}
