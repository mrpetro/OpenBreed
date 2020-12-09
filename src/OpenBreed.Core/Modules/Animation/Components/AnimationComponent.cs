using OpenBreed.Core.Components;
using OpenBreed.Core.Modules.Animation.Builders;
using OpenBreed.Core.Modules.Animation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Modules.Animation.Components
{
    public interface IAnimationComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        float Speed { get; set; }
        bool Loop { get; set; }
        string AnimName { get; set; }
        string Transition { get; set; }

        #endregion Public Properties
    }

    public class Animator
    {
        #region Internal Constructors
        internal Animator()
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

    public class AnimationComponent : IEntityComponent
    {
        #region Internal Constructors

        internal AnimationComponent(AnimationComponentBuilderEx builder)
        {
            Items = new List<Animator>();
            Items.Add(new Animator()
            {
                AnimId = builder.AnimId,
                Loop = builder.Loop,
                Speed = builder.Speed,
                Transition = builder.Transition
            });
        }

        //internal AnimationComponent(AnimationComponentBuilder builder)
        //{
        //    Items = builder.animatorBuilders.Select(item => item.Build()).ToList();
        //}

        #endregion Internal Constructors

        #region Public Properties

        public List<Animator> Items { get; }

        #endregion Public Properties
    }

    public sealed class AnimationComponentFactory : ComponentFactoryBase<IAnimationComponentTemplate>
    {
        #region Public Constructors

        public AnimationComponentFactory(ICore core) : base(core)
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IAnimationComponentTemplate template)
        {
            var builder = AnimationComponentBuilderEx.New(core);

            if(template.AnimName != null)
                builder.SetByName(template.AnimName);

            builder.SetLoop(template.Loop);
            builder.SetSpeed(template.Speed);

            if(template.Transition != null)
                builder.SetTransition(template.Transition);

            return builder.Build();
        }

        #endregion Protected Methods
    }

    public class AnimationComponentBuilderEx
    {
        #region Private Fields

        private ICore core;

        #endregion Private Fields

        #region Private Constructors

        private AnimationComponentBuilderEx(ICore core)
        {
            this.core = core;
        }

        #endregion Private Constructors

        #region Internal Properties

        internal float Speed = 0.0f;
        internal bool Loop = false;
        internal int AnimId = -1;
        internal FrameTransition Transition = FrameTransition.None;

        #endregion Internal Properties

        #region Public Methods

        public static AnimationComponentBuilderEx New(ICore core)
        {
            return new AnimationComponentBuilderEx(core);
        }

        public AnimationComponent Build()
        {
            return new AnimationComponent(this);
        }

        public void SetById(int animId)
        {
            AnimId = animId;
        }

        public void SetByName(string animName)
        {
            AnimId = core.Animations.GetByName(animName).Id;
        }

        public void SetLoop(bool loop)
        {
            Loop = loop;
        }

        public void SetSpeed(float speed)
        {
            Speed = speed;
        }

        public void SetTransition(FrameTransition transition)
        {
            Transition = transition;
        }

        public void SetTransition(string transitionName)
        {
            if (transitionName is "LinearInterpolation")
                SetTransition(FrameTransition.LinearInterpolation);
            else if (transitionName is "None")
                SetTransition(FrameTransition.None);
            else
                throw new Exception();
        }

        #endregion Public Methods
    }
}