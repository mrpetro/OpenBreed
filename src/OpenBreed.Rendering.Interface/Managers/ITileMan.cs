using System.Drawing;

namespace OpenBreed.Rendering.Interface.Managers
{
    public interface ITileMan
    {
        #region Public Methods

        ITileAtlas GetById(int id);

        ITileAtlas GetByAlias(string alias);

        ITileAtlas Create(string atlas, int textureId, int tileSize, Point[] points);

        ITileAtlas Create(string atlas, int textureId, int tileSize, int tileColumns, int tileRows);

        /// <summary>
        /// Unloads all textures
        /// </summary>
        void UnloadAll();

        #endregion Public Methods
    }
}