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
        /// Get texture by it's name
        /// </summary>
        /// <param name="name">Given name of texture</param>
        /// <returns>Return ITexture object if found, false otherwise</returns>
        ITexture GetByName(string name);

        /// <summary>
        /// Get texture object by it's ID
        /// </summary>
        /// <param name="id">Given ID of texture</param>
        /// <returns>Return ITexture object if found, false otherwise</returns>
        ITexture GetById(int id);

        /// <summary>
        /// Creates texture object from given bitmap and return it
        /// </summary>
        /// <param name="bitmap">Bitmap to create texture from</param>
        /// <param name="name">Obligatory name of texture to create</param>
        /// <returns>ITexture object</returns>
        ITexture Load(Bitmap bitmap, string name);

        /// <summary>
        /// Creates texture object from image file path and return it
        /// If Name parameter is not set, texture name will be set to file path
        /// </summary>
        /// <param name="bitmap">File path to image file</param>
        /// <param name="name">Optional name of texture to create</param>
        /// <returns>ITexture object</returns>
        ITexture Load(string filePath, string name = null);

        /// <summary>
        /// Unloads all textures
        /// </summary>
        void UnloadAll();

        #endregion Public Methods
    }
}