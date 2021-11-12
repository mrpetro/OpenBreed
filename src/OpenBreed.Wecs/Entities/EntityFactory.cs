using OpenBreed.Common.Tools.Xml;
using OpenBreed.Wecs.Components;
using OpenBreed.Wecs.Entities.Xml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace OpenBreed.Wecs.Entities
{


    public class EntityFactory : IEntityFactory
    {
        #region Private Fields

        private readonly IEntityMan entityMan;

        private readonly Dictionary<Type, IComponentFactory> componentFactories = new Dictionary<Type, IComponentFactory>();

        #endregion Private Fields

        #region Internal Constructors

        internal EntityFactory(IEntityMan entityMan)
        {
            this.entityMan = entityMan;
        }

        #endregion Internal Constructors

        #region Public Methods

        public void RegisterComponentFactory<T>(IComponentFactory factory) where T : IComponentTemplate
        {
            Debug.Assert(!componentFactories.ContainsKey(typeof(T)), $"Component '{typeof(T)}' factory already registered.");

            componentFactories.Add(typeof(T), factory);
        }

        public ITemplateEntityBuilder Create(string entityTemplateName)
        {
            return new TemplateEntityBuilder(this, entityTemplateName);
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