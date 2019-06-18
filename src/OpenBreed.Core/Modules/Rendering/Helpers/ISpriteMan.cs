namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    public interface ISpriteMan
    {
        #region Public Methods

        ISpriteAtlas GetById(int id);

        ISpriteAtlas Create(ITexture texture, int spriteWidth, int spriteHeight, int spriteColumns, int spriteRows);

        /// <summary>
        /// Unloads all textures
        /// </summary>
        void UnloadAll();

        #endregion Public Methods
    }
}