using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Sprites;
using System.Drawing.Drawing2D;

namespace OpenBreed.Editor.UI.WinForms.Controls.Sprites
{
    public partial class SpriteEditorViewCtrl : UserControl
    {
        private SpriteFromImageEditorVM vm;

        public SpriteEditorViewCtrl()
        {
            InitializeComponent();
        }

        public void Initialize(SpriteFromImageEditorVM vm)
        {
            this.vm = vm;

            MouseMove += (s, a) => vm.CursorMove(a.Location);
            MouseDown += (s, a) => vm.CursorDown(a.Location);
            MouseUp += (s, a) => vm.CursorUp(a.Location);
            vm.RefreshAction = Invalidate;
            vm.PropertyChanged += vm_PropertyChanged;

            UpdateViewState();
        }

        private void UpdateViewState()
        {
            Width = vm.Parent.SourceImage.Width;
            Height = vm.Parent.SourceImage.Height;
            Invalidate();
        }

        private void vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(vm.SpriteRectangle):
                    UpdateViewState();
                    break;
                default:
                    break;
            }
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

            vm.DrawSpriteEditorView(e.Graphics);

            base.OnPaint(e);
        }

    }
}
