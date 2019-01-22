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

        private void OnEditableChanged(AssetVM asset)
        {
            Controls.Clear();

            if (asset == null)
                return;

            if (asset is FileAssetVM)
            {
                var control = new FileAssetCtrl();
                control.Initialize((FileAssetVM)asset);
                control.Dock = DockStyle.Fill;
                Controls.Add(control);
            }
            else if (asset is EPFArchiveFileAssetVM)
            {
                var control = new EpfArchiveAssetCtrl();
                control.Initialize((EPFArchiveFileAssetVM)asset);
                control.Dock = DockStyle.Fill;
                Controls.Add(control);
            }
        }
    }
}
