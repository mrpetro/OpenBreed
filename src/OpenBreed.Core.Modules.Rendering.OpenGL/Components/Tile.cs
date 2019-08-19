namespace OpenBreed.Core.Modules.Rendering.Components
{
    /// <summary>
    /// Axis-aligned tile render component with same height and width
    /// </summary>
    public class Tile : ITile
    {
        #region Public Constructors

        public Tile()
        {
        }

        #endregion Public Constructors

        #region Private Constructors

        private Tile(int atlasId, int imageId)
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

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Create tile render component using given tile atlas
        /// </summary>
        /// <param name="atlasId">Id of tile atlas to use for this tile component</param>
        /// <param name="imageId">Optiona initial tile atlas image id</param>
        /// <returns>Tile component</returns>
        public static Tile Create(int atlasId, int imageId = 0)
        {
            return new Tile(atlasId, imageId);
        }

        #endregion Public Methods
    }
}