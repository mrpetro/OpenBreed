using System.Drawing;
using System.Drawing.Drawing2D;

namespace OpenBreed.Editor.VM.Renderer
{
    public class RenderTarget
    {
        #region Private Fields

        private System.Drawing.Bitmap _bitmap;

        private Graphics gfx;

        #endregion Private Fields

        #region Public Constructors

        public RenderTarget(int width, int height)
        {
            Resize(width, height);
        }

        #endregion Public Constructors

        #region Public Properties

        public int Height { get { return _bitmap.Height; } }
        public int Width { get { return _bitmap.Width; } }

        public RectangleF ClipBounds { get => gfx.ClipBounds; }

        public Matrix Transform
        {
            get => gfx.Transform;
            set => gfx.Transform = value;
        }

        #endregion Public Properties

        #region Public Methods

        public void Flush(Graphics outGfx)
        {
            gfx.Flush();
            outGfx.DrawImage(_bitmap, 0, 0, _bitmap.Width, _bitmap.Height);
            gfx.Clear(Color.Black);
        }

        public void Resize(int width, int height)
        {
            if (_bitmap != null)
            {
                if (Width == width && Height == height)
                    return;

                gfx.Dispose();
                _bitmap.Dispose();
            }

            _bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            gfx = Graphics.FromImage(_bitmap);
            InitGfx();
        }

        public void DrawImage(Image image, float x, float y, float width, float height)
        {
            gfx.DrawImage(image, x, y, width, height);
        }

        public void DrawImage(Bitmap image, int x, int y, Rectangle srcRect)
        {
            gfx.DrawImage(image, x, y, srcRect, GraphicsUnit.Pixel);
        }

        public void FillRectangle(Brush brush, Rectangle rect)
        {
            gfx.FillRectangle(brush, rect);
        }

        public void DrawRectangle(Pen pen, Rectangle rect)
        {
            gfx.DrawRectangle(pen, rect);
        }

        public void DrawString(string s, Font font, Brush brush, float x, float y)
        {
            gfx.DrawString(s, font, brush, x, y);
        }

        #endregion Public Methods

        #region Private Methods

        private void InitGfx()
        {
            gfx.PixelOffsetMode = PixelOffsetMode.Half;
            gfx.CompositingQuality = CompositingQuality.AssumeLinear;
            gfx.CompositingMode = CompositingMode.SourceOver;
            gfx.SmoothingMode = SmoothingMode.None;
            gfx.InterpolationMode = InterpolationMode.NearestNeighbor;
            gfx.Clear(Color.Black);
        }

        #endregion Private Methods
    }
}