using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Modules.Rendering.Systems;
using OpenBreed.Core.Systems;
using OpenTK;
using System.Drawing;

namespace OpenBreed.Core.Modules.Rendering
{
    /// <summary>
    /// Core module interface specialized in graphical rendering related work
    /// </summary>
    public interface IRenderModule : ICoreModule
    {
        #region Public Methods

        ///// <summary>
        ///// Creates render system and return it
        ///// </summary>
        ///// <param name="gridWidth">Tiles grid width size</param>
        ///// <param name="gridHeight">Tiles grid height size</param>
        ///// <param name="tileSize">Grid tile size</param>
        ///// <returns>Render system interface</returns>
        //IRenderSystem CreateRenderSystem(int gridWidth, int gridHeight, float tileSize);

        /// <summary>
        /// Create system for handling tiles
        /// </summary>
        /// <returns>Tile system</returns>
        IWorldSystem CreateTileSystem(int gridWidth, int gridHeight, float tileSize);

        /// <summary>
        /// Create system for handling sprites
        /// </summary>
        /// <returns>Sprite system</returns>
        IWorldSystem CreateSpriteSystem();

        /// <summary>
        /// Create system for handling texts
        /// </summary>
        /// <returns>Text system</returns>
        IWorldSystem CreateTextSystem();

        /// <summary>
        /// Creates text component using given font
        /// </summary>
        /// <param name="fontId">Id of font to use for this text component</param>
        /// <param name="offset">Offset position from position component</param>
        /// <param name="value">Optional initial text value</param>
        /// <returns>Text component</returns>
        IText CreateText(int fontId, Vector2 offset, string value = null);

        /// <summary>
        /// Create sprite component using given sprite atlas
        /// </summary>
        /// <param name="atlasId">Id of sprite atlas to use for this sprite component</param>
        /// <param name="imageId">Optiona initial sprite atlas image id</param>
        /// <returns>Sprite component</returns>
        ISprite CreateSprite(int atlasId, int imageId = 0);

        /// <summary>
        /// Create tile component using given tile atlas
        /// </summary>
        /// <param name="atlasId">Id of tile atlas to use for this tile component</param>
        /// <param name="imageId">Optiona initial tile atlas image id</param>
        /// <returns>Sprite component</returns>
        ITile CreateTile(int atlasId, int imageId = 0);

        /// <summary>
        /// Textures manager
        /// </summary>
        ITextureMan Textures { get; }

        /// <summary>
        /// Sprite manager
        /// </summary>
        ISpriteMan Sprites { get; }

        /// <summary>
        /// Tile manager
        /// </summary>
        ITileMan Tiles { get; }

        /// <summary>
        /// Font manager
        /// </summary>
        IFontMan Fonts { get; }

        /// <summary>
        /// Viewports manager
        /// </summary>
        IViewportMan Viewports { get; }

        void Cleanup();


        void Draw(float dt);

        #endregion Public Methods
    }
}