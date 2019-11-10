namespace OpenBreed.Core.Modules.Rendering.Components
{
    /// <summary>
    /// Axis-aligned sprite render component
    /// Shared components:
    ///  - axis-aligned box shape
    ///  - position
    /// </summary>
    public class SpriteComponent : ISpriteComponent
    {
        #region Private Constructors

        private SpriteComponent(int atlasId, int imageId)
        {
            AtlasId = atlasId;
            this.ImageId = ImageId;
        }

        #endregion Private Constructors

        #region Public Properties

        /// <summary>
        /// Id of sprite atlas
        /// </summary>
        public int AtlasId { get; set; }

        /// <summary>
        /// Id of sprite image from the atlas
        /// </summary>
        public int ImageId { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Create sprite render component using given sprite atlas
        /// </summary>
        /// <param name="atlasId">Id of sprite atlas to use for this sprite component</param>
        /// <param name="imageId">Optiona initial sprite atlas image id</param>
        /// <returns>Sprite component</returns>
        public static SpriteComponent Create(int atlasId, int imageId = 0)
        {
            return new SpriteComponent(atlasId, imageId);
        }

        #endregion Public Methods
    }
}