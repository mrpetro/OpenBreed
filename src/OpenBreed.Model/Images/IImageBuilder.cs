using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace OpenBreed.Model.Images
{
    public interface IImageBuilder
    {
        #region Public Methods

        void SetSize(int width, int height);

        void SetPixelFormat(PixelFormat pixelFormat);

        void SetPalette(Color[] palette);

        void SetData(byte[] data);

        #endregion Public Methods
    }
}