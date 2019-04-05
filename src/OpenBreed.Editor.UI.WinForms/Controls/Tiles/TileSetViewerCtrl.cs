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
        private TileSetViewerVM _vm;

        public TileSetViewerCtrl()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        public void Initialize(TileSetViewerVM vm)
        {
            _vm = vm;

            KeyDown += TileSelectorCtrl_KeyDown;
            KeyUp += TileSelectorCtrl_KeyUp;
            MouseDown += TileSelectorCtrl_MouseDown;
            MouseUp += TileSelectorCtrl_MouseUp;
            MouseMove += TileSelectorCtrl_MouseMove;

            _vm.PropertyChanged += _vm_PropertyChanged;

            UpdateViewState();
        }

        private void SetNoTileSetState()
        {
            Invalidate();
        }

        private void SetTileSetState()
        {
            Width = _vm.Bitmap.Width;
            Height = _vm.Bitmap.Height;
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
            if (_vm.Selector.SelectMode != SelectModeEnum.Nothing)
            {
                _vm.Selector.UpdateSelection(e.Location);
                Invalidate();
            }
        }

        private void TileSelectorCtrl_MouseUp(object sender, MouseEventArgs e)
        {
            if (_vm.Selector.SelectMode == SelectModeEnum.Nothing)
                return;

            _vm.Selector.FinishSelection(e.Location);
            Invalidate();
        }

        private void TileSelectorCtrl_MouseDown(object sender, MouseEventArgs e)
        {
            if (_vm.Selector.SelectMode != SelectModeEnum.Nothing)
                return;

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                _vm.Selector.StartSelection(SelectModeEnum.Select, e.Location);
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                _vm.Selector.StartSelection(SelectModeEnum.Deselect, e.Location);

            Invalidate();
        }

        private void TileSelectorCtrl_KeyUp(object sender, KeyEventArgs e)
        {
            if (!e.Control)
                _vm.Selector.MultiSelect = false;
        }

        private void TileSelectorCtrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
                _vm.Selector.MultiSelect = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _vm.Draw(e.Graphics);
            _vm.Selector.DrawSelection(e.Graphics);

            Pen selectedPen = new Pen(Color.LightGreen);
            Pen selectPen = new Pen(Color.LightBlue);
            Pen deselectPen = new Pen(Color.Red);

            base.OnPaint(e);
        }

    }
}
