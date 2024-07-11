using OpenTK;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Components.Rendering
{
    public class StampData
    {
        #region Public Constructors

        public StampData(int stampId, int layerNo, Vector2 position)
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

    public class StampPutterComponent : IEntityComponent
    {
        #region Public Constructors

        public StampPutterComponent()
        {
            Items = new List<StampData>();
        }

        public StampPutterComponent(int stampId, int layerNo, Vector2 position)
        {
            Items = new List<StampData>();
            Items.Add(new StampData(stampId, layerNo, position));
        }

        #endregion Public Constructors

        #region Public Properties

        public List<StampData> Items { get; }

        #endregion Public Properties
    }
}