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
using OpenBreed.Editor.VM.Images;
using System.Drawing.Drawing2D;

namespace OpenBreed.Editor.UI.WinForms.Controls.Images
{
    public partial class ImageFromFileEditorCtrl : UserControl
    {
        private ImageFromFileEditorVM vm;


        public ImageFromFileEditorCtrl()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        public void Initialize(ImageFromFileEditorVM vm)
        {
            this.vm = vm;

            this.vm.PropertyChanged += _vm_PropertyChanged;
            this.vm.RefreshAction = Invalidate;

            UpdateViewState();
        }

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(vm.Image):
                    UpdateViewState();
                    break;
                default:
                    break;
            }
        }

        private void UpdateViewState()
        {
            if (vm.Image == null)
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
            Width = vm.Image.Width;
            Height = vm.Image.Height;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (vm == null)
                return;

            e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
            e.Graphics.CompositingQuality = CompositingQuality.AssumeLinear;
            e.Graphics.CompositingMode = CompositingMode.SourceOver;
            e.Graphics.SmoothingMode = SmoothingMode.None;
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            vm.Draw(e.Graphics);

            base.OnPaint(e);
        }

    }
}
