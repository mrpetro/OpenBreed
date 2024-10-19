using OpenBreed.Wecs.Attributes;
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

    public class EntityEmit
    {
        public EntityEmit(string templateName, Dictionary<string, object> options)
        {
            TemplateName = templateName;
            Options = options;
        }

        public string TemplateName { get; }
        public Dictionary<string, object> Options { get; }
    }

    [ComponentName("EntityEmitter")]
    public class EntityEmitterComponent : IEntityComponent
    {
        public EntityEmitterComponent()
        {
            ToEmit = new List<EntityEmit>();
        }

        public List<EntityEmit> ToEmit { get; }
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
