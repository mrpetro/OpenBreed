using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Windows.Drawing.Wrappers;
using OpenBreed.Common.Windows.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace OpenBreed.Common.Windows.Drawing
{
    public class DrawingContext : IDrawingContext
    {
        #region Private Fields

        private readonly Graphics graphics;

        #endregion Private Fields

        #region Public Constructors

        public DrawingContext(Graphics graphics)
        {
            this.graphics = graphics;
        }

        #endregion Public Constructors

        #region Public Properties

        public IMatrix Transform
        {
            get => new MatrixWrapper(graphics.Transform);
            set => graphics.Transform = ((MatrixWrapper)value).Wrapped;
        }

        #endregion Public Properties

        #region Public Methods

        public void Clear(MyColor color)
        {
            graphics.Clear(color.ToColor());
        }

        public void Dispose()
        {
            graphics.Dispose();
        }

        public void DrawImage(IImage image, int x, int y, MyRectangle srcRect)
        {
            graphics.DrawImage(((ImageWrapper)image).Wrapped, x, y, srcRect.ToRectangle(), GraphicsUnit.Pixel);
        }

        public void DrawImage(IImage image, float x, float y, MyRectangle srcRect)
        {
            graphics.DrawImage(((ImageWrapper)image).Wrapped, x, y, srcRect.ToRectangle(), GraphicsUnit.Pixel);
        }

        public void DrawImage(IImage image, float x, float y, float width, float height)
        {
            graphics.DrawImage(((ImageWrapper)image).Wrapped, x, y, width, height);
        }

        public void DrawRectangle(IPen pen, MyRectangle rectangle)
        {
            graphics.DrawRectangle(((PenWrapper)pen).Wrapped, rectangle.ToRectangle());
        }

        public void DrawString(string text, IFont font, IBrush brush, float x, float y)
        {
            graphics.DrawString(text, ((FontWrapper)font).Wrapped, ((SolidBrushWrapper)brush).Wrapped, new PointF(x, y));
        }

        public void FillRectangle(IBrush brush, MyRectangle rectangle)
        {
            graphics.FillRectangle(((SolidBrushWrapper)brush).Wrapped, rectangle.ToRectangle());
        }

        public void Flush()
        {
            graphics.Flush();
        }

        public void Setup()
        {
            graphics.PixelOffsetMode = PixelOffsetMode.Half;
            graphics.CompositingQuality = CompositingQuality.AssumeLinear;
            graphics.CompositingMode = CompositingMode.SourceOver;
            graphics.SmoothingMode = SmoothingMode.None;
            graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
        }

        #endregion Public Methods
    }
}