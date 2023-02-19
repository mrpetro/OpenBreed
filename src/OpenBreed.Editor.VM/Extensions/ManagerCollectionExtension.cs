using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Formats;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Logging;
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
using System;

namespace OpenBreed.Editor.VM.Extensions
{
    public static class ManagerCollectionExtension
    {
        #region Public Methods

        public static void SetupDbEntrySubEditors(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddTransient<IEntryEditor<IDbTileAtlasFromBlk>, TileSetFromBlkEditorVM>(
                    (sp) => new TileSetFromBlkEditorVM(
                        sp.GetService<TileAtlasDataProvider>(),                                                                                         
                        sp.GetService<PalettesDataProvider>()));


                services.AddTransient<IEntryEditor<IDbActionSet>, ActionSetEmbeddedEditorVM>(
                    (sp) => new ActionSetEmbeddedEditorVM(sp.GetService<ActionSetsDataProvider>()));


                services.AddTransient<IEntryEditor<IDbFileDataSource>, FileDataSourceEditorVM>(
                    (sp) => new FileDataSourceEditorVM());

                services.AddTransient<IEntryEditor<IDbEpfArchiveDataSource>, EpfArchiveFileDataSourceEditorVM>(
                    (sp) => new EpfArchiveFileDataSourceEditorVM());


                services.AddTransient<IEntryEditor<IDbEntityTemplateFromFile>, EntityTemplateFromFileEditorVM>(
                    (sp) => new EntityTemplateFromFileEditorVM(
                        sp.GetService<EntityTemplatesDataProvider>(),                                                                                                         
                        sp.GetService<IModelsProvider>()));

                services.AddTransient<IEntryEditor<IDbImage>, ImageFromFileEditorVM>(
                    (sp) => new ImageFromFileEditorVM(
                        sp.GetService<IWorkspaceMan>(),                                                                                        
                        sp.GetService<IDialogProvider>(),                                                                                           
                        sp.GetService<IModelsProvider>()));

                services.AddTransient<IEntryEditor<IDbSpriteAtlasFromSpr>, SpriteSetFromSprEditorVM>(
                    (sp) => new SpriteSetFromSprEditorVM(
                        sp.GetService<SpriteAtlasDataProvider>(),                                                                                          
                        sp.GetService<PalettesDataProvider>(),                                                                                         
                        sp.GetService<IModelsProvider>()));

                services.AddTransient<IEntryEditor<IDbSpriteAtlasFromImage>, SpriteSetFromImageEditorVM>(
                    (sp) => new SpriteSetFromImageEditorVM(
                        sp.GetService<SpriteAtlasDataProvider>(),                                                                                          
                        sp.GetService<PalettesDataProvider>(),                                                                                        
                        sp.GetService<IModelsProvider>()));


                services.AddTransient<IEntryEditor<IDbPaletteFromBinary>, PaletteFromBinaryEditorVM>(
                    (sp) => new PaletteFromBinaryEditorVM(
                        sp.GetService<PalettesDataProvider>(),
                        sp.GetService<IRepositoryProvider>(),
                        sp.GetService<IModelsProvider>()));


                services.AddTransient<IEntryEditor<IDbPaletteFromMap>, PaletteFromMapEditorVM>(
                    (sp) => new PaletteFromMapEditorVM(
                        sp.GetService<PalettesDataProvider>(),                                                                                          
                        sp.GetService<IModelsProvider>()));

                services.AddTransient<IEntryEditor<IDbPaletteFromLbm>, PaletteFromLbmEditorVM>(
                    (sp) => new PaletteFromLbmEditorVM(
                        sp.GetService<PalettesDataProvider>(),
                        sp.GetService<IModelsProvider>()));

                services.AddTransient<IEntryEditor<IDbTextEmbedded>, TextEmbeddedEditorVM>(
                    (sp) => new TextEmbeddedEditorVM(
                        sp.GetService<TextsDataProvider>()));

                services.AddTransient<IEntryEditor<IDbTextFromMap>, TextFromMapEditorVM>(
                    (sp) => new TextFromMapEditorVM(
                        sp.GetService<TextsDataProvider>(),                                                                                          
                        sp.GetService<IModelsProvider>()));

                services.AddTransient<IEntryEditor<IDbSound>, SoundFromPcmEditorVM>(
                    (sp) => new SoundFromPcmEditorVM(
                        sp.GetService<SoundsDataProvider>()));

                services.AddTransient<IEntryEditor<IDbScriptEmbedded>, ScriptEmbeddedEditorVM>(
                    (sp) => new ScriptEmbeddedEditorVM(
                        sp.GetService<ScriptsDataProvider>()));

                services.AddTransient<IEntryEditor<IDbScriptFromFile>, ScriptFromFileEditorVM>(
                    (sp) => new ScriptFromFileEditorVM(
                        sp.GetService<IWorkspaceMan>(),                                                                                                 
                        sp.GetService<ScriptsDataProvider>(),                                                                                         
                        sp.GetService<IModelsProvider>()));
            });
        }

        public static void SetupCommonViewModels(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddTransient<LoggerVM>((sp) => new LoggerVM(sp.GetService<ILogger>()));
            });
        }

        public static void SetupDbEntryEditors(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddTransient<DbEditorVM>(
                    (sp) => new DbEditorVM(
                        sp,                                                    
                        sp.GetService<DbEntryEditorFactory>(),                                                      
                        sp.GetService<IRepositoryProvider>()));

                services.AddTransient<DbTablesEditorVM>(
                    (sp) => new DbTablesEditorVM(
                        sp.GetService<IWorkspaceMan>(),                                                                   
                        sp.GetService<DbEntryFactory>()));

                services.AddTransient<TileSetEditorVM>(
                    (sp) => new TileSetEditorVM(
                        sp.GetService<DbEntrySubEditorFactory>(),                                                                 
                        sp.GetService<IWorkspaceMan>(),                                                                 
                        sp.GetService<IDialogProvider>()));

                services.AddTransient<SpriteSetEditorVM>(
                    (sp) => new SpriteSetEditorVM(
                        sp.GetService<DbEntrySubEditorFactory>(),                                                                    
                        sp.GetService<IWorkspaceMan>(),
                        sp.GetService<IDialogProvider>()));

                services.AddTransient<ActionSetEditorVM>(
                    (sp) => new ActionSetEditorVM(
                        sp.GetService<DbEntrySubEditorFactory>(),
                        sp.GetService<IWorkspaceMan>(),
                        sp.GetService<IDialogProvider>()));

                services.AddTransient<PaletteEditorVM>(
                    (sp) => new PaletteEditorVM(
                        sp.GetService<DbEntrySubEditorFactory>(),                                                                  
                        sp.GetService<IWorkspaceMan>(),                                                                 
                        sp.GetService<IDialogProvider>()));
                services.AddTransient<TextEditorVM>(
                    (sp) => new TextEditorVM(
                        sp.GetService<DbEntrySubEditorFactory>(),
                        sp.GetService<IWorkspaceMan>(),
                        sp.GetService<IDialogProvider>()));

                services.AddTransient<ScriptEditorVM>(
                    (sp) => new ScriptEditorVM(
                        sp.GetService<DbEntrySubEditorFactory>(),
                        sp.GetService<IWorkspaceMan>(),
                        sp.GetService<IDialogProvider>()));

                services.AddTransient<EntityTemplateEditorVM>(
                    (sp) => new EntityTemplateEditorVM(
                        sp.GetService<DbEntrySubEditorFactory>(),
                        sp.GetService<IWorkspaceMan>(),
                        sp.GetService<IDialogProvider>()));

                services.AddTransient<ImageEditorVM>(
                    (sp) => new ImageEditorVM(
                        sp.GetService<DbEntrySubEditorFactory>(),
                        sp.GetService<IWorkspaceMan>(),
                        sp.GetService<IDialogProvider>()));
                services.AddTransient<SoundEditorVM>(
                    (sp) => new SoundEditorVM(
                        sp.GetService<DbEntrySubEditorFactory>(),                                                            
                        sp.GetService<IWorkspaceMan>(),                                                            
                        sp.GetService<IDialogProvider>()));
                services.AddTransient<MapEditorVM>(
                    (sp) => new MapEditorVM(
                        sp.GetService<IWorkspaceMan>(),                                                        
                        sp.GetService<MapsDataProvider>(),                                                      
                        sp.GetService<PalettesDataProvider>(),                                                      
                        sp.GetService<ActionSetsDataProvider>(),                                                        
                        sp.GetService<TileAtlasDataProvider>(),                                                    
                        sp.GetService<IDialogProvider>()));
                services.AddTransient<DataSourceEditorVM>(
                    (sp) => new DataSourceEditorVM(
                        sp.GetService<DbEntrySubEditorFactory>(),
                        sp.GetService<IWorkspaceMan>(),
                        sp.GetService<IDialogProvider>()));
            });
        }

        public static void SetupDbEntryEditorFactory(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<DbEntryEditorFactory>((sp) =>
                {
                    var entryEditorFactory = new DbEntryEditorFactory(sp);
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
            });
        }

        public static void SetupDbEntrySubEditorFactory(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<DbEntrySubEditorFactory>((sp) =>
                {
                    var subEditorFactory = new DbEntrySubEditorFactory(sp);
                    subEditorFactory.Register<IDbActionSet, IDbActionSet>();
                    subEditorFactory.Register<IDbFileDataSource, IDbDataSource>();
                    subEditorFactory.Register<IDbEpfArchiveDataSource, IDbDataSource>();
                    subEditorFactory.Register<IDbEntityTemplateFromFile, IDbEntityTemplate>();
                    subEditorFactory.Register<IDbImage, IDbImage>();
                    subEditorFactory.Register<IDbPaletteFromBinary, IDbPalette>();
                    subEditorFactory.Register<IDbPaletteFromMap, IDbPalette>();
                    subEditorFactory.Register<IDbPaletteFromLbm, IDbPalette>();
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
            });


        }

        #endregion Public Methods
    }
}