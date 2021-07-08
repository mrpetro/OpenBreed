using OpenBreed.Animation.Generic;
using OpenBreed.Animation.Interface;
using OpenBreed.Common;
using System;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Components.Animation
{
    public interface IAnimationComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        float Speed { get; set; }
        bool Loop { get; set; }
        string AnimName { get; set; }

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
                Speed = builder.Speed
            });
        }

        #endregion Internal Constructors

        //internal AnimationComponent(AnimationComponentBuilder builder)
        //{
        //    Items = builder.animatorBuilders.Select(item => item.Build()).ToList();
        //}

        #region Public Properties

        public List<Animator> Items { get; }

        #endregion Public Properties
    }

    public sealed class AnimationComponentFactory : ComponentFactoryBase<IAnimationComponentTemplate>
    {
        #region Private Fields

        private readonly IManagerCollection managerCollection;

        #endregion Private Fields

        #region Public Constructors

        public AnimationComponentFactory(IManagerCollection managerCollection)
        {
            this.managerCollection = managerCollection;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IAnimationComponentTemplate template)
        {
            var builder = managerCollection.GetManager<AnimationComponentBuilder>();

            if (template.AnimName != null)
                builder.SetByName(template.AnimName);

            builder.SetLoop(template.Loop);
            builder.SetSpeed(template.Speed);

            return builder.Build();
        }

        #endregion Protected Methods
    }

    public class AnimationComponentBuilder
    {
        #region Internal Fields

        internal float Speed = 0.0f;
        internal bool Loop = false;
        internal int AnimId = -1;

        #endregion Internal Fields

        #region Private Fields

        private readonly IAnimationMan animationMan;

        #endregion Private Fields

        #region Internal Constructors

        internal AnimationComponentBuilder(IAnimationMan animationMan)
        {
            this.animationMan = animationMan;
        }

        #endregion Internal Constructors

        #region Public Methods

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
            AnimId = animationMan.GetByName(animName).Id;
        }

        public void SetLoop(bool loop)
        {
            Loop = loop;
        }

        public void SetSpeed(float speed)
        {
            Speed = speed;
        }

        #endregion Public Methods
    }
}