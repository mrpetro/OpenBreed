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
            return new Animator(speed, loop);
        }

        public override void SetProperty(object key, object value)
        {
            var propertyName = Convert.ToString(key);

            if (propertyName == "Speed")
                speed = Convert.ToSingle(value);
            else if (propertyName == "Loop")
                loop = Convert.ToBoolean(value);
            else
                throw new ArgumentException("Too many property keys given.");
        }

        #endregion Public Methods
    }
}