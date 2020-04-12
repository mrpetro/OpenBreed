using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Common.Builders;
using OpenBreed.Core.Modules.Animation.Helpers;
using System.Collections.Generic;
using OpenBreed.Core.Modules.Animation.Builders;

namespace OpenBreed.Core.Modules.Animation.Components
{
    public class Animator
    {
        #region Public Constructors

        public Animator(int animatorId)
        {
            AnimatorId = animatorId;
        }

        #endregion Public Constructors

        #region Public Properties

        public int AnimatorId { get; }

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
    }

    public class AnimationComponent : IEntityComponent
    {
        #region Public Constructors

        public AnimationComponent(float speed = 1.0f, bool loop = false, int animId = 0, FrameTransition transition = FrameTransition.None)
        {
            //Speed = speed;
            //Loop = loop;
            //AnimId = animId;
            //Transition = transition;

            Items = new List<Animator>(new Animator[]
            {
                new Animator(10)
                {
                    Speed = speed,
                    Loop = loop,
                    AnimId = animId,
                    Transition = transition
                }
            });
        }

        #endregion Public Constructors

        #region Internal Constructors

        internal AnimationComponent(AnimationComponentBuilder builder)
        {
            //Speed = builder.Speed;
            //Loop = builder.Loop;
            //AnimId = builder.AnimId;

            //TODO: add this to builder
            //Transition = FrameTransition.None;

            Items = new List<Animator>(new Animator[]
            {
                new Animator(10)
                {
                    Speed = builder.Speed,
                    Loop = builder.Loop,
                    AnimId = builder.AnimId,
                    Transition = FrameTransition.None
                }
            });
        }

        #endregion Internal Constructors

        #region Public Properties

        public List<Animator> Items { get; }

        #endregion Public Properties
    }
}