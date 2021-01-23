
using OpenBreed.Core.Commands;

namespace OpenBreed.Wecs.Systems.Animation.Commands
{
    public struct PauseAnimCommand : ICommand
    {
        #region Public Fields

        public const string TYPE = "PAUSE_ANIM";

        #endregion Public Fields

        #region Public Constructors

        public PauseAnimCommand(int entityId, int animatorId, string id)
        {
            EntityId = entityId;
            AnimatorId = animatorId;
            Id = id;
        }

        #endregion Public Constructors

        #region Public Properties

        public int EntityId { get; }
        public string Name { get { return TYPE; } }
        public int AnimatorId { get; }
        public string Id { get; }

        #endregion Public Properties
    }
}