namespace OpenBreed.Rendering.Interface
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
    }
}