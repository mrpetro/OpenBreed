namespace OpenBreed.Rendering.Abstractions
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
        /// Load this tile atlas into render context
        /// </summary>
        /// <param name="renderContext">Render context</param>
        void Load(IRenderContext renderContext);

        #endregion Public Methods
    }
}