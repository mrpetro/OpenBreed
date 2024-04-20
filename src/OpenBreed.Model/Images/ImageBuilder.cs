using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Tools;
using OpenBreed.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Model.Images
{
    public class ImageBuilder : IImageBuilder
    {
        internal int Width;
        internal int Height;
        internal MyPixelFormat PixelFormat;
        internal byte[] Data;
        internal MyColor[] Palette;
        private readonly IBitmapProvider bitmapProvider;

        public ImageBuilder(IBitmapProvider bitmapProvider)
        {
            this.bitmapProvider = bitmapProvider;
        }

        public void SetSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void SetPixelFormat(MyPixelFormat pixelFormat)
        {
            PixelFormat = pixelFormat;
        }

        public void SetPalette(MyColor[] palette)
        {
            Palette = palette;
        }

        public void SetData(byte[] data)
        {
            Data = data;
        }

        public static ImageBuilder NewImage(IBitmapProvider bitmapProvider)
        {
            return new ImageBuilder(bitmapProvider);
        }

        public IImage Build()
        {
            var newImage = bitmapProvider.FromBytes(Width, Height, Data);
            bitmapProvider.SetPaletteColors(newImage, Palette);
            return newImage;
        }
    }
}
