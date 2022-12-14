using System;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems
{
    /// <summary>
    /// Provider for system requirements
    /// </summary>
    public interface ISystemRequirementsProvider
    {
        #region Public Methods

        /// <summary>
        /// Try to get system requirements based on system type
        /// </summary>
        /// <param name="systemType">Type of system</param>
        /// <param name="requirements">output requirements, valid only if method returns true</param>
        /// <returns>True if requirements were found for system of specified type, false otherwise</returns>
        bool TryGetRequirements(Type systemType, out (HashSet<Type> Allowed, HashSet<Type> Forbidden) requirements);

        /// <summary>
        /// Register requirements for system of specified type
        /// </summary>
        /// <param name="systemType">Type of system</param>
        void RegisterRequirements(Type systemType);

        #endregion Public Methods
    }
}