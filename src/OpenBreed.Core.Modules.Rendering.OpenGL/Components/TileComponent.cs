namespace OpenBreed.Core.Modules.Rendering.Components
{
    /// <summary>
    /// Axis-aligned tile render component with same height and width
    /// </summary>
    public class TileComponent : ITileComponent
    {
        #region Private Constructors

        private TileComponent(int atlasId, int imageId, float order)
        {
            AtlasId = atlasId;
            ImageId = imageId;
            Order = order;
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

        /// <summary>
        /// Order of drawing, higher value object is rendered on top of lower value objects
        /// </summary>
        public float Order { get; set; }

        public bool IsEmpty { get { return ImageId == 0 && AtlasId == 0; } }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Create tile render component using given tile atlas
        /// </summary>
        /// <param name="atlasId">Id of tile atlas to use for this tile component</param>
        /// <param name="imageId">Optiona initial tile atlas image id</param>
        /// <param name="order">Optional initial object rendering order</param>
        /// <returns>Tile component</returns>
        public static TileComponent Create(int atlasId, int imageId = 0, float order = 0.0f)
        {
            return new TileComponent(atlasId, imageId, order);
        }

        #endregion Public Methods
    }
}