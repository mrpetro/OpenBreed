using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenBreed.Editor.VM.Levels;
using OpenBreed.Editor.VM.Levels.Readers.XML;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Sources;
using OpenBreed.Editor.VM.Database.Sources;
using OpenBreed.Editor.VM.Tiles;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Editor.VM.Props;
using OpenBreed.Editor.VM.Maps.Tools;
using OpenBreed.Editor.VM.Base;
using System.IO;
using OpenBreed.Common;

namespace OpenBreed.Editor.VM.Project
{
    public enum ProjectState
    {
        Closed,
        New,
        Opened,
        Closing
    }

    public class ProjectVM : BaseViewModel
    {

        #region Private Fields

        private LevelDef _currentLevel;
        private ProjectState _state;

        #endregion Private Fields

        #region Public Constructors

        public ProjectVM(EditorVM root)
        {
            Root = root;
        }

        #endregion Public Constructors

        #region Public Properties

        public LevelDef CurrentLevel
        {
            get { return _currentLevel; }
            set { SetProperty(ref _currentLevel, value); }
        }

        public bool IsLevelOpened { get { return CurrentLevel != null; } }

        public EditorVM Root { get; private set; }

        public ProjectState State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        #endregion Public Properties

        #region Public Methods

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

            var mapSourceDef = Root.CurrentDatabase.GetSourceDef(levelDef.MapResourceRef);
            if (mapSourceDef != null)
                Root.Map.Load(mapSourceDef);

            CurrentLevel = levelDef;
        }

        public void TryNewLevel()
        {
            Root.DialogProvider.ShowMessage("Creating new project is not implemented yet.", "Feature not implemented");
        }

        public bool TryOpenABTALevel()
        {
            if (Root.CurrentDatabase == null)
                Root.OpenABTADatabase();

            return TryOpenLevelDef();
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

        public bool TryOpenABHCLevel()
        {
            if (Root.CurrentDatabase == null)
                Root.OpenABHCDatabase();

            return TryOpenLevelDef();
        }

        #endregion Public Methods

        #region Internal Methods

        public bool TryClose()
        {


            Root.Map.TryClose();
            return true;
        }

        #endregion Internal Methods

    }
}
