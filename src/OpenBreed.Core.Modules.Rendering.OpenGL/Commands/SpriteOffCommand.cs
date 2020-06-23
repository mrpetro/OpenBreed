using OpenBreed.Core.Commands;
using OpenBreed.Core.Entities;

namespace OpenBreed.Core.Modules.Rendering.Commands
{
    public struct SpriteOffCommand : ICommand
    {
        #region Public Fields

        public const string TYPE = "SPRITE_OFF";

        #endregion Public Fields

        #region Public Constructors

        public SpriteOffCommand(int entityId)
        {
            EntityId = entityId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int EntityId { get; }
        public string Name { get { return TYPE; } }

        #endregion Public Properties
    }
}