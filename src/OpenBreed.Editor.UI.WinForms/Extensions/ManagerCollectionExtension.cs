using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Formats;
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
using OpenBreed.Editor.VM.DataSources;
using OpenBreed.Editor.VM.EntityTemplates;
using OpenBreed.Editor.VM.Images;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Editor.VM.Palettes;
using OpenBreed.Editor.VM.Scripts;
using OpenBreed.Editor.VM.Sounds;
using OpenBreed.Editor.VM.Sprites;
using OpenBreed.Editor.VM.Texts;
using OpenBreed.Editor.VM.Tiles;

namespace OpenBreed.Editor.UI.WinForms.Extensions
{
    public static class ManagerCollectionExtension
    {
        #region Public Methods


        public static void SetupDbEntrySubEditors(this IManagerCollection managerCollection)
        {
            managerCollection.AddTransient<IEntryEditor<ITileSetFromBlkEntry>>(() => new TileSetFromBlkEditorVM(managerCollection.GetManager<DataProvider>().TileSets,
                                                                                                                managerCollection.GetManager<DataProvider>().Palettes));
        }

        public static void SetupDbEntryEditorFactory(this IManagerCollection managerCollection)
        {
            managerCollection.AddSingleton<DbEntryEditorFactory>(() =>
            {
                var entryEditorFactory = new DbEntryEditorFactory();
                entryEditorFactory.Register<IRepository<ITileSetEntry>, TileSetEditorVM>();
                entryEditorFactory.Register<IRepository<ISpriteSetEntry>, SpriteSetEditorVM>();
                entryEditorFactory.Register<IRepository<IActionSetEntry>, ActionSetEditorVM>();
                entryEditorFactory.Register<IRepository<IPaletteEntry>, PaletteEditorVM>();
                entryEditorFactory.Register<IRepository<ITextEntry>, TextEditorVM>();
                entryEditorFactory.Register<IRepository<IScriptEntry>, ScriptEditorVM>();
                entryEditorFactory.Register<IRepository<IEntityTemplateEntry>, EntityTemplateEditorVM>();
                entryEditorFactory.Register<IRepository<IImageEntry>, ImageEditorVM>();
                entryEditorFactory.Register<IRepository<ISoundEntry>, SoundEditorVM>();
                entryEditorFactory.Register<IRepository<IMapEntry>, MapEditorVM>();
                entryEditorFactory.Register<IRepository<IDataSourceEntry>, DataSourceEditorVM>();
                return entryEditorFactory;
            });
        }

        #endregion Public Methods
    }
}