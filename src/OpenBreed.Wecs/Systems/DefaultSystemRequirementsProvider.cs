using OpenBreed.Common.Interface;
using OpenBreed.Wecs.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace OpenBreed.Wecs.Systems
{
    /// <summary>
    /// Default implementation for ISystemRequirementsProvider interface
    /// </summary>
    public class DefaultSystemRequirementsProvider : ISystemRequirementsProvider
    {
        #region Private Fields

        private static readonly Dictionary<Type, (HashSet<Type> Allowed, HashSet<Type> Forbidden)> requirementsLookup = new Dictionary<Type, (HashSet<Type>, HashSet<Type>)>();

        private readonly ITypeAttributesProvider typeAttributesProvider;

        #endregion Private Fields

        #region Public Constructors

        public DefaultSystemRequirementsProvider(ITypeAttributesProvider typeAttributesProvider)
        {
            this.typeAttributesProvider = typeAttributesProvider;
        }

        #endregion Public Constructors

        #region Public Methods

        public void RegisterRequirements(Type systemType)
        {
            ArgumentNullException.ThrowIfNull(systemType);

            if (!typeof(ISystem).IsAssignableFrom(systemType))
                throw new ArgumentException($"Expected type of {nameof(ISystem)}.");

            var attributes = typeAttributesProvider.GetAttributes(systemType);

            for (int i = 0; i < attributes.Length; i++)
            {
                switch (attributes[i])
                {
                    case RequireEntityWithAttribute requireEntityWithAttribute:
                        foreach (var componentType in requireEntityWithAttribute.ComponentTypes)
                            AddAllowedComponentType(systemType, componentType);
                        break;

                    case RequireEntityWithoutAttribute requireEntityWithoutAttribute:
                        foreach (var componentType in requireEntityWithoutAttribute.ComponentTypes)
                            AddForbiddenComponentType(systemType, componentType);
                        break;

                    default:
                        break;
                }
            }
        }

        public bool TryGetRequirements(Type systemType, out (HashSet<Type> Allowed, HashSet<Type> Forbidden) requirements)
        {
            return requirementsLookup.TryGetValue(systemType, out requirements);
        }

        #endregion Public Methods

        #region Private Methods

        private static void AddAllowedComponentType(Type systemType, Type componentType)
        {
            if (!requirementsLookup.TryGetValue(systemType, out (HashSet<Type> Allowed, HashSet<Type> Forbidden) requirements))
            {
                requirements = new(new HashSet<Type>(), new HashSet<Type>());
                requirementsLookup.Add(systemType, requirements);
            }

            requirements.Allowed.Add(componentType);
        }

        private static void AddForbiddenComponentType(Type systemType, Type componentType)
        {
            if (!requirementsLookup.TryGetValue(systemType, out (HashSet<Type> Allowed, HashSet<Type> Forbidden) requirements))
            {
                requirements = new(new HashSet<Type>(), new HashSet<Type>());
                requirementsLookup.Add(systemType, requirements);
            }

            requirements.Forbidden.Add(componentType);
        }

        #endregion Private Methods
    }
}