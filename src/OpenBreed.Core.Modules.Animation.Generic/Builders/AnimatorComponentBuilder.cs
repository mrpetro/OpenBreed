using OpenBreed.Core.Common.Builders;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Modules.Animation.Components;
using System;

namespace OpenBreed.Core.Modules.Animation.Builders
{
    public class AnimatorComponentBuilder : BaseComponentBuilder<AnimatorComponentBuilder>
    {
        #region Internal Fields

        internal float Speed;
        internal bool Loop;
        internal int AnimId;

        #endregion Internal Fields

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

        public static void Register(ICore core)
        {
            core.Entities.RegisterComponentBuilder(nameof(AnimatorComponent), New);

            RegisterSetter(nameof(Speed), (o, value) => { o.Speed = Convert.ToSingle(value); });
            RegisterSetter(nameof(Loop), (o, value) => { o.Loop = Convert.ToBoolean(value); });
            RegisterSetter(nameof(AnimId), (o, value) => { o.AnimId = o.ToAnimId(value); });
        }

        public override IEntityComponent Build()
        {
            return new AnimatorComponent(this);
        }

        #endregion Public Methods

        #region Private Methods

        private int ToAnimId(object value)
        {
            if (value is int)
                return (int)value;

            return Core.Animations.Anims.GetByName(Convert.ToString(value)).Id;
        }

        #endregion Private Methods
    }
}