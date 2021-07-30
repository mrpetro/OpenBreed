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
using OpenBreed.Database.Interface.Items.Tiles;

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

        private void OnSubeditorChanged(IEntryEditor<IDbTileAtlas> subeditor)
        {
            Panel.Controls.Clear();

            if (subeditor == null)
                return;

            if (subeditor is TileSetFromBlkEditorVM)
            {
                var control = new TileSetFromBlkEditorCtrl();
                control.Initialize((TileSetFromBlkEditorVM)_vm.Subeditor);
                control.Dock = DockStyle.Fill;
                Panel.Controls.Add(control);
            }
        }
    }
}
