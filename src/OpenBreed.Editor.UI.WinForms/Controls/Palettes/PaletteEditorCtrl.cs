using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Palettes;

namespace OpenBreed.Editor.UI.WinForms.Controls.Palettes
{
    public partial class PaletteEditorCtrl : UserControl
    {
        private PaletteEditorVM _vm;

        public PaletteEditorCtrl()
        {
            InitializeComponent();
        }

        public void Initialize(PaletteEditorVM vm)
        {
            _vm = vm;

            ColorEditor.Initialize(_vm);
            ColorSelector.Initialize(_vm);

            //_vm.PropertyChanged += _vm_PropertyChanged;
            //ColorSelector.ColorSelected += ColorSelector_ColorSelected;

            //ColorSelector.Colors = _vm.CurrentPalette.Colors.ToArray();
        }

        //private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    switch (e.PropertyName)
        //    {
        //        case nameof(_vm.CurrentPalette):
        //            ColorSelector.Colors = _vm.CurrentPalette.Colors.ToArray();
        //            break;
        //        default:
        //            break;
        //    }
        //}

        //private void ColorSelector_ColorSelected(object sender, ColorSelectedEventArgs e)
        //{
        //    _vm.CurrentColorIndex = e.ColorIndex;
        //}

    }
}
