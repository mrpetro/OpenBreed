using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Entities;

namespace OpenBreed.Core.Modules.Animation.Events
{
    public class FrameChangedEvent<T> : ISystemEvent
    {
        #region Public Fields

        public const string TYPE = "FRAME_CHANGED";

        #endregion Public Fields

        #region Public Constructors

        public FrameChangedEvent(T frame)
        {
            Frame = frame;
        }

        #endregion Public Constructors

        #region Public Properties

        public T Frame { get; }
        public string Type { get { return TYPE; } }
        public object Data { get { return Frame; } }

        #endregion Public Properties
    }
}