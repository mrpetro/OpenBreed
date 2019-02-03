using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Maps;

namespace OpenBreed.Editor.UI.WinForms.Controls.Maps
{
    public partial class MapEditorCtrl : EntryEditorInnerCtrl
    {
        private MapEditorVM _vm;

        public MapEditorCtrl()
        {
            InitializeComponent();
        }

        public override void Initialize(EntryEditorVM vm)
        {
            _vm = vm as MapEditorVM ?? throw new InvalidOperationException(nameof(vm));

            MapView.Initialize(_vm.MapView);
        }
    }
}
