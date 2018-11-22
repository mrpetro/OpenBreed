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
using OpenBreed.Editor.VM.Project;
using OpenBreed.Editor.VM.Maps.Tools;

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
        Tools
    }


    public partial class ProjectView : DockPanel
    {
        private ProjectVM _vm;

        private MapPalettesView m_MapPalettesView = null;
        private TileSetsView m_TileSetView = null;
        private SpriteSetsView m_SpriteSetsView = null;
        private PropSetsView m_PropSetsView = null;
        private MapPropertiesView m_MapPropertiesView = null;
        private MapBodyEditorView m_MapBodyView = null;
        private ToolsView m_ToolsView = null;

        private DeserializeDockContent _deserializeDockContent;
        private bool m_bSaveLayout = true;

        public void Initialize(ProjectVM vm)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));

            _vm = vm;

            _vm.Root.ToolsMan.ToolActivated += ToolsMan_ToolActivated;

            RestoreLayout();
        }

        public ProjectView()
        {
            InitializeComponent();

            _deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
        }

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

        protected DockContent GetView(ProjectViewType viewType)
        {
            switch (viewType)
            {
                case ProjectViewType.MapBody:
                    return m_MapBodyView;
                case ProjectViewType.MapProperties:
                    return m_MapPropertiesView;
                case ProjectViewType.MapPalettes:
                    return m_MapPalettesView;
                case ProjectViewType.TileSet:
                    return m_TileSetView;
                case ProjectViewType.SpriteSets:
                    return m_SpriteSetsView;
                case ProjectViewType.PropertySet:
                    return m_PropSetsView;
                case ProjectViewType.Tools:
                    return m_ToolsView;
                default:
                    throw new InvalidOperationException("Unknown ProjectViewType: " + viewType);
            }
        }

        public void ShowView(ProjectViewType type )
        {
            var view = GetView(type);

            view.Show();
        }

        public void CloseView(ProjectViewType type)
        {
            var view = GetView(type);
            view.Close();
        }

        public void RestoreLayout()
        {
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "OpenAEdGUI.cfg");

            if (File.Exists(configFile))
                LoadFromXml(configFile, _deserializeDockContent);
        }

        public void StoreLayout()
        {
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "OpenAEdGUI.cfg");
            if (m_bSaveLayout)
                SaveAsXml(configFile);
            else if (File.Exists(configFile))
                File.Delete(configFile);
        }

        public void InitViews()
        {
            m_TileSetView = new TileSetsView();
            m_SpriteSetsView = new SpriteSetsView();
            m_PropSetsView = new PropSetsView();
            m_MapPalettesView = new MapPalettesView();
            m_MapPropertiesView = new MapPropertiesView();
            m_MapBodyView = new MapBodyEditorView();
            m_ToolsView = new ToolsView(_vm.Root.ToolsMan);

            m_TileSetView.Initialize(_vm.Root.TileSets);
            m_SpriteSetsView.Initialize(_vm.Root.SpriteSets);
            m_PropSetsView.Initialize(_vm.Root.PropSets); 
            m_MapPalettesView.Initialize(_vm.Root.Palettes);
            m_MapPropertiesView.Initialize(_vm.Root.Map.Properties);
            m_MapBodyView.Initialize(_vm.Root.MapBodyViewer);

            m_TileSetView.Show(this, DockState.DockLeft);
            m_SpriteSetsView.Show(this, DockState.DockLeft);
            m_PropSetsView.Show(this, DockState.DockLeft);
            m_MapPalettesView.Show(this, DockState.DockLeft);
            m_MapPropertiesView.Show(this, DockState.DockLeft);
            m_MapBodyView.Show(this, DockState.Document);
            m_ToolsView.Show(this, DockState.DockTop);
        }

        public void DeinitViews()
        {
            m_MapBodyView.Close();
            //m_MapBodyView = null;
            m_MapPropertiesView.Close();
            //m_MapPropertiesView = null;
            m_MapPalettesView.Close();
            //m_MapPalettesView = null;
            m_PropSetsView.Close();
            //m_PropertySetView = null;
            m_TileSetView.Close();
            //m_TileSetView = null;
            m_SpriteSetsView.Close();
            //m_SpriteSetsView = null;
            m_ToolsView.Close();
            //m_ToolsView = null;
        }    
    }
}
