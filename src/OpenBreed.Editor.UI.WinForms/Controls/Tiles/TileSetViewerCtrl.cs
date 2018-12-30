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
using OpenBreed.Editor.VM.Tiles.Helpers;

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
            Width = _vm.CurrentTileSet.Bitmap.Width;
            Height = _vm.CurrentTileSet.Bitmap.Height;
            Invalidate();
        }

        private void UpdateViewState()
        {
            if (_vm.CurrentTileSet == null)
                SetNoTileSetState();
            else
                SetTileSetState();
        }

        private void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_vm.CurrentTileSet):
                    UpdateViewState();
                    break;
                default:
                    break;
            }
        }

        private void TileSelectorCtrl_MouseMove(object sender, MouseEventArgs e)
        {
            if (_vm.SelectMode != SelectModeEnum.Nothing)
            {
                _vm.UpdateSelection(e.Location);
                Invalidate();
            }
        }

        private void TileSelectorCtrl_MouseUp(object sender, MouseEventArgs e)
        {
            if (_vm.SelectMode == SelectModeEnum.Nothing)
                return;

            _vm.FinishSelection(e.Location);
            Invalidate();
        }

        private void TileSelectorCtrl_MouseDown(object sender, MouseEventArgs e)
        {
            if (_vm.SelectMode != SelectModeEnum.Nothing)
                return;

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                _vm.StartSelection(SelectModeEnum.Select, e.Location);
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                _vm.StartSelection(SelectModeEnum.Deselect, e.Location);

            Invalidate();
        }

        private void TileSelectorCtrl_KeyUp(object sender, KeyEventArgs e)
        {
            if (!e.Control)
                _vm.MultiSelect = false;
        }

        private void TileSelectorCtrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
                _vm.MultiSelect = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_vm == null)
                return;

            if (_vm.CurrentTileSet == null)
                return;

            _vm.CurrentTileSet.Draw(e.Graphics);
            _vm.DrawSelection(e.Graphics);

            Pen selectedPen = new Pen(Color.LightGreen);
            Pen selectPen = new Pen(Color.LightBlue);
            Pen deselectPen = new Pen(Color.Red);

            base.OnPaint(e);
        }

    }
}
