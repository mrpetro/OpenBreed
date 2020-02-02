using OpenBreed.Core.Common.Builders;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Modules.Physics.Components;
using System;

namespace OpenBreed.Core.Modules.Physics.Builders
{
    public class BodyComponentBuilder : BaseComponentBuilder
    {
        #region Private Fields

        private float cofFactor;

        private float corFactor;

        private string type;

        #endregion Private Fields

        #region Protected Constructors

        protected BodyComponentBuilder(ICore core) : base(core)
        {
        }

        #endregion Protected Constructors

        #region Public Methods

        public static IComponentBuilder New(ICore core)
        {
            return new BodyComponentBuilder(core);
        }

        public override IEntityComponent Build()
        {
            return new Body() { CofFactor = cofFactor, CorFactor = corFactor, Tag = type };
        }

        public override void SetProperty(object key, object value)
        {
            var propertyName = Convert.ToString(key);
            switch (propertyName)
            {
                case "CofFactor":
                    cofFactor = Convert.ToSingle(value);
                    break;

                case "CorFactor":
                    corFactor = Convert.ToSingle(value);
                    break;

                case "type":
                    type = Convert.ToString(value);
                    break;

                default:
                    break;
            }
        }

        #endregion Public Methods
    }
}