using OpenBreed.Core.Commands;
using OpenBreed.Core.Entities;

namespace OpenBreed.Core.Modules.Animation.Commands
{
    public struct PauseAnimCommand : IEntityCommand
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
        public string Type { get { return TYPE; } }
        public int AnimatorId { get; }
        public string Id { get; }

        #endregion Public Properties
    }
}