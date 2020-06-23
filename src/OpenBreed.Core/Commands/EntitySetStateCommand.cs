namespace OpenBreed.Core.Commands
{
    public class SetStateCommand : ICommand
    {
        #region Public Fields

        public const string TYPE = "SET_STATE";

        #endregion Public Fields

        #region Public Constructors

        public SetStateCommand(int entityId, int fsmId, int impulseId)
        {
            EntityId = entityId;
            FsmId = fsmId;
            ImpulseId = impulseId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int EntityId { get; }
        public string Name { get { return TYPE; } }
        public int FsmId { get; }
        public int ImpulseId { get; }

        #endregion Public Properties
    }
}