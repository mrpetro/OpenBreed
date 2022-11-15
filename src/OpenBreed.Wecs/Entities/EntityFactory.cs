using OpenBreed.Common.Tools.Xml;
using OpenBreed.Wecs.Components;
using OpenBreed.Wecs.Entities.Xml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace OpenBreed.Wecs.Entities
{
    internal class EntityFactory : IEntityFactory
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IComponentFactoryProvider componentFactoryProvider;
        private readonly Dictionary<Type, IComponentFactory> componentFactories = new Dictionary<Type, IComponentFactory>();

        #endregion Private Fields

        #region Internal Constructors

        public EntityFactory(IEntityMan entityMan, IComponentFactoryProvider componentFactoryProvider)
        {
            this.entityMan = entityMan;
            this.componentFactoryProvider = componentFactoryProvider;
        }

        #endregion Internal Constructors

        #region Public Methods

        public ITemplateEntityBuilder Create(string entityTemplateName)
        {
            return new TemplateEntityBuilder(this, entityMan, componentFactoryProvider, entityTemplateName);
        }

        #endregion Public Methods
    }
}