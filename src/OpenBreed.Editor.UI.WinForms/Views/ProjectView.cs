﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using OpenABEd;
using System.IO;
using OpenBreed.Editor.VM.Levels.Tools;
using OpenBreed.Common;
using OpenBreed.Editor.VM.Database;

namespace OpenBreed.Editor.UI.WinForms.Views
{
    public enum ProjectViewType
    {
        MapBody,
        MapProperties,
        MapPalettes,
        TileSet,
        SpriteSets,
        PropertySet,
        Images,
        Tools,
        DatabaseViewer
    }


    public partial class ProjectView : DockPanel
    {
        #region Private Fields

        private const string LAYOUT_CFG_FILE_NAME = "Layout.cfg";

        //private ToolsView _toolsView = new ToolsView();
        private DatabaseView _databaseView = new DatabaseView();

        private DeserializeDockContent _deserializeDockContent;
        private ImageView _imagesView = new ImageView();
        private LevelBodyEditorView _levelBodyEditorView = new LevelBodyEditorView();
        private LevelPalettesView _levelPalettesView = new LevelPalettesView();
        //private MapPropertiesView _mapPropertiesView = new MapPropertiesView();
        private LevelPropSelectorView _levelPropSelectorView = new LevelPropSelectorView();

        //private SpriteSetsView _spriteSetsView = new SpriteSetsView();
        private LevelTileSelectorView _levelTileSelectorView = new LevelTileSelectorView();

        private PropSetEditorView _propSetEditorView = new PropSetEditorView();
        private bool _saveLayout = true;
        private DatabaseVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public ProjectView()
        {
            InitializeComponent();

            _deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
        }

        #endregion Public Constructors

        //public void CloseView(ProjectViewType type)
        //{
        //    var view = GetView(type);
        //    view.Close();
        //}

        #region Public Methods

        public void CloseDatabaseView()
        {
            if (_databaseView == null)
                return;
            _databaseView.Close();
            _databaseView = null;
        }

        public void CloseImagesView()
        {
            if (_imagesView == null)
                return;
            _imagesView.Close();
            _imagesView = null;
        }

        public void CloseLevelBodyView()
        {
            if (_levelBodyEditorView == null)
                return;
            _levelBodyEditorView.Close();
            _levelBodyEditorView = null;
        }

        public void CloseLevelPalettesView()
        {
            if (_levelPalettesView == null)
                return;

            _levelPalettesView.Close();
            _levelPalettesView = null;
        }

        public void CloseLevelPropSelectorView()
        {
            if (_levelPropSelectorView == null)
                return;
            _levelPropSelectorView.Close();
            _levelPropSelectorView = null;
        }

        public void CloseLevelTileSelectorView()
        {
            if (_levelTileSelectorView == null)
                return;
            _levelTileSelectorView.Close();
            _levelTileSelectorView = null;
        }

        public void HideAllViews()
        {
            CloseDatabaseView();
            CloseImagesView();
            CloseLevelTileSelectorView();
            CloseLevelPropSelectorView();
            CloseLevelPalettesView();
            CloseLevelBodyView();
        }

