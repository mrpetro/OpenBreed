namespace OpenBreed.Wecs.Abstractions.Systems
{
    public interface IOnAddEntityActionSystem : IActionOnTriggerSystem
    {
        #region Public Methods

        /// <summary>
        /// Method that is called when entity is added to world
        /// </summary>
        /// <param name="world">World of entity that is being added to it.</param>
        /// <param name="entity">Entity that is being added.</param>
        void OnAddEntity(IWorld world, IEntity entity);

        #endregion Public Methods
    }
}