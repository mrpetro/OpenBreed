using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Entities;
using OpenTK;

namespace OpenBreed.Core.Modules.Rendering.Messages
{
    public struct TileSetMsg : IWorldMsg
    {
        #region Public Fields

        public const string TYPE = "TILE_SET";

        #endregion Public Fields

        #region Public Constructors

        public TileSetMsg(int worldId, int imageId, Vector2 position)
        {
            WorldId = worldId;
            ImageId = imageId;
            Position = position;
        }

        #endregion Public Constructors

        #region Public Properties

        public int WorldId { get; }
        public string Type { get { return TYPE; } }

        public Vector2 Position { get; }

        public int ImageId { get; }

        #endregion Public Properties
    }
}