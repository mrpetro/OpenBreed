using OpenBreed.Core.Systems;

namespace OpenBreed.Core.Modules
{
    /// <summary>
    /// Core module interface specialized in physics simulation related work
    /// </summary>
    public interface IPhysicsModule : ICoreModule
    {
        #region Public Methods

        /// <summary>
        /// Creates physics system and return it
        /// </summary>
        /// <param name="gridWidth">Map grid width size</param>
        /// <param name="gridHeight">Map grid height size</param>
        /// <returns>Physics system interface</returns>
        IPhysicsSystem CreatePhysicsSystem(int gridWidth, int gridHeight);

        #endregion Public Methods
    }
}