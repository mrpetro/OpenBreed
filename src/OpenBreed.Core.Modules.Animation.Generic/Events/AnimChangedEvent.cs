using OpenBreed.Core.Common.Helpers;

namespace OpenBreed.Core.Modules.Animation.Events
{
    public class AnimChangedEvent<T> : IEvent
    {
        #region Public Fields

        public const string TYPE = "ANIMATION_CHANGED";

        #endregion Public Fields

        #region Public Constructors

        public AnimChangedEvent(T frame)
        {
            Frame = frame;
        }

        #endregion Public Constructors

        #region Public Properties

        public T Frame { get; }
        public string Type { get { return TYPE; } }

        #endregion Public Properties
    }



}