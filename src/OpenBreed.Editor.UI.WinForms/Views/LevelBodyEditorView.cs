using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Levels;
using System;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace OpenBreed.Editor.UI.WinForms.Views
{
    public partial class LevelBodyEditorView : DockContent
    {
        #region Private Fields

        private MapEditorViewVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public LevelBodyEditorView()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(MapEditorViewVM vm)
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