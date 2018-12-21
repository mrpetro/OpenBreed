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
        private EditorVM _vm;

        public LevelTileSelectorView()
        {
            InitializeComponent();
        }

        public void Initialize(EditorVM vm)
        {
            _vm = vm;

            TileSets.Initialize(_vm.TileSetSelector);
            TileSelector.Initialize(_vm.TileSetViewer);

            _vm.PropertyChanged += _vm_PropertyChanged;

            TabText = _vm.TileSetSelector.Title;
        }

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_vm.TileSetSelector.Title):
                    TabText = _vm.TileSetSelector.Title;
                    break;
                default:
                    break;
            }
        }
    }
}
