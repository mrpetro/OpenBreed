namespace OpenBreed.Rendering.Interface
{
    /// <summary>
    /// Interface for accessing sprite atlas
    /// </summary>
    public interface ISpriteAtlas
    {
        #region Public Properties

        /// <summary>
        /// Id of this sprite atlas
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Atlas sprite width
        /// </summary>
        float SpriteWidth { get; }

        /// <summary>
        /// Atlas sprite height
        /// </summary>
        float SpriteHeight { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Draw sprite with given image Id
        /// </summary>
        /// <param name="imageId">Atlas image id to draw</param>
        void Draw(int imageId);

        #endregion Public Methods
    }
}