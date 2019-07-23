using OpenBreed.Core.Entities;

namespace OpenBreed.Core.Modules.Animation.Messages
{
    public struct PlayAnimMsg : IEntityMsg
    {
        #region Public Fields

        public const string TYPE = "PLAY_ANIM";

        #endregion Public Fields

        #region Public Constructors

        public PlayAnimMsg(IEntity entity, string id)
        {
            Entity = entity;
            Data = id;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; }
        public string Type { get { return TYPE; } }
        public object Data { get; }
        public string Id { get { return (string)Data; } }

        #endregion Public Properties
    }
}