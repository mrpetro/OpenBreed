using OpenBreed.Core.Components;
using OpenBreed.Core.Modules.Physics.Builders;

namespace OpenBreed.Core.Modules.Rendering.Components
{
    /// <summary>
    /// Axis-aligned tile render component with same height and width
    /// </summary>
    public class TileComponent : IEntityComponent
    {
        #region Internal Constructors

        internal TileComponent(TileComponentBuilder builder)
        {
            AtlasId = builder.AtlasId;
            ImageId = builder.ImageId;
            Order = builder.Order;
        }

        #endregion Internal Constructors

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
    }
}