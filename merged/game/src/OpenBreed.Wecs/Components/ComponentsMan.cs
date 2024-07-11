using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components
{
    public class ComponentsMan : IComponentsMan
    {
        #region Private Fields

        private readonly Dictionary<Type, Dictionary<int, IEntityComponent>> components = new Dictionary<Type, Dictionary<int, IEntityComponent>>();
        private readonly Dictionary<int, Dictionary<Type, IEntityComponent>> types = new Dictionary<int, Dictionary<Type, IEntityComponent>>();

        #endregion Private Fields

        #region Public Methods

        public void Add<TComponent>(int entityId, IEntityComponent component) where TComponent : class
        {
            Dictionary<int, IEntityComponent> typeComponents = null;

            if (!components.TryGetValue(typeof(TComponent), out typeComponents))
            {
                typeComponents = new Dictionary<int, IEntityComponent>();
                components.Add(typeof(TComponent), typeComponents);
            }

            typeComponents.Add(entityId, component);
        }

        public bool Set<TComponent>(int id, TComponent component) where TComponent : class, IEntityComponent
        {
            var result = false;
            if (!components.TryGetValue(typeof(TComponent), out Dictionary<int, IEntityComponent> typeComponents))
            {
                typeComponents = new Dictionary<int, IEntityComponent>();
                components.Add(typeof(TComponent), typeComponents);
                result = true;
            }

            if (!typeComponents.ContainsKey(id))
            {
                result = true;
            }

            typeComponents[id] = component;

            return result;
        }

        public bool Contains<TComponent>(int entityId)
        {
            if (!components.TryGetValue(typeof(TComponent), out Dictionary<int, IEntityComponent> typeComponents))
            {
                return false;
            }

            return typeComponents.ContainsKey(entityId);
        }

        public bool TryGet<TComponent>(int entityId, out TComponent component) where TComponent : class
        {
            if (!components.TryGetValue(typeof(TComponent), out Dictionary<int, IEntityComponent>  typeComponents))
            {
                component = default;
                return false;
            }

            if (!typeComponents.TryGetValue(entityId, out IEntityComponent cmp))
            {
                component = default;
                return false;
            }

            component = (TComponent)cmp;
            return true;
        }

        public TComponent Get<TComponent>(int entityId)
        {
            if (!components.TryGetValue(typeof(TComponent), out Dictionary<int, IEntityComponent> typeComponents))
                return default;

            IEntityComponent component;

            if (!typeComponents.TryGetValue(entityId, out component))
                return default;

            return (TComponent)component;
        }

        public bool Remove<TComponent>(int entityId)
        {
            Dictionary<int, IEntityComponent> typeComponents = null;

            if (!components.TryGetValue(typeof(TComponent), out typeComponents))
                return false;

            return typeComponents.Remove(entityId);
        }

        public ICollection<Type> Types(int id)
        {
            throw new NotImplementedException("This needs proper implementation.");
        }

        #endregion Public Methods
    }
}