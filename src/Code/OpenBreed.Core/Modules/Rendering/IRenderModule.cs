using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Systems;
using System.Drawing;

namespace OpenBreed.Core.Modules.Rendering
{
    /// <summary>
    /// Core module interface specialized in graphical rendering related work
    /// </summary>
    public interface IRenderModule : ICoreModule
    {
        #region Public Methods

        /// <summary>
        /// Creates render system and return it
        /// </summary>
        /// <param name="gridWidth">Tiles grid width size</param>
        /// <param name="gridHeight">Tiles grid height size</param>
        /// <returns>Render system interface</returns>
        IRenderSystem CreateRenderSystem(int gridWidth, int gridHeight);

        /// <summary>
        /// Creates text component using given font
        /// </summary>
        /// <param name="font">IFont object to use for this text component</param>
        /// <param name="value">Optional initial text value</param>
        /// <returns>Text component</returns>
        IText CreateText(IFont font, string value = null);

        /// <summary>
        /// Create sprite component using given sprite atlas
        /// </summary>
        /// <param name="spriteAtlas">Sprite atlas to use fr this sprite component</param>
        /// <param name="imageId">Optiona initial sprite atlas image id</param>
        /// <returns>Sprite component</returns>
        ISprite CreateSprite(ISpriteAtlas spriteAtlas, int imageId = 0);

        /// <summary>
        /// Create tile component using given tile atlas
        /// </summary>
        /// <param name="tileAtlas">Tile atlas to use fr this tile component</param>
        /// <param name="imageId">Optiona initial tile atlas image id</param>
        /// <returns>Sprite component</returns>
        ITile CreateTile(ITileAtlas tileAtlas, int imageId = 0);

        /// <summary>
        /// Textures manager
        /// </summary>
        ITextureMan Textures { get; }

        #endregion Public Methods
    }
}