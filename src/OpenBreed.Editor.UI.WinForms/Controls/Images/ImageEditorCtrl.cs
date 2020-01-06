using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Images;
using System.Drawing.Drawing2D;
using OpenBreed.Editor.VM;

namespace OpenBreed.Editor.UI.WinForms.Controls.Images
{
    public partial class ImageEditorCtrl : EntryEditorInnerCtrl
    {
        private ImageEditorVM _vm;

        public ImageEditorCtrl()
        {
            InitializeComponent();

        }

        public override void Initialize(EntryEditorVM vm)
        {
            _vm = vm as ImageEditorVM ?? throw new InvalidOperationException(nameof(vm));

            ImageAssetRefIdEditor.Initialize(_vm.ImageAssetRefIdEditor);
            ImageView.Initialize(_vm);
        }
    }
}
