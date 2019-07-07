using System.Drawing;

namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    /// <summary>
    /// Textures manager interface  which handles creating textures from various sources
    /// Or just retrieving them by Id.
    /// </summary>
    public interface ITextureMan
    {
        #region Public Methods

        /// <summary>
        /// Get texture by it's Id
        /// </summary>
        /// <param name="name">Given Id of texture</param>
        /// <returns>Return ITexture object if found, false otherwise</returns>
        ITexture GetById(int id);

        /// <summary>
        /// Creates texture object from given bitmap and return it
        /// </summary>
        /// <param name="bitmap">Bitmap to create texture from</param>
        /// <returns>ITexture object</returns>
        ITexture Create(Bitmap bitmap);

        /// <summary>
        /// Creates texture object from image file path and return it
        /// </summary>
        /// <param name="filePath">File path to image file</param>
        /// <returns>ITexture object</returns>
        ITexture Create(string filePath);

        /// <summary>
        /// Unloads all textures
        /// </summary>
        void UnloadAll();

        #endregion Public Methods
    }
}