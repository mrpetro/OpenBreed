using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Common;
using OpenBreed.Editor.VM.Tiles;
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

namespace OpenBreed.Editor.UI.Wpf.Tiles
{
    public class TileSetViewerCanvas : Canvas
    {
        public TileSetViewerCanvas()
        {
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.NearestNeighbor);
            SnapsToDevicePixels = true;
            Focusable = true;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            Keyboard.Focus(this);

            if (DataContext is not TileSetViewerVM vm)
            {
                return;
            }

            if (vm.Selector.SelectMode != SelectModeEnum.Nothing)
            {
                return;
            }

            var pos = e.GetPosition(this);

            if (e.ChangedButton == MouseButton.Left)
            {

                vm.Selector.StartSelection(SelectModeEnum.Select, new MyPoint((int)pos.X, (int)pos.Y));
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                vm.Selector.StartSelection(SelectModeEnum.Deselect, new MyPoint((int)pos.X, (int)pos.Y));
            }

            InvalidateVisual();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (DataContext is not TileSetViewerVM vm)
            {
                return;
            }

            Keyboard.Focus(this);

            if (vm.Selector.SelectMode != SelectModeEnum.Nothing)
            {
                var pos = e.GetPosition(this);

                vm.Selector.UpdateSelection(new MyPoint((int)pos.X, (int)pos.Y));
                InvalidateVisual();
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (DataContext is not TileSetViewerVM vm)
            {
                return;
            }

            if (vm.Selector.SelectMode == SelectModeEnum.Nothing)
                return;


            var pos = e.GetPosition(this);

            vm.Selector.FinishSelection(new MyPoint((int)pos.X, (int)pos.Y));
            InvalidateVisual();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (DataContext is not TileSetViewerVM vm)
            {
                return;
            }

            if (e.KeyboardDevice.IsKeyDown(Key.RightCtrl))
            {
                vm.Selector.MultiSelect = true;
            }
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (DataContext is not TileSetViewerVM vm)
            {
                return;
            }

            if (e.Key == Key.RightCtrl)
            {
                vm.Selector.MultiSelect = false;
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (DataContext is not TileSetViewerVM vm)
            {
                return;
            }

            var wpfDc = new WpfDrawingContext(dc);

            vm.Draw(wpfDc);
            vm.Selector.DrawSelection(wpfDc);

            var colorM = System.Windows.Media.Color.FromArgb(0, 255, 255, 255);
            var brushM = new System.Windows.Media.SolidColorBrush(colorM);
            var penM = new System.Windows.Media.Pen(brushM, 1);
            var rectangleM = new Rect(10,10,32, 32);

            dc.DrawRectangle(null, penM, rectangleM);
        }
    }
}
