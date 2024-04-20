using OpenBreed.Common.Interface.Drawing;
using System.IO;

namespace OpenBreed.Model.Images
{
    public interface IImageBuilder
    {
        #region Public Methods

        void SetSize(int width, int height);

        void SetPixelFormat(MyPixelFormat pixelFormat);

        void SetPalette(MyColor[] palette);

        void SetData(byte[] data);

        #endregion Public Methods
    }
}