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
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Editor.VM.Palettes;
using OpenBreed.Editor.VM.Scripts;
using OpenBreed.Editor.VM.Sounds;
using OpenBreed.Editor.VM.Sprites;
using OpenBreed.Editor.VM.Texts;
using OpenBreed.Editor.VM.Tiles;
using System;

namespace OpenBreed.Editor.UI.WinForms
{
    internal static class Program
    {
        #region Private Methods

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            //Application.Run(new GLTestForm());

            var application = new EditorApplication();
            application.RegisterInterface<IDialogProvider>(() => new DialogProvider());
            application.RegisterInterface<DbEntryEditorFactory>(() => CreateEntryEditorFactory(application));
            application.RegisterInterface<DbTableFactory>(() => new DbTableFactory());
            application.RegisterInterface<DbEntryFactory>(() => new DbEntryFactory());
            application.RegisterInterface<EditorApplicationVM>(() => new EditorApplicationVM(application));
            application.Run();
        }

        private static DbEntryEditorFactory CreateEntryEditorFactory(EditorApplication application)
        {
            var entryEditorFactory = new DbEntryEditorFactory(application);
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
        }

        #endregion Private Methods
    }
}