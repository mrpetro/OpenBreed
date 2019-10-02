using OpenBreed.Core.Common.Helpers;

namespace OpenBreed.Core.Modules.Animation.Events
{
    public class AnimChangedEvent : IEvent
    {
        #region Public Fields

        public const string TYPE = "ANIMATION_CHANGED";

        #endregion Public Fields

        #region Public Constructors

        public AnimChangedEvent(object frame)
        {
            Frame = frame;
        }

        #endregion Public Constructors

        #region Public Properties

        public object Frame { get; }
        public string Type { get { return TYPE; } }

        #endregion Public Properties
    }



}