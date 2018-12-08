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
using OpenBreed.Editor.VM.Maps.Tools;

namespace OpenBreed.Editor.UI.WinForms.Controls.Images
{
    public partial class ImageViewerCtrl : UserControl
    {
        private ImageViewerVM _vm;

        public ImageViewerCtrl()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        public void Initialize(ImageViewerVM vm)
        {
            _vm = vm;

            _vm.PropertyChanged += _vm_PropertyChanged;

            UpdateViewState();
        }

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_vm.Image):
                    UpdateViewState();
                    break;
                default:
                    break;
            }
        }

        private void UpdateViewState()
        {
            if (_vm.Image == null)
                SetNoImageState();
            else
                SetImageState();
        }

        private void SetNoImageState()
        {
            Invalidate();
        }

        private void SetImageState()
        {
            Width = _vm.Image.Width;
            Height = _vm.Image.Height;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
            e.Graphics.CompositingQuality = CompositingQuality.AssumeLinear;
            e.Graphics.CompositingMode = CompositingMode.SourceOver;
            e.Graphics.SmoothingMode = SmoothingMode.None;
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            _vm.Draw(e.Graphics, 0, 0, 1);

            base.OnPaint(e);
        }
    }
}
