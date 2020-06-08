namespace OpenBreed.Core.Commands
{
    public class TextCaretSetPosition : IEntityCommand
    {
        #region Public Fields

        public const string TYPE = "TEXT_CARET_SET_POSITION";

        #endregion Public Fields

        #region Public Constructors

        public TextCaretSetPosition(int entityId, int newPosition)
        {
            EntityId = entityId;
            NewPosition = newPosition;
        }

        #endregion Public Constructors

        #region Public Properties

        public int NewPosition { get; }

        public int EntityId { get; }

        public string Type { get { return TYPE; } }

        #endregion Public Properties
    }
}