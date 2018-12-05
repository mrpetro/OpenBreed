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
using OpenBreed.Editor.VM.Sources;
using OpenBreed.Editor.VM.Props;
using OpenBreed.Editor.VM.Sprites;
using OpenBreed.Editor.VM.Maps.Tools;
using OpenBreed.Common.Palettes;
using System.Diagnostics;
using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Editor.Cfg;
using System.ComponentModel;
using OpenBreed.Editor.VM.Base;
using OpenBreed.Editor.VM.Images;
using OpenBreed.Common.Database.Sources;

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
            TileSetSelector = new TileSetSelectorVM(this);
            Palettes = new PalettesVM(this);

            SpriteSets = new BindingList<SpriteSetVM>();
            SpriteSets.ListChanged += (s, e) => OnPropertyChanged(nameof(SpriteSets));

            TileSets = new BindingList<TileSetVM>();
            TileSets.ListChanged += (s, e) => OnPropertyChanged(nameof(TileSets));

            TileSetViewer = new TileSetViewerVM(this);
            SpriteSetViewer = new SpriteSetSelectorVM(this);
            SpriteViewer = new SpriteViewerVM(this);
            ImageViewer = new ImageViewerVM(this);
            PropSets = new PropSetsVM(this);
            Map = new MapVM(this);
            MapBodyViewer = new MapBodyEditorVM(this);
            Sources = new SourcesHandler(this);
        }

        #endregion Public Constructors

        #region Public Properties

        public DatabaseVM Database
        {
            get { return _database; }
            set { SetProperty(ref _database, value); }
        }

        public IDialogProvider DialogProvider { get; private set; }

        public ImageViewerVM ImageViewer { get; private set; }

        public MapVM Map { get; private set; }

        public MapBodyEditorVM MapBodyViewer { get; private set; }

        public PalettesVM Palettes { get; private set; }

        public PropSetsVM PropSets { get; private set; }

        public SettingsMan Settings { get; private set; }

        public SourcesHandler Sources { get; set; }

        public BindingList<SpriteSetVM> SpriteSets { get; private set; }

        public SpriteSetSelectorVM SpriteSetViewer { get; set; }

        public SpriteViewerVM SpriteViewer { get; set; }

        public EditorState State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        public BindingList<TileSetVM> TileSets { get; private set; }

        public TileSetSelectorVM TileSetSelector { get; private set; }

        public TileSetViewerVM TileSetViewer { get; private set; }

        public ToolsMan ToolsMan { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void AddSpriteSet(string spriteSetRef)
        {
            var spriteSetSourceDef = Database.GetSourceDef(spriteSetRef);
            if (spriteSetSourceDef == null)
                throw new Exception("No SpriteSetSource definition found!");

            var source = Sources.GetSource(spriteSetSourceDef);

            if (source == null)
                throw new Exception("SpriteSet source error: " + spriteSetRef);

            SpriteSets.Add(SpriteSetVM.Create(this, source));
        }

        public void AddTileSet(string tileSetRef)
        {
            var tileSetSourceDef = Database.GetSourceDef(tileSetRef);
            if (tileSetSourceDef == null)
                throw new Exception("No TileSetSource definition found with name: " + tileSetRef);

            var source = Sources.GetSource(tileSetSourceDef);

            if (source == null)
                throw new Exception("TileSet source error: " + tileSetRef);

            TileSets.Add(TileSetVM.Create(this, source));
        }

        public void Dispose()
        {
            Settings.Store();
        }

        public void Run()
        {
            try
            {
                Settings.Restore();
            }
            catch (Exception ex)
            {
                DialogProvider.ShowMessage("Critical exception: " + ex, "Critial exception");
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

        #region Private Methods

        private void Initialize()
        {
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
