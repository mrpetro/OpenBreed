using OpenBreed.Core.Entities;

namespace OpenBreed.Core.Modules.Animation.Messages
{
    public struct PlayAnimMsg : IEntityMsg
    {
        #region Public Fields

        public const string TYPE = "PLAY_ANIM";

        #endregion Public Fields

        #region Public Constructors

        public PlayAnimMsg(string id)
        {
            Data = id;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Type { get { return TYPE; } }
        public object Data { get; }
        public string Id { get { return (string)Data; } }

        #endregion Public Properties
    }
}