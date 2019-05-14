using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace OpenBreed.Core.Systems.Rendering.Helpers
{
    public class Texture : ITexture, IDisposable
    {
        #region Private Constructors

        private Texture(int id)
        {
            Id = id;
        }

        #endregion Private Constructors

        #region Public Properties

        public int Id { get; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public static PixelInternalFormat ToGlPixelFormat(System.Drawing.Imaging.PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case System.Drawing.Imaging.PixelFormat.Indexed:
                    break;
                case System.Drawing.Imaging.PixelFormat.Gdi:
                    break;
                case System.Drawing.Imaging.PixelFormat.Alpha:
                    break;
                case System.Drawing.Imaging.PixelFormat.PAlpha:
                    break;
                case System.Drawing.Imaging.PixelFormat.Extended:
                    break;
                case System.Drawing.Imaging.PixelFormat.Canonical:
                    break;
                case System.Drawing.Imaging.PixelFormat.Undefined:
                    break;
                case System.Drawing.Imaging.PixelFormat.Format1bppIndexed:
                    break;
                case System.Drawing.Imaging.PixelFormat.Format4bppIndexed:
                    break;
                case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
                    break;
                case System.Drawing.Imaging.PixelFormat.Format16bppGrayScale:
                    break;
                case System.Drawing.Imaging.PixelFormat.Format16bppRgb555:
                    break;
                case System.Drawing.Imaging.PixelFormat.Format16bppRgb565:
                    break;
                case System.Drawing.Imaging.PixelFormat.Format16bppArgb1555:
                    break;
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                    break;
                case System.Drawing.Imaging.PixelFormat.Format32bppRgb:
                    return PixelInternalFormat.Rgb;
                case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                    return PixelInternalFormat.Rgba;
                case System.Drawing.Imaging.PixelFormat.Format32bppPArgb:
                    break;
                case System.Drawing.Imaging.PixelFormat.Format48bppRgb:
                    break;
                case System.Drawing.Imaging.PixelFormat.Format64bppArgb:
                    break;
                case System.Drawing.Imaging.PixelFormat.Format64bppPArgb:
                    break;
                case System.Drawing.Imaging.PixelFormat.Max:
                    break;
                default:
                    break;
            }

            throw new NotSupportedException();
        }

        private static System.Drawing.Imaging.PixelFormat GetSupportedPixelFormat(System.Drawing.Imaging.PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case System.Drawing.Imaging.PixelFormat.Indexed:
                    break;
                case System.Drawing.Imaging.PixelFormat.Gdi:
                    break;
                case System.Drawing.Imaging.PixelFormat.Alpha:
                    break;
                case System.Drawing.Imaging.PixelFormat.PAlpha:
                    break;
                case System.Drawing.Imaging.PixelFormat.Extended:
                    break;
                case System.Drawing.Imaging.PixelFormat.Canonical:
                    break;
                case System.Drawing.Imaging.PixelFormat.Undefined:
                    break;
                case System.Drawing.Imaging.PixelFormat.Format1bppIndexed:
                    break;
                case System.Drawing.Imaging.PixelFormat.Format4bppIndexed:
                    return System.Drawing.Imaging.PixelFormat.Format32bppArgb;
                case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
                    return System.Drawing.Imaging.PixelFormat.Format32bppArgb;
                case System.Drawing.Imaging.PixelFormat.Format16bppGrayScale:
                    break;
                case System.Drawing.Imaging.PixelFormat.Format16bppRgb555:
                    break;
                case System.Drawing.Imaging.PixelFormat.Format16bppRgb565:
                    break;
                case System.Drawing.Imaging.PixelFormat.Format16bppArgb1555:
                    break;
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                    return System.Drawing.Imaging.PixelFormat.Format32bppArgb;
                case System.Drawing.Imaging.PixelFormat.Format32bppRgb:
                    break;
                case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                    break;
                case System.Drawing.Imaging.PixelFormat.Format32bppPArgb:
                    break;
                case System.Drawing.Imaging.PixelFormat.Format48bppRgb:
                    break;
                case System.Drawing.Imaging.PixelFormat.Format64bppArgb:
                    break;
                case System.Drawing.Imaging.PixelFormat.Format64bppPArgb:
                    break;
                case System.Drawing.Imaging.PixelFormat.Max:
                    break;
                default:
                    break;
            }

            return pixelFormat;
        }

        public static Texture CreateFromBitmap(Bitmap bitmap)
        {   
            //Generate empty texture
            var textureId = GL.GenTexture();
            //Link empty texture to texture2d
            GL.BindTexture(TextureTarget.Texture2D, textureId);

            var supportedPixelFormat = GetSupportedPixelFormat(bitmap.PixelFormat);
            //Lock pixel data to memory and prepare for pass through
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                                             System.Drawing.Imaging.ImageLockMode.ReadOnly,
                                             supportedPixelFormat);

            var glPixelFormat = ToGlPixelFormat(supportedPixelFormat);

            GL.TexImage2D(TextureTarget.Texture2D, 0, glPixelFormat, bitmap.Width, bitmap.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);

            //Release from memory
            bitmap.UnlockBits(bitmapData);

            var texture = new Texture(textureId);
            texture.Width = bitmap.Width;
            texture.Height = bitmap.Height;

            return texture;
        }

        public void Dispose()
        {
            GL.DeleteTexture(Id);
        }

        #endregion Public Methods
    }
}