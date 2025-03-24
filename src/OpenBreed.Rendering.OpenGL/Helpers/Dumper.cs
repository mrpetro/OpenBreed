using OpenBreed.Common.Interface.Drawing;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.OpenGL.Helpers
{
    public static class Dumper
    {
        private static int dumpCount = 0;

        private static bool triggered;

        public static void DumpWhenRequested(string dirPath, string filePrefix)
        {
            if (triggered)
            {
                triggered = false;

                DumpStencil(dirPath, filePrefix);
            }
        }

        public static void DumpStencil(string dirPath, string filePrefix)
        {
            GL.Finish();
            var dims = new int[4];

            GL.GetInteger(GetPName.Viewport, dims);
            var fbWidth = dims[2];
            var fbHeight = dims[3];

            //var data = new int[fbWidth * fbHeight];
            //GL.ReadPixels(0, 0, fbWidth, fbHeight, PixelFormat.StencilIndex, PixelType.UnsignedInt, data);

            var bitmap = DumpBitmap(fbWidth, fbHeight);

            var filePath = Path.Combine(dirPath, $"{filePrefix}_{dumpCount}.bmp");

            using (var file = File.OpenWrite(filePath))
            {
                bitmap.Save(file, System.Drawing.Imaging.ImageFormat.Bmp);
            }

            dumpCount++;
        }

        private static Bitmap DumpBitmap(int width, int height)
        {
            var rectangle = new Rectangle(0, 0, width, height);

            var bmp = new Bitmap(width, height);
            var bmpData =
                bmp.LockBits(rectangle, System.Drawing.Imaging.ImageLockMode.WriteOnly,
                             System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

            GL.ReadPixels(0, 0, width, height, PixelFormat.StencilIndex, PixelType.UnsignedByte, bmpData.Scan0);
            bmp.UnlockBits(bmpData);
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return bmp;
        }

        public static void Trigger()
        {
            triggered = true;
        }
    }
}
