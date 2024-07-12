using OpenBreed.Model.Sprites;
using System.Collections.Generic;

namespace OpenBreed.Model
{
    /// <summary>
    /// Interface for merging sprites into single sprite bitmap with collection of sprite bounds
    /// </summary>
    public interface ISpriteMerger
    {
        #region Public Methods

        void Merge(List<SpriteModel> sprites, out byte[] outData, out int width, out int height, out List<(int X, int Y, int Width, int Height)> bounds);

        #endregion Public Methods
    }
}