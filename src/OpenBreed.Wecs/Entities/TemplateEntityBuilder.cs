using OpenBreed.Common.Tools.Xml;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Components;
using OpenBreed.Wecs.Entities.Xml;
using OpenBreed.Wecs.Services;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace OpenBreed.Wecs.Entities
{
    internal class TemplateEntityBuilder : ITemplateEntityBuilder
    {
        #region Private Fields

        private readonly EntityFactory entityFactory;
        private readonly EntityMan entityMan;
        private readonly IComponentFactoryProvider componentFactoryProvider;
        private readonly IEntityTemplateLoader entityTemplateLoader;
        private readonly string templateName;
        private string tag;
        private readonly Dictionary<string, string> templateParameters = new Dictionary<string, string>();

        #endregion Private Fields

        #region Public Constructors

        public TemplateEntityBuilder(
            EntityFactory entityFactory,
            EntityMan entityMan,
            IComponentFactoryProvider componentFactoryProvider,
            IEntityTemplateLoader entityTemplateLoader,
            string templateName)
        {
            this.entityFactory = entityFactory;
            this.entityMan = entityMan;
            this.componentFactoryProvider = componentFactoryProvider;
            this.entityTemplateLoader = entityTemplateLoader;
            this.templateName = templateName;
        }

        #endregion Public Constructors

        #region Public Methods

        public IEntityBuilder SetTag(string tag)
        {
            this.tag = tag;
            return this;
        }

        public ITemplateEntityBuilder SetParameter<TValue>(string parameterName, TValue parameterValue)
        {
            templateParameters[parameterName] = Convert.ToString(parameterValue, CultureInfo.InvariantCulture);
            return this;
        }

        public IEntity Build()
        {
            var entityTemplate = entityTemplateLoader.Load(templateName, templateParameters);

            var components = new List<IEntityComponent>();

            foreach (var componentTemplate in entityTemplate.Components)
            {
                var componentFactory = componentFactoryProvider.GetFactory(componentTemplate.GetType());

                if (componentFactory is null)
                    throw new Exception($"Don't know how to create component based on template '{componentTemplate.GetType()}'");

                components.Add(componentFactory.Create(componentTemplate));
            }

            var newEntity = new Entity(entityMan, tag, components);
            entityMan.Register(newEntity);
            return newEntity;
        }

        public IEntityBuilder AddComponent<TEntityComponent>(TEntityComponent component) where TEntityComponent : IEntityComponent
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}