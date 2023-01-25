using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Wecs.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Entities
{
    internal class ComponentFactoryProvider : IComponentFactoryProvider
    {
        #region Private Fields

        private readonly Dictionary<Type, IComponentFactory> factories = new Dictionary<Type, IComponentFactory>();

        #endregion Private Fields

        #region Public Constructors

        public ComponentFactoryProvider(IServiceCollection serviceCollection, IServiceProvider serviceProvider)
        {
            var serviceDescriptors = serviceCollection.Where(service => service.ServiceType.IsAssignableTo(typeof(IComponentFactory))).ToList();

            foreach (var descriptor in serviceDescriptors)
            {
                var factoryInterface = descriptor.ServiceType.GetInterfaces().FirstOrDefault(item => item.GenericTypeArguments.Any());

                if (factoryInterface is null)
                    continue;

                var templateType = factoryInterface.GenericTypeArguments.FirstOrDefault(type => type.IsAssignableTo(typeof(IComponentTemplate)));

                if (templateType is null)
                    continue;

                var componentService = (IComponentFactory)serviceProvider.GetService(descriptor.ImplementationType);
                RegisterComponentFactory(templateType, componentService);
            }
        }

        #endregion Public Constructors

        #region Public Methods

        public IComponentFactory GetFactory(Type componentType)
        {
            if (factories.TryGetValue(componentType, out IComponentFactory componentFactory))
                return componentFactory;

            foreach (var componentInterface in componentType.GetInterfaces())
            {
                if (factories.TryGetValue(componentInterface, out componentFactory))
                    return componentFactory;
            }

            return null;
        }

        #endregion Public Methods

        #region Private Methods

        private void RegisterComponentFactory(Type componentTemplateType, IComponentFactory componentFactory)
        {
            factories.Add(componentTemplateType, componentFactory);
        }

        #endregion Private Methods
    }
}