using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Windows.Drawing.Converters;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Windows.Drawing.Wrappers
{
    public class BitmapDataWrapper : IBitmapData
    {
        public BitmapData Wrapped { get; }

        public BitmapDataWrapper(BitmapData wrapped)
        {
            Wrapped = wrapped;
        }

        public int Height
        {
            get => Wrapped.Height;
            set => Wrapped.Height = value;
        }

        public int Width
        {
            get => Wrapped.Width;
            set => Wrapped.Width = value;
        }

        public MyPixelFormat PixelFormat
        {
            get => PixelFormatConverter.ToMyPixelFormat(Wrapped.PixelFormat);
            set => Wrapped.PixelFormat = PixelFormatConverter.ToPixelFormat(value);
        }

        public int Reserved
        {
            get => Wrapped.Reserved;
            set => Wrapped.Reserved = value;
        }

        public IntPtr Scan0
        {
            get => Wrapped.Scan0;
            set => Wrapped.Scan0 = value;
        }

        public int Stride
        {
            get => Wrapped.Stride;
            set => Wrapped.Stride = value;
        }
    }
}
