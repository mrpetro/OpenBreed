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
using OpenBreed.Editor.VM.Levels;
using OpenBreed.Editor.VM.Sources;
using OpenBreed.Editor.VM.Props;
using OpenBreed.Editor.VM.Database.Sources;
using OpenBreed.Editor.VM.Sprites;
using OpenBreed.Editor.VM.Levels.Builders;
using OpenBreed.Editor.VM.Maps.Tools;
using OpenBreed.Common.Palettes;
using OpenBreed.Editor.VM.Project;
using System.Diagnostics;
using OpenBreed.Common;
using OpenBreed.Common.Logging;
using OpenBreed.Editor.Cfg;
using System.ComponentModel;
using OpenBreed.Editor.VM.Base;

namespace OpenBreed.Editor.VM
{
    public delegate void DatabaseChangedEventHandler(object sender, DatabaseChangedEventArgs e);

    public class DatabaseChangedEventArgs : EventArgs
    {

        #region Public Constructors

        public DatabaseChangedEventArgs(GameDatabase database)
        {
            Database = database;
        }

        #endregion Public Constructors

        #region Public Properties

        public GameDatabase Database { get; set; }

        #endregion Public Properties

    }

    public class EditorVM : BaseViewModel, IDisposable
    {
        public IDialogProvider DialogProvider { get; private set; }

        #region Public Constructors

        public EditorVM(IDialogProvider dialogProvider)
        {
            DialogProvider = dialogProvider;

            Settings = new SettingsMan();
            ToolsMan = new ToolsMan();
            Project = new ProjectVM(this);
            TileSets = new TileSetsVM(this);
            Palettes = new PalettesVM(this);

            SpriteSets = new BindingList<SpriteSetVM>();
            SpriteSets.ListChanged += (s, e) => OnPropertyChanged(nameof(SpriteSets));

            SpriteSetViewer = new SpriteSetViewerVM(this);
            SpriteViewer = new SpriteViewerVM(this);
            PropSets = new PropSetsVM(this);
            Projects = new ProjectsHandler(this);
            Map = new MapVM(this);
            MapBodyViewer = new MapBodyEditorM(this);
            Sources = new SourcesHandler(this);
        }

        #endregion Public Constructors

        #region Public Events

        public event DatabaseChangedEventHandler DatabaseChanged;

        #endregion Public Events

        #region Public Properties

        public GameDatabase CurrentDatabase { get; private set; }
        public MapVM Map { get; private set; }
        public PalettesVM Palettes { get; private set; }
        public ProjectVM Project { get; private set; }

        public MapBodyEditorM MapBodyViewer { get; private set; }

        public ProjectsHandler Projects { get; set; }

        public PropSetsVM PropSets { get; private set; }

        public SettingsMan Settings { get; private set; }

        public SourcesHandler Sources { get; set; }

        public SpriteSetViewerVM SpriteSetViewer { get; set; }
        public SpriteViewerVM SpriteViewer { get; set; }
        public BindingList<SpriteSetVM> SpriteSets { get; private set; }

        public TileSetsVM TileSets { get; private set; }

        public ToolsMan ToolsMan { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void CloseDatabase()
        {
            if (CurrentDatabase == null)
            {
                LogMan.Instance.LogWarning("No database currently opened.");
                return;
            }

            Sources.CloseAll();

            CurrentDatabase = null;
            OnDatabaseChanged(new DatabaseChangedEventArgs(CurrentDatabase));
        }

        public void Dispose()
        {
            Settings.Store();
        }

        public void AddSpriteSet(string spriteSetRef)
        {
            var spriteSetSourceDef = CurrentDatabase.GetSourceDef(spriteSetRef);
            if (spriteSetSourceDef == null)
                throw new Exception("No SpriteSetSource definition found!");

            var source = Sources.GetSource(spriteSetSourceDef);

            if (source == null)
                throw new Exception("SpriteSet source error: " + spriteSetRef);

            SpriteSets.Add(SpriteSetVM.Create(this, source));
        }

        public void OpenABTADatabase()
        {
            string abtaDBPath = Path.Combine(ProgramTools.AppDir, GameDatabaseDef.DEFAULT_ABTA_DB_PATH);
            OpenDatabase(abtaDBPath);
        }

        public void OpenDatabase(string filePath)
        {
            if (CurrentDatabase != null)
            {
                if (OpenBreed.Common.Tools.GetNormalizedPath(filePath) == OpenBreed.Common.Tools.GetNormalizedPath(CurrentDatabase.FilePath))
                {
                    LogMan.Instance.LogWarning("Database already opened.");
                    return;
                }
            }

            var databaseDef = GameDatabaseDef.Load(filePath);
            CurrentDatabase = new GameDatabase(databaseDef, filePath);
            OnDatabaseChanged(new DatabaseChangedEventArgs(CurrentDatabase));
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

        public void TryRunABTAGame()
        {
            Tools.TryAction(RunABTAGame);
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void OnDatabaseChanged(DatabaseChangedEventArgs e)
        {
            if (DatabaseChanged != null) DatabaseChanged(this, e);
        }

        #endregion Protected Methods

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
