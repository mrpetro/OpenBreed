using OpenBreed.Game.Entities.Components;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OpenBreed.Game.Common
{
    public class ComponentSystem<T> : IComponentSystem where T : IEntityComponent
    {
        #region Private Fields

        private readonly List<T> components = new List<T>();
        private readonly List<T> toAdd = new List<T>();
        private readonly List<T> toRemove = new List<T>();

        #endregion Private Fields

        #region Protected Constructors

        protected ComponentSystem()
        {
            Components = new ReadOnlyCollection<T>(components);
        }

        #endregion Protected Constructors

        #region Protected Properties

        protected ReadOnlyCollection<T> Components { get; }

        #endregion Protected Properties

        #region Public Methods

        public virtual void AddComponent(T component)
        {
            toAdd.Add(component);
        }

        public void AddComponent(IEntityComponent component)
        {
            AddComponent((T)component);
        }

        public void RemoveComponent(IEntityComponent component)
        {
            RemoveComponent((T)component);
        }

        public virtual void RemoveComponent(T component)
        {
            toRemove.Add(component);
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Cleanup()
        {
            if (toRemove.Any())
            {
                //Process components to remove
                for (int i = 0; i < toRemove.Count; i++)
                {
                    toRemove[i].Deinitialize(this);
                    components.Remove(toRemove[i]);
                }

                toRemove.Clear();
            }

            if (toAdd.Any())
            {
                //Process components to add
                for (int i = 0; i < toAdd.Count; i++)
                {
                    components.Add(toAdd[i]);
                    toAdd[i].Initialize(this);
                }

                toAdd.Clear();
            }
        }

        #endregion Protected Methods
    }
}