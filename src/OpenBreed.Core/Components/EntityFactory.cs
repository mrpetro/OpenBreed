using OpenBreed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenBreed.Core.Components
{
    public class EntityFactory
    {
        #region Private Fields

        private readonly ICore core;

        private readonly Dictionary<Type, IComponentFactory> componentFactories = new Dictionary<Type, IComponentFactory>();

        #endregion Private Fields

        #region Public Constructors

        public void RegisterComponentFactory<T>(IComponentFactory factory) where T : IComponentTemplate
        {
            Debug.Assert(!componentFactories.ContainsKey(typeof(T)), $"Component '{typeof(T)}' factory already regisered.");

            componentFactories.Add(typeof(T), factory);
        }

        public EntityFactory(ICore core)
        {
            this.core = core;
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

            return core.Entities.Create(components);
        }

        #endregion Public Constructors
    }
}