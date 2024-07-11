using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Formats;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Interface.Logging;
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
using OpenBreed.Database.Interface.Items.TileStamps;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Actions;
using OpenBreed.Editor.VM.Database;
using OpenBreed.Editor.VM.DataSources;
using OpenBreed.Editor.VM.EntityTemplates;
using OpenBreed.Editor.VM.Images;
using OpenBreed.Editor.VM.Logging;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Editor.VM.Options;
using OpenBreed.Editor.VM.Palettes;
using OpenBreed.Editor.VM.Renderer;
using OpenBreed.Editor.VM.Scripts;
using OpenBreed.Editor.VM.Sounds;
using OpenBreed.Editor.VM.Sprites;
using OpenBreed.Editor.VM.Texts;
using OpenBreed.Editor.VM.Tiles;
using OpenBreed.Editor.VM.TileStamps;
using OpenBreed.Editor.VM.Tools;
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
                services.AddTransient<LoggerVM>();
            });
        }



        public static void SetupEditorViewModels(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddTransient<EntryEditorVM<IDbImage>>();
                services.AddTransient<EntryEditorVM<IDbPalette>>();
                services.AddTransient<EntryEditorVM<IDbEntityTemplate>>();
                services.AddTransient<EntryEditorVM<IDbSound>>();
                services.AddTransient<EntryEditorVM<IDbMap>>();
                services.AddTransient<EntryEditorVM<IDbDataSource>>();
                services.AddTransient<EntryEditorVM<IDbText>>();
                services.AddTransient<EntryEditorVM<IDbScript>>();
                services.AddTransient<EntryEditorVM<IDbActionSet>>();
                services.AddTransient<EntryEditorVM<IDbSpriteAtlas>>();
                services.AddTransient<EntryEditorVM<IDbTileAtlas>>();
                services.AddTransient<EntryEditorVM<IDbTileStamp>>();

                services.AddTransient<DbEditorVM>();
                services.AddTransient<DbTablesEditorVM>();
                services.AddTransient<DbTableSelectorVM>();
                services.AddTransient<DbTableEditorVM>();
                services.AddTransient<DbEntriesEditorVM>();
                services.AddTransient<TileSetFromBlkEditorVM>();
                services.AddTransient<TileSetFromAcbmEditorVM>();
                services.AddTransient<TileStampEditorVM>();
                services.AddTransient<ActionSetEmbeddedEditorVM>();
                services.AddTransient<SpriteSetFromImageEditorVM>();
                services.AddTransient<SpriteSetFromSprEditorVM>();
                services.AddTransient<PaletteFromMapEditorVM>();
                services.AddTransient<PaletteFromLbmEditorVM>();
                services.AddTransient<PaletteFromBinaryEditorVM>();
                services.AddTransient<ImageFromAcbmEditorVM>();
                services.AddTransient<ImageFromIffEditorVM>();
                services.AddTransient<ScriptEmbeddedEditorVM>();
                services.AddTransient<ScriptFromFileEditorVM>();
                services.AddTransient<TextEmbeddedEditorVM>();
                services.AddTransient<TextFromMapEditorVM>();
                services.AddTransient<PcmSoundEditorVM>();
                services.AddTransient<MapEditorVM>();
                services.AddTransient<EntityTemplateFromFileEditorVM>();
                services.AddTransient<EpfArchiveFileDataSourceEditorVM>();
                services.AddTransient<FileDataSourceEditorVM>();
                services.AddTransient<OptionsVM>();
                services.AddTransient<AbtaPasswordGeneratorVM>();
            });
        }

        public static void SetupDbEntryEditorFactory(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<DbEntryEditorFactory>((sp) =>
                {
                    var entryEditorFactory = new DbEntryEditorFactory(sp);

                    entryEditorFactory.RegisterEditor<IDbMap>();
                    entryEditorFactory.RegisterEditor<IDbSound>();
                    entryEditorFactory.RegisterEditor<IDbPalette>();
                    entryEditorFactory.RegisterEditor<IDbTileAtlas>();
                    entryEditorFactory.RegisterEditor<IDbTileStamp>();
                    entryEditorFactory.RegisterEditor<IDbImage>();
                    entryEditorFactory.RegisterEditor<IDbActionSet>();
                    entryEditorFactory.RegisterEditor<IDbSpriteAtlas>();
                    entryEditorFactory.RegisterEditor<IDbScript>();
                    entryEditorFactory.RegisterEditor<IDbText>();
                    entryEditorFactory.RegisterEditor<IDbEntityTemplate>();
                    entryEditorFactory.RegisterEditor<IDbDataSource>();


                    entryEditorFactory.Register<IDbMap, MapEditorVM>();
                    entryEditorFactory.Register<IDbSound, PcmSoundEditorVM>();
                    entryEditorFactory.Register<IDbPaletteFromMap, PaletteFromMapEditorVM>();
                    entryEditorFactory.Register<IDbPaletteFromLbm, PaletteFromLbmEditorVM>();
                    entryEditorFactory.Register<IDbPaletteFromBinary, PaletteFromBinaryEditorVM>();
                    entryEditorFactory.Register<IDbTileAtlasFromBlk, TileSetFromBlkEditorVM>();
                    entryEditorFactory.Register<IDbTileAtlasFromAcbm, TileSetFromAcbmEditorVM>();
                    entryEditorFactory.Register<IDbTileStamp, TileStampEditorVM>();
                    entryEditorFactory.Register<IDbIffImage, ImageFromIffEditorVM>();
                    entryEditorFactory.Register<IDbAcbmImage, ImageFromAcbmEditorVM>();
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

        #endregion Public Methods
    }
}