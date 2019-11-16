using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Modules.Animation;
using OpenBreed.Core.Modules.Animation.Helpers;

namespace OpenBreed.Core.Modules.Animation.Components
{
    public class Animator : IEntityComponent
    {
        #region Public Constructors

        public Animator(float speed = 1.0f, bool loop = false, int animId = 0, FrameTransition transition = FrameTransition.None)
        {
            Speed = speed;
            Loop = loop;
            AnimId = animId;
            Transition = transition;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Transition method from frame to frame 
        /// </summary>
        public FrameTransition Transition { get; set; }

        public int AnimId { get; set; }
        public float Position { get; set; }
        public bool Paused { get; set; }
        public float Speed { get; set; }
        public bool Loop { get; set; }
        public object Frame { get; set; }

        #endregion Public Properties

        #region Public Methods

        #endregion Public Methods
    }
}