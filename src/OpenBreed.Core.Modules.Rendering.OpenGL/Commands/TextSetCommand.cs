using OpenBreed.Core.Commands;
using OpenBreed.Core.Entities;

namespace OpenBreed.Core.Modules.Rendering.Commands
{
    public struct TextSetCommand : IEntityCommand
    {
        #region Public Fields

        public const string TYPE = "TEXT_SET";

        #endregion Public Fields

        #region Public Constructors

        public TextSetCommand(int entityId, int partId, string text)
        {
            EntityId = entityId;
            PartId = partId;
            Text = text;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Type { get { return TYPE; } }

        public int EntityId { get; }
        public int PartId { get; }
        public string Text { get; }

        #endregion Public Properties
    }
}