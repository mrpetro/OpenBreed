using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Systems.Components;
using System;

namespace OpenBreed.Core.Common.Builders
{
    public class FsmComponentBuilder : BaseComponentBuilder<FsmComponentBuilder, FsmComponent>
    {
        #region Private Fields

        private float x;

        private float y;

        #endregion Private Fields

        #region Protected Constructors

        protected FsmComponentBuilder(ICore core) : base(core)
        {
        }

        #endregion Protected Constructors

        #region Internal Properties

        internal string Name { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public static IComponentBuilder New(ICore core)
        {
            return new FsmComponentBuilder(core);
        }

        public override IEntityComponent Build()
        {
            return new FsmComponent(this);
        }

        public override void SetProperty(object key, object value)
        {
        }

        #endregion Public Methods
    }
}