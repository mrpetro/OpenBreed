namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    /// <summary>
    /// Interface for accessing tile atlas
    /// </summary>
    public interface ITileAtlas
    {
        #region Public Properties

        /// <summary>
        /// Id of this tile atlas
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Atlas tile size
        /// </summary>
        float TileSize { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Draw tile with given image Id
        /// </summary>
        /// <param name="imageId">Atlas image id to draw</param>
        void Draw(int imageId);

        #endregion Public Methods
    }
}