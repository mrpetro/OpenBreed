using OpenBreed.Core.Modules.Rendering.Helpers;
using System.Drawing;

namespace OpenBreed.Core.Managers
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
        /// Get texture by user friendly alias
        /// </summary>
        /// <param name="alias">Alias name</param>
        /// <returns></returns>
        ITexture GetByAlias(string alias);

        /// <summary>
        /// Creates texture object from given bitmap and return it
        /// </summary>
        /// <param name="alias">Alias name to access the texture</param>
        /// <param name="bitmap">Bitmap to create texture from</param>
        /// <returns>ITexture object</returns>
        ITexture Create(string alias, Bitmap bitmap);

        /// <summary>
        /// Creates texture object from image file path and return it
        /// </summary>
        /// <param name="alias">Alias name to access the texture</param>
        /// <param name="filePath">File path to image file</param>
        /// <returns>ITexture object</returns>
        ITexture Create(string alias, string filePath);

        /// <summary>
        /// Unloads all textures
        /// </summary>
        void UnloadAll();

        #endregion Public Methods
    }
}