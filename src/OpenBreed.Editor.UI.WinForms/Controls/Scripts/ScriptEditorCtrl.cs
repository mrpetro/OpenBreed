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
using OpenBreed.Database.Interface.Items.Scripts;

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
            OnSubeditorChanged(_vm.Subeditor);
        }

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

        private void OnSubeditorChanged(IEntryEditor<IScriptEntry> subeditor)
        {
            Controls.Clear();

            if (subeditor == null)
                return;

            if (subeditor is ScriptEmbeddedEditorVM)
            {
                var control = new ScriptEmbeddedCtrl();
                control.Initialize((ScriptEmbeddedEditorVM)subeditor);
                control.Dock = DockStyle.Fill;
                Controls.Add(control);
            }
            else if (subeditor is ScriptFromFileEditorVM)
            {
                var control = new ScriptFromFileCtrl();
                control.Initialize((ScriptFromFileEditorVM)subeditor);
                control.Dock = DockStyle.Fill;
                Controls.Add(control);
            }
        }

    }
}
