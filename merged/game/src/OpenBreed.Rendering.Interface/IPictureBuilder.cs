namespace OpenBreed.Rendering.Interface
{
    /// <summary>
    /// Interface for picture builder
    /// </summary>
    public interface IPictureBuilder
    {
        #region Public Methods

        /// <summary>
        /// Sets created picture name
        /// </summary>
        /// <param name="name">Proposed picture name</param>
        /// <returns>This builder instance</returns>
        IPictureBuilder SetName(string name);

        /// <summary>
        /// Sets picture texture
        /// </summary>
        /// <param name="textureId">Texture ID</param>
        /// <returns>This builder instance</returns>
        IPictureBuilder SetTexture(int textureId);

        /// <summary>
        /// Build picture
        /// </summary>
        /// <returns>Picture object</returns>
        IPicture Build();


        #endregion Public Methods
    }
}