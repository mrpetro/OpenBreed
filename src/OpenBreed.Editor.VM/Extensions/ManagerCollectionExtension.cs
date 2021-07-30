using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Formats;
using OpenBreed.Common.Logging;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Actions;
using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Database.Interface.Items.EntityTemplates;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Database.Interface.Items.Texts;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Actions;
using OpenBreed.Editor.VM.Database;
using OpenBreed.Editor.VM.DataSources;
using OpenBreed.Editor.VM.EntityTemplates;
using OpenBreed.Editor.VM.Images;
using OpenBreed.Editor.VM.Logging;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Editor.VM.Palettes;
using OpenBreed.Editor.VM.Scripts;
using OpenBreed.Editor.VM.Sounds;
using OpenBreed.Editor.VM.Sprites;
using OpenBreed.Editor.VM.Texts;
using OpenBreed.Editor.VM.Tiles;

namespace OpenBreed.Editor.VM.Extensions
{
    public static class ManagerCollectionExtension
    {
        #region Public Methods


        public static void SetupDbEntrySubEditors(this IManagerCollection managerCollection)
        {
            managerCollection.AddTransient<IEntryEditor<IDbTileAtlasFromBlk>>(() => new TileSetFromBlkEditorVM(managerCollection.GetManager<TileAtlasDataProvider>(),
                                                                                                                managerCollection.GetManager<PalettesDataProvider>()));


            managerCollection.AddTransient<IEntryEditor<IDbActionSet>>(() => new ActionSetEmbeddedEditorVM(managerCollection.GetManager<ActionSetsDataProvider>()));


            managerCollection.AddTransient<IEntryEditor<IDbFileDataSource>>(() => new FileDataSourceEditorVM());

            managerCollection.AddTransient<IEntryEditor<IDbEpfArchiveDataSource>>(() => new EpfArchiveFileDataSourceEditorVM());


            managerCollection.AddTransient<IEntryEditor<IDbEntityTemplateFromFile>>(() => new EntityTemplateFromFileEditorVM(managerCollection.GetManager<EntityTemplatesDataProvider>(),
                                                                                                                                managerCollection.GetManager<IModelsProvider>()));


            managerCollection.AddTransient<IEntryEditor<IDbImage>>(() => new ImageFromFileEditorVM(managerCollection.GetManager<IWorkspaceMan>(),
                                                                                                                managerCollection.GetManager<IDialogProvider>(),
                                                                                                                managerCollection.GetManager<IModelsProvider>()));


            managerCollection.AddTransient<IEntryEditor<IDbSpriteAtlasFromSpr>>(() => new SpriteSetFromSprEditorVM(managerCollection.GetManager<SpriteAtlasDataProvider>(),
                                                                                                                managerCollection.GetManager<PalettesDataProvider>(),
                                                                                                                managerCollection.GetManager<IModelsProvider>()));



            managerCollection.AddTransient<IEntryEditor<IDbSpriteAtlasFromImage>>(() => new SpriteSetFromImageEditorVM(managerCollection.GetManager<SpriteAtlasDataProvider>(),
                                                                                                                managerCollection.GetManager<PalettesDataProvider>(),
                                                                                                                managerCollection.GetManager<IModelsProvider>()));


            managerCollection.AddTransient<IEntryEditor<IDbPaletteFromBinary>>(() => new PaletteFromBinaryEditorVM(managerCollection.GetManager<PalettesDataProvider>(),
                                                                                                                managerCollection.GetManager<IModelsProvider>()));


            managerCollection.AddTransient<IEntryEditor<IDbPaletteFromMap>>(() => new PaletteFromMapEditorVM(managerCollection.GetManager<PalettesDataProvider>(),
                                                                                                                managerCollection.GetManager<IModelsProvider>()));


            managerCollection.AddTransient<IEntryEditor<IDbTextEmbedded>>(() => new TextEmbeddedEditorVM(managerCollection.GetManager<TextsDataProvider>()));



            managerCollection.AddTransient<IEntryEditor<IDbTextFromMap>>(() => new TextFromMapEditorVM(managerCollection.GetManager<TextsDataProvider>(),
                                                                                                          managerCollection.GetManager<IModelsProvider>()));



            managerCollection.AddTransient<IEntryEditor<IDbSound>>(() => new SoundFromPcmEditorVM(managerCollection.GetManager<SoundsDataProvider>()));

            managerCollection.AddTransient<IEntryEditor<IDbScriptEmbedded>>(() => new ScriptEmbeddedEditorVM(managerCollection.GetManager<ScriptsDataProvider>()));


            managerCollection.AddTransient<IEntryEditor<IDbScriptFromFile>>(() => new ScriptFromFileEditorVM(managerCollection.GetManager<IWorkspaceMan>(),
                                                                                                                managerCollection.GetManager<ScriptsDataProvider>(),
                                                                                                                managerCollection.GetManager<IModelsProvider>()));



        }

