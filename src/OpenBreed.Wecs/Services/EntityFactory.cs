using OpenBreed.Common.Tools.Xml;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Components;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Entities.Xml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace OpenBreed.Wecs.Services
{
    internal class EntityFactory : IEntityFactory
    {
        #region Private Fields

        private readonly EntityMan entityMan;
        private readonly IEntityTemplateLoader entityTemplateLoader;
        private readonly IServiceProvider serviceProvider;

        #endregion Private Fields

        #region Internal Constructors

        public EntityFactory(
            EntityMan entityMan,
            IEntityTemplateLoader entityTemplateLoader,
            IServiceProvider serviceProvider)
        {
            this.entityMan = entityMan;
            this.entityTemplateLoader = entityTemplateLoader;
            this.serviceProvider = serviceProvider;
        }

        #endregion Internal Constructors

        #region Public Methods

        public ITemplateEntityBuilder Create(string entityTemplateName)
        {
            return new TemplateEntityBuilder(
                this,
                entityMan,
                serviceProvider,
                entityTemplateLoader,
                entityTemplateName);
        }

        #endregion Public Methods
    }
}