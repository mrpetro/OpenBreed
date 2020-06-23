namespace OpenBreed.Core.Commands
{
    public class TimerStopCommand : ICommand
    {
        #region Public Fields

        public const string TYPE = "TIMER_STOP";

        #endregion Public Fields

        #region Public Constructors

        public TimerStopCommand(int entityId, int timerId)
        {
            EntityId = entityId;
            TimerId = timerId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int EntityId { get; }
        public string Type { get { return TYPE; } }
        public int TimerId { get; }

        #endregion Public Properties
    }
}