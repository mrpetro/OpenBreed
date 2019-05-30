using OpenBreed.Core.Modules;
using OpenBreed.Core.Modules.Rendering;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using System;
using System.Drawing;

namespace OpenBreed.Core.Modules.Rendering
{
    public class OpenGLModule : IRenderModule
    {
        #region Private Fields

        private TextureMan textureMan = new TextureMan();

        #endregion Private Fields

        #region Public Constructors

        public OpenGLModule(ICore core)
        {
            Core = core ?? throw new ArgumentNullException(nameof(core));

            Textures = new TextureMan();
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        public ITextureMan Textures { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Creates render system and return it
        /// </summary>
        /// <returns>Render system interface</returns>
        public IRenderSystem CreateRenderSystem(int gridWidth, int gridHeight)
        {
            return new RenderSystem(Core, gridWidth, gridHeight);
        }

        public IText CreateText(IFont font, string value = null)
        {
            return new Text(font, value);
        }

        public ISprite CreateSprite(ISpriteAtlas atlas, int imageId = 0)
        {
            return new Sprite(atlas, imageId);
        }

        public ITile CreateTile(ITileAtlas atlas, int imageId = 0)
        {
            return new Tile(atlas, imageId);
        }

        #endregion Public Methods
    }
}