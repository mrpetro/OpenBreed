using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Entities;

namespace OpenBreed.Core.Modules.Rendering.Messages
{
    public struct TileSetMsg : IEntityMsg
    {
        #region Public Fields

        public const string TYPE = "TILE_SET";

        #endregion Public Fields

        #region Public Constructors

        public TileSetMsg(IEntity entity, int imageId)
        {
            Entity = entity;
            //TileId = tileId;
            //AtlasId = atlasId;
            ImageId = imageId;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; }
        public string Type { get { return TYPE; } }
        //public int TileId { get; }
        //public int AtlasId { get; }
        public int ImageId { get; }

        #endregion Public Properties
    }
}