using OpenBreed.Wecs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Wecs.Components
{
    public interface IAmmoComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        int Value { get; }
        int MaximumValue { get; }

        #endregion Public Properties
    }

    public class AmmoComponent : IEntityComponent
    {
        public AmmoComponent(
            int maximumValue,
            int value)
        {
            MaximumValue = maximumValue;
            Value = value;
        }

        #region Public Properties

        public int MaximumValue { get; set; }
        public int Value { get; set; }

        #endregion Public Properties
    }

    public sealed class AmmoComponentFactory : ComponentFactoryBase<IAmmoComponentTemplate>
    {
        #region Internal Constructors

        public AmmoComponentFactory()
        {
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IAmmoComponentTemplate template)
        {
            return new AmmoComponent(
                template.MaximumValue,
                template.Value);
        }

        #endregion Protected Methods
    }
}
