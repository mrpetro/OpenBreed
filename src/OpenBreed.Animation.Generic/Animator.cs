using OpenBreed.Animation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Animation.Generic
{
    public class Animator : IAnimator
    {
        #region Internal Constructors
        public Animator()
        {
        }

        internal Animator(AnimatorBuilder builder)
        {
            AnimId = builder.AnimId;
            Position = builder.Position;
            Speed = builder.Speed;
            Paused = builder.Paused;
            Loop = builder.Loop;
            Transition = builder.Transition;
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Transition method from frame to frame
        /// </summary>
        public FrameTransition Transition { get; set; }

        public int AnimId { get; set; }
        public float Position { get; set; }
        public float Speed { get; set; }
        public bool Paused { get; set; }
        public bool Loop { get; set; }

        #endregion Public Properties
    }
}
