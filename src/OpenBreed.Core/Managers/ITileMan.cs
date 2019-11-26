using OpenBreed.Core.Modules.Rendering.Components.Builders;
using OpenBreed.Core.Modules.Rendering.Helpers;

namespace OpenBreed.Core.Managers
{
    public interface ITileMan
    {
        #region Public Methods

        ITileAtlas GetById(int id);

        ITileAtlas GetByAlias(string alias);

        ITileAtlas Create(string atlas, int textureId, int tileSize, int tileColumns, int tileRows);

        /// <summary>
        /// Unloads all textures
        /// </summary>
        void UnloadAll();

        #endregion Public Methods
    }
}