using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBreed.Editor.VM.Tiles;
using OpenBreed.Editor.VM.Common;

namespace OpenBreed.Editor.UI.WinForms.Controls.Tiles
{
    public partial class TileSetViewerCtrl : UserControl
    {
        private TileSetViewerVM vm;

        public TileSetViewerCtrl()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        public void Initialize(TileSetViewerVM vm)
        {
            this.vm = vm;

            KeyDown += TileSelectorCtrl_KeyDown;
            KeyUp += TileSelectorCtrl_KeyUp;
            MouseDown += TileSelectorCtrl_MouseDown;
            MouseUp += TileSelectorCtrl_MouseUp;
            MouseMove += TileSelectorCtrl_MouseMove;

            this.vm.PropertyChanged += _vm_PropertyChanged;

            UpdateViewState();
        }

        private void SetNoTileSetState()
        {
            Invalidate();
        }

        private void SetTileSetState()
        {
            Width = vm.Bitmap.Width;
            Height = vm.Bitmap.Height;
            Invalidate();
        }

        private void UpdateViewState()
        {
            SetTileSetState();
        }

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateViewState();
        }

        private void TileSelectorCtrl_MouseMove(object sender, MouseEventArgs e)
        {
            if (vm.Selector.SelectMode != SelectModeEnum.Nothing)
            {
                vm.Selector.UpdateSelection(e.Location);
                Invalidate();
            }
        }

        private void TileSelectorCtrl_MouseUp(object sender, MouseEventArgs e)
        {
            if (vm.Selector.SelectMode == SelectModeEnum.Nothing)
                return;

            vm.Selector.FinishSelection(e.Location);
            Invalidate();
        }

        private void TileSelectorCtrl_MouseDown(object sender, MouseEventArgs e)
        {
            if (vm.Selector.SelectMode != SelectModeEnum.Nothing)
                return;

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                vm.Selector.StartSelection(SelectModeEnum.Select, e.Location);
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                vm.Selector.StartSelection(SelectModeEnum.Deselect, e.Location);

            Invalidate();
        }

        private void TileSelectorCtrl_KeyUp(object sender, KeyEventArgs e)
        {
            if (!e.Control)
                vm.Selector.MultiSelect = false;
        }

        private void TileSelectorCtrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
                vm.Selector.MultiSelect = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (vm is null)
                return;

            vm.Draw(e.Graphics);
            vm.Selector.DrawSelection(e.Graphics);

            base.OnPaint(e);
        }

    }
}
