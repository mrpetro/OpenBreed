using OpenBreed.Animation.Interface;
using OpenBreed.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OpenBreed.Wecs.Components.Animation
{
    public interface IAnimationComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        ReadOnlyCollection<IAnimationStateTemplate> States { get; }

        #endregion Public Properties
    }

    public interface IAnimationStateTemplate
    {
        #region Public Properties

        float Speed { get; set; }
        bool Loop { get; set; }
        string ClipName { get; set; }

        #endregion Public Properties
    }

    public class AnimationComponent : IEntityComponent
    {
        #region Internal Constructors

        internal AnimationComponent(AnimationComponentBuilder builder)
        {
            States = new List<Animator>();

            foreach (var state in builder.States)
            {
                States.Add(state.Build());
            }
        }

        #endregion Internal Constructors

        //internal AnimationComponent(AnimationComponentBuilder builder)
        //{
        //    Items = builder.animatorBuilders.Select(item => item.Build()).ToList();
        //}

        #region Public Properties

        public List<Animator> States { get; }

        #endregion Public Properties
    }

    public sealed class AnimationComponentFactory : ComponentFactoryBase<IAnimationComponentTemplate>
    {
        #region Private Fields

        private readonly IBuilderFactory builderFactory;

        #endregion Private Fields

        #region Public Constructors

        public AnimationComponentFactory(IBuilderFactory builderFactory)
        {
            this.builderFactory = builderFactory;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IAnimationComponentTemplate template)
        {
            var builder = builderFactory.GetBuilder<AnimationComponentBuilder>();

            foreach (var stateTemplate in template.States)
            {
                builder.AddState()
                            .SetClipByName(stateTemplate.ClipName)
                            .SetLoop(stateTemplate.Loop)
                            .SetSpeed(stateTemplate.Speed);
            }

            return builder.Build();
        }

        #endregion Protected Methods
    }

    public class AnimationComponentBuilder : IBuilder<AnimationComponent>
    {
        #region Internal Fields

        internal readonly List<AnimationStateBuilder> States = new List<AnimationStateBuilder>();

        #endregion Internal Fields

        #region Private Fields

        private readonly IClipMan clipMan;

        #endregion Private Fields

        #region Internal Constructors

        internal AnimationComponentBuilder(IClipMan clipMan)
        {
            this.clipMan = clipMan;
        }

        #endregion Internal Constructors

        #region Public Methods

        public AnimationComponent Build()
        {
            return new AnimationComponent(this);
        }

        #endregion Public Methods

        #region Internal Methods

        public AnimationStateBuilder AddState()
        {
            var newAnimationStateBuilder = new AnimationStateBuilder(clipMan);
            States.Add(newAnimationStateBuilder);
            return newAnimationStateBuilder;
        }

        #endregion Internal Methods
    }

    public class AnimationStateBuilder
    {
        #region Internal Fields

        internal float Speed = 0.0f;
        internal bool Loop = false;
        internal int ClipId = -1;
        internal float Time = 0.0f;
        internal bool Paused = false;

        #endregion Internal Fields

        #region Private Fields

        private readonly IClipMan clipMan;

        #endregion Private Fields

        #region Internal Constructors

        internal AnimationStateBuilder(IClipMan clipMan)
        {
            this.clipMan = clipMan;
        }

        #endregion Internal Constructors

        #region Public Methods

        public AnimationStateBuilder SetClipByName(string clipName)
        {
            ClipId = clipMan.GetByName(clipName).Id;
            return this;
        }

        public AnimationStateBuilder SetById(int animId)
        {
            ClipId = animId;
            return this;
        }

        public AnimationStateBuilder SetLoop(bool loop)
        {
            Loop = loop;
            return this;
        }

        public AnimationStateBuilder SetSpeed(float speed)
        {
            Speed = speed;
            return this;
        }

        public Animator Build()
        {
            return new Animator(this);
        }

        #endregion Public Methods
    }
}