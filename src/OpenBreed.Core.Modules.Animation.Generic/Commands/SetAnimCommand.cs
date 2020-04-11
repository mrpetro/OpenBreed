using OpenBreed.Core.Commands;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Entities;

namespace OpenBreed.Core.Modules.Animation.Commands
{
    public struct SetAnimCommand : IEntityCommand
    {
        #region Public Fields

        public const string TYPE = "SET_ANIM";

        #endregion Public Fields

        #region Public Constructors

        public SetAnimCommand(int entityId, string id, int animatorId)
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