using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Palettes;
using WeifenLuo.WinFormsUI.Docking;
using System.Drawing.Imaging;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Common.Palettes;

namespace OpenBreed.Editor.UI.WinForms.Views
{
    public partial class LevelPalettesView : DockContent
    {
        private MapEditorPalettesToolVM _vm;

        public LevelPalettesView()
        {
            InitializeComponent();
        }

        public void Initialize(MapEditorPalettesToolVM vm)
        {
            _vm = vm;

            Palettes.Initialize(vm);
            //PaletteEditor.Initialize(vm.Parent.Root.PaletteEditor);
        }

    }
}
