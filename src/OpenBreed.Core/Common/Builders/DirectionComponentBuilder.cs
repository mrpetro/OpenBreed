using OpenBreed.Core.Common.Systems.Components;
using OpenTK;
using System;

namespace OpenBreed.Core.Common.Builders
{
    public class DirectionComponentBuilder : BaseComponentBuilder<DirectionComponentBuilder, DirectionComponent>
    {
        #region Private Fields

        private float x;
        private float y;

        internal Vector2 Value;

        #endregion Private Fields

        #region Protected Constructors

        protected DirectionComponentBuilder(ICore core) : base(core)
        {
        }

        #endregion Protected Constructors

        #region Public Methods

        public static IComponentBuilder New(ICore core)
        {
            return new DirectionComponentBuilder(core);
        }

        public override IEntityComponent Build()
        {
            Value = new Vector2(x, y);
            return new DirectionComponent(this);
        }

        public override void SetProperty(object key, object value)
        {
            var index = Convert.ToInt64(key);

            if (index == 1)
                x = Convert.ToSingle(value);
            else if (index == 2)
                y = Convert.ToSingle(value);
            else
                throw new ArgumentException("Too many property keys given.");
        }

        public static void Register(ICore core)
        {
            core.Entities.RegisterComponentBuilder(nameof(DirectionComponent), DirectionComponentBuilder.New);
        }

        #endregion Public Methods
    }
}