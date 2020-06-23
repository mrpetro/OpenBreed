namespace OpenBreed.Core.Commands
{
    public class PauseWorldCommand : ICommand
    {
        #region Public Fields

        public const string TYPE = "PAUSE_WORLD";

        #endregion Public Fields

        #region Public Constructors

        public PauseWorldCommand(int worldId, bool pause)
        {
            WorldId = worldId;
            Pause = pause;
        }

        #endregion Public Constructors

        #region Public Properties

        public int WorldId { get; }
        public bool Pause { get; }
        public string Type { get { return TYPE; } }

        #endregion Public Properties
    }
}