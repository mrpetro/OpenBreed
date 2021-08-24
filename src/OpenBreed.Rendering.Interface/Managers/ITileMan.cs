using System.Drawing;

namespace OpenBreed.Rendering.Interface.Managers
{
    public interface ITileMan
    {
        #region Public Methods

        ITileAtlas GetById(int id);

        /// <summary>
        /// Checks if atlas with given name already exists
        /// </summary>
        /// <param name="atlasName">Name of atlas to check</param>
        /// <returns>True if exits, false otherwise</returns>
        bool Contains(string atlasName);

        ITileAtlas GetByAlias(string alias);

        /// <summary>
        /// Creates new sprite atlas
        /// </summary>
        /// <returns>Sprite atlas builder</returns>
        ITileAtlasBuilder CreateAtlas();

        /// <summary>
        /// Unloads all textures
        /// </summary>
        void UnloadAll();

        /// <summary>
        /// Render particular tile giving it's atlas ID, image ID and index position
        /// </summary>
        /// <param name="atlasId">Atlas ID of rendered tile</param>
        /// <param name="imageId">Image ID of rendered tile</param>
        /// <param name="xIndex">X index coordinate</param>
        /// <param name="yIndex">Y index coordinate</param>
        void Render(int atlasId, int imageId, int xIndex, int yIndex);

        #endregion Public Methods
    }
}