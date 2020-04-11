using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Modules.Animation.Builders;
using OpenBreed.Core.Modules.Animation.Helpers;
using System.Collections.Generic;

namespace OpenBreed.Core.Modules.Animation.Components
{
    public class AnimationData
    {
        #region Public Constructors

        public AnimationData(int animatorId)
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

            Items = new List<AnimationData>(new AnimationData[]
            {
                new AnimationData(10)
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

            Items = new List<AnimationData>(new AnimationData[]
            {
                new AnimationData(10)
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

        public List<AnimationData> Items { get; }

        #endregion Public Properties
    }
}