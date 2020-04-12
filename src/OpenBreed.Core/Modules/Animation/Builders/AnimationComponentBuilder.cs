using OpenBreed.Core.Common.Builders;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Common.Components;
using System;
using System.Collections.Generic;
using OpenBreed.Core.Modules.Animation.Components;

namespace OpenBreed.Core.Modules.Animation.Builders
{
    public class AnimationComponentBuilder : BaseComponentBuilder<AnimationComponentBuilder>
    {
        #region Internal Fields

        internal float Speed;
        internal bool Loop;
        internal int AnimId;

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
            return new AnimationComponentBuilder(core);
        }

        public static void Register(ICore core)
        {
            core.Entities.RegisterComponentBuilder(nameof(AnimationComponent), New);

            RegisterSetter(nameof(Speed), (o, value) => { o.Speed = Convert.ToSingle(value); });
            RegisterSetter(nameof(Loop), (o, value) => { o.Loop = Convert.ToBoolean(value); });
            RegisterSetter(nameof(AnimId), (o, value) => { o.AnimId = o.ToAnimId(value); });
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

        #endregion Private Methods
    }
}