        public static void SetupCommonViewModels(this IManagerCollection managerCollection)
        {
            managerCollection.AddTransient<LoggerVM>(() => new LoggerVM(managerCollection.GetManager<ILogger>()));
        }

        public static void SetupDbEntryEditors(this IManagerCollection managerCollection)
        {
            managerCollection.AddTransient<DbEditorVM>(() => new DbEditorVM(managerCollection,
                                                                            managerCollection.GetManager<DbEntryEditorFactory>(),
                                                                            managerCollection.GetManager<IRepositoryProvider>()));
            managerCollection.AddTransient<DbTablesEditorVM>(() => new DbTablesEditorVM(managerCollection.GetManager<IWorkspaceMan>(),
                                                                                        managerCollection.GetManager<DbEntryFactory>()));
            managerCollection.AddTransient<TileSetEditorVM>(() => new TileSetEditorVM(managerCollection.GetManager<DbEntrySubEditorFactory>(),
                                                                                      managerCollection.GetManager<IWorkspaceMan>(),
                                                                                      managerCollection.GetManager<IDialogProvider>()));
            managerCollection.AddTransient<SpriteSetEditorVM>(() => new SpriteSetEditorVM(managerCollection.GetManager<DbEntrySubEditorFactory>(),
                                                                                          managerCollection.GetManager<IWorkspaceMan>(),
                                                                                           managerCollection.GetManager<IDialogProvider>()));
            managerCollection.AddTransient<ActionSetEditorVM>(() => new ActionSetEditorVM(managerCollection.GetManager<DbEntrySubEditorFactory>(),
                                                                                          managerCollection.GetManager<IWorkspaceMan>(),
                                                                                          managerCollection.GetManager<IDialogProvider>()));
            managerCollection.AddTransient<PaletteEditorVM>(() => new PaletteEditorVM(managerCollection.GetManager<DbEntrySubEditorFactory>(),
                                                                                      managerCollection.GetManager<IWorkspaceMan>(),
                                                                                      managerCollection.GetManager<IDialogProvider>()));
            managerCollection.AddTransient<TextEditorVM>(() => new TextEditorVM(managerCollection.GetManager<DbEntrySubEditorFactory>(),
                                                                                managerCollection.GetManager<IWorkspaceMan>(),
                                                                                managerCollection.GetManager<IDialogProvider>()));
            managerCollection.AddTransient<ScriptEditorVM>(() => new ScriptEditorVM(managerCollection.GetManager<DbEntrySubEditorFactory>(),
                                                                                    managerCollection.GetManager<IWorkspaceMan>(),
                                                                                    managerCollection.GetManager<IDialogProvider>()));
            managerCollection.AddTransient<EntityTemplateEditorVM>(() => new EntityTemplateEditorVM(managerCollection.GetManager<DbEntrySubEditorFactory>(),
                                                                                                    managerCollection.GetManager<IWorkspaceMan>(),
                                                                                                    managerCollection.GetManager<IDialogProvider>()));
            managerCollection.AddTransient<ImageEditorVM>(() => new ImageEditorVM(managerCollection.GetManager<DbEntrySubEditorFactory>(),
                                                                                  managerCollection.GetManager<IWorkspaceMan>(),
                                                                                  managerCollection.GetManager<IDialogProvider>()));
            managerCollection.AddTransient<SoundEditorVM>(() => new SoundEditorVM(managerCollection.GetManager<DbEntrySubEditorFactory>(),
                                                                                  managerCollection.GetManager<IWorkspaceMan>(),
                                                                                  managerCollection.GetManager<IDialogProvider>()));
            managerCollection.AddTransient<MapEditorVM>(() => new MapEditorVM(managerCollection.GetManager<IWorkspaceMan>(),
                                                                              managerCollection.GetManager<MapsDataProvider>(),
                                                                              managerCollection.GetManager<PalettesDataProvider>(),
                                                                              managerCollection.GetManager<ActionSetsDataProvider>(),
                                                                              managerCollection.GetManager<TileAtlasDataProvider>(),
                                                                              managerCollection.GetManager<IDialogProvider>()));
            managerCollection.AddTransient<DataSourceEditorVM>(() => new DataSourceEditorVM(managerCollection.GetManager<DbEntrySubEditorFactory>(),
                                                                                            managerCollection.GetManager<IWorkspaceMan>(),
                                                                                            managerCollection.GetManager<IDialogProvider>()));
        }

