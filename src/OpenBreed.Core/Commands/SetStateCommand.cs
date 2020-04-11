using OpenBreed.Core.Commands;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Entities;

namespace OpenBreed.Core.Commands
{
    public class EntitySetStateCommand : IEntityCommand
    {
        #region Public Fields

        public const string TYPE = "ENTITY_SET_STATE";

        #endregion Public Fields

        #region Public Constructors

        public EntitySetStateCommand(int entityId, string fsmName, string stateId)
        {
            EntityId = entityId;
            FsmName = fsmName;
            StateId = stateId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int EntityId { get; }
        public string Type { get { return TYPE; } }
        public string FsmName { get; }
        public string StateId { get; }

        #endregion Public Properties
    }
}