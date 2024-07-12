using OpenTK;
using System;

namespace OpenBreed.Rendering.Interface.Managers
{
    /// <summary>
    /// Picture manager interface
    /// </summary>
    public interface IPictureMan
    {
        #region Public Methods

        /// <summary>
        /// Get picture by it's ID
        /// </summary>
        /// <param name="pictureId">Id of picture to get</param>
        /// <returns>Picture object</returns>
        IPicture GetById(int pictureId);

        /// <summary>
        /// Get picture by it's name
        /// </summary>
        /// <param name="pictureName">Name of picture to get</param>
        /// <returns>Picture object</returns>
        IPicture GetByName(string pictureName);

        /// <summary>
        /// Get picture name based on it's ID
        /// </summary>
        /// <param name="pictureId">ID of picture</param>
        /// <returns>Picture name</returns>
        string GetName(int pictureId);

        /// <summary>
        /// Checks if picture given name already exists
        /// </summary>
        /// <param name="pictureName">Name of picture to check</param>
        /// <returns>True if exits, false otherwise</returns>
        bool Contains(string pictureName);

        /// <summary>
        /// Creates new picture
        /// </summary>
        /// <returns>Picture builder</returns>
        IPictureBuilder CreatePicture();

        /// <summary>
        /// Unloads all textures
        /// </summary>
        void UnloadAll();

        #endregion Public Methods
    }
}