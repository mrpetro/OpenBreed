namespace OpenBreed.Core.Commands
{
    public class TextDataInsert : ICommand
    {
        #region Public Fields

        public const string TYPE = "TEXT_DATA_INSERT";

        #endregion Public Fields

        #region Public Constructors

        public TextDataInsert(int entityId, string text)
        {
            EntityId = entityId;
            Text = text;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Text { get; }

        public int EntityId { get; }

        public string Type { get { return TYPE; } }

        #endregion Public Properties
    }
}