using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Assets;
using OpenBreed.Editor.VM;

namespace OpenBreed.Editor.UI.WinForms.Controls.Assets
{
    public partial class AssetEditorCtrl : EntryEditorInnerCtrl
    {
        private AssetEditorVM _vm;

        public AssetEditorCtrl()
        {
            InitializeComponent();
        }

        public override void Initialize(EntryEditorVM vm)
        {
            _vm = vm as AssetEditorVM ?? throw new InvalidOperationException(nameof(vm));
            //_vm.PropertyChanged += _vm_PropertyChanged;

            //UpdateViewState();
        }
    }
}
