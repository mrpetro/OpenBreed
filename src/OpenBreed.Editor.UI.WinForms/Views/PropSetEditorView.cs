using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Drawing.Imaging;
using OpenBreed.Editor.VM.Props;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.UI.WinForms.Controls.Props;

namespace OpenBreed.Editor.UI.WinForms.Views
{
    public partial class PropSetEditorView : DockContent
    {
        #region Private Fields

        private EntryEditorVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public PropSetEditorView()
        {
            InitializeComponent();

            EntryEditor.InnerCtrl = new PropSetEditorCtrl();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(EntryEditorVM vm)
        {
            _vm = vm ?? throw new ArgumentNullException(nameof(vm));
            _vm.PropertyChanged += _vm_PropertyChanged;

            EntryEditor.Initialize(_vm);
            OnEditableNameChanged();
        }

        #endregion Public Methods

        #region Private Methods

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_vm.EditableName):
                    OnEditableNameChanged();
                    break;
                default:
                    break;
            }
        }

        private void OnEditableNameChanged()
        {
            if (_vm.EditableName == null)
                Text = $"{_vm.EditorName} - no entry to edit";
            else
                Text = $"{_vm.EditorName} - {_vm.EditableName}";
        }

        #endregion Private Methods

    }
}
