namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    /// <summary>
    /// Tile system cell data
    /// </summary>
    internal class TileCell
    {
        #region Private Constructors

        private TileCell(int atlasId, int imageId)
        {
            AtlasId = atlasId;
            ImageId = imageId;
        }

        #endregion Private Constructors

        #region Public Properties

        /// <summary>
        /// Id of tile atlas
        /// </summary>
        public int AtlasId { get; set; }

        /// <summary>
        /// Id of tile image from the atlas
        /// </summary>
        public int ImageId { get; set; }

        public bool IsEmpty { get { return AtlasId == -1; } }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Create TileCell using given tile atlas
        /// </summary>
        /// <param name="atlasId">Id of tile atlas to use for this TileCell</param>
        /// <param name="imageId">Optiona initial tile atlas image id</param>
        /// <returns>Tile component</returns>
        public static TileCell Create(int atlasId = -1, int imageId = 0)
        {
            return new TileCell(atlasId, imageId);
        }

        #endregion Public Methods
    }
}