        public void Initialize(DatabaseVM vm)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));

            _vm = vm;

            //_mapPropertiesView.Initialize(_vm.Root.Map.Properties);

            //_vm.Root.ToolsMan.ToolActivated += ToolsMan_ToolActivated;

            RestoreLayout();
        }

        public void RestoreLayout()
        {
            string configFile = Path.Combine(ProgramTools.AppProductDataDir, LAYOUT_CFG_FILE_NAME);

            if (File.Exists(configFile))
                LoadFromXml(configFile, _deserializeDockContent);
        }
        public void ShowDatabaseView()
        {
            if (_databaseView == null)
                _databaseView = new DatabaseView();

            _databaseView.Initialize(_vm.Root.DatabaseViewer);
            _databaseView.Show(this, DockState.DockLeft);
        }

        public void ShowImageView()
        {
            if (_imagesView == null)
                _imagesView = new ImageView();

            _imagesView.Initialize(_vm.Root.ImageViewer);
            _imagesView.Show(this, DockState.Document);
        }

        public void ShowLevelBodyEditorView()
        {
            if (_levelBodyEditorView == null)
                _levelBodyEditorView = new LevelBodyEditorView();

            _levelBodyEditorView.Initialize(_vm.Root.LevelEditor.BodyEditor);
            _levelBodyEditorView.Show(this, DockState.Document);
        }

        public void ShowLevelPalettesView()
        {
            if (_levelPalettesView == null)
                _levelPalettesView = new LevelPalettesView();

            _levelPalettesView.Initialize(_vm.Root.LevelEditor.PaletteSelector);
            _levelPalettesView.Show(this, DockState.DockLeft);
        }

        public void ShowLevelPropSelectorView()
        {
            if (_levelPropSelectorView == null)
                _levelPropSelectorView = new LevelPropSelectorView();

            _levelPropSelectorView.Initialize(_vm.Root.LevelEditor.PropSelector);
            _levelPropSelectorView.Show(this, DockState.DockLeft);
        }

        public void ShowLevelTileSelectorView()
        {
            if (_levelTileSelectorView == null)
                _levelTileSelectorView = new LevelTileSelectorView();

            _levelTileSelectorView.Initialize(_vm.Root.LevelEditor.TileSelector);
            _levelTileSelectorView.Show(this, DockState.DockLeft);
        }

        public void ShowPropSetEditorView()
        {
            if(_propSetEditorView == null)
                _propSetEditorView = new PropSetEditorView();

            _propSetEditorView.Initialize(_vm.Root.PropSetEditor);
            _propSetEditorView.Show(this, DockState.Document);
        }
        public void StoreLayout()
        {
            string configFile = Path.Combine(ProgramTools.AppProductDataDir, LAYOUT_CFG_FILE_NAME);
            if (_saveLayout)
                SaveAsXml(configFile);
            else if (File.Exists(configFile))
                File.Delete(configFile);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void ShowLevelView()
        {
            ShowLevelTileSelectorView();
            ShowLevelPropSelectorView();
            ShowLevelPalettesView();
            ShowLevelBodyEditorView();
        }

        internal void ShowPaletteEditorView()
        {
            throw new NotImplementedException();
        }

        internal void ShowSpriteSetEditorView()
        {
            throw new NotImplementedException();
        }
        internal void ShowTileSetEditorView()
        {
            throw new NotImplementedException();
        }

        #endregion Internal Methods

        //public void ShowView(ProjectViewType type)
        //{
        //    var view = GetView(type);

        //    view.Show();
        //}
        //protected DockContent GetView(ProjectViewType viewType)
        //{
        //    switch (viewType)
        //    {
        //        case ProjectViewType.MapBody:
        //            return _mapBodyView;
        //        case ProjectViewType.MapProperties:
        //            return _mapPropertiesView;
        //        case ProjectViewType.MapPalettes:
        //            return _mapPalettesView;
        //        case ProjectViewType.TileSet:
        //            return _tileSetView;
        //        case ProjectViewType.SpriteSets:
        //            return _spriteSetsView;
        //        case ProjectViewType.PropertySet:
        //            return _propSetsView;
        //        case ProjectViewType.Images:
        //            return _imagesView;
        //        case ProjectViewType.Tools:
        //            return _toolsView;
        //        default:
        //            throw new InvalidOperationException("Unknown ProjectViewType: " + viewType);
        //    }
        //}

        #region Private Methods

        private IDockContent GetContentFromPersistString(string persistString)
        {
            //if (persistString == typeof(DummySolutionExplorer).ToString())
            //    return m_solutionExplorer;
            //else if (persistString == typeof(DummyPropertyWindow).ToString())
            //    return m_propertyWindow;
            //else if (persistString == typeof(DummyToolbox).ToString())
            //    return m_toolbox;
            //else if (persistString == typeof(DummyOutputWindow).ToString())
            //    return m_outputWindow;
            //else if (persistString == typeof(DummyTaskList).ToString())
            //    return m_taskList;
            //else
            return null;
        }

        #endregion Private Methods

        //void ToolsMan_ToolActivated(object sender, ToolActivatedEventArgs e)
        //{
        //    if (e.ToolName == "InsertPropertyTool")
        //        ShowView(ProjectViewType.PropertySet);
        //    else if (e.ToolName == "InsertTileTool")
        //        ShowView(ProjectViewType.TileSet);
        //}

    }
}
