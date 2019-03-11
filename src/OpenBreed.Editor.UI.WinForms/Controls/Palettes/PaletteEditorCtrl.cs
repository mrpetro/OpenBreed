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
using OpenBreed.Editor.VM;

namespace OpenBreed.Editor.UI.WinForms.Controls.Palettes
{
    public partial class PaletteEditorCtrl : EntryEditorInnerCtrl
    {
        private PaletteEditorVM _vm;

        public PaletteEditorCtrl()
        {
            InitializeComponent();
        }

        //public override void Initialize(EntryEditorVM vm)
        //{
        //    _vm = vm as PaletteEditorVM ?? throw new InvalidOperationException(nameof(vm));

            //ColorEditor.Initialize(_vm);
            //ColorSelector.Initialize(_vm);

            //_vm.PropertyChanged += _vm_PropertyChanged;
            //ColorSelector.ColorSelected += ColorSelector_ColorSelected;

            //ColorSelector.Colors = _vm.CurrentPalette.Colors.ToArray();
        //}

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

        public override void Initialize(EntryEditorVM vm)
        {
            _vm = vm as PaletteEditorVM ?? throw new InvalidOperationException(nameof(vm));

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

        private void OnEditableChanged(PaletteVM palette)
        {
            Controls.Clear();

            if (palette == null)
                return;

            if (palette is PaletteFromBinaryVM)
            {
                var control = new PaletteFromBinaryCtrl();
                control.Initialize((PaletteFromBinaryVM)palette);
                control.Dock = DockStyle.Fill;
                Controls.Add(control);
            }
            else if (palette is PaletteFromMapVM)
            {
                var control = new PaletteFromMapCtrl();
                control.Initialize((PaletteFromMapVM)palette);
                control.Dock = DockStyle.Fill;
                Controls.Add(control);
            }
        }






    }
}
