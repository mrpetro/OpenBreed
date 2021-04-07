using OpenBreed.Wecs.Commands;

namespace OpenBreed.Wecs.Systems.Core.Commands
{
    public class TextDataBackspace : IEntityCommand
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

        public string Name { get { return TYPE; } }

        #endregion Public Properties
    }
}