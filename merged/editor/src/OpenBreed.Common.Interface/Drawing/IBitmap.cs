using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Interface.Drawing
{
    public enum ImageLockMode
    {
        WriteOnly
    }

    public interface IBitmap : IImage
    {
        #region Public Properties

        MyPixelFormat PixelFormat { get; }

        #endregion Public Properties

        #region Public Methods

        IBitmapData LockBits(MyRectangle myRectangle, ImageLockMode lockMode, MyPixelFormat pixelFormat);

        void UnlockBits(IBitmapData bmpData);

        #endregion Public Methods
    }
}