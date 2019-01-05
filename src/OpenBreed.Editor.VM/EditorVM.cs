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
using OpenBreed.Common.Sources;
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

        private DatabaseVM _database;
        private EditorState _state;

        #endregion Private Fields

        #region Public Constructors

        public EditorVM(IDialogProvider dialogProvider)
        {
            DialogProvider = dialogProvider;

            Settings = new SettingsMan();
            ToolsMan = new ToolsMan();

            TileSetEditor = new TileSetEditorVM(this);
            PropSetEditor = new PropSetEditorVM(this);
            PaletteEditor = new PaletteEditorVM(this);
            DbTablesEditor = new DbTablesEditorVM(this);
            SpriteViewer = new SpriteViewerVM(this);
            ImageEditor = new ImageEditorVM(this);
            SoundEditor = new SoundEditorVM(this);
            LevelEditor = new LevelEditorVM(this);
            DataSourceProvider.ExpandVariables = Settings.ExpandVariables;
        }

        #endregion Public Constructors

        #region Public Properties

        public DatabaseVM Database
        {
            get { return _database; }
            set { SetProperty(ref _database, value); }
        }

        public DbTablesEditorVM DbTablesEditor { get; }
        public IDialogProvider DialogProvider { get; }
        public DataProvider DataProvider { get; private set; }
        public ImageEditorVM ImageEditor { get; }
        public SoundEditorVM SoundEditor { get; }
        public LevelEditorVM LevelEditor { get; }
        public PaletteEditorVM PaletteEditor { get; }
        public PropSetEditorVM PropSetEditor { get; }
        public SettingsMan Settings { get; private set; }
        //public SourceMan SourceMan { get; }

        public SpriteViewerVM SpriteViewer { get; }
        public EditorState State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        public TileSetEditorVM TileSetEditor { get; }
        public ToolsMan ToolsMan { get; }
        public IUnitOfWork UnitOfWork { get; internal set; }


        #endregion Public Properties

        #region Public Methods

        public LevelVM CreateLevel()
        {
            return new LevelVM(this);
        }

        internal SoundVM CreateSound()
        {
            return new SoundVM();
        }

        private void UpdateSound(SoundVM source, SoundModel target)
        {
            target.BitsPerSample = source.BitsPerSample;
            target.Channels = source.Channels;
            target.SampleRate = source.SampleRate;
            target.Data = source.Data;
        }

        internal TileSetVM CreateTileSet(TileSetModel tileSet)
        {
            return new TileSetVM(tileSet);
        }

        internal SpriteSetVM CreateSpiteSet(SpriteSetModel spriteSet)
        {
            return new SpriteSetVM(this, spriteSet);
        }

        public PaletteVM CreatePalette(PaletteModel palette)
        {
            var paletteVM = new PaletteVM();
            return paletteVM;
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

        public DatabaseVM OpenXmlDatabase(string xmlFilePath)
        {
            var xmlDatabase = new XmlDatabase(xmlFilePath, DatabaseMode.Read);
            UnitOfWork = new XmlUnitOfWork(xmlDatabase);
            DataProvider = new DataProvider(UnitOfWork);

            return new DatabaseVM(this, UnitOfWork);
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

        public void SaveDatabase(string filePath)
        {
            throw new NotImplementedException();
        }

        public bool TryCloseDatabase()
        {
            return EditorVMHelper.TryCloseDatabase(this);
        }

        public bool TryExit()
        {
            return EditorVMHelper.TryExit(this);
        }

        public void TryOpenDatabase()
        {
            EditorVMHelper.TryOpenDatabase(this);
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
            PropSetEditor.Connect();
            DbTablesEditor.Connect();
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
