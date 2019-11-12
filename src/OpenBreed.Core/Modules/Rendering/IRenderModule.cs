using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Common.Systems.Components;
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
        /// Get current rendering frames per second
        /// </summary>
        float Fps { get; }

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
        /// Stamp manager
        /// </summary>
        IStampMan Stamps { get; }

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
        /// <param name="layersNo">Number of tile layers</param>
        /// <param name="tileSize">Grid tile size</param>
        /// <param name="gridVisible">Grid visibility flag for debugging purpose</param>
        /// <returns>Tile system object</returns>
        ITileSystem CreateTileSystem(int gridWidth, int gridHeight, int layersNo, float tileSize, bool gridVisible = false);

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
        /// Create wireframe render component
        /// </summary>
        /// <param name="thickness">Thickness of wireframe lines</param>
        /// <param name="color">Color of wireframe lines</param>
        /// <returns></returns>
        IWireframe CreateWireframe(float thickness, Color4 color);

        /// <summary>
        /// Create sprite entity component
        /// </summary>
        /// <param name="spriteAlias">Sprite atlas alias to use</param>
        /// <param name="order">Initial order of rendering for this sprite</param>
        /// <returns>Sprite entity component</returns>
        ISpriteComponent CreateSprite(string spriteAlias, float order = 0.0f);

        /// <summary>
        /// Create tile entity component
        /// </summary>
        /// <param name="tileAlias">Tile atlas alias to use</param>
        /// <returns>Tile entity component</returns>
        ITileComponent CreateTile(string tileAtlas);

        void Cleanup();

        void Draw(float dt);

        #endregion Public Methods
    }
}