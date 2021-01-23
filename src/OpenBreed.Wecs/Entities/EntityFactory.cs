using OpenBreed.Wecs.Components;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenBreed.Wecs.Entities
{
    public interface IEntityFactory
    {
        #region Public Methods

        void RegisterComponentFactory<T>(IComponentFactory factory) where T : IComponentTemplate;

        Entity Create(IEntityTemplate template);

        #endregion Public Methods
    }

    public class EntityFactory : IEntityFactory
    {
        #region Private Fields

        private readonly IEntityMan entityMan;

        private readonly Dictionary<Type, IComponentFactory> componentFactories = new Dictionary<Type, IComponentFactory>();

        #endregion Private Fields

        #region Public Constructors

        public EntityFactory(IEntityMan entityMan)
        {
            this.entityMan = entityMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void RegisterComponentFactory<T>(IComponentFactory factory) where T : IComponentTemplate
        {
            Debug.Assert(!componentFactories.ContainsKey(typeof(T)), $"Component '{typeof(T)}' factory already regisered.");

            componentFactories.Add(typeof(T), factory);
        }

        public Entity Create(IEntityTemplate template)
        {
            var components = new List<IEntityComponent>();

            foreach (var componentTemplate in template.Components)
            {
                if (componentFactories.TryGetValue(componentTemplate.GetType(), out IComponentFactory componentFactory))
                    components.Add(componentFactory.Create(componentTemplate));
                else
                    throw new Exception($"Don't know how to create component based on template '{componentTemplate.GetType()}'");
            }

            return entityMan.Create(components);
        }

        #endregion Public Methods
    }
}