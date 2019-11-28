using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Entities;

namespace OpenBreed.Core.Modules.Rendering.Messages
{
    public struct TextSetMsg : IWorldMsg
    {
        #region Public Fields

        public const string TYPE = "TEXT_SET";

        #endregion Public Fields

        #region Public Constructors

        public TextSetMsg(int worldId, int entityId, string text)
        {
            WorldId = worldId;
            EntityId = entityId;
            Text = text;
        }

        #endregion Public Constructors

        #region Public Properties

        public int WorldId { get; }
        public int EntityId { get; }
        public string Type { get { return TYPE; } }
        public string Text { get; }

        #endregion Public Properties
    }
}