using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Editor.VM.Palettes;
using OpenBreed.Editor.VM.Levels;
using OpenBreed.Editor.VM.Tiles;
using System.Drawing;
using OpenBreed.Common.Maps.Readers.MAP;
using OpenBreed.Common.Maps.Builders;
using OpenBreed.Editor.VM.Database;
using OpenBreed.Editor.VM.Props;
using OpenBreed.Editor.VM.Sprites;
using OpenBreed.Editor.VM.Levels.Tools;
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
using OpenBreed.Common.Props;
using OpenBreed.Common.XmlDatabase;
using OpenBreed.Common.Sounds;
using OpenBreed.Editor.VM.Sounds;

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

        public EditorVM(IDialogProvider dialogProvider)
        {
            DialogProvider = dialogProvider;

            Settings = new SettingsMan();
            ToolsMan = new ToolsMan();

            DbEditor = new DbEditorVM(this);
            TileSetEditor = new TileSetEditorVM(this);
            PropSetEditor = new PropSetEditorVM(this);
            PaletteEditor = new PaletteEditorVM(this);
            SpriteViewer = new SpriteViewerVM(this);
            ImageEditor = new ImageEditorVM(this);
            SoundEditor = new SoundEditorVM(this);
            LevelEditor = new LevelEditorVM(this);
            AssetsDataProvider.ExpandVariables = Settings.ExpandVariables;
        }

        #endregion Public Constructors

        #region Public Properties

        public DbEditorVM DbEditor { get; }

        public IDialogProvider DialogProvider { get; }
        public DataProvider DataProvider { get; set; }
        public ImageEditorVM ImageEditor { get; }
        public SoundEditorVM SoundEditor { get; }
        public LevelEditorVM LevelEditor { get; }
        public PaletteEditorVM PaletteEditor { get; }
        public PropSetEditorVM PropSetEditor { get; }
        public TileSetEditorVM TileSetEditor { get; }
        public SettingsMan Settings { get; private set; }
        //public SourceMan SourceMan { get; }

        public SpriteViewerVM SpriteViewer { get; }
        public EditorState State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        public ToolsMan ToolsMan { get; }
        public IUnitOfWork UnitOfWork { get; internal set; }


        #endregion Public Properties

        #region Public Methods

        public LevelVM CreateLevel()
        {
            return new LevelVM(this);
        }

        internal TileSetVM CreateTileSet(TileSetModel tileSet)
        {
            return new TileSetVM(tileSet);
        }

        internal SpriteSetVM CreateSpiteSet(SpriteSetModel spriteSet)
        {
            return new SpriteSetVM(this, spriteSet);
        }

        internal PropSetVM CreatePropSet(IPropSetEntry propSet)
        {
            return new PropSetVM(this, propSet);
        }

        public SpriteSetVM CreateSpriteSet()
        {
            return new SpriteSetVM(this);
        }

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

        public bool TryExit()
        {
            return EditorVMHelper.TryExit(this);
        }

        public void TryRunABTAGame()
        {
            Tools.TryAction(RunABTAGame);
        }

        #endregion Public Methods

        #region Internal Methods

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
            var dbTableEditorConnector = new DbTableEditorConnector(DbEditor.DbTablesEditor.DbTableViewer);
            dbTableEditorConnector.ConnectTo(DbEditor.DbTablesEditor.DbTableSelector);

            LevelEditor.Connect();
            SpriteViewer.Connect();

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
