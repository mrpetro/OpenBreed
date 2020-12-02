using OpenBreed.Core.Components;
using OpenTK;
using System;

namespace OpenBreed.Core.Builders
{
    public class AngularPositionComponentBuilder : BaseComponentBuilder<AngularPositionComponentBuilder, AngularPositionComponent>
    {
        #region Private Fields

        internal float Value { get; private set; }

        #endregion Private Fields

        #region Protected Constructors

        protected AngularPositionComponentBuilder(ICore core) : base(core)
        {
        }

        #endregion Protected Constructors

        #region Public Methods

        public static IComponentBuilder New(ICore core)
        {
            return new AngularPositionComponentBuilder(core);
        }

        public override IEntityComponent Build()
        {
            return new AngularPositionComponent(this);
        }

        public override void SetProperty(object key, object value)
        {
            var index = Convert.ToInt64(key);

            if (index == 1)
                Value = Convert.ToSingle(value);
            else
                throw new ArgumentException("Too many property keys given.");
        }

        #endregion Public Methods
    }
}