using System;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Core.Components
{
    public interface IEntityEmitterComponentTemplate : IComponentTemplate
    {
    }

    public class EntityEmit
    {
        public EntityEmit(Guid id, string templateName, string tag, Dictionary<string, object> options)
        {
            Id = id;
            TemplateName = templateName;
            Tag = tag;
            Options = options;
        }

        public string TemplateName { get; }
        public Guid Id { get; }
        public string Tag { get; } 
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
}
