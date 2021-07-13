using OpenBreed.Core.Commands;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Commands;

namespace OpenBreed.Wecs.Systems.Rendering.Commands
{
    public struct SpriteOffMsg : IMsg
    {
    }

    public struct SpriteOnMsg : IMsg
    {
    }

    public struct SpriteOffCommand : IEntityCommand
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