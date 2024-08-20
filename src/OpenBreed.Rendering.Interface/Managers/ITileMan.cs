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

        ITileAtlas GetByName(string alias);

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
        /// Render particular tile giving it's atlas and image ID
        /// </summary>
        /// <param name="atlasId">Atlas ID of rendered tile</param>
        /// <param name="imageId">Image ID of rendered tile</param>
        void Render(IRenderView view, int atlasId, int imageId);

        #endregion Public Methods
    }
}