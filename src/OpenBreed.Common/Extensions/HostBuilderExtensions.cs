using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Common.Data;
using OpenBreed.Common.Formats;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBreed.Common.Extensions
{
    public static class HostBuilderExtensions
    {
        public static void SetupBuilderFactory(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IBuilderFactory, BuilderFactory>();
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

        public static void SetupVariableManager(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IVariableMan, VariableMan>();
            });
        }

        public static void SetupABFormats(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton((s) =>
                {
                    var formatMan = new DataFormatMan();
                    formatMan.RegisterFormat("ABSE_MAP", new ABSEMAPFormat());
                    formatMan.RegisterFormat("ABHC_MAP", new ABHCMAPFormat());
                    formatMan.RegisterFormat("ABTA_MAP", new ABTAMAPFormat());
                    formatMan.RegisterFormat("ABTABLK", new ABTABLKFormat());
                    formatMan.RegisterFormat("ABTASPR", new ABTASPRFormat());
                    formatMan.RegisterFormat("ABTAODDSPR", new ABTAODDSPRFormat());
                    formatMan.RegisterFormat("ACBM_TILE_SET", new ACBMTileSetFormat());
                    formatMan.RegisterFormat("ACBM_IMAGE", new ACBMImageFormat());
                    formatMan.RegisterFormat("IFF_IMAGE", new IFFImageFormat());
                    formatMan.RegisterFormat("BINARY", new BinaryFormat());
                    formatMan.RegisterFormat("PCM_SOUND", new PCMSoundFormat());
                    formatMan.RegisterFormat("TEXT", new TextFormat());
                    return formatMan;
                });
            });
        }

    }
}
