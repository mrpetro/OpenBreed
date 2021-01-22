using OpenBreed.Core.Commands;
using OpenTK;

namespace OpenBreed.Systems.Control.Commands
{
    public struct WalkingControlCommand : ICommand
    {
        #region Public Fields

        public const string TYPE = "WALKING_CONTROL";

        #endregion Public Fields

        #region Public Constructors

        public WalkingControlCommand(int entityId, Vector2 direction)
        {
            EntityId = entityId;
            Direction = direction;
        }

        #endregion Public Constructors

        #region Public Properties

        public int EntityId { get; }
        public string Name { get { return TYPE; } }
        public Vector2 Direction { get; }

        #endregion Public Properties
    }
}