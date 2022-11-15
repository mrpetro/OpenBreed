using OpenBreed.Wecs.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenBreed.Wecs.Entities
{
    internal class ComponentFactoryProvider : IComponentFactoryProvider
    {
        #region Private Fields

        private readonly Dictionary<Type, IComponentFactory> factories = new Dictionary<Type, IComponentFactory>();

        #endregion Private Fields

        #region Public Constructors

        public ComponentFactoryProvider()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public IComponentFactory GetFactory(Type componentType)
        {
            if (factories.TryGetValue(componentType, out IComponentFactory componentFactory))
                return componentFactory;

            return null;
        }

        public void RegisterComponentFactory<T>(IComponentFactory factory) where T : IComponentTemplate
        {
            Debug.Assert(!factories.ContainsKey(typeof(T)), $"Component '{typeof(T)}' factory already registered.");

            factories.Add(typeof(T), factory);
        }

        #endregion Public Methods
    }
}