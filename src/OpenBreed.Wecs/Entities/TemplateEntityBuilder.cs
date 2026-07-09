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
        private readonly IServiceProvider serviceProvider;
        private readonly IEntityTemplateLoader entityTemplateLoader;
        private readonly IEntityClassMan entityClassMan;
        private readonly string templateName;
        private string tag;
        private readonly Dictionary<string, string> templateParameters = new Dictionary<string, string>();

        #endregion Private Fields

        #region Public Constructors

        public TemplateEntityBuilder(
            EntityFactory entityFactory,
            EntityMan entityMan,
            IServiceProvider serviceProvider,
            IEntityTemplateLoader entityTemplateLoader,
            IEntityClassMan entityClassMan,
            string templateName)
        {
            this.entityFactory = entityFactory;
            this.entityMan = entityMan;
            this.serviceProvider = serviceProvider;
            this.entityTemplateLoader = entityTemplateLoader;
            this.entityClassMan = entityClassMan;
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

        private IEntityClass GetClass(string className)
        {
            if (string.IsNullOrEmpty(className))
            {
                return entityClassMan.RootClass;
            }

            return entityClassMan.GetByName(className);
        }

        public IEntity Build()
        {
            var entityTemplate = entityTemplateLoader.Load(templateName, templateParameters);

            var entityClass = GetClass(entityTemplate.ClassName);

            var components = new List<IEntityComponent>();

            foreach (var componentTemplate in entityTemplate.Components)
            {
                var component = componentTemplate.ToComponent(serviceProvider);

                if (component != null)
                {
                    components.Add(component);
                    continue;
                }
            }

            var newEntity = new Entity(entityMan, tag, entityClass.Id, components);
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