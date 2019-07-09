using OpenBreed.Core.Entities;

namespace OpenBreed.Core.Modules.Animation.Messages
{
    public struct PauseAnimMsg : IEntityMsg
    {
        #region Public Fields

        public const string TYPE = "PAUSE_ANIM";

        #endregion Public Fields

        #region Public Constructors

        public PauseAnimMsg(string id)
        {
            Data = id;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Type { get { return TYPE; } }
        public object Data { get; }

        #endregion Public Properties
    }
}