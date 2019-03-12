using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Palettes;

namespace OpenBreed.Editor.UI.WinForms.Controls.Palettes
{
    public partial class ColorEditorCtrl : UserControl
    {
        #region Private Fields

        private PaletteVM _vm;

        #endregion Private Fields

        #region Public Constructors

        public ColorEditorCtrl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(PaletteVM vm)
        {
            _vm = vm;

            _vm.PropertyChanged += _vm_PropertyChanged;

            SetupWithColor(_vm.CurrentColorIndex, _vm.CurrentColor);
        }

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_vm.CurrentColorIndex):
                    SetupWithColor(_vm.CurrentColorIndex, _vm.CurrentColor);
                    break;
                case nameof(_vm.CurrentColor):
                    SetupWithColor(_vm.CurrentColorIndex, _vm.CurrentColor);
                    break;
                default:
                    break;
            }
        }

        public void SetupWithColor(int index, Color color)
        {
            lblColor.BackColor = color;
            lblColor.ForeColor = Color.FromArgb(color.ToArgb() ^ 0xffffff);
            lblColor.Text = index.ToString();
            sliderR.Value = color.R;
            dmR.Text = color.R.ToString();
            sliderG.Value = color.G;
            dmG.Text = color.G.ToString();
            sliderB.Value = color.B;
            dmB.Text = color.B.ToString();
            Invalidate();
        }

        #endregion Public Methods

        #region Private Methods

        private void SetB(byte value)
        {
            Color oldColor = _vm.CurrentColor;
            _vm.CurrentColor = Color.FromArgb(oldColor.R, oldColor.G, value);
        }

        private void SetG(byte value)
        {
            Color oldColor = _vm.CurrentColor;
            _vm.CurrentColor = Color.FromArgb(oldColor.R, value, oldColor.B);
        }

        private void SetR(byte value)
        {
            Color oldColor = _vm.CurrentColor;
            _vm.CurrentColor = Color.FromArgb(value, oldColor.G, oldColor.B);
        }
        private void sliderB_ValueChanged(object sender, EventArgs e)
        {
            SetB((byte)sliderB.Value);
        }

        private void sliderG_ValueChanged(object sender, EventArgs e)
        {
            SetG((byte)sliderG.Value);
        }

        private void sliderR_ValueChanged(object sender, EventArgs e)
        {
            SetR((byte)sliderR.Value);
        }

        #endregion Private Methods
    }
}
