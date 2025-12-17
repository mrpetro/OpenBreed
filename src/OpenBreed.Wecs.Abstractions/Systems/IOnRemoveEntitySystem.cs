namespace OpenBreed.Wecs.Abstractions.Systems
{    
    /// <summary>
    /// System that updates when entity is removed from world. 
    /// </summary>
    public interface IOnRemoveEntitySystem
    {
        #region Public Methods

        /// <summary>
        /// Method that is called when entity is removed from world
        /// </summary>
        /// <param name="world">World of entity that is being removed from it.</param>
        /// <param name="entity">Entity that is being removed.</param>
        void OnRemoveEntity(IWorld world, IEntity entity);

        #endregion Public Methods
    }
}