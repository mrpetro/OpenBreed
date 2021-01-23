using OpenBreed.Core.Commands;
namespace OpenBreed.Wecs.Systems.Rendering.Commands
{
    public struct TextSetCommand : ICommand
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

        public string Name { get { return TYPE; } }

        public int EntityId { get; }
        public int PartId { get; }
        public string Text { get; }

        #endregion Public Properties
    }
}