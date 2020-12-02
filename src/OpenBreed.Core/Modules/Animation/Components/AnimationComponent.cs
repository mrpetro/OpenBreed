using OpenBreed.Core.Components;
using OpenBreed.Core.Modules.Animation.Builders;
using OpenBreed.Core.Modules.Animation.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Modules.Animation.Components
{
    public class Animator
    {
        #region Internal Constructors

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

    public class AnimationComponent : IEntityComponent
    {
        #region Internal Constructors

        internal AnimationComponent(AnimationComponentBuilder builder)
        {
            Items = builder.animatorBuilders.Select(item => item.Build()).ToList();
        }

        #endregion Internal Constructors

        #region Public Properties

        public List<Animator> Items { get; }

        #endregion Public Properties
    }
}