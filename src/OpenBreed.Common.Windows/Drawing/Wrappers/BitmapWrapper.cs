using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Windows.Drawing.Converters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Windows.Drawing.Wrappers
{
    public class BitmapWrapper : ImageWrapper, IBitmap
    {
        #region Private Fields

        private ColorPaletteWrapper _palette;

        #endregion Private Fields

        #region Public Constructors

        public BitmapWrapper(System.Drawing.Bitmap bitmap) : base(bitmap)
        {
            Bitmap = bitmap;
        }

        #endregion Public Constructors

        #region Public Properties

        public new System.Drawing.Bitmap Bitmap { get; }

        public MyPixelFormat PixelFormat => PixelFormatConverter.ToMyPixelFormat(Bitmap.PixelFormat);


        public IBitmapData LockBits(MyRectangle myRectangle, ImageLockMode lockMode, MyPixelFormat pixelFormat)
        {
            var iPixelFormat = PixelFormatConverter.ToPixelFormat(pixelFormat);
            var iRectangle = new Rectangle(myRectangle.X, myRectangle.Y, myRectangle.Width, myRectangle.Height);

            var iBitmapData = Bitmap.LockBits(iRectangle, System.Drawing.Imaging.ImageLockMode.WriteOnly, iPixelFormat);

            return new BitmapDataWrapper(iBitmapData);
        }

        public void UnlockBits(IBitmapData bmpData)
        {
            Bitmap.UnlockBits(((BitmapDataWrapper)bmpData).Wrapped);
        }

        #endregion Public Properties
    }
}
