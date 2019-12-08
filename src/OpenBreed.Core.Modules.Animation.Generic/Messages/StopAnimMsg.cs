using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Entities;

namespace OpenBreed.Core.Modules.Animation.Messages
{
    public struct StopAnimMsg : IEntityMsg
    {
        #region Public Fields

        public const string TYPE = "STOP_ANIM";

        #endregion Public Fields

        #region Public Constructors

        public StopAnimMsg(int entityId, string id)
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