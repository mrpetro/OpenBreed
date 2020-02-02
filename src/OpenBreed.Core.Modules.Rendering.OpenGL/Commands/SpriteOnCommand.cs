using OpenBreed.Core.Commands;
using OpenBreed.Core.Entities;

namespace OpenBreed.Core.Modules.Rendering.Commands
{
    public struct SpriteOnCommand : IEntityCommand
    {
        #region Public Fields

        public const string TYPE = "SPRITE_ON";

        #endregion Public Fields

        #region Public Constructors

        public SpriteOnCommand(int entityId)
        {
            EntityId = entityId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int EntityId { get; }
        public string Type { get { return TYPE; } }

        #endregion Public Properties
    }
}