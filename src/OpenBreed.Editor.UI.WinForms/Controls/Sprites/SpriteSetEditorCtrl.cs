using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Tiles;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Sprites;

namespace OpenBreed.Editor.UI.WinForms.Controls.Sprites
{
    public partial class SpriteSetEditorCtrl : EntryEditorInnerCtrl
    {
        private SpriteSetEditorVM _vm;

        public SpriteSetEditorCtrl()
        {
            InitializeComponent();
        }

        public override void Initialize(EntryEditorVM vm)
        {
            _vm = vm as SpriteSetEditorVM ?? throw new InvalidOperationException(nameof(vm));

            cbxPalettes.DataSource = _vm.PaletteIds;
            cbxPalettes.DataBindings.Add(nameof(cbxPalettes.SelectedItem), _vm, nameof(_vm.CurrentPaletteId), false, DataSourceUpdateMode.OnPropertyChanged);

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

        private void OnEditableChanged(SpriteSetVM spriteSet)
        {
            Panel.Controls.Clear();

            if (spriteSet == null)
                return;

            if (spriteSet is SpriteSetFromSprVM)
            {
                var control = new SpriteSetFromSprCtrl();
                control.Initialize((SpriteSetFromSprVM)spriteSet);
                control.Dock = DockStyle.Fill;
                Panel.Controls.Add(control);
            }
            else if (spriteSet is SpriteSetFromImageVM)
            {
                var control = new SpriteSetFromImageCtrl();
                control.Initialize((SpriteSetFromImageVM)spriteSet);
                control.Dock = DockStyle.Fill;
                Panel.Controls.Add(control);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }
    }
}
