using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Scripts;
using OpenBreed.Editor.VM;

namespace OpenBreed.Editor.UI.WinForms.Controls.Scripts
{
    public partial class ScriptEditorCtrl : EntryEditorInnerCtrl
    {
        private ScriptEditorVM _vm;

        public ScriptEditorCtrl()
        {
            InitializeComponent();
        }

        public override void Initialize(EntryEditorVM vm)
        {
            _vm = vm as ScriptEditorVM ?? throw new InvalidOperationException(nameof(vm));

            _vm.PropertyChanged += _vm_PropertyChanged;
            OnEditableChanged(_vm.Editable);
        }

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_vm.Editable):
                    OnEditableChanged(_vm.Editable);
                    break;
                default:
                    break;
            }
        }

        private void OnEditableChanged(ScriptVM script)
        {
            Controls.Clear();

            if (script == null)
                return;

            if (script is ScriptEmbeddedVM)
            {
                var control = new ScriptEmbeddedCtrl();
                control.Initialize((ScriptEmbeddedVM)script);
                control.Dock = DockStyle.Fill;
                Controls.Add(control);
            }
            else if (script is ScriptFromFileVM)
            {
                var control = new ScriptFromFileCtrl();
                control.Initialize((ScriptFromFileVM)script);
                control.Dock = DockStyle.Fill;
                Controls.Add(control);
            }
        }

    }
}
