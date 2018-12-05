using System;
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
using OpenBreed.Editor.VM.Maps.Tools;
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
        Tools
    }


    public partial class ProjectView : DockPanel
    {
        private const string LAYOUT_CFG_FILE_NAME = "Layout.cfg";

        #region Private Fields

        private DeserializeDockContent _deserializeDockContent;
        private DatabaseVM _vm;

        private bool _saveLayout = true;
        private MapBodyEditorView _mapBodyView = new MapBodyEditorView();
        private MapPalettesView _mapPalettesView = new MapPalettesView();
        private MapPropertiesView _mapPropertiesView = new MapPropertiesView();
        private PropSetsView _propSetsView = new PropSetsView();
        private SpriteSetsView _spriteSetsView = new SpriteSetsView();
        private TileSetsView _tileSetView = new TileSetsView();
        private ImagesView _imagesView = new ImagesView();
        private ToolsView _toolsView = new ToolsView();

        #endregion Private Fields

        #region Public Constructors

        public ProjectView()
        {
            InitializeComponent();

            _deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
        }

        #endregion Public Constructors

        #region Public Methods

        public void CloseView(ProjectViewType type)
        {
            var view = GetView(type);
            view.Close();
        }

        public void DeinitViews()
        {
            _mapBodyView.Close();
            _mapPropertiesView.Close();
            _mapPalettesView.Close();
            _propSetsView.Close();
            _tileSetView.Close();
            _spriteSetsView.Close();
            _toolsView.Close();
        }

        public void Initialize(DatabaseVM vm)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));

            _vm = vm;

            _toolsView.Initialize(_vm.Root.ToolsMan);
            _tileSetView.Initialize(_vm.Root);
            _spriteSetsView.Initialize(_vm.Root);
            _propSetsView.Initialize(_vm.Root.PropSets);
            _mapPalettesView.Initialize(_vm.Root.Palettes);
            _mapPropertiesView.Initialize(_vm.Root.Map.Properties);
            _mapBodyView.Initialize(_vm.Root.MapBodyViewer);
            _imagesView.Initialize(_vm.Root.ImageViewer);

            _vm.Root.ToolsMan.ToolActivated += ToolsMan_ToolActivated;

            RestoreLayout();
        }
        public void InitViews()
        {
            _tileSetView.Show(this, DockState.DockLeft);
            _spriteSetsView.Show(this, DockState.DockLeft);
            _propSetsView.Show(this, DockState.DockLeft);
            _mapPalettesView.Show(this, DockState.DockLeft);
            _mapPropertiesView.Show(this, DockState.DockLeft);
            _mapBodyView.Show(this, DockState.Document);
            _toolsView.Show(this, DockState.DockTop);
        }

        public void RestoreLayout()
        {
            string configFile = Path.Combine(ProgramTools.AppProductDataDir, LAYOUT_CFG_FILE_NAME);

            if (File.Exists(configFile))
                LoadFromXml(configFile, _deserializeDockContent);
        }

        public void ShowView(ProjectViewType type)
        {
            var view = GetView(type);

            view.Show();
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

        #region Protected Methods

        protected DockContent GetView(ProjectViewType viewType)
        {
            switch (viewType)
            {
                case ProjectViewType.MapBody:
                    return _mapBodyView;
                case ProjectViewType.MapProperties:
                    return _mapPropertiesView;
                case ProjectViewType.MapPalettes:
                    return _mapPalettesView;
                case ProjectViewType.TileSet:
                    return _tileSetView;
                case ProjectViewType.SpriteSets:
                    return _spriteSetsView;
                case ProjectViewType.PropertySet:
                    return _propSetsView;
                case ProjectViewType.Images:
                    return _imagesView;
                case ProjectViewType.Tools:
                    return _toolsView;
                default:
                    throw new InvalidOperationException("Unknown ProjectViewType: " + viewType);
            }
        }

        #endregion Protected Methods

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

        void ToolsMan_ToolActivated(object sender, ToolActivatedEventArgs e)
        {
            if (e.ToolName == "InsertPropertyTool")
                ShowView(ProjectViewType.PropertySet);
            else if (e.ToolName == "InsertTileTool")
                ShowView(ProjectViewType.TileSet);
        }

        #endregion Private Methods
    }
}
