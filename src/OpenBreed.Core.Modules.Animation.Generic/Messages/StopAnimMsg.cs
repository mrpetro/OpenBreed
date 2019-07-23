using OpenBreed.Core.Entities;

namespace OpenBreed.Core.Modules.Animation.Messages
{
    public struct StopAnimMsg : IEntityMsg
    {
        #region Public Fields

        public const string TYPE = "STOP_ANIM";

        #endregion Public Fields

        #region Public Constructors

        public StopAnimMsg(IEntity entity, string id)
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