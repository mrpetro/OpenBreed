using OpenBreed.Common.Data;
using OpenBreed.Common.Formats;
using OpenBreed.Common.Logging;
using OpenBreed.Database.Interface;

namespace OpenBreed.Common.Extensions
{
    public static class ManagerCollectionExtension
    {
        #region Public Methods

        public static void SetupModelProvider(this IManagerCollection managerCollection)
        {
            managerCollection.AddSingleton<IModelsProvider>(() => new ModelsProvider(managerCollection.GetManager<ILogger>(),
                                                                                 managerCollection.GetManager<AssetsDataProvider>()));
        }

        public static void SetupDataProviders(this IManagerCollection managerCollection)
        {
            managerCollection.AddSingleton<DataSourceProvider>(() => new DataSourceProvider(managerCollection.GetManager<IRepositoryProvider>(),
                                                                                                    managerCollection.GetManager<ILogger>(),
                                                                                                    managerCollection.GetManager<IVariableMan>()));

            managerCollection.AddSingleton<AssetsDataProvider>(() => new AssetsDataProvider(managerCollection.GetManager<IRepositoryProvider>(),
                                                                                                    managerCollection.GetManager<DataSourceProvider>(),
                                                                                                    managerCollection.GetManager<DataFormatMan>()));

            managerCollection.AddSingleton<ActionSetsDataProvider>(() => new ActionSetsDataProvider(managerCollection.GetManager<IModelsProvider>(),
                                                                                                    managerCollection.GetManager<IRepositoryProvider>()));

            managerCollection.AddSingleton<SpriteAtlasDataProvider>(() => new SpriteAtlasDataProvider(managerCollection.GetManager<IModelsProvider>(),
                                                                                                    managerCollection.GetManager<IRepositoryProvider>()));

            managerCollection.AddSingleton<TileAtlasDataProvider>(() => new TileAtlasDataProvider(managerCollection.GetManager<IModelsProvider>(),
                                                                                                    managerCollection.GetManager<IRepositoryProvider>()));

            managerCollection.AddSingleton<ScriptsDataProvider>(() => new ScriptsDataProvider(managerCollection.GetManager<IModelsProvider>(),
                                                                                                    managerCollection.GetManager<IRepositoryProvider>()));

            managerCollection.AddSingleton<EntityTemplatesDataProvider>(() => new EntityTemplatesDataProvider(managerCollection.GetManager<IModelsProvider>(),
                                                                                                    managerCollection.GetManager<IRepositoryProvider>()));

            managerCollection.AddSingleton<PalettesDataProvider>(() => new PalettesDataProvider(managerCollection.GetManager<IModelsProvider>(),
                                                                                                    managerCollection.GetManager<IRepositoryProvider>()));

            managerCollection.AddSingleton<TextsDataProvider>(() => new TextsDataProvider(managerCollection.GetManager<IModelsProvider>(),
                                                                                                    managerCollection.GetManager<IRepositoryProvider>()));

            managerCollection.AddSingleton<MapsDataProvider>(() => new MapsDataProvider(managerCollection.GetManager<IModelsProvider>(),
                                                                                        managerCollection.GetManager<IRepositoryProvider>(),
                                                                                        managerCollection.GetManager<TileAtlasDataProvider>(),
                                                                                        managerCollection.GetManager<PalettesDataProvider>(),
                                                                                        managerCollection.GetManager<ActionSetsDataProvider>()));

            managerCollection.AddSingleton<ImagesDataProvider>(() => new ImagesDataProvider(managerCollection.GetManager<IModelsProvider>(),
                                                                                                    managerCollection.GetManager<IRepositoryProvider>()));

            managerCollection.AddSingleton<SoundsDataProvider>(() => new SoundsDataProvider(managerCollection.GetManager<IModelsProvider>(),
                                                                                                    managerCollection.GetManager<IRepositoryProvider>()));
        }

        public static void SetupBuilderFactory(this IManagerCollection managerCollection)
        {
            managerCollection.AddSingleton<IBuilderFactory>(() => new BuilderFactory());
        }

        public static void SetupVariableManager(this IManagerCollection managerCollection)
        {
            managerCollection.AddSingleton<IVariableMan>(() => new VariableMan(managerCollection.GetManager<ILogger>()));
        }

        public static void SetupABFormats(this IManagerCollection managerCollection)
        {
            managerCollection.AddSingleton<DataFormatMan>(() =>
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
        }

        #endregion Public Methods
    }
}