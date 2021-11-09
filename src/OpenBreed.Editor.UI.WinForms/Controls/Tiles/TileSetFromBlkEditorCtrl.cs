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

namespace OpenBreed.Editor.UI.WinForms.Controls.Tiles
{
    public partial class TileSetFromBlkEditorCtrl : UserControl
    {
        private TileSetFromBlkEditorVM _vm;

        public TileSetFromBlkEditorCtrl()
        {
            InitializeComponent();
        }

        public void Initialize(TileSetFromBlkEditorVM vm)
        {
            _vm = vm;

            cbxPalettes.DataSource = _vm.PaletteIds;

            TileSetViewer.Initialize(_vm.Viewer);

            cbxPalettes.DataBindings.Add(nameof(cbxPalettes.SelectedIndex), _vm, nameof(_vm.CurrentPaletteIndex), false, DataSourceUpdateMode.OnPropertyChanged);
            CurrentIndexInfoLbl.DataBindings.Add(nameof(CurrentIndexInfoLbl.Text), _vm.Viewer, nameof(_vm.Viewer.Info), false, DataSourceUpdateMode.OnPropertyChanged);

        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }
    }
}
