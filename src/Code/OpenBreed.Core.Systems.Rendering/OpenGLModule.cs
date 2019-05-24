using OpenBreed.Core.Modules;
using System;

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
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

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

        /// <summary>
        /// Get the texture given by image in the filePath
        /// </summary>
        /// <param name="filePath">File path to the image</param>
        /// <returns>Texture interface</returns>
        public ITexture GetTexture(string filePath)
        {
            return textureMan.Load(filePath);
        }

        #endregion Public Methods
    }
}