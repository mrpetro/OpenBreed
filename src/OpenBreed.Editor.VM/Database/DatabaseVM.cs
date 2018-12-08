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
using OpenBreed.Editor.VM.Database.Items;
using OpenBreed.Common.Database;
using System.ComponentModel;
using OpenBreed.Common.Database.Items.Sources;
using OpenBreed.Common.Database.Tables.Sources;
using OpenBreed.Common.Database.Items.Levels;
using OpenBreed.Common.Database.Items.Images;
using OpenBreed.Common.Database.Tables;
using OpenBreed.Common.Database.Tables.Images;
using OpenBreed.Editor.VM.Database.Tables;
using OpenBreed.Common.Database.Tables.Levels;

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

        private DatabaseItemVM _openedItem;
        private LevelDef _currentLevel;
        private DatabaseDef _databaseDef;

        private ProjectState _state;

        #endregion Private Fields

        #region Private Constructors

        internal DatabaseVM(EditorVM root)
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

        public string Name { get; set; }


        public EditorVM Root { get; private set; }

        public ProjectState State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        public DatabaseItemVM OpenedItem
        {
            get { return _openedItem; }
            set { SetProperty(ref _openedItem, value); }
        }

        #endregion Public Properties

        #region Public Methods

        public List<SourceDef> GetAllSourcesOfType(string type)
        {
            return _databaseDef.Tables.OfType<DatabaseSourceTableDef>().FirstOrDefault().Items.FindAll(item => item.Type == type);
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
            var sourceDef = _databaseDef.Tables.OfType<DatabaseSourceTableDef>().FirstOrDefault().Items.FirstOrDefault(item => item.Name == sourceRef);

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

        internal DatabaseTableVM CreateTable(DatabaseTableDef tableDef)
        {
            if (tableDef is DatabaseImageTableDef)
                return new DatabaseImageTableVM(this);
            else if (tableDef is DatabaseLevelTableDef)
                return new DatabaseLevelTableVM(this);
            else if (tableDef is DatabaseSourceTableDef)
                return new DatabaseSourceTableVM(this);
            else
                throw new NotImplementedException(tableDef.ToString());
        }

        internal DatabaseItemVM CreateItem(DatabaseItemDef itemDef)
        {
            if (itemDef is ImageDef)
                return new DatabaseImageItemVM(this);
            else if (itemDef is LevelDef)
                return new DatabaseLevelItemVM(this);
            else if (itemDef is SourceDef)
                return new DatabaseSourceItemVM(this);
            if (itemDef is DatabaseTableDef)
                return CreateTable((DatabaseTableDef)itemDef);

            else
                throw new NotImplementedException(itemDef.ToString());
        }

        internal IEnumerable<DatabaseTableVM> GetTables()
        {
            foreach (var tableDef in _databaseDef.Tables)
            {
                var tableVM = CreateTable(tableDef);
                tableVM.Load(tableDef);
                yield return tableVM;
            }
        }

        internal void Load(string filePath )
        {
            _databaseDef = Tools.RestoreFromXml<DatabaseDef>(filePath);
            FilePath = filePath;
        }

        #endregion Internal Methods

    }
}
