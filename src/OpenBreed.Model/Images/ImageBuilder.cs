using OpenBreed.Common.Tools;
using OpenBreed.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Model.Images
{
    public class ImageBuilder : IImageBuilder
    {
        internal int Width;
        internal int Height;
        internal PixelFormat PixelFormat;
        internal byte[] Data;
        internal Color[] Palette;

        public void SetSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void SetPixelFormat(PixelFormat pixelFormat)
        {
            PixelFormat = pixelFormat;
        }

        public void SetPalette(Color[] palette)
        {
            Palette = palette;
        }

        public void SetData(byte[] data)
        {
            Data = data;
        }

        public static ImageBuilder NewImage()
        {
            return new ImageBuilder();
        }

        public Image Build()
        {
            var newImage = BitmapHelper.FromBytes(Width, Height, Data);
            BitmapHelper.SetPaletteColors(newImage, Palette);
            return newImage;
        }
    }
}
