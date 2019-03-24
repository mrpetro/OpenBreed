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

        private void OnEditableChanged(TextVM text)
        {
            Controls.Clear();

            if (text == null)
                return;

            if (text is TextEmbeddedVM)
            {
                var control = new TextEmbeddedCtrl();
                control.Initialize((TextEmbeddedVM)text);
                control.Dock = DockStyle.Fill;
                Controls.Add(control);
            }
            else if (text is TextFromMapVM)
            {
                var control = new TextFromMapCtrl();
                control.Initialize((TextFromMapVM)text);
                control.Dock = DockStyle.Fill;
                Controls.Add(control);
            }
        }

    }
}
