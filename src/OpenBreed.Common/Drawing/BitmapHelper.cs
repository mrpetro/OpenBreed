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
            byte[] bytes = new byte[rectangle.Width * rectangle.Height];
            BitmapData bmpData = bmp.LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
            Marshal.Copy(bmpData.Scan0, bytes, 0, bytes.Length);
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
