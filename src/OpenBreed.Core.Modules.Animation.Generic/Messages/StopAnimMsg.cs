using OpenBreed.Core.Entities;

namespace OpenBreed.Core.Modules.Animation.Messages
{
    public struct StopAnimMsg : IEntityMsg
    {
        #region Public Fields

        public const string TYPE = "STOP_ANIM";

        #endregion Public Fields

        #region Public Constructors

        public StopAnimMsg(string id)
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