using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace OpenBreed.Common.Drawing
{
    public static class BitmapHelper
    {
        public static byte[] ToBytes(Bitmap bmp, Rectangle rectangle)
        {
            var bmpData = bmp.LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
            // create byte array to copy pixel values
            var bytes = new byte[rectangle.Width * rectangle.Height];
            var iptr = bmpData.Scan0;

            // Copy data from pointer to array
            // Not working for negative Stride see. http://stackoverflow.com/a/10360753/1498252
            //Marshal.Copy(Iptr, Pixels, 0, Pixels.Length);
            // Solution for positive and negative Stride:
            for (int y = 0; y < rectangle.Height; y++)
            {
                // This is the important part.  Stride will likely be 16 bytes.
                // Image is 5px wide x 3 bytes per pixel = 15 bytes per line
                // Stride is rounded up to the nearest multiple of 4 to keep sensible memory alignment
                int ptrOffset = y * bmpData.Stride;

                int lineBytes = bmpData.Width * 1; // 6 bytes
                int bufferOffset = y * lineBytes;

                Marshal.Copy(IntPtr.Add(iptr, ptrOffset), bytes, bufferOffset, lineBytes);
            }

            bmp.UnlockBits(bmpData);


            return bytes;
        }

        public static byte[] ToBytes(Bitmap bmp)
        {
            byte[] bytes = new byte[bmp.Width * bmp.Height];
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
            Marshal.Copy(bmpData.Scan0, bytes, 0, bytes.Length);
            bmp.UnlockBits(bmpData);
            return bytes;
        }

        public static Bitmap FromBytes(int width, int height, byte[] bytes)
        {
            Bitmap bmp = new Bitmap(width,height,PixelFormat.Format8bppIndexed);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            Marshal.Copy(bytes, 0, bmpData.Scan0, bmp.Width * bmp.Height);
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        public static void SetPaletteColors(Image image, Color[] newColors)
        {
            ColorPalette palette = image.Palette;
            for (int colorIndex = 0; colorIndex < newColors.Length; colorIndex++)
                palette.Entries[colorIndex] = newColors[colorIndex];

            for (int colorIndex = newColors.Length; colorIndex < palette.Entries.Length; colorIndex++)
                palette.Entries[colorIndex] = Color.Black;

            image.Palette = palette;
        }


        public static Bitmap GetBitmapEx(Bitmap bitmap, Rectangle rectangle)
        {
            Bitmap cut = new Bitmap(rectangle.Width, rectangle.Height, bitmap.PixelFormat);

            using (Graphics gfx = Graphics.FromImage(cut))
            {
                gfx.DrawImage(bitmap, new Rectangle(0, 0, cut.Width, cut.Height), rectangle, GraphicsUnit.Pixel);
            }

            return cut;
        }

        public static Bitmap GetBitmap(Bitmap bitmap, Rectangle rectangle)
        {
            byte[] srcBmpData = ToBytes(bitmap, rectangle);
            return FromBytes(rectangle.Width, rectangle.Height, srcBmpData);
        }

        public static Bitmap ConvertTo32bppPArgb(Bitmap bitmap)
        {
            Bitmap clone = new Bitmap(bitmap.Width, bitmap.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            using (Graphics gr = Graphics.FromImage(clone))
            {
                gr.DrawImage(clone, new Rectangle(0, 0, clone.Width, clone.Height));
            }

            return clone;
        }
    }
}
