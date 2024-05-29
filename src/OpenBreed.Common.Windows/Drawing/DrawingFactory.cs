using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Windows.Drawing.Converters;
using OpenBreed.Common.Windows.Drawing.Wrappers;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Windows.Drawing
{
    public class DrawingFactory : IDrawingFactory
    {
        private readonly IDrawingContextProvider drawingContextProvider;

        public DrawingFactory(IDrawingContextProvider drawingContextProvider)
        {
            this.drawingContextProvider = drawingContextProvider;
        }

        #region Public Methods

        public IBitmap CreateBitmap(int width, int height, MyPixelFormat pixelFormat)
        {
            return new BitmapWrapper(new Bitmap(width, height, PixelFormatConverter.ToPixelFormat(pixelFormat)));
        }

        public IFont CreateFont(string fontName, int fontSize) => new FontWrapper(new Font(fontName, fontSize));

        public IMatrix CreateMatrix() => new MatrixWrapper(new Matrix());

        public IPen CreatePen(MyColor color)
        {
            return new PenWrapper(new Pen(Color.FromArgb(color.A, color.R, color.G, color.B)));
        }

        public IPen CreatePen(MyColor color, float width)
        {
            return new PenWrapper(new Pen(Color.FromArgb(color.A, color.R, color.G, color.B), width));
        }

        public IRenderTarget CreateRenderTarget(int width, int height)
        {
            return new RenderTarget(this, drawingContextProvider, width, height);
        }

        public IBrush CreateSolidBrush(MyColor color)
        {
            return new SolidBrushWrapper(new SolidBrush(Color.FromArgb(color.A, color.R, color.G, color.B)));
        }

        #endregion Public Methods
    }
}