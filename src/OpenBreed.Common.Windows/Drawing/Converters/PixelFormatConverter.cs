using OpenBreed.Common.Interface.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Windows.Drawing.Converters
{
    public static class PixelFormatConverter
    {
        #region Public Methods

        public static MyPixelFormat ToMyPixelFormat(PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                    return MyPixelFormat.Format24bppRgb;

                case PixelFormat.Format8bppIndexed:
                    return MyPixelFormat.Format8bppIndexed;

                case PixelFormat.Format32bppArgb:
                    return MyPixelFormat.Format32bppArgb;

                default:
                    throw new NotImplementedException();
            }
        }

        public static PixelFormat ToPixelFormat(MyPixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case MyPixelFormat.Format24bppRgb:
                    return PixelFormat.Format24bppRgb;

                case MyPixelFormat.Format8bppIndexed:
                    return PixelFormat.Format8bppIndexed;

                case MyPixelFormat.Format32bppArgb:
                    return PixelFormat.Format32bppArgb;

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion Public Methods
    }
}