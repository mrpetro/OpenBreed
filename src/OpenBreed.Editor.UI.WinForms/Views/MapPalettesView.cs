﻿using System;
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
    public partial class MapPalettesView : DockContent
    {
        private PalettesVM _vm;

        public MapPalettesView()
        {
            InitializeComponent();
        }

        public void Initialize(PalettesVM vm)
        {
            _vm = vm;

            Palettes.Initialize(vm);
            PaletteEditor.Initialize(vm.Editor);
        }

    }
}
