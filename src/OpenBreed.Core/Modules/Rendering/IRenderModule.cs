using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Modules.Rendering.Systems;
using OpenTK;
using OpenTK.Graphics;

namespace OpenBreed.Core.Modules.Rendering
{
    /// <summary>
    /// Core module interface specialized in graphical rendering related work
    /// </summary>
    public interface IRenderModule : ICoreModule
    {
        #region Public Properties

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

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Create system for handling wireframes
        /// </summary>
        /// <returns>Wireframe system</returns>
        IWireframeSystem CreateWireframeSystem();

        /// <summary>
        /// Create system for handling tiles
        /// </summary>
        /// <param name="gridWidth">Tile grid width</param>
        /// <param name="gridHeight">Tile grid height</param>
        /// <param name="tileSize">Grid tile size</param>
        /// <param name="gridVisible">Grid visibility flag for debugging purpose</param>
        /// <returns>Tile system object</returns>
        ITileSystem CreateTileSystem(int gridWidth, int gridHeight, float tileSize, bool gridVisible = false);

        /// <summary>
        /// Create system for handling sprites
        /// </summary>
        /// <returns>Sprite system</returns>
        ISpriteSystem CreateSpriteSystem();

        /// <summary>
        /// Create system for handling texts
        /// </summary>
        /// <returns>Text system</returns>
        ITextSystem CreateTextSystem();

        /// <summary>
        /// Creates text render component using given font
        /// </summary>
        /// <param name="fontId">Id of font to use for this text component</param>
        /// <param name="offset">Offset position from position component</param>
        /// <param name="value">Optional initial text value</param>
        /// <returns>Text component</returns>
        IText CreateText(int fontId, Vector2 offset, string value = null);

        /// <summary>
        /// Create wireframe render component
        /// </summary>
        /// <param name="thickness">Thickness of wireframe lines</param>
        /// <param name="color">Color of wireframe lines</param>
        /// <returns></returns>
        IWireframe CreateWireframe(float thickness, Color4 color);

        /// <summary>
        /// Create sprite render component using given sprite atlas
        /// </summary>
        /// <param name="atlasId">Id of sprite atlas to use for this sprite component</param>
        /// <param name="imageId">Optiona initial sprite atlas image id</param>
        /// <returns>Sprite component</returns>
        ISprite CreateSprite(int atlasId, int imageId = 0);

        /// <summary>
        /// Create tile render component using given tile atlas
        /// </summary>
        /// <param name="atlasId">Id of tile atlas to use for this tile component</param>
        /// <param name="imageId">Optiona initial tile atlas image id</param>
        /// <returns>Sprite component</returns>
        ITile CreateTile(int atlasId, int imageId = 0);

        void Cleanup();

        void Draw(float dt);

        #endregion Public Methods
    }
}