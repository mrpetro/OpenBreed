using OpenBreed.Common.Interface.Drawing;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace OpenBreed.Common.Windows.Drawing
{

    public class RenderTarget : IRenderTarget
    {
        #region Private Fields

        private readonly IDrawingFactory drawingFactory;
        private readonly IDrawingContextProvider drawingContextProvider;
        private IBitmap _bitmap;

        private IDrawingContext gfx;

        private MyRectangleF _clipBounds;

        #endregion Private Fields

        #region Public Constructors

        public RenderTarget(IDrawingFactory drawingFactory, IDrawingContextProvider drawingContextProvider, int width, int height)
        {
            this.drawingFactory = drawingFactory;
            this.drawingContextProvider = drawingContextProvider;

            Resize(width, height);

            _clipBounds = new MyRectangleF(0, 0, 0, 0);
        }

        #endregion Public Constructors

        #region Public Properties

        public int Height
        { get { return _bitmap.Height; } }

        public int Width
        { get { return _bitmap.Width; } }

        public MyRectangleF ClipBounds
        {
            get
            {
                return _clipBounds;
            }
        }

        public IMatrix Transform
        {
            get => gfx.Transform;
            set => gfx.Transform = value;
        }

        #endregion Public Properties

        #region Public Methods

        public void Flush(IDrawingContext outGfx)
        {
            gfx.Flush();
            outGfx.DrawImage(_bitmap, 0, 0, _bitmap.Width, _bitmap.Height);
            gfx.Clear(MyColor.Black);
        }

        public void Resize(int width, int height)
        {
            if (width <= 0 || height <= 0)
            {
                return;
            }

            if (_bitmap != null)
            {
                if (Width == width && Height == height)
                {
                    return;
                }

                gfx.Dispose();
                _bitmap.Dispose();
            }

            _bitmap = drawingFactory.CreateBitmap(width, height, MyPixelFormat.Format24bppRgb);
            gfx = drawingContextProvider.FromImage(_bitmap);
            InitGfx();
        }

        public void DrawImage(IImage image, float x, float y, float width, float height)
        {
            gfx.DrawImage(image, x, y, width, height);
        }

        public void DrawImage(IBitmap image, int x, int y, MyRectangle srcRect)
        {
            gfx.DrawImage(image, x, y, srcRect);
        }

        public void DrawImage(IImage image, float x, float y, int width, int height)
        {
            gfx.DrawImage(image, x, y, width, height);
        }

        public void DrawRectangle(IPen pen, MyRectangle myRectangle)
        {
            gfx.DrawRectangle(pen, myRectangle);
        }

        public void DrawImage(IImage image, float x, float y, MyRectangle srcRect)
        {
            gfx.DrawImage(image, x, y, srcRect);
        }

        public void FillRectangle(IBrush brush, MyRectangle rect)
        {
            gfx.FillRectangle(brush, rect);
        }

        public void DrawString(string s, IFont font, IBrush brush, float x, float y)
        {
            gfx.DrawString(s, font, brush, x, y);
        }

        #endregion Public Methods

        #region Private Methods

        private void InitGfx()
        {
            gfx.Setup();

            gfx.Clear(MyColor.Black);
        }

        #endregion Private Methods
    }
}