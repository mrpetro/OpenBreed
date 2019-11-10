namespace OpenBreed.Core.Modules.Rendering.Components
{
    /// <summary>
    /// Axis-aligned tile render component with same height and width
    /// </summary>
    public class TileComponent : ITileComponent
    {
        #region Private Constructors

        private TileComponent(int atlasId, int imageId)
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

        public bool IsEmpty { get { return ImageId == 0 && AtlasId == 0; } }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Create tile render component using given tile atlas
        /// </summary>
        /// <param name="atlasId">Id of tile atlas to use for this tile component</param>
        /// <param name="imageId">Optiona initial tile atlas image id</param>
        /// <returns>Tile component</returns>
        public static TileComponent Create(int atlasId, int imageId = 0)
        {
            return new TileComponent(atlasId, imageId);
        }

        #endregion Public Methods
    }
}