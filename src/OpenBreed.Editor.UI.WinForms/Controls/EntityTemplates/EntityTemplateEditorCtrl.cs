using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.EntityTemplates;
using OpenBreed.Editor.VM;

namespace OpenBreed.Editor.UI.WinForms.Controls.EntityTemplates
{
    public partial class EntityTemplateEditorCtrl : EntryEditorInnerCtrl
    {
        private EntityTemplateEditorVM _vm;

        public EntityTemplateEditorCtrl()
        {
            InitializeComponent();
        }

        public override void Initialize(EntryEditorVM vm)
        {
            _vm = vm as EntityTemplateEditorVM ?? throw new InvalidOperationException(nameof(vm));

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

        private void OnEditableChanged(EntityTemplateVM entityTemplate)
        {
            Controls.Clear();

            if (entityTemplate == null)
                return;

            if (entityTemplate is EntityTemplateFromFileVM)
            {
                var control = new EntityTemplateFromFileCtrl();
                control.Initialize((EntityTemplateFromFileVM)entityTemplate);
                control.Dock = DockStyle.Fill;
                Controls.Add(control);
            }
        }

    }
}
