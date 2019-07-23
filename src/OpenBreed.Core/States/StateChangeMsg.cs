using OpenBreed.Core.Entities;

namespace OpenBreed.Core.States
{
    public class StateChangeMsg : IEntityMsg
    {
        #region Public Fields

        public const string TYPE = "STATE_CHANGE";

        #endregion Public Fields

        #region Public Constructors

        public StateChangeMsg(IEntity entity, string stateId, params object[] args)
        {
            Entity = entity; 
            StateId = stateId;
            Args = args;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; }
        public string Type { get { return TYPE; } }
        public object Data { get { return StateId; } }
        public string StateId { get; }
        public object[] Args { get; }

        #endregion Public Properties
    }
}