using OpenBreed.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Managers
{
    public class ComponentsMan
    {
        private Dictionary<Type, Dictionary<int, IEntityComponent>> components = new Dictionary<Type, Dictionary<int, IEntityComponent>>();

        public void AddEntityComponent<T>(int entityId, IEntityComponent component) where T : class
        {
            Dictionary<int, IEntityComponent> typeComponents = null;

            if (!components.TryGetValue(typeof(T), out typeComponents))
            {
                typeComponents = new Dictionary<int, IEntityComponent>();
                components.Add(typeof(T), typeComponents);
            }

            typeComponents.Add(entityId, component);
        }

        public bool RemoveEntityComponent<T>(int entityId)
        {
            Dictionary<int, IEntityComponent> typeComponents = null;

            if (!components.TryGetValue(typeof(T), out typeComponents))
                return false;

            return typeComponents.Remove(entityId);
        }

        public T GetEntityComponent<T>(int entityId) where T : class
        {
            Dictionary<int, IEntityComponent> typeComponents = null;

            if (!components.TryGetValue(typeof(T), out typeComponents))
                return null;

            IEntityComponent component;

            if (!typeComponents.TryGetValue(entityId, out component))
                return null;

            return (T)component;
        }
    }
}
