namespace OpenBreed.Game.Entities
{
    public interface IWorldEntity : IEntity
    {
        #region Public Properties

        /// <summary>
        /// World that this entity is currently in
        /// </summary>
        World CurrentWorld { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Method that performs entering world given in the argument
        /// </summary>
        /// <param name="world">World to enter</param>
        void EnterWorld(World world);

        /// <summary>
        /// Method that performs leaving current world
        /// </summary>
        void LeaveWorld();

        #endregion Public Methods
    }
}