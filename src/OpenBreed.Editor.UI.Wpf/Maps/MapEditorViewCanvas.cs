using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Editor.UI.Wpf.Extensions;
using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Maps;
using OpenBreed.Editor.VM.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace OpenBreed.Editor.UI.Wpf.Maps
{
    internal class MapEditorViewCanvas : Canvas, IToolView
    {
        #region Private Fields

        private MapEditorViewVM? _vm;
        private ScrollTool? _scrollTool;
        private ZoomTool? _zoomTool;

        public Action KeyDownAction { get; set; }
        public Action KeyUpAction { get; set; }
        public Action<CursorButtons, MyPoint> MouseDownAction { get; set; }
        public Action<CursorButtons, MyPoint> MouseUpAction { get; set; }
        public Action<CursorButtons, MyPoint> MouseMoveAction { get; set; }
        public Action<int, MyPoint> MouseWheelAction { get; set; }
        public Action MouseEnterAction { get; set; }
        public Action MouseLeaveAction { get; set; }
        public Action PaintAction { get; set; }
        string IToolView.Cursor { get; set; }

        #endregion Private Fields

        #region Public Constructors

        public MapEditorViewCanvas()
        {
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.NearestNeighbor);
            SnapsToDevicePixels = true;
            Focusable = true;

            DataContextChanged += MapEditorViewCanvas_DataContextChanged;
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

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            MouseLeaveAction?.Invoke();

            if (_vm is null)
            {
                return;
            }

            _vm.Cursor.Leave();

            InvalidateVisual();
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            var pos = e.GetPosition(this).ToMyPoint();
            MouseWheelAction?.Invoke(e.Delta, pos);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            MouseEnterAction?.Invoke();

            if (_vm is null)
            {
                return;
            }

            _vm.Cursor.Hover();

            InvalidateVisual();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            var pos = e.GetPosition(this).ToMyPoint();
            MouseDownAction?.Invoke(ToCursorButtons(e.ChangedButton), pos);

            if (_vm is null)
            {
                return;
            }

            Keyboard.Focus(this);

            _vm.Cursor.Down(ToCursorButtons(e.ChangedButton), pos);

            InvalidateVisual();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            var pos = e.GetPosition(this).ToMyPoint();
            MouseMoveAction?.Invoke(ToCursorButtons(e), pos);

            if (_vm is null)
            {
                return;
            }

            Keyboard.Focus(this);


            _vm.Cursor.Move(ToCursorButtons(e), pos);

            InvalidateVisual();
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            var pos = e.GetPosition(this).ToMyPoint();
            MouseUpAction?.Invoke(ToCursorButtons(e.ChangedButton), pos);

            if (_vm is null)
            {
                return;
            }

            _vm.Cursor.Up(ToCursorButtons(e.ChangedButton), pos);

            InvalidateVisual();
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
        }

        #endregion Protected Methods

        #region Private Methods

        private void MapEditorViewCanvas_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is not MapEditorViewVM vm)
            {
                _vm = null;
                return;
            }

            _vm = vm;

            _vm.RefreshAction = this.InvalidateVisual;

            _scrollTool = new ScrollTool(_vm, this);
            _scrollTool.Activate();
            _zoomTool = new ZoomTool(_vm, this);
            _zoomTool.Activate();

            _vm.Resize((int)this.RenderSize.Width, (int)this.RenderSize.Height);

            InvalidateVisual();
        }

        private CursorButtons ToCursorButtons(MouseButton buttons)
        {
            var cursorButtons = CursorButtons.None;

            if (buttons.HasFlag(MouseButton.Left))
                cursorButtons |= CursorButtons.Left;
            if (buttons.HasFlag(MouseButton.Right))
                cursorButtons |= CursorButtons.Right;
            if (buttons.HasFlag(MouseButton.Middle))
                cursorButtons |= CursorButtons.Middle;

            return cursorButtons;
        }

        private CursorButtons ToCursorButtons(MouseEventArgs mouseEventArgs)
        {
            var cursorButtons = CursorButtons.None;

            if (mouseEventArgs.LeftButton == MouseButtonState.Pressed)
            {
                cursorButtons |= CursorButtons.Left;
            }

            if (mouseEventArgs.RightButton == MouseButtonState.Pressed)
            {
                cursorButtons |= CursorButtons.Right;
            }

            if (mouseEventArgs.MiddleButton == MouseButtonState.Pressed)
            {
                cursorButtons |= CursorButtons.Middle;
            }

            return cursorButtons;
        }

        public void Invalidate() => InvalidateVisual();

        #endregion Private Methods
    }
}