using OpenBreed.Wecs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Wecs.Components
{
    public interface ILivesComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        int Value { get; }

        #endregion Public Properties
    }

    public class LivesComponent : IEntityComponent
    {
        public LivesComponent(int value)
        {
            Value = value;
        }

        #region Public Properties

        public int Value { get; set; }

        public List<int> ToAdd { get; } = new List<int>();

        #endregion Public Properties
    }

    public sealed class LivesComponentFactory : ComponentFactoryBase<ILivesComponentTemplate>
    {
        #region Internal Constructors

        public LivesComponentFactory()
        {
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override IEntityComponent Create(ILivesComponentTemplate template)
        {
            return new LivesComponent(
                template.Value);
        }

        #endregion Protected Methods
    }
}
