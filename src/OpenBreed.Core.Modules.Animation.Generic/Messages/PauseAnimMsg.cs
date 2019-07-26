using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Entities;

namespace OpenBreed.Core.Modules.Animation.Messages
{
    public struct PauseAnimMsg : IEntityMsg
    {
        #region Public Fields

        public const string TYPE = "PAUSE_ANIM";

        #endregion Public Fields

        #region Public Constructors

        public PauseAnimMsg(IEntity entity, string id)
        {
            Entity = entity;
            Data = id;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; }
        public string Type { get { return TYPE; } }
        public object Data { get; }

        #endregion Public Properties
    }
}