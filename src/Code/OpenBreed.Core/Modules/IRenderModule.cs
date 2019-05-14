using OpenBreed.Core.Systems;

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
        /// Get the texture given by image in the filePath
        /// </summary>
        /// <param name="filePath">File path to the image</param>
        /// <returns>Texture interface</returns>
        ITexture GetTexture(string filePath);

        #endregion Public Methods
    }
}