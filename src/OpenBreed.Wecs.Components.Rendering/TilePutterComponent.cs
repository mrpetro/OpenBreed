using OpenTK;

namespace OpenBreed.Wecs.Components.Rendering
{
    public class TilePutterComponent : IEntityComponent
    {
        #region Public Constructors

        public TilePutterComponent(int atlasId, int tileId, int layerNo, Vector2 position)
        {
            AtlasId = atlasId;
            TileId = tileId;
            LayerNo = layerNo;
            Position = position;
        }

        #endregion Public Constructors

        #region Public Properties

        public int AtlasId { get; }
        public int TileId { get; }
        public int LayerNo { get; }
        public Vector2 Position { get; }

        #endregion Public Properties
    }
}