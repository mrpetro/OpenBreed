using OpenBreed.Wecs.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Core.Systems.Extensions
{
    public static class EntityExtensions
    {
        public class EmitBuilder
        {
            private readonly EntityEmitterComponent emitterComponent;
            private readonly string templateName;
            private readonly Dictionary<string, object> options = new Dictionary<string, object>();
            private string tag;

            internal EmitBuilder(EntityEmitterComponent emitterComponent, string templateName)
            {
                this.emitterComponent = emitterComponent;
                this.templateName = templateName;
            }

            public EmitBuilder SetOption(string name, object value)
            {
                options[name] = value;
                return this;
            }

            public EmitBuilder SetTag(string value)
            {
                tag = value;
                return this;
            }

            public Guid Finish()
            {
                var newEmit = new EntityEmit(Guid.NewGuid(), templateName, tag, options.ToDictionary(item => item.Key, item => item.Value));
                emitterComponent.ToEmit.Add(newEmit);
                return newEmit.Id;
            }
        }

        public static EmitBuilder StartEmit(this IEntity entity, string templateName)
        {
            var emitterComponent = entity.Get<EntityEmitterComponent>();
            return new EmitBuilder(emitterComponent, templateName);
        }

        public static void Emit(this IEntity entity, string templateName, string tag = null)
        {
            var emitComponent = entity.Get<EntityEmitterComponent>();
            var newEmit = new EntityEmit(Guid.NewGuid(), templateName, tag, new Dictionary<string, object>());
            emitComponent.ToEmit.Add(newEmit);
        }

        public static int GetSourceEntityId(this IEntity entity)
        {
            return entity.Get<SourceEntityComponent>().EntityId;
        }


        public static int StartTimer(this IEntity entity, double interval)
        {
            var timerCmp = entity.Get<TimerComponent>();
            var id = timerCmp.Items.Count;
            timerCmp.Items.Add(new TimerData(id, interval));
            return id;
        }
    }
}
