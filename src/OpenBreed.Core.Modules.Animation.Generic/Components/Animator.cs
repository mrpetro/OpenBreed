using OpenBreed.Core.Modules.Animation;
using OpenBreed.Core.Modules.Animation.Helpers;
using OpenBreed.Core.Systems.Common.Components;

namespace OpenBreed.Core.Modules.Animation.Components
{
    public class Animator<T> : IEntityComponent
    {
        #region Public Constructors

        public Animator(float speed = 1.0f, bool loop = false)
        {
            Speed = speed;
            Loop = loop;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Transition method from frame to frame 
        /// </summary>
        public FrameTransition Transition { get; set; }

        public IAnimationData<T> Data { get; set; }
        public float Position { get; set; }
        public bool Paused { get; set; }
        public float Speed { get; set; }
        public bool Loop { get; set; }
        public T Frame { get; set; }

        #endregion Public Properties

        #region Public Methods

        #endregion Public Methods
    }
}