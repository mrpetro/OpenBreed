using OpenTK;

namespace OpenBreed.Wecs.Components.Rendering
{
    public class StampPutterComponent : IEntityComponent
    {
        #region Public Constructors

        public StampPutterComponent(int stampId, int layerNo, Vector2 position)
        {
            StampId = stampId;
            LayerNo = layerNo;
            Position = position;
        }

        #endregion Public Constructors

        #region Public Properties

        public int StampId { get; }
        public int LayerNo { get; }
        public Vector2 Position { get; }

        #endregion Public Properties
    }
}