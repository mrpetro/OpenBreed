using OpenBreed.Common.Tools.Xml;
using OpenBreed.Wecs.Entities.Xml;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace OpenBreed.Wecs.Entities
{
    internal class TemplateEntityBuilder : ITemplateEntityBuilder
    {
        #region Private Fields

        private readonly EntityFactory entityFactory;
        private readonly string templateName;
        private readonly Dictionary<string, string> templateParameters = new Dictionary<string, string>();

        #endregion Private Fields

        #region Public Constructors

        public TemplateEntityBuilder(EntityFactory entityFactory, string templateName)
        {
            this.entityFactory = entityFactory;
            this.templateName = templateName;
        }

        #endregion Public Constructors

        #region Public Methods

        public ITemplateEntityBuilder SetParameter<TValue>(string parameterName, TValue parameterValue)
        {
            templateParameters[parameterName] = Convert.ToString(parameterValue, CultureInfo.InvariantCulture);
            return this;
        }

        public Entity Build()
        {
            var entityTemplate = XmlHelper.RestoreFromXml<XmlEntityTemplate>(templateName, templateParameters);

            return entityFactory.Create(entityTemplate);
        }

        #endregion Public Methods
    }
}