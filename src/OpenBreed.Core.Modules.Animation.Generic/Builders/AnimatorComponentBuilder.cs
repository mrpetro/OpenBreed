using OpenBreed.Core.Common.Builders;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Modules.Animation.Components;
using System;

namespace OpenBreed.Core.Modules.Animation.Builders
{
    public class AnimatorComponentBuilder : BaseComponentBuilder
    {
        #region Private Fields

        private float speed;
        private bool loop;
        private string animationAlias;

        #endregion Private Fields

        #region Protected Constructors

        protected AnimatorComponentBuilder(ICore core) : base(core)
        {
        }

        #endregion Protected Constructors

        #region Public Methods

        public static IComponentBuilder New(ICore core)
        {
            return new AnimatorComponentBuilder(core);
        }

        public override IEntityComponent Build()
        {
            var anim = Core.Animations.Anims.GetByName(animationAlias);

            if (anim == null)
                return new Animator(speed, loop);
            else
                return new Animator(speed, loop, anim.Id);
        }

        public override void SetProperty(object key, object value)
        {
            var propertyName = Convert.ToString(key);

            if (propertyName == "Speed")
                speed = Convert.ToSingle(value);
            else if (propertyName == "Loop")
                loop = Convert.ToBoolean(value);
            else if (propertyName == "AnimationAlias")
                animationAlias = Convert.ToString(value);
            else
                throw new ArgumentException($"Unknown '{propertyName}' property key given.");
        }

        #endregion Public Methods
    }
}