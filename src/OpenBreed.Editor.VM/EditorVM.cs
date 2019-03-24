using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Editor.VM.Palettes;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Editor.VM.Tiles;
using System.Drawing;
using OpenBreed.Common.Maps.Readers.MAP;
using OpenBreed.Common.Maps.Builders;
using OpenBreed.Editor.VM.Database;
using OpenBreed.Editor.VM.Actions;
using OpenBreed.Editor.VM.Sprites;
using OpenBreed.Common.Palettes;
using System.Diagnostics;
using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Editor.Cfg;
using System.ComponentModel;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Images;
using OpenBreed.Common.Formats;
using OpenBreed.Common.Tiles;
using OpenBreed.Common.Sprites;
using OpenBreed.Common.Actions;
using OpenBreed.Common.XmlDatabase;
using OpenBreed.Common.Sounds;
using OpenBreed.Editor.VM.Sounds;
using OpenBreed.Editor.VM.Database.Entries;
using OpenBreed.Editor.VM.Assets;
using OpenBreed.Common.Images;
using OpenBreed.Common.Maps;
using OpenBreed.Common.Assets;
using OpenBreed.Common.Data;
using OpenBreed.Editor.VM.Logging;
using OpenBreed.Common.Texts;
using OpenBreed.Editor.VM.Texts;

namespace OpenBreed.Editor.VM
{
    public enum EditorState
    {
        Active,
        Exiting,
        Exited
    }

    public class EditorVM : BaseViewModel, IDisposable
    {

        #region Private Fields

        private EditorState _state;

        #endregion Private Fields

        #region Public Constructors

        public EditorVM()
        {
            Logger = new LoggerVM();

            ServiceLocator.Instance.RegisterService<EditorVM>(this);

            var entryEditorFactory = new DbEntryEditorFactory();
            entryEditorFactory.Register<IRepository<ITileSetEntry>, TileSetEditorVM>();
            entryEditorFactory.Register<IRepository<IActionSetEntry>, ActionSetEditorVM>();
            entryEditorFactory.Register<IRepository<IPaletteEntry>, PaletteEditorVM>();
            entryEditorFactory.Register<IRepository<ITextEntry>, TextEditorVM>();
            entryEditorFactory.Register<IRepository<IImageEntry>, ImageEditorVM>();
            entryEditorFactory.Register<IRepository<ISoundEntry>, SoundEditorVM>();
            entryEditorFactory.Register<IRepository<IMapEntry>, MapEditorVM>();
            entryEditorFactory.Register<IRepository<IAssetEntry>, AssetEditorVM>();
            ServiceLocator.Instance.RegisterService<DbEntryEditorFactory>(entryEditorFactory);

            ServiceLocator.Instance.RegisterService<DbTableFactory>(new DbTableFactory());
            ServiceLocator.Instance.RegisterService<DbEntryFactory>(new DbEntryFactory());


            DialogProvider = ServiceLocator.Instance.GetService<IDialogProvider>();


            Settings = new SettingsMan();

            DbEditor = new DbEditorVM();
            //PaletteEditor = new PaletteEditorVM();
            //SpriteViewer = new SpriteViewerVM(this);
            AssetsDataProvider.ExpandVariables = Settings.ExpandVariables;
        }

        #endregion Public Constructors

        #region Public Properties

        public DbEditorVM DbEditor { get; }
        public IDialogProvider DialogProvider { get; }
        public LoggerVM Logger { get; }
        public PaletteEditorVM PaletteEditor { get; }
        public SettingsMan Settings { get; private set; }
        public Action<LoggerVM> ShowLoggerAction { get; set; }

        //public SourceMan SourceMan { get; }
        public EditorState State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        #endregion Public Properties

        #region Public Methods

        //public SpriteViewerVM SpriteViewer { get; }
        public void Dispose()
        {
            Settings.Store();
        }

        public void Run()
        {
            try
            {
                Initialize();
                DialogProvider.ShowEditorView(this);
            }
            catch (Exception ex)
            {
                DialogProvider.ShowMessage("Critical exception: " + ex, "Open Breed Editor critial exception");
            }
        }

        public void ShowLogger()
        {
            ShowLoggerAction?.Invoke(Logger);
        }

        public bool TryExit()
        {
            return EditorVMHelper.TryExit(this);
        }

        public void TryRunABTAGame()
        {
            Other.TryAction(RunABTAGame);
        }

        #endregion Public Methods

        #region Internal Methods

        internal SpriteSetVM CreateSpiteSet(SpriteSetModel spriteSet)
        {
            return new SpriteSetVM(this, spriteSet);
        }

        internal bool TrySaveDatabase()
        {
            return EditorVMHelper.TrySaveDatabase(this);
        }

        #endregion Internal Methods

        #region Private Methods

        private void Initialize()
        {
            var dbTableSelectorConnector = new DbTableSelectorConnector(DbEditor.DbTablesEditor.DbTableSelector);
            dbTableSelectorConnector.ConnectTo(DbEditor);
            var dbTableEditorConnector = new DbTableEditorConnector(DbEditor.DbTablesEditor.DbTableEditor);
            dbTableEditorConnector.ConnectTo(DbEditor.DbTablesEditor.DbTableSelector);

            //LevelEditor.Connect();
            //SpriteViewer.Connect();

            Settings.Restore();
        }
        private void RunABTAGame()
        {
            Process proc = new Process();
            proc.StartInfo.FileName = Settings.Cfg.Options.ABTA.GameRunFilePath;
            proc.StartInfo.Arguments = Settings.Cfg.Options.ABTA.GameRunFileArgs;
            proc.StartInfo.WorkingDirectory = Settings.Cfg.Options.ABTA.GameFolderPath;
            proc.Start();
        }

        #endregion Private Methods

    }
}
