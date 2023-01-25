using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Common
{
    public interface IEntityEmitterComponentTemplate : IComponentTemplate
    {
    }

    public class EntityEmitterComponent : IEntityComponent
    {
        public EntityEmitterComponent()
        {
            ToEmit = new List<string>();
        }

        public List<string> ToEmit { get; }
    }

    public sealed class EntityEmitterComponentFactory : ComponentFactoryBase<IEntityEmitterComponentTemplate>
    {
        public EntityEmitterComponentFactory()
        {

        }

        protected override IEntityComponent Create(IEntityEmitterComponentTemplate template)
        {
            return new EntityEmitterComponent();
        }
    }
}
