using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Interface.Drawing
{
    public enum MyPixelFormat
    {
        Format8bppIndexed,
        Format32bppArgb,
        Format24bppRgb
    }

    public interface IDrawingFactory
    {
        #region Public Methods

        IBitmap CreateBitmap(int width, int height, MyPixelFormat pixelFormat);

        IFont CreateFont(string fontName, int fontSize);

        IMatrix CreateMatrix();

        IPen CreatePen(MyColor color);
        IPen CreatePen(MyColor color, float width);
        IRenderTarget CreateRenderTarget(int width, int height);
        IBrush CreateSolidBrush(MyColor color);

        #endregion Public Methods
    }
}