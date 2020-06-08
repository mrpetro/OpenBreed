namespace OpenBreed.Core.Commands
{
    public class TextInsertStringCommand : IEntityCommand
    {
        #region Public Fields

        public const string TYPE = "TEXT_INSERT_STRING";

        public string Str;

        #endregion Public Fields

        #region Public Constructors

        public TextInsertStringCommand(int entityId, string str)
        {
            EntityId = entityId;
            Str = str;
        }

        #endregion Public Constructors

        #region Public Properties

        public int EntityId { get; }

        public string Type => TYPE;

        #endregion Public Properties
    }
}