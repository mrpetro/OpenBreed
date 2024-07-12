using OpenBreed.Core;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems
{
    /// <summary>
    /// Interface to system that matches entities based on their components
    /// </summary>
    public interface IMatchingSystem : ISystem
    {
        #region Public Methods

        bool ContainsEntity(IEntity entity);

        void AddEntity(IEntity entity);

        void RemoveEntity(IEntity entity);

        #endregion Public Methods
    }
}