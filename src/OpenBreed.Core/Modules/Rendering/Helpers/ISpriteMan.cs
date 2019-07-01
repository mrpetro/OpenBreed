namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    public interface ISpriteMan
    {
        #region Public Methods

        /// <summary>
        /// Get sprite atlas by it's id
        /// </summary>
        /// <param name="id">Id of sprite atlas to get</param>
        /// <returns>Sprite atlas object</returns>
        ISpriteAtlas GetById(int id);

        ISpriteAtlas Create(ITexture texture, int spriteWidth, int spriteHeight, int spriteColumns, int spriteRows);

        /// <summary>
        /// Unloads all textures
        /// </summary>
        void UnloadAll();

        #endregion Public Methods
    }
}