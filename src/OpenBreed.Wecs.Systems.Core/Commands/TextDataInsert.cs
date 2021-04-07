using OpenBreed.Wecs.Commands;

namespace OpenBreed.Wecs.Systems.Core.Commands
{
    public class TextDataInsert : IEntityCommand
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

        public string Name { get { return TYPE; } }

        #endregion Public Properties
    }
}