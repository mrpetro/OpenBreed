using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Systems
{
    /// <summary>
    /// Default implementation for IEntityToSystemMatcher interface
    /// </summary>
    public class DefaultEntityToSystemMatcher : IEntityToSystemMatcher
    {
        #region Private Fields

        private readonly ISystemRequirementsProvider systemRequirementsProvider;

        #endregion Private Fields

        #region Public Constructors

        public DefaultEntityToSystemMatcher(ISystemRequirementsProvider systemRequirementsProvider)
        {
            this.systemRequirementsProvider = systemRequirementsProvider;
        }

        #endregion Public Constructors

        #region Public Methods

        public bool AreMatch(IMatchingSystem system, IEntity entity)
        {
            if (!systemRequirementsProvider.TryGetRequirements(system.GetType(), out (HashSet<Type> Allowed, HashSet<Type> Forbidden) requirements))
                return true;

            foreach (var type in requirements.Forbidden)
            {
                if (entity.ComponentTypes.Any(item => type.IsAssignableFrom(item)))
                    return false;
            }

            foreach (var type in requirements.Allowed)
            {
                if (!entity.ComponentTypes.Any(item => type.IsAssignableFrom(item)))
                    return false;
            }

            return true;
        }

        #endregion Public Methods
    }
}