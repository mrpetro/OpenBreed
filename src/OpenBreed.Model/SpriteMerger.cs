using OpenBreed.Model.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Model
{
    /// <summary>
    /// Sprite merger implementation
    /// Uses biggest sprite size in input collection as offset between all sprite bounds in resulting data
    /// </summary>
    public class SpriteMerger : ISpriteMerger
    {
        #region Public Constructors

        public SpriteMerger()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public void Merge(List<SpriteModel> sprites, out byte[] outData, out int width, out int height, out List<(int X, int Y, int Width, int Height)> bounds)
        {
            var totalNo = sprites.Count;
            var maxWidth = sprites.Max(sprite => sprite.Width);
            var maxHeight = sprites.Max(sprite => sprite.Height);

            var noX = (int)Math.Sqrt(totalNo);
            var rest = totalNo % noX;
            var noY = totalNo / noX + rest;

            width = noX * maxWidth;
            height = noY * maxHeight;
            outData = new byte[width * height];
            bounds = new List<(int X, int Y, int Width, int Height)>();

            for (int j = 0; j < noY; j++)
            {
                var dy = j * maxHeight;

                for (int i = 0; i < noX; i++)
                {
                    var dx = i * maxWidth;
                    var srcIndex = i + noX * j;

                    if (srcIndex >= totalNo)
                        return;

                    var src = sprites[srcIndex];

                    Copy(src.Data, src.Width, src.Height, outData, width, height, dx, dy);
                    bounds.Add((dx, dy, src.Width, src.Height));
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void Copy(
            byte[] srcData,
            int srcWidth,
            int srcHeight,
            byte[] dstData,
            int dstWidth,
            int dstHeight,
            int dstX,
            int dstY)
        {
            for (int i = 0; i < srcHeight; i++)
            {
                Array.Copy(srcData, i * srcWidth, dstData, dstX + dstWidth * (dstY + i), srcWidth);
            }
        }

        #endregion Private Methods
    }
}