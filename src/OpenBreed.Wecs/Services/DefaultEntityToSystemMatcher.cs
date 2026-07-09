using OpenBreed.Wecs.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Services
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

        public bool AreMatch(ISystem system, IEntity entity)
        {
            if (!systemRequirementsProvider.TryGetRequirements(system.GetType(), out ISystemRequirements requirements))
                return true;

            foreach (var type in requirements.ForbiddenComponents)
            {
                if (entity.ComponentTypes.Any(item => type.IsAssignableFrom(item)))
                    return false;
            }

            foreach (var type in requirements.AllowedComponents)
            {
                if (!entity.ComponentTypes.Any(item => type.IsAssignableFrom(item)))
                    return false;
            }

            if (requirements.Tag is not null)
            {
                if (entity.Tag != requirements.Tag)
                {
                    return false;
                }
            }

            if (requirements.Class is not null)
            {
                if (entity.ClassId != requirements.Class.Id)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion Public Methods
    }
}