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
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Database.Interface.Items.Texts;

namespace OpenBreed.Editor.UI.WinForms.Controls.Sprites
{
    public partial class SpriteSetEditorCtrl : EntryEditorInnerCtrl
    {
        private SpriteSetEditorVM _vm;

        public SpriteSetEditorCtrl()
        {
            InitializeComponent();

            cbxPalettes.SelectionChangeCommitted += combobox1_SelectionChangesCommitted;
        }

        private void combobox1_SelectionChangesCommitted(Object sender, EventArgs e)
        {
            ((ComboBox)sender).DataBindings["SelectedItem"].WriteValue();
        }

        public override void Initialize(EntryEditorVM vm)
        {
            _vm = vm as SpriteSetEditorVM ?? throw new InvalidOperationException(nameof(vm));

            cbxPalettes.DataSource = _vm.PaletteIds;
            cbxPalettes.DataBindings.Add(nameof(cbxPalettes.SelectedItem), _vm, nameof(_vm.CurrentPaletteId), false, DataSourceUpdateMode.OnPropertyChanged);

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

        private void OnSubeditorChanged(IEntryEditor<ISpriteSetEntry> subeditor)
        {
            Panel.Controls.Clear();

            if (subeditor == null)
                return;

            if (subeditor is SpriteSetFromSprEditorVM)
            {
                var control = new SpriteSetFromSprEditorCtrl();
                control.Initialize((SpriteSetFromSprEditorVM)_vm.Subeditor);
                control.Dock = DockStyle.Fill;
                Panel.Controls.Add(control);
            }
            else if (subeditor is SpriteSetFromImageEditorVM)
            {
                var control = new SpriteSetFromImageEditorCtrl();
                control.Initialize((SpriteSetFromImageEditorVM)_vm.Subeditor);
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
