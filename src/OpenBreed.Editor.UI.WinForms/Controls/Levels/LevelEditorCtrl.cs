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
using OpenBreed.Editor.VM.Levels;

namespace OpenBreed.Editor.UI.WinForms.Controls.Levels
{
    public partial class LevelEditorCtrl : EntryEditorInnerCtrl
    {
        private LevelEditorVM _vm;

        public LevelEditorCtrl()
        {
            InitializeComponent();
        }

        public override void Initialize(EntryEditorVM vm)
        {
            _vm = vm as LevelEditorVM ?? throw new InvalidOperationException(nameof(vm));

            BodyEditorCtrl.Initialize(_vm.BodyEditor);
        }
    }
}
