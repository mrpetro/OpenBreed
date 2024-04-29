using OpenBreed.Editor.VM;
using OpenBreed.Editor.VM.Common;
using OpenBreed.Editor.VM.Sprites;
using OpenBreed.Editor.VM.Tiles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace OpenBreed.Editor.UI.Wpf.Sprites
{
    public class SpriteFromImageSelectionCanvas : Canvas
    {
        #region Public Constructors

        public SpriteFromImageSelectionCanvas()
        {
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.NearestNeighbor);
            SnapsToDevicePixels = true;
            Focusable = true;

            DataContextChanged += SpriteFromImageSelectionCanvas_DataContextChanged;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (DataContext is not SpriteFromImageEditorVM vm)
            {
                return;
            }

            var pos = e.GetPosition(this);
            vm.CursorDown(DrawingHelpers.ToPoint(pos));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (DataContext is not SpriteFromImageEditorVM vm)
            {
                return;
            }

            var pos = e.GetPosition(this);
            vm.CursorMove(DrawingHelpers.ToPoint(pos));
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (DataContext is not SpriteFromImageEditorVM vm)
            {
                return;
            }

            var pos = e.GetPosition(this);
            vm.CursorUp(DrawingHelpers.ToPoint(pos));
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (DataContext is not SpriteFromImageEditorVM vm)
            {
                return;
            }

            var wpfDc = new WpfDrawingContext(dc);

            vm.DrawSpriteEditorView(wpfDc);
        }

        #endregion Protected Methods

        #region Private Methods

        private void SpriteFromImageSelectionCanvas_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is not SpriteFromImageEditorVM vm)
            {
                return;
            }

            vm.RefreshAction = () => InvalidateVisual();
        }

        #endregion Private Methods
    }
}