using OpenBreed.Common.Interface.Tools;
using OpenBreed.Rendering.Abstractions;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace OpenBreed.Rendering.OpenGL.Helpers
{
    public class Texture : ITexture
    {
        #region Private Fields

        private readonly RenderContextItem<int> contextItem = new RenderContextItem<int>();

        #endregion Private Fields

        #region Public Constructors

        public Texture()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public TextureDataMode DataMode { get; private set; }
        public int Height { get; private set; }
        public byte[] Data { get; private set; }
        public int Id { get; internal set; }
        public int Width { get; private set; }

        public int MaskIndex { get; private set; }

        #endregion Public Properties

        #region Internal Properties

        internal OpenTK.Graphics.OpenGL4.PixelInternalFormat InternalPixelFormat { get; private set; }
        internal OpenTK.Graphics.OpenGL4.PixelFormat PixelFormat { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public static byte[] ToBytes(Bitmap bmp, System.Drawing.Imaging.PixelFormat pixelFormat)
        {
            var pixelSize = Image.GetPixelFormatSize(pixelFormat) / 8;

            var bytes = new byte[bmp.Width * bmp.Height * pixelSize];
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, pixelFormat);
            Marshal.Copy(bmpData.Scan0, bytes, 0, bytes.Length);
            bmp.UnlockBits(bmpData);
            return bytes;
        }

        public static Texture CreateFromBitmap(Bitmap bitmap)
        {
            Debug.Assert(ThreadTools.IsMainThread, "Called on non-main thread!");

            var supportedPixelFormat = GetSupportedPixelFormat(bitmap.PixelFormat);

            var data = ToBytes(bitmap, supportedPixelFormat);

            var texture = new Texture();

            texture.Width = bitmap.Width;
            texture.Height = bitmap.Height;
            texture.Data = data;
            texture.DataMode = TextureDataMode.Rgba;
            texture.InternalPixelFormat = ToGlPixelFormat(supportedPixelFormat);
            texture.PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat.Bgra;

            return texture;
        }

        public static Texture CreateFromIndexArray(int width, int height, byte[] data, int maskIndex = -1)
        {
            Debug.Assert(ThreadTools.IsMainThread, "Called on non-main thread!");

            var texture = new Texture();
            texture.Width = width;
            texture.Height = height;
            texture.Data = data;
            texture.DataMode = TextureDataMode.Index;
            texture.InternalPixelFormat = PixelInternalFormat.R8ui;
            texture.PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat.RedInteger;
            texture.MaskIndex = maskIndex;

            return texture;
        }

        public static Texture LoadFromFile(string path)
        {
            using (var image = new Bitmap(path))
                return CreateFromBitmap(image);
        }

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

        public void Unload(IRenderContext renderContext)
        {
            var glId = contextItem.Get(renderContext);

            GL.DeleteTexture(glId);
        }

        public void Use(IRenderContext renderContext)
        {
            var glId = contextItem.GetOrAdd(renderContext, LoadTexture);

            GL.BindTexture(TextureTarget.Texture2D, glId);
        }

        public void Use(IRenderContext renderContext, TextureUnit unit)
        {
            var glId = contextItem.Get(renderContext);

            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, glId);
        }

        #endregion Public Methods

        #region Private Methods

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

        private int LoadTexture(IRenderContext renderContext)
        {
            var pixelType = PixelType.UnsignedByte;

            // Generate handle
            int textureId = GL.GenTexture();

            // Bind the handle
            GL.BindTexture(TextureTarget.Texture2D, textureId);

            GL.TexImage2D(TextureTarget.Texture2D, 0, InternalPixelFormat, Width, Height, 0, PixelFormat, pixelType, Data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);

            return textureId;
        }

        #endregion Private Methods
    }
}