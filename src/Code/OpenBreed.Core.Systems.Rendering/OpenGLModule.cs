using OpenBreed.Core.Modules;
using System;
using System.Drawing;

namespace OpenBreed.Core.Systems.Rendering
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

        #endregion Public Methods
    }
}