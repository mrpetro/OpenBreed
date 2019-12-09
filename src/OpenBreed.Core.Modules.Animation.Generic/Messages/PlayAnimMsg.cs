using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Entities;

namespace OpenBreed.Core.Modules.Animation.Messages
{
    public struct PlayAnimMsg : IEntityMsg
    {
        #region Public Fields

        public const string TYPE = "PLAY_ANIM";

        #endregion Public Fields

        #region Public Constructors

        public PlayAnimMsg(int entityId, string id)
        {
            EntityId = entityId;
            Id = id;
        }

        #endregion Public Constructors

        #region Public Properties

        public int EntityId { get; }
        public string Type { get { return TYPE; } }
        public string Id { get; }

        #endregion Public Properties
    }
}