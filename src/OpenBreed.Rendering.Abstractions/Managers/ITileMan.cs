using System.Drawing;

namespace OpenBreed.Rendering.Abstractions.Managers
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
        /// Unload all tile atlases from render context.
        /// </summary>
        /// <param name="renderContext">Render context</param>
        void UnloadAll(IRenderContext renderContext);

        #endregion Public Methods
    }
}