using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Texts;
using OpenBreed.Editor.VM;
using OpenBreed.Database.Interface.Items.Texts;

namespace OpenBreed.Editor.UI.WinForms.Controls.Texts
{
    public partial class TextEditorCtrl : EntryEditorInnerCtrl
    {
        private TextEditorVM _vm;

        public TextEditorCtrl()
        {
            InitializeComponent();
        }

        public override void Initialize(EntryEditorVM vm)
        {
            _vm = vm as TextEditorVM ?? throw new InvalidOperationException(nameof(vm));

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

        private void OnSubeditorChanged(IEntryEditor<IDbText> subeditor)
        {
            Controls.Clear();

            if (subeditor == null)
                return;

            if (subeditor is TextEmbeddedEditorVM)
            {
                var control = new TextEmbeddedEditorCtrl();
                control.Initialize((TextEmbeddedEditorVM)subeditor);
                control.Dock = DockStyle.Fill;
                Controls.Add(control);
            }
            else if (subeditor is TextFromMapEditorVM)
            {
                var control = new TextFromMapEditorCtrl();
                control.Initialize((TextFromMapEditorVM)subeditor);
                control.Dock = DockStyle.Fill;
                Controls.Add(control);
            }
        }

    }
}
