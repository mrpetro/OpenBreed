using OpenBreed.Core.Systems;
using System.Drawing;

namespace OpenBreed.Core.Modules
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
        /// Textures manager
        /// </summary>
        ITextureMan Textures { get; }

        #endregion Public Methods
    }
}