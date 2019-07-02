namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    public interface ITileMan
    {
        #region Public Methods

        ITileAtlas GetById(int id);

        ITileAtlas Create(ITexture texture, int tileSize, int tileColumns, int tileRows);

        /// <summary>
        /// Unloads all textures
        /// </summary>
        void UnloadAll();

        #endregion Public Methods
    }
}