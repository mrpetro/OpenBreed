using OpenBreed.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Builders.Images
{
    public class ImageBuilder
    {
        internal int Width;
        internal int Height;
        internal PixelFormat PixelFormat;
        internal byte[] Data;
        internal Color[] Palette;

        internal void SetSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        internal void SetPixelFormat(PixelFormat pixelFormat)
        {
            PixelFormat = pixelFormat;
        }

        internal void SetPalette(Color[] palette)
        {
            Palette = palette;
        }

        internal void SetData(byte[] data)
        {
            Data = data;
        }

        public static ImageBuilder NewImage()
        {
            return new ImageBuilder();
        }

        internal Image Build()
        {
            var newImage = BitmapHelper.FromBytes(Width, Height, Data);
            BitmapHelper.SetPaletteColors(newImage, Palette);
            return newImage;
        }
    }
}
