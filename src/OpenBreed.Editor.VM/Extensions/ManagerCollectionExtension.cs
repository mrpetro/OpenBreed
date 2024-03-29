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

                services.AddTransient<TileSetFromBlkEditorVM>();
                services.AddTransient<ActionSetEmbeddedEditorVM>();
                services.AddTransient<SpriteSetFromImageEditorVM>();
                services.AddTransient<SpriteSetFromSprEditorVM>();
                services.AddTransient<PaletteFromMapEditorVM>();
                services.AddTransient<PaletteFromLbmEditorVM>();
                services.AddTransient<PaletteFromBinaryEditorVM>();
                services.AddTransient<ImageFromFileEditorVM>();
                services.AddTransient<ScriptEmbeddedEditorVM>();
                services.AddTransient<ScriptFromFileEditorVM>();
                services.AddTransient<TextEmbeddedEditorVM>();
                services.AddTransient<TextFromMapEditorVM>();
                services.AddTransient<PcmSoundEditorVM>();
                services.AddTransient<MapEditorVM>();
                services.AddTransient<EntityTemplateFromFileEditorVM>();
                services.AddTransient<EpfArchiveFileDataSourceEditorVM>();
                services.AddTransient<FileDataSourceEditorVM>();
            });
        }

        public static void SetupDbEntryEditorFactory(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<DbEntryEditorFactory>((sp) =>
                {
                    var entryEditorFactory = new DbEntryEditorFactory(sp);

                    entryEditorFactory.Register<IDbMap, MapEditorVM>();
                    entryEditorFactory.Register<IDbSound, PcmSoundEditorVM>();
                    entryEditorFactory.Register<IDbPaletteFromMap, PaletteFromMapEditorVM>();
                    entryEditorFactory.Register<IDbPaletteFromLbm, PaletteFromLbmEditorVM>();
                    entryEditorFactory.Register<IDbPaletteFromBinary, PaletteFromBinaryEditorVM>();
                    entryEditorFactory.Register<IDbTileAtlasFromBlk, TileSetFromBlkEditorVM>();
                    entryEditorFactory.Register<IDbImage, ImageFromFileEditorVM>();
                    entryEditorFactory.Register<IDbActionSet, ActionSetEmbeddedEditorVM>();
                    entryEditorFactory.Register<IDbSpriteAtlasFromImage, SpriteSetFromImageEditorVM>();
                    entryEditorFactory.Register<IDbSpriteAtlasFromSpr, SpriteSetFromSprEditorVM>();
                    entryEditorFactory.Register<IDbScriptEmbedded, ScriptEmbeddedEditorVM>();
                    entryEditorFactory.Register<IDbScriptFromFile, ScriptFromFileEditorVM>();
                    entryEditorFactory.Register<IDbTextEmbedded, TextEmbeddedEditorVM>();
                    entryEditorFactory.Register<IDbTextFromMap, TextFromMapEditorVM>();
                    entryEditorFactory.Register<IDbEntityTemplateFromFile, EntityTemplateFromFileEditorVM>();
                    entryEditorFactory.Register<IDbEpfArchiveDataSource, EpfArchiveFileDataSourceEditorVM>();
                    entryEditorFactory.Register<IDbFileDataSource, FileDataSourceEditorVM>();
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
                    subEditorFactory.Register<IDbScriptEmbedded, IDbScript>();
                    subEditorFactory.Register<IDbScriptFromFile, IDbScript>();
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