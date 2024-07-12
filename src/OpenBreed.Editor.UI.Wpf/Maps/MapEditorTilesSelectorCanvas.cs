using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Common;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Editor.VM.Tools;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OpenBreed.Editor.UI.Wpf.Maps
{
    public class MapEditorTilesSelectorCanvas : Canvas
    {
        #region Private Fields

        private MapEditorTilesSelectorVM? _vm;

        #endregion Private Fields

        #region Public Constructors

        public MapEditorTilesSelectorCanvas()
        {
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.NearestNeighbor);
            SnapsToDevicePixels = true;
            Focusable = true;

            DataContextChanged += MapEditorTilesSelectorCanvas_DataContextChanged;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            if (_vm is null)
            {
                return;
            }

            _vm.Resize((int)sizeInfo.NewSize.Width, (int)sizeInfo.NewSize.Height);

            InvalidateVisual();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            Keyboard.Focus(this);

            if (_vm is null)
            {
                return;
            }

            if (_vm.SelectMode != SelectModeEnum.Nothing)
            {
                return;
            }

            var pos = e.GetPosition(this);

            if (e.ChangedButton == MouseButton.Left)
            {
                _vm.StartSelection(SelectModeEnum.Select, new MyPoint((int)pos.X, (int)pos.Y));
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                _vm.StartSelection(SelectModeEnum.Deselect, new MyPoint((int)pos.X, (int)pos.Y));
            }

            InvalidateVisual();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_vm is null)
            {
                return;
            }

            Keyboard.Focus(this);

            if (_vm.SelectMode != SelectModeEnum.Nothing)
            {
                var pos = e.GetPosition(this);

                _vm.UpdateSelection(new MyPoint((int)pos.X, (int)pos.Y));
                InvalidateVisual();
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (_vm is null)
            {
                return;
            }

            if (_vm.SelectMode == SelectModeEnum.Nothing)
                return;

            var pos = e.GetPosition(this);

            _vm.FinishSelection(new MyPoint((int)pos.X, (int)pos.Y));
            InvalidateVisual();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (_vm is null)
            {
                return;
            }

            if (e.KeyboardDevice.IsKeyDown(Key.RightCtrl))
            {
                _vm.MultiSelect = true;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (_vm is null)
            {
                return;
            }

            if (e.Key == Key.RightCtrl)
            {
                _vm.MultiSelect = false;
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (_vm is null)
            {
                return;
            }

            var wpfDc = new WpfDrawingContext(dc);

            _vm.Render(wpfDc);

            var colorM = System.Windows.Media.Color.FromArgb(0, 255, 255, 255);
            var brushM = new System.Windows.Media.SolidColorBrush(colorM);
            var penM = new System.Windows.Media.Pen(brushM, 1);
            var rectangleM = new Rect(10, 10, 32, 32);

            dc.DrawRectangle(null, penM, rectangleM);
        }

        #endregion Protected Methods

        #region Private Methods

        private void MapEditorTilesSelectorCanvas_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is not MapEditorTilesSelectorVM vm)
            {
                _vm = null;
                return;
            }

            _vm = vm;

            _vm.Resize((int)this.RenderSize.Width, (int)this.RenderSize.Height);

            InvalidateVisual();
        }

        #endregion Private Methods
    }
}