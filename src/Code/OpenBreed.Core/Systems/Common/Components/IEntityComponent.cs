using OpenBreed.Core.Entities;
using System;

namespace OpenBreed.Core.Systems.Common.Components
{
    /// <summary>
    /// Component inteface for all entities
    /// </summary>
    public interface IEntityComponent
    {
        #region Public Properties

        /// <summary>
        /// System type that this component is part of
        /// </summary>
        Type SystemType { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Initialize this component
        /// </summary>
        /// <param name="entity">Entity which this component belongs to</param>
        void Initialize(IEntity entity);

        /// <summary>
        /// Deinitialize this component
        /// </summary>
        /// <param name="entity">Entity which this component belongs to</param>
        void Deinitialize(IEntity entity);

        #endregion Public Methods
    }
}