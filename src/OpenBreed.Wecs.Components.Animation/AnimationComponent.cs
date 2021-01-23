using OpenBreed.Animation.Generic;
using OpenBreed.Animation.Interface;
using OpenBreed.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenBreed.Wecs.Components;

namespace OpenBreed.Wecs.Components.Animation
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

    public class AnimationComponent : IEntityComponent
    {
        #region Internal Constructors

        internal AnimationComponent(AnimationComponentBuilder builder)
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
            var builder = AnimationComponentBuilder.New(core);

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

    public class AnimationComponentBuilder
    {
        #region Private Fields

        private ICore core;

        #endregion Private Fields

        #region Private Constructors

        private AnimationComponentBuilder(ICore core)
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

        public static AnimationComponentBuilder New(ICore core)
        {
            return new AnimationComponentBuilder(core);
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
            AnimId = core.GetManager<IAnimMan>().GetByName(animName).Id;
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