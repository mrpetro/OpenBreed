using OpenBreed.Wecs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Wecs.Components
{
    public interface IPickableComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        int Value { get; }

        #endregion Public Properties
    }

    public class PickableComponent : IEntityComponent
    {
        #region Public Constructors

        public PickableComponent(int value)
        {
            Value = value;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Value { get; set; }

        #endregion Public Properties
    }

    public sealed class PickableComponentFactory : ComponentFactoryBase<IPickableComponentTemplate>
    {
        #region Internal Constructors

        public PickableComponentFactory()
        {
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IPickableComponentTemplate template)
        {
            return new PickableComponent(
                template.Value);
        }

        #endregion Protected Methods
    }
}