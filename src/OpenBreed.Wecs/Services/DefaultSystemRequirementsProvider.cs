using OpenBreed.Common.Interface;
using OpenBreed.Wecs.Abstractions.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace OpenBreed.Wecs.Services
{
    /// <summary>
    /// Default implementation for ISystemRequirementsProvider interface
    /// </summary>
    public class DefaultSystemRequirementsProvider : ISystemRequirementsProvider
    {
        #region Private Fields

        private static readonly Dictionary<Type, DefaultSystemRequirements> requirementsLookup = new Dictionary<Type, DefaultSystemRequirements>();

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
            {
                throw new ArgumentException($"Expected type of {nameof(ISystem)}.");
            }

            var attributes = typeAttributesProvider.GetAttributes(systemType);

            for (int i = 0; i < attributes.Length; i++)
            {
                switch (attributes[i])
                {
                    case RequireEntityWithTagAttribute requireEntityWithTagAttribute:

                        AddTag(systemType, requireEntityWithTagAttribute.Tag);

                        break;

                    case RequireEntityWithAttribute requireEntityWithAttribute:
                        foreach (var componentType in requireEntityWithAttribute.ComponentTypes)
                        {
                            AddAllowedComponentType(systemType, componentType);
                        }

                        break;

                    case RequireEntityWithoutAttribute requireEntityWithoutAttribute:
                        foreach (var componentType in requireEntityWithoutAttribute.ComponentTypes)
                        {
                            AddForbiddenComponentType(systemType, componentType);
                        }

                        break;

                    default:
                        break;
                }
            }
        }

        public bool TryGetRequirements(Type systemType, out ISystemRequirements requirements)
        {
            if (!requirementsLookup.TryGetValue(systemType, out DefaultSystemRequirements defaultRequirements))
            {
                requirements = null;
                return false;
            }

            requirements = defaultRequirements;
            return true;
        }

        #endregion Public Methods

        #region Private Methods

        private static DefaultSystemRequirements GetRequirements(Type systemType)
        {
            if (!requirementsLookup.TryGetValue(systemType, out DefaultSystemRequirements requirements))
            {
                requirements = new DefaultSystemRequirements();
                requirementsLookup.Add(systemType, requirements);
            }

            return requirements;
        }

        private static void AddAllowedComponentType(Type systemType, Type componentType)
        {
            var requirements = GetRequirements(systemType);

            requirements.AllowedComponents.Add(componentType);
        }

        private static void AddForbiddenComponentType(Type systemType, Type componentType)
        {
            var requirements = GetRequirements(systemType);

            requirements.ForbiddenComponents.Add(componentType);
        }

        private static void AddTag(Type systemType, string tag)
        {
            var requirements = GetRequirements(systemType);

            requirements.Tag = tag;
        }

        #endregion Private Methods
    }

    internal class DefaultSystemRequirements : ISystemRequirements
    {
        #region Public Properties

        public HashSet<Type> AllowedComponents { get; } = new HashSet<Type>();

        public HashSet<Type> ForbiddenComponents { get; } = new HashSet<Type>();

        IReadOnlySet<Type> ISystemRequirements.AllowedComponents => AllowedComponents;

        IReadOnlySet<Type> ISystemRequirements.ForbiddenComponents => ForbiddenComponents;

        public string Tag { get; set; }

        #endregion Public Properties
    }
}