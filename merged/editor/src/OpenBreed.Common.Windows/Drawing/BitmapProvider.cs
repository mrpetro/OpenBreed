using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Windows.Drawing.Wrappers;
using OpenBreed.Common.Windows.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Windows.Drawing
{
    internal class BitmapProvider : IBitmapProvider
    {
        #region Public Constructors

        public BitmapProvider()
        {
            ErrorIcon = new ImageWrapper(System.Drawing.SystemIcons.Error.ToBitmap());
        }

        #endregion Public Constructors

        #region Public Properties

        public IImage ErrorIcon { get; }

        #endregion Public Properties

        #region Public Methods

        public IImage FromBytes(int width, int height, byte[] bytes)
        {
            return new ImageWrapper(BitmapHelper.FromBytes(width, height, bytes));
        }

        public void SetPaletteColors(IImage sourceImage, MyColor[] data)
        {
            BitmapHelper.SetPaletteColors(((ImageWrapper)sourceImage).Wrapped, data.Select(c => c.ToColor()).ToArray());
        }

        public byte[] ToBytes(IImage sourceImage, MyRectangle sourceRectangle)
        {
            return BitmapHelper.ToBytes((Bitmap)((ImageWrapper)sourceImage).Wrapped);
        }

        #endregion Public Methods
    }
}