﻿using OpenBreed.Rendering.Interface;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace OpenBreed.Rendering.OpenGL.Helpers
{
    public class Texture : ITexture, IDisposable
    {
        #region Public Constructors

        public Texture(int id)
        {
            InternalId = id;
        }

        #endregion Public Constructors

        #region Public Properties

        public TextureDataMode DataMode { get; private set; }
        public int Height { get; private set; }
        public int Id { get; internal set; }
        public int InternalId { get; }
        public int Width { get; private set; }

        public int MaskIndex { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public static Texture CreateFromBitmap(Bitmap bitmap)
        {
            OpenTK.Graphics.OpenGL4.PixelFormat pixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat.Bgra;
            PixelType pixelType = PixelType.UnsignedByte;
            System.Drawing.Imaging.PixelFormat supportedPixelFormat = GetSupportedPixelFormat(bitmap.PixelFormat);
            PixelInternalFormat internalPixelFormat = ToGlPixelFormat(supportedPixelFormat);

            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                                      ImageLockMode.ReadOnly,
                                      supportedPixelFormat);

            // Generate handle
            int textureId = GL.GenTexture();

            // Bind the handle
            GL.BindTexture(TextureTarget.Texture2D, textureId);

            GL.TexImage2D(TextureTarget.Texture2D, 0, internalPixelFormat, bitmap.Width, bitmap.Height, 0, pixelFormat, pixelType, bitmapData.Scan0);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);

            //Release from memory
            bitmap.UnlockBits(bitmapData);

            var texture = new Texture(textureId);
            texture.Width = bitmap.Width;
            texture.Height = bitmap.Height;
            texture.DataMode = TextureDataMode.Rgba;

            return texture;
        }

        public static Texture CreateFromIndexArray(int width, int height, byte[] data, int maskIndex = -1)
        {
            PixelInternalFormat internalPixelFormat = PixelInternalFormat.R8ui;
            OpenTK.Graphics.OpenGL4.PixelFormat pixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat.RedInteger;
            PixelType pixelType = PixelType.UnsignedByte;

            // Generate handle
            int textureId = GL.GenTexture();

            // Bind the handle
            GL.BindTexture(TextureTarget.Texture2D, textureId);

            GL.TexImage2D(TextureTarget.Texture2D, 0, internalPixelFormat, width, height, 0, pixelFormat, pixelType, data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);

            var texture = new Texture(textureId);
            texture.Width = width;
            texture.Height = height;
            texture.DataMode = TextureDataMode.Index;
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

        public void Dispose()
        {
            GL.DeleteTexture(InternalId);
        }

        public void Use(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, InternalId);
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

        #endregion Private Methods
    }
}