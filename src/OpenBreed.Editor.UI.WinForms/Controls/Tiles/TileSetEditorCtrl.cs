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
using OpenBreed.Editor.VM.Tiles.Helpers;
using OpenBreed.Editor.VM;

namespace OpenBreed.Editor.UI.WinForms.Controls.Tiles
{
    public partial class TileSetEditorCtrl : EntryEditorInnerCtrl
    {
        private TileSetEditorVM _vm;

        public TileSetEditorCtrl()
        {
            InitializeComponent();
        }

        public override void Initialize(EntryEditorVM vm)
        {
            _vm = vm as TileSetEditorVM ?? throw new InvalidOperationException(nameof(vm));

            TileSetViewer.Initialize(_vm.TileSetViewer);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }
    }
}
