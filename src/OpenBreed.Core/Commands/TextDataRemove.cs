namespace OpenBreed.Core.Commands
{
    public class TextDataBackspace : ICommand
    {
        #region Public Fields

        public const string TYPE = "TEXT_DATA_BACKSPACE";

        #endregion Public Fields

        #region Public Constructors

        public TextDataBackspace(int entityId)
        {
            EntityId = entityId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int EntityId { get; }

        public string Type { get { return TYPE; } }

        #endregion Public Properties
    }
}