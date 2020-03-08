using OpenBreed.Core.Commands;

namespace OpenBreed.Core.Modules.Rendering.Commands
{
    public class ViewportResizeCommand : IEntityCommand
    {
        #region Public Fields

        public const string TYPE = "RESIZE_VIEWPORT";

        #endregion Public Fields

        #region Public Constructors

        public ViewportResizeCommand(int entityId, float width, float height)
        {
            EntityId = entityId;
            Width = width;
            Height = height;
        }

        #endregion Public Constructors

        #region Public Properties

        public int EntityId { get; }
        public string Type { get { return TYPE; } }
        public float Width { get; }
        public float Height { get; }

        #endregion Public Properties
    }
}