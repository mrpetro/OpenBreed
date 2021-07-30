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
using OpenBreed.Database.Interface.Items.EntityTemplates;

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

        private void OnSubeditorChanged(IEntryEditor<IDbEntityTemplate> subeditor)
        {
            Controls.Clear();

            if (subeditor == null)
                return;

            if (subeditor is EntityTemplateFromFileEditorVM)
            {
                var control = new EntityTemplateFromFileCtrl();
                control.Initialize((EntityTemplateFromFileEditorVM)subeditor);
                control.Dock = DockStyle.Fill;
                Controls.Add(control);
            }
        }
    }
}
