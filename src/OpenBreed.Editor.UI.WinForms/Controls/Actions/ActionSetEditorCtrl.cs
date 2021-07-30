using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Actions;
using OpenBreed.Editor.VM;
using OpenBreed.Database.Interface.Items.Actions;

namespace OpenBreed.Editor.UI.WinForms.Controls.Actions
{
    public partial class ActionSetEditorCtrl : EntryEditorInnerCtrl
    {
        #region Private Fields

        private ActionSetEditorVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public ActionSetEditorCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(EntryEditorVM vm)
        {
            _vm = vm as ActionSetEditorVM ?? throw new InvalidOperationException(nameof(vm));

            _vm.PropertyChanged += _vm_PropertyChanged;

            OnSubeditorChanged(_vm.Subeditor);
        }

        #endregion Public Methods

        #region Private Methods

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_vm.Subeditor):
                    OnSubeditorChanged(_vm.Subeditor);
                    break;
                default:
                    break;
            }
        }

        private void OnSubeditorChanged(IEntryEditor<IDbActionSet> subeditor)
        {
            Controls.Clear();

            if (subeditor == null)
                return;

            if (subeditor is ActionSetEmbeddedEditorVM)
            {
                var control = new ActionSetEmbeddedEditorCtrl();
                control.Initialize((ActionSetEmbeddedEditorVM)subeditor);
                control.Dock = DockStyle.Fill;
                Controls.Add(control);
            }
        }

        #endregion Private Methods
    }
}