        public static void SetupDbEntryEditorFactory(this IManagerCollection managerCollection)
        {
            managerCollection.AddSingleton<DbEntryEditorFactory>(() =>
            {
                var entryEditorFactory = new DbEntryEditorFactory(managerCollection);
                entryEditorFactory.Register<IRepository<IDbTileAtlas>, TileSetEditorVM>();
                entryEditorFactory.Register<IRepository<IDbSpriteAtlas>, SpriteSetEditorVM>();
                entryEditorFactory.Register<IRepository<IDbActionSet>, ActionSetEditorVM>();
                entryEditorFactory.Register<IRepository<IDbPalette>, PaletteEditorVM>();
                entryEditorFactory.Register<IRepository<IDbText>, TextEditorVM>();
                entryEditorFactory.Register<IRepository<IDbScript>, ScriptEditorVM>();
                entryEditorFactory.Register<IRepository<IDbEntityTemplate>, EntityTemplateEditorVM>();
                entryEditorFactory.Register<IRepository<IDbImage>, ImageEditorVM>();
                entryEditorFactory.Register<IRepository<IDbSound>, SoundEditorVM>();
                entryEditorFactory.Register<IRepository<IDbMap>, MapEditorVM>();
                entryEditorFactory.Register<IRepository<IDbDataSource>, DataSourceEditorVM>();
                return entryEditorFactory;
            });
        }

        public static void SetupDbEntrySubEditorFactory(this IManagerCollection managerCollection)
        {
            managerCollection.AddSingleton<DbEntrySubEditorFactory>(() =>
            {
                var subEditorFactory = new DbEntrySubEditorFactory(managerCollection);
                subEditorFactory.Register<IDbActionSet, IDbActionSet>();
                subEditorFactory.Register<IDbFileDataSource, IDbDataSource>();
                subEditorFactory.Register<IDbEpfArchiveDataSource, IDbDataSource>();
                subEditorFactory.Register<IDbEntityTemplateFromFile, IDbEntityTemplate>();
                subEditorFactory.Register<IDbImage, IDbImage>();
                subEditorFactory.Register<IDbPaletteFromBinary, IDbPalette>();
                subEditorFactory.Register<IDbPaletteFromMap, IDbPalette>();
                subEditorFactory.Register<IDbScriptEmbedded, IDbScript>();
                subEditorFactory.Register<IDbScriptFromFile, IDbScript>();
                subEditorFactory.Register<IDbSound, IDbSound>();
                subEditorFactory.Register<IDbSpriteAtlasFromSpr, IDbSpriteAtlas>();
                subEditorFactory.Register<IDbSpriteAtlasFromImage, IDbSpriteAtlas>();
                subEditorFactory.Register<IDbTextEmbedded, IDbText>();
                subEditorFactory.Register<IDbTextFromMap, IDbText>();
                subEditorFactory.Register<IDbTileAtlasFromBlk, IDbTileAtlas>();

                return subEditorFactory;
            });
        }

        #endregion Public Methods
    }
}