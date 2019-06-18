using OpenBreed.Core.Systems.Common.Components;

namespace OpenBreed.Core.Systems
{
    /// <summary>
    /// Interface to system which is part of some world
    /// </summary>
    public interface IWorldSystem : IWorldSystemEx
    {
        #region Public Methods

        /// <summary>
        /// Add the component to this system when entity is added to it's world
        /// </summary>
        /// <param name="component">Component to add</param>
        void AddComponent(IEntityComponent component);

        /// <summary>
        /// Remove the component from this system when entity is being removed from it's world
        /// </summary>
        /// <param name="component">Component to remove</param>
        void RemoveComponent(IEntityComponent component);

        #endregion Public Methods
    }
}