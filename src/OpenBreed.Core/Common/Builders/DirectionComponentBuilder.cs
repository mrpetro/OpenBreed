using OpenBreed.Core.Common.Systems.Components;
using System;

namespace OpenBreed.Core.Common.Builders
{
    public class DirectionComponentBuilder : BaseComponentBuilder
    {
        #region Private Fields

        private float x;

        private float y;

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
            return Direction.Create(x, y);
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

        #endregion Public Methods
    }
}