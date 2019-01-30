using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Renderer
{
    public class RenderTarget
    {

        #region Private Fields

        private System.Drawing.Bitmap _bitmap;

        #endregion Private Fields

        #region Public Constructors

        public RenderTarget(int width, int height)
        {
            Resize(width, height);
        }

        #endregion Public Constructors

        #region Public Properties

        internal Graphics Gfx { get; private set; }
        public int Height { get { return _bitmap.Height; } }
        public int Width { get { return _bitmap.Width; } }

        #endregion Public Properties

        #region Public Methods

        public void Flush(Graphics outGfx)
        {
            Gfx.Flush();
            outGfx.DrawImage(_bitmap, 0, 0, _bitmap.Width, _bitmap.Height);
            Gfx.Clear(Color.Black);
        }

        public void Resize(int width, int height)
        {
            if (_bitmap != null)
            {
                if (Width == width && Height == height)
                    return;

                Gfx.Dispose();
                _bitmap.Dispose();
            }

            _bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Gfx = Graphics.FromImage(_bitmap);
            InitGfx();
        }

        #endregion Public Methods

        #region Private Methods

        private void InitGfx()
        {
            Gfx.PixelOffsetMode = PixelOffsetMode.Half;
            Gfx.CompositingQuality = CompositingQuality.AssumeLinear;
            Gfx.CompositingMode = CompositingMode.SourceOver;
            Gfx.SmoothingMode = SmoothingMode.None;
            Gfx.InterpolationMode = InterpolationMode.NearestNeighbor;
            Gfx.Clear(Color.Black);
        }

        #endregion Private Methods
    }
}
