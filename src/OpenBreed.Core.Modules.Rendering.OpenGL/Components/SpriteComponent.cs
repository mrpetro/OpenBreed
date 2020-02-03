using OpenBreed.Core.Common.Systems.Components;

namespace OpenBreed.Core.Modules.Rendering.Components
{
    /// <summary>
    /// Axis-aligned sprite render component
    /// Shared components:
    ///  - axis-aligned box shape
    ///  - position
    /// </summary>
    public class SpriteComponent : IEntityComponent
    {
        #region Private Constructors

        private SpriteComponent(int atlasId, int imageId, float order)
        {
            AtlasId = atlasId;
            ImageId = ImageId;
            Order = order;
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

        /// <summary>
        /// Order of drawing, higher value object is rendered on top of lower value objects
        /// </summary>
        public float Order { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Create sprite render component using given sprite atlas
        /// </summary>
        /// <param name="atlasId">Id of sprite atlas to use for this sprite component</param>
        /// <param name="imageId">Optional initial sprite atlas image id</param>
        /// <param name="order">Optional initial object rendering order</param>
        /// <returns>Sprite component</returns>
        public static SpriteComponent Create(int atlasId, int imageId = 0, float order = 0.0f)
        {
            return new SpriteComponent(atlasId, imageId, order);
        }

        #endregion Public Methods
    }
}