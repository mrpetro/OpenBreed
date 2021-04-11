using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Commands;
using OpenTK;

namespace OpenBreed.Wecs.Systems.Rendering.Commands
{
    public struct TileSetCommand : IEntityCommand
    {
        #region Public Fields

        public const string TYPE = "TILE_SET";

        #endregion Public Fields

        #region Public Constructors

        public TileSetCommand(int entityId, int atlasId, int imageId, Vector2 position)
        {
            EntityId = entityId;

            AtlasId = atlasId;
            ImageId = imageId;
            Position = position;
        }

        #endregion Public Constructors

        #region Public Properties

        public int EntityId { get; }
        public int AtlasId { get; }
        public int ImageId { get; }
        public Vector2 Position { get; }

        public string Name { get { return TYPE; } }

        #endregion Public Properties
    }
}