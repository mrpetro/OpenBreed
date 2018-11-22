using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Editor.VM.Maps.Tools;
using System;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace OpenBreed.Editor.UI.WinForms.Views
{
    public partial class MapBodyEditorView : DockContent, IToolController
    {
        #region Private Fields

        private MapBodyEditorM _vm;

        #endregion Private Fields

        #region Public Constructors

        public MapBodyEditorView()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(MapBodyEditorM vm)
        {
            _vm = vm;

            MapBodyViewer.Initialize(_vm);

            _vm.PropertyChanged += _vm_PropertyChanged;
        }

        #endregion Public Methods

        #region Private Methods

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_vm.Title):
                    TabText = _vm.Title;
                    break;
                default:
                    break;
            }
        }

        #endregion Private Methods
    }
}