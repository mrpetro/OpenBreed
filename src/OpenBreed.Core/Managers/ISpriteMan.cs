using OpenBreed.Core.Modules.Rendering.Helpers;

namespace OpenBreed.Core.Managers
{
    /// <summary>
    /// Sprite manager interface
    /// </summary>
    public interface ISpriteMan
    {
        #region Public Methods

        /// <summary>
        /// Get sprite atlas by it's id
        /// </summary>
        /// <param name="id">Id of sprite atlas to get</param>
        /// <returns>Sprite atlas object</returns>
        ISpriteAtlas GetById(int id);

        /// <summary>
        /// Get sprite atlas by it's name
        /// </summary>
        /// <param name="name">Name of sprite atlas to get</param>
        /// <returns>Sprite atlas object</returns>
        ISpriteAtlas GetByName(string name);

        ISpriteAtlas Create(string name, int textureId, int spriteWidth, int spriteHeight, int spriteColumns, int spriteRows, int offsetX = 0, int offsetY = 0);

        /// <summary>
        /// Unloads all textures
        /// </summary>
        void UnloadAll();

        #endregion Public Methods
    }
}