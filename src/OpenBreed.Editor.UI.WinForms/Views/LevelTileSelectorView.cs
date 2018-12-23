using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Tiles;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Palettes;
using WeifenLuo.WinFormsUI.Docking;
using OpenBreed.Editor.UI.WinForms;

namespace OpenBreed.Editor.UI.WinForms.Views
{
    public partial class LevelTileSelectorView : DockContent, IToolController
    {
        private LevelTileSelectorVM _vm;

        public LevelTileSelectorView()
        {
            InitializeComponent();
        }

        public void Initialize(LevelTileSelectorVM vm)
        {
            _vm = vm;

            TileSets.Initialize(_vm);
            TileSelector.Initialize(_vm.Parent.Root.TileSetViewer);

            _vm.PropertyChanged += _vm_PropertyChanged;

            TabText = _vm.Title;
        }

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_vm.Title):
                    TabText = _vm.Title;
                    break;
                default:
                    break;
            }
        }
    }
}
