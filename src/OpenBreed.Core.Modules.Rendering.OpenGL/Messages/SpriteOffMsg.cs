using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Entities;

namespace OpenBreed.Core.Modules.Rendering.Messages
{
    public struct SpriteOffMsg : IEntityMsg
    {
        #region Public Fields

        public const string TYPE = "SPRITE_OFF";

        #endregion Public Fields

        #region Public Constructors

        public SpriteOffMsg(int entityId)
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