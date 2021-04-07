using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Commands;

namespace OpenBreed.Wecs.Systems.Rendering.Commands
{
    public struct SpriteSetCommand : IEntityCommand
    {
        #region Public Fields

        public const string TYPE = "SPRITE_SET";

        #endregion Public Fields

        #region Public Constructors

        public SpriteSetCommand(int entityId, int imageId)
        {
            EntityId = entityId;
            ImageId = imageId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int EntityId { get; }
        public string Name { get { return TYPE; } }
        public int ImageId { get; }


        #endregion Public Properties
    }
}