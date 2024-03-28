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
using OpenBreed.Editor.UI.WinForms.Helpers;

namespace OpenBreed.Editor.UI.WinForms.Controls.Palettes
{
    public partial class PaletteFromMapCtrl : EntryEditorInnerCtrl
    {
        private PaletteFromMapEditorVM _vm;

        public PaletteFromMapCtrl()
        {
            InitializeComponent();
        }

        public override void Initialize(EntryEditorVM vm)
        {
            _vm = vm as PaletteFromMapEditorVM ?? throw new InvalidOperationException(nameof(vm));

            var colorSelector = WpfHelper.CreateWpfControl<Wpf.Palettes.ColorSelectorCtrl>(_vm);
            colorSelector.Dock = DockStyle.Fill;
            grpPalette.Controls.Add(colorSelector);

            var colorEditor = WpfHelper.CreateWpfControl<Wpf.Palettes.ColorEditorCtrl>(_vm.ColorEditor);
            colorEditor.Dock = DockStyle.Top;
            grpPalette.Controls.Add(colorEditor);

            tbxMapDataRef.DataBindings.Add(nameof(tbxMapDataRef.Text), _vm, nameof(_vm.DataRef), false, DataSourceUpdateMode.OnPropertyChanged);

            cbxMapBlockName.DataSource = _vm.BlockNames;
            cbxMapBlockName.DataBindings.Add(nameof(cbxMapBlockName.Text), _vm, nameof(_vm.BlockName), false, DataSourceUpdateMode.OnPropertyChanged);

            colorEditor.DataBindings.Add(nameof(colorEditor.Enabled), _vm, nameof(_vm.EditEnabled), false, DataSourceUpdateMode.OnPropertyChanged);
        }
    }
}
