using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Commands;

namespace OpenBreed.Wecs.Systems.Rendering.Commands
{
    public struct SpriteSetAtlasCommand : IEntityCommand
    {
        #region Public Fields

        public const string TYPE = "SPRITE_SET_ATLAS";

        #endregion Public Fields

        #region Public Constructors

        public SpriteSetAtlasCommand(int entityId, int atlasId)
        {
            EntityId = entityId;
            AtlasId = atlasId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int EntityId { get; }
        public string Name { get { return TYPE; } }
        public int AtlasId { get; }


        #endregion Public Properties
    }
}