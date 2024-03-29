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
using OpenBreed.Common;
using OpenBreed.Editor.VM.Database;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Sounds;
using OpenBreed.Editor.VM.Images;
using OpenBreed.Editor.VM.Palettes;
using OpenBreed.Editor.VM.Tiles;
using OpenBreed.Editor.VM.Actions;
using OpenBreed.Editor.UI.WinForms.Controls.Sounds;
using OpenBreed.Editor.UI.WinForms.Controls.Tiles;
using OpenBreed.Editor.UI.WinForms.Controls.Images;
using OpenBreed.Editor.UI.WinForms.Controls.Actions;
using OpenBreed.Editor.UI.WinForms.Controls.Palettes;
using OpenBreed.Editor.UI.WinForms.Controls.Maps;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Editor.VM.DataSources;
using OpenBreed.Editor.UI.WinForms.Controls.DataSources;
using OpenBreed.Editor.VM.Texts;
using OpenBreed.Editor.UI.WinForms.Controls.Texts;
using OpenBreed.Editor.VM.Sprites;
using OpenBreed.Editor.UI.WinForms.Controls.Sprites;
using OpenBreed.Editor.VM.Scripts;
using OpenBreed.Editor.UI.WinForms.Controls.Scripts;
using OpenBreed.Editor.VM.EntityTemplates;
using OpenBreed.Editor.UI.WinForms.Controls.EntityTemplates;
using System.Drawing.Design;

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


    public partial class DbEditorView : DockPanel
    {
        private ViewFactory _viewFactory = new ViewFactory();

        #region Private Fields

        private const string LAYOUT_CFG_FILE_NAME = "Layout.cfg";

        //private ToolsView _toolsView = new ToolsView();
        //private DbTablesEditorView _databaseView = new DbTablesEditorView();

        private DeserializeDockContent _deserializeDockContent;

        private bool _saveLayout = true;
        private DbEditorVM _vm;

        public DbEditorVM VM => _vm;

        #endregion Private Fields

        #region Public Constructors

        public DbEditorView()
        {
            InitializeComponent();

            _deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);

            _viewFactory.Register<PaletteFromMapEditorVM, EntryEditorView<PaletteFromMapCtrl>>();
            _viewFactory.Register<PaletteFromLbmEditorVM, EntryEditorView<PaletteFromLbmCtrl>>();
            _viewFactory.Register<PaletteFromBinaryEditorVM, EntryEditorView<PaletteFromBinaryCtrl>>();
            _viewFactory.Register<TileSetFromBlkEditorVM, EntryEditorView<TileSetFromBlkEditorCtrl>>();
            _viewFactory.Register<MapEditorVM, EntryEditorView<MapEditorCtrl>>();
            _viewFactory.Register<ImageFromFileEditorVM, EntryEditorView<ImageFromFileEditorCtrl>>();
            _viewFactory.Register<PcmSoundEditorVM, EntryEditorView<PcmSoundEditorCtrl>>();
            _viewFactory.Register<ActionSetEmbeddedEditorVM, EntryEditorView<ActionSetEmbeddedEditorCtrl>>();
            _viewFactory.Register<SpriteSetFromImageEditorVM, EntryEditorView<SpriteSetFromImageEditorCtrl>>();
            _viewFactory.Register<SpriteSetFromSprEditorVM, EntryEditorView<SpriteSetFromSprEditorCtrl>>();
            _viewFactory.Register<ScriptEmbeddedEditorVM, EntryEditorView<ScriptEmbeddedCtrl>>();
            _viewFactory.Register<ScriptFromFileEditorVM, EntryEditorView<ScriptFromFileCtrl>>();
            _viewFactory.Register<TextEmbeddedEditorVM, EntryEditorView<TextEmbeddedEditorCtrl>>();
            _viewFactory.Register<TextFromMapEditorVM, EntryEditorView<TextFromMapEditorCtrl>>();
            _viewFactory.Register<EntityTemplateFromFileEditorVM, EntryEditorView<EntityTemplateFromFileCtrl>>();
            _viewFactory.Register<FileDataSourceEditorVM, EntryEditorView<FileDataSourceCtrl>>();
            _viewFactory.Register<EpfArchiveFileDataSourceEditorVM, EntryEditorView<EpfArchiveDataSourceCtrl>>();

            Theme = new WeifenLuo.WinFormsUI.Docking.VS2015LightTheme();
        }

        #endregion Public Constructors

        #region Public Methods

        public void HideAllViews()
        {
            _vm.CloseAllEditors();

            //TODO:
            //CloseDatabaseView();
        }

        public void Initialize(DbEditorVM vm)
        {
            _vm = vm ?? throw new ArgumentNullException(nameof(vm));

            _vm.EntryEditorOpeningAction = (editor) => OnEntryEditorOpening(editor);
            //_vm.EntryEditorActivateAction = (editor) => OnEntryEditorActivate(editor);

            RestoreLayout();
        }

        public void RestoreLayout()
        {
            string configFile = Path.Combine(ProgramTools.AppProductDataDir, LAYOUT_CFG_FILE_NAME);

            if (File.Exists(configFile))
                LoadFromXml(configFile, _deserializeDockContent);
        }

        //public void ShowDatabaseView()
        //{
        //    if (_databaseView == null)
        //        _databaseView = new DbTablesEditorView();

        //    _databaseView.Initialize(_vm.DbTablesEditor);
        //    _databaseView.Show(this, DockState.DockLeft);
        //}

        //public void ShowLevelBodyEditorView()
        //{
        //    if (_levelBodyEditorView == null)
        //        _levelBodyEditorView = new LevelBodyEditorView();

        //    _levelBodyEditorView.Initialize(_vm.Root.LevelEditor.BodyEditor);
        //    _levelBodyEditorView.Show(this, DockState.Document);
        //}

        //public void ShowLevelPalettesView()
        //{
        //    if (_levelPalettesView == null)
        //        _levelPalettesView = new LevelPalettesView();

        //    _levelPalettesView.Initialize(_vm.Root.LevelEditor.PaletteSelector);
        //    _levelPalettesView.Show(this, DockState.DockLeft);
        //}

        //public void ShowLevelPropSelectorView()
        //{
        //    if (_levelPropSelectorView == null)
        //        _levelPropSelectorView = new LevelPropSelectorView();

        //    _levelPropSelectorView.Initialize(_vm.Root.LevelEditor.PropSelector);
        //    _levelPropSelectorView.Show(this, DockState.DockLeft);
        //}

        //public void ShowLevelTileSelectorView()
        //{
        //    if (_levelTileSelectorView == null)
        //        _levelTileSelectorView = new LevelTileSelectorView();

        //    _levelTileSelectorView.Initialize(_vm.Root.LevelEditor.TileSelector);
        //    _levelTileSelectorView.Show(this, DockState.DockLeft);
        //}

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

        #endregion Internal Methods

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

        private void OnEntryEditorActivate(EntryEditorVM editor)
        {
            var editorView = _viewFactory.CreateView(editor.GetType());
            editorView.Initialize(editor);
            editorView.Show(this, DockState.Document);
        }

        private void OnEntryEditorOpening(EntryEditorVM editor)
        {
            var editorView = _viewFactory.CreateView(editor.GetType());
            editorView.Initialize(editor);
            editorView.Show(this, DockState.Document);
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
