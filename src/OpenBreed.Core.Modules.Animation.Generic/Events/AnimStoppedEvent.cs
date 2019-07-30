using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Modules.Animation.Components;

namespace OpenBreed.Core.Modules.Animation.Events
{
    /// <summary>
    /// Animation stoped event class, occurs when animation has been stoped
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AnimStoppedEvent<T> : IEvent
    {
        #region Public Fields

        public const string TYPE = "ANIMATION_STOPPED";

        #endregion Public Fields

        #region Public Constructors

        public AnimStoppedEvent(Animator<T> animator)
        {
            Animator = animator;
        }

        #endregion Public Constructors

        #region Public Properties

        public Animator<T> Animator { get; }
        public string Type { get { return TYPE; } }

        #endregion Public Properties
    }
}