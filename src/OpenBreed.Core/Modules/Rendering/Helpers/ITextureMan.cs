using System.Drawing;

namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    /// <summary>
    /// Textures manager interface  which handles creating textures from various sources
    /// Or just retrieving them by ID or name.
    /// </summary>
    public interface ITextureMan
    {
        #region Public Methods

        /// <summary>
        /// Get texture by it's ID
        /// </summary>
        /// <param name="name">Given ID of texture</param>
        /// <returns>Return ITexture object if found, false otherwise</returns>
        ITexture GetById(string name);

        /// <summary>
        /// Creates texture object from given bitmap and return it
        /// </summary>
        /// <param name="bitmap">Bitmap to create texture from</param>
        /// <param name="id">Obligatory ID of texture to create</param>
        /// <returns>ITexture object</returns>
        ITexture Load(Bitmap bitmap, string id);

        /// <summary>
        /// Creates texture object from image file path and return it
        /// If id parameter is not set, texture id will be set to file path
        /// </summary>
        /// <param name="filePath">File path to image file</param>
        /// <param name="id">Optional id of texture to create</param>
        /// <returns>ITexture object</returns>
        ITexture Load(string filePath, string id = null);

        /// <summary>
        /// Unloads all textures
        /// </summary>
        void UnloadAll();

        #endregion Public Methods
    }
}