using OpenBreed.Core.Common.Components;
using System;

namespace OpenBreed.Core.Common.Builders
{
    public class ClassComponentBuilder : BaseComponentBuilder<ClassComponentBuilder, ClassComponent>
    {
        #region Private Fields

        private float x;

        private float y;

        #endregion Private Fields

        #region Protected Constructors

        protected ClassComponentBuilder(ICore core) : base(core)
        {
        }

        #endregion Protected Constructors

        #region Internal Properties

        internal string Name { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public static IComponentBuilder New(ICore core)
        {
            return new ClassComponentBuilder(core);
        }

        public override IEntityComponent Build()
        {
            return new ClassComponent(this);
        }

        public override void SetProperty(object key, object value)
        {
            var index = Convert.ToInt64(key);

            if (index == 1)
                Name = Convert.ToString(value);
            else
                throw new ArgumentException("Too many property keys given.");
        }

        #endregion Public Methods
    }
}