using OpenBreed.Common;
using OpenBreed.Rendering.Interface.Managers;

namespace OpenBreed.Wecs.Components.Rendering
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

    public class TileComponentBuilder : IBuilder<TileComponent>
    {
        #region Internal Fields

        internal int AtlasId;
        internal int ImageId;
        internal float Order;

        #endregion Internal Fields

        #region Private Fields

        private readonly ITileMan tileMan;

        #endregion Private Fields

        #region Internal Constructors

        internal TileComponentBuilder(ITileMan tileMan)
        {
            this.tileMan = tileMan;
        }

        #endregion Internal Constructors

        #region Public Methods

        public TileComponent Build()
        {
            return new TileComponent(this);
        }

        public void SetAtlasById(int atlasId)
        {
            AtlasId = atlasId;
        }

        public void SetAtlasByName(string atlasName)
        {
            var atlas = tileMan.GetByAlias(atlasName);

            SetAtlasById(atlas.Id);
        }

        public void SetImageIndex(int imageIndex)
        {
            ImageId = imageIndex;
        }

        #endregion Public Methods
    }
}