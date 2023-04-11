using OpenBreed.Wecs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Wecs.Components
{
    public interface ILifesComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        int Value { get; }

        #endregion Public Properties
    }

    public class LifesComponent : IEntityComponent
    {
        public LifesComponent(int value)
        {
            Value = value;
        }

        #region Public Properties

        public int Value { get; set; }

        #endregion Public Properties
    }

    public sealed class LifesComponentFactory : ComponentFactoryBase<ILifesComponentTemplate>
    {
        #region Internal Constructors

        public LifesComponentFactory()
        {
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override IEntityComponent Create(ILifesComponentTemplate template)
        {
            return new LifesComponent(
                template.Value);
        }

        #endregion Protected Methods
    }
}
