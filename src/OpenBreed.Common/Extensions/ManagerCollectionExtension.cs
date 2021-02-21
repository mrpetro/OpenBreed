using OpenBreed.Common.Data;
using OpenBreed.Common.Formats;
using OpenBreed.Common.Logging;

namespace OpenBreed.Common.Extensions
{
    public static class ManagerCollectionExtension
    {
        #region Public Methods

        public static void SetupDataProviders(this IManagerCollection managerCollection)
        {
            managerCollection.AddSingleton<DataSourceProvider>(() => new DataSourceProvider(managerCollection.GetManager<IWorkspaceMan>(),
                                                                                                    managerCollection.GetManager<ILogger>(),
                                                                                                    managerCollection.GetManager<IVariableMan>()));

            managerCollection.AddSingleton<AssetsDataProvider>(() => new AssetsDataProvider(managerCollection.GetManager<IWorkspaceMan>(),
                                                                                                    managerCollection.GetManager<DataSourceProvider>(),
                                                                                                    managerCollection.GetManager<DataFormatMan>()));

            managerCollection.AddSingleton<ActionSetsDataProvider>(() => new ActionSetsDataProvider(managerCollection.GetManager<IDataProvider>(),
                                                                                                    managerCollection.GetManager<IWorkspaceMan>()));

            managerCollection.AddSingleton<SpriteSetsDataProvider>(() => new SpriteSetsDataProvider(managerCollection.GetManager<IDataProvider>(),
                                                                                                    managerCollection.GetManager<IWorkspaceMan>()));

            managerCollection.AddSingleton<TileSetsDataProvider>(() => new TileSetsDataProvider(managerCollection.GetManager<IDataProvider>(),
                                                                                                    managerCollection.GetManager<IWorkspaceMan>()));

            managerCollection.AddSingleton<ScriptsDataProvider>(() => new ScriptsDataProvider(managerCollection.GetManager<IDataProvider>(),
                                                                                                    managerCollection.GetManager<IWorkspaceMan>()));

            managerCollection.AddSingleton<EntityTemplatesDataProvider>(() => new EntityTemplatesDataProvider(managerCollection.GetManager<IDataProvider>(),
                                                                                                    managerCollection.GetManager<IWorkspaceMan>()));

            managerCollection.AddSingleton<PalettesDataProvider>(() => new PalettesDataProvider(managerCollection.GetManager<IDataProvider>(),
                                                                                                    managerCollection.GetManager<IWorkspaceMan>()));

            managerCollection.AddSingleton<TextsDataProvider>(() => new TextsDataProvider(managerCollection.GetManager<IDataProvider>(),
                                                                                                    managerCollection.GetManager<IWorkspaceMan>()));

            managerCollection.AddSingleton<MapsDataProvider>(() => new MapsDataProvider(managerCollection.GetManager<IDataProvider>(),
                                                                                        managerCollection.GetManager<IWorkspaceMan>(),
                                                                                        managerCollection.GetManager<TileSetsDataProvider>(),
                                                                                        managerCollection.GetManager<PalettesDataProvider>(),
                                                                                        managerCollection.GetManager<ActionSetsDataProvider>()));

            managerCollection.AddSingleton<ImagesDataProvider>(() => new ImagesDataProvider(managerCollection.GetManager<IDataProvider>(),
                                                                                                    managerCollection.GetManager<IWorkspaceMan>()));

            managerCollection.AddSingleton<SoundsDataProvider>(() => new SoundsDataProvider(managerCollection.GetManager<IDataProvider>(),
                                                                                                    managerCollection.GetManager<IWorkspaceMan>()));


            managerCollection.AddSingleton<IDataProvider>(() => new DataProvider(managerCollection.GetManager<ILogger>(),
                                                                                 managerCollection.GetManager<DataSourceProvider>(),
                                                                                 managerCollection.GetManager<AssetsDataProvider>()));

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