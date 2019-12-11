using OpenBreed.Core.Commands;
using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Entities;

namespace OpenBreed.Core.States
{
    public class StateChangeCommand : IEntityCommand
    {
        #region Public Fields

        public const string TYPE = "STATE_CHANGE";

        #endregion Public Fields

        #region Public Constructors

        public StateChangeCommand(int entityId, string fsmName, string stateId)
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