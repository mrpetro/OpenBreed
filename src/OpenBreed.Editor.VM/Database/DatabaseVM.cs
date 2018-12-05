using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Sources;
using OpenBreed.Editor.VM.Tiles;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Editor.VM.Props;
using OpenBreed.Editor.VM.Maps.Tools;
using OpenBreed.Editor.VM.Base;
using System.IO;
using OpenBreed.Common;
using OpenBreed.Editor.VM.Database;
using OpenBreed.Common.Database.Sources;

namespace OpenBreed.Editor.VM.Database
{
    public enum ProjectState
    {
        Closed,
        New,
        Opened,
        Closing
    }

    public class DatabaseVM : BaseViewModel
    {

        #region Private Fields

        private LevelDef _currentLevel;
        private GameDatabaseDef _databaseDef;
        private ProjectState _state;

        #endregion Private Fields

        #region Private Constructors

        private DatabaseVM(EditorVM root)
        {
            Root = root;
        }

        #endregion Private Constructors

        #region Public Properties

        public LevelDef CurrentLevel
        {
            get { return _currentLevel; }
            set { SetProperty(ref _currentLevel, value); }
        }

        public string FilePath { get; private set; }
        public bool IsLevelOpened { get { return CurrentLevel != null; } }

        public List<LevelDef> LevelDefs { get { return _databaseDef.LevelDefs; } }
        public string Name { get; set; }
        public EditorVM Root { get; private set; }

        public ProjectState State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public List<SourceDef> GetAllSourcesOfType(string type)
        {
            return _databaseDef.SourceDefs.FindAll(item => item.Type == type);
        }

        public LevelDef GetLevelDef(int id)
        {
            var levelDef = _databaseDef.LevelDefs.FirstOrDefault(item => item.Id == id);

            if (levelDef == null)
                throw new InvalidOperationException("Level(" + id + ") not found!");

            return levelDef;
        }

        public SourceDef GetSourceDef(string sourceRef)
        {
            var sourceDef = _databaseDef.SourceDefs.FirstOrDefault(item => item.Name == sourceRef);

            if (sourceDef == null)
                throw new InvalidOperationException("Source " + sourceRef + " not found!");

            return sourceDef;
        }

        public void Load(SourceDef sourceDef)
        {
            var source = Root.Sources.GetSource(sourceDef);

            var levelDef = source.Load() as LevelDef;

            Root.TileSets.Clear();
            Root.AddTileSet(levelDef.TileSetResourceRef);

            Root.TileSetSelector.CurrentItem = Root.TileSets.FirstOrDefault();

            Root.SpriteSets.Clear();
            foreach (var spriteSetSourceRef in levelDef.SpriteSetResourceRefs)
                Root.AddSpriteSet(spriteSetSourceRef);

            Root.SpriteSetViewer.CurrentItem = Root.SpriteSets.FirstOrDefault();
            if (Root.SpriteSetViewer.CurrentItem != null)
                Root.SpriteViewer.CurrentItem = Root.SpriteSetViewer.CurrentItem.Items.FirstOrDefault();

            if (levelDef.PropertySetResourceRef != null)
                Root.PropSets.AddPropertySet(levelDef.PropertySetResourceRef);

            var mapSourceDef = GetSourceDef(levelDef.MapResourceRef);
            if (mapSourceDef != null)
                Root.Map.Load(mapSourceDef);

            CurrentLevel = levelDef;
        }

        public bool TryClose()
        {


            Root.Map.TryClose();
            return true;
        }

        public void TryNewLevel()
        {
            Root.DialogProvider.ShowMessage("Creating new project is not implemented yet.", "Feature not implemented");
        }

        public bool TryOpenLevelDef()
        {
            var openFileDialog = Root.DialogProvider.OpenFileDialog();
            openFileDialog.Title = "Open Map project file...";
            openFileDialog.Filter = "OpenABEd project files (*.xml)|*.xml|All Files (*.*)|*.*";
            openFileDialog.Multiselect = false;
            var answer = openFileDialog.Show();

            if (answer == DialogAnswer.OK)
            {
                string filePath = openFileDialog.FileName;

                var sourceDef = new DirectoryFileSourceDef();
                sourceDef.DirectoryPath = Path.GetDirectoryName(filePath);
                sourceDef.Name = Path.GetFileName(filePath);
                sourceDef.Type = "LevelXML";

                Load(sourceDef);

                return true;
            }

            return false;
        }

        #endregion Public Methods

        #region Internal Methods

        internal static DatabaseVM Create(EditorVM root, string filePath)
        {
            var database = new DatabaseVM(root);
            database._databaseDef = GameDatabaseDef.Load(filePath);
            database.FilePath = filePath;

            return database;
        }

        #endregion Internal Methods

    }
}
