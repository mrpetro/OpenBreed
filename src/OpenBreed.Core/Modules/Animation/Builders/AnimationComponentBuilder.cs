using OpenBreed.Core.Builders;
using OpenBreed.Core.Components;
using System;
using System.Collections.Generic;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Modules.Animation.Helpers;

namespace OpenBreed.Core.Modules.Animation.Builders
{
    public class AnimationComponentBuilder : BaseComponentBuilder<AnimationComponentBuilder, AnimationComponent>
    {
        #region Internal Fields

        internal int AnimId = -1;

        #endregion Internal Fields

        #region Protected Constructors

        protected AnimationComponentBuilder(ICore core) : base(core)
        {
        }

        #endregion Protected Constructors

        #region Internal Properties

        #endregion Internal Properties

        #region Public Methods

        public static IComponentBuilder New(ICore core)
        {
            var builder = new AnimationComponentBuilder(core);
            builder.AddAnimator();
            return builder;
        }

        public static AnimationComponentBuilder NewAnimation(ICore core)
        {
            return new AnimationComponentBuilder(core);
        }

        internal List<AnimatorBuilder> animatorBuilders = new List<AnimatorBuilder>();

        public AnimatorBuilder AddAnimator()
        {
            var animatorBuilder = new AnimatorBuilder(Core);
            animatorBuilders.Add(animatorBuilder);
            return animatorBuilder;
        }

        public static void Register(ICore core)
        {
            core.Entities.RegisterComponentBuilder(nameof(AnimationComponent), New);

            RegisterSetter("Speed", (o, value) => { o.animatorBuilders[0].SetSpeed(Convert.ToSingle(value)); });
            RegisterSetter("Loop", (o, value) => { o.animatorBuilders[0].SetLoop(Convert.ToBoolean(value)); });
            RegisterSetter("AnimId", (o, value) => { o.animatorBuilders[0].SetAnimId(o.ToAnimId(value)); });
            RegisterSetter("Transition", (o, value) => { o.animatorBuilders[0].SetTransition(o.ToTransition(value)); });
        }

        public override IEntityComponent Build()
        {
            return new AnimationComponent(this);
        }

        #endregion Public Methods

        #region Private Methods

        private int ToAnimId(object value)
        {
            if (value is int)
                return (int)value;

            return Core.Animations.GetByName(Convert.ToString(value)).Id;
        }

        private FrameTransition ToTransition(object value)
        {
            if (value is "LinearInterpolation")
                return FrameTransition.LinearInterpolation;
            else if (value is "None")
                return FrameTransition.None;
            else
                throw new Exception();
        }

        #endregion Private Methods
    }
}