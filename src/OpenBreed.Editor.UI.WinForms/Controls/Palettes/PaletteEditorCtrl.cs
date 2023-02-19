using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Palettes;
using OpenBreed.Editor.VM;
using OpenBreed.Database.Interface.Items.Palettes;

namespace OpenBreed.Editor.UI.WinForms.Controls.Palettes
{
    public partial class PaletteEditorCtrl : EntryEditorInnerCtrl
    {
        private PaletteEditorVM _vm;

        public PaletteEditorCtrl()
        {
            InitializeComponent();
        }

        public override void Initialize(EntryEditorVM vm)
        {
            _vm = vm as PaletteEditorVM ?? throw new InvalidOperationException(nameof(vm));

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

        private void OnSubeditorChanged(IEntryEditor<IDbPalette> subeditor)
        {
            Controls.Clear();

            if (subeditor == null)
                return;

            if (subeditor is PaletteFromBinaryEditorVM)
            {
                var control = new PaletteFromBinaryCtrl();
                control.Initialize((PaletteFromBinaryEditorVM)subeditor);
                control.Dock = DockStyle.Fill;
                Controls.Add(control);
            }
            else if (subeditor is PaletteFromMapEditorVM)
            {
                var control = new PaletteFromMapCtrl();
                control.Initialize((PaletteFromMapEditorVM)subeditor);
                control.Dock = DockStyle.Fill;
                Controls.Add(control);
            }
            else if (subeditor is PaletteFromLbmEditorVM)
            {
                var control = new PaletteFromLbmCtrl();
                control.Initialize((PaletteFromLbmEditorVM)subeditor);
                control.Dock = DockStyle.Fill;
                Controls.Add(control);
            }
        }






    }
}
