using OpenBreed.Core.Commands;
using OpenBreed.Core.Entities;
using OpenTK;

namespace OpenBreed.Core.Modules.Rendering.Commands
{
    public struct TileSetCommand : ICommand
    {
        #region Public Fields

        public const string TYPE = "TILE_SET";

        #endregion Public Fields

        #region Public Constructors

        public TileSetCommand(int worldId, int atlasId, int imageId, Vector2 position)
        {
            WorldId = worldId;

            AtlasId = atlasId;
            ImageId = imageId;
            Position = position;
        }

        #endregion Public Constructors

        #region Public Properties

        public int WorldId { get; }
        public int AtlasId { get; }
        public int ImageId { get; }
        public Vector2 Position { get; }

        public string Name { get { return TYPE; } }

        #endregion Public Properties
    }
}