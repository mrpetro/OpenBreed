using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Interface.Drawing
{
    public interface IRenderTarget
    {
        #region Public Properties

        int Height { get; }
        int Width { get; }

        MyRectangleF ClipBounds { get; }
        IMatrix Transform { get; set; }

        #endregion Public Properties

        #region Public Methods

        void DrawImage(IImage image, float x, float y, MyRectangle rectangle);

        void DrawImage(IImage image, float x, float y, int width, int height);

        void DrawRectangle(IPen pen, MyRectangle rectangle);

        void DrawString(string text, IFont font, IBrush brush, float x, float y);

        void FillRectangle(IBrush brush, MyRectangle rectangle);

        void Flush(IDrawingContext outGfx);

        void Resize(int width, int height);

        #endregion Public Methods
    }
}