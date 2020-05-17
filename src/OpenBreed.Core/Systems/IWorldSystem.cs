using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;
using OpenBreed.Core.Entities;

namespace OpenBreed.Core.Systems
{
    /// <summary>
    /// Interface to system which is part of some world
    /// </summary>
    public interface IWorldSystem : ISystem
    {
        #region Public Methods

        /// <summary>
        /// World which owns this system
        /// </summary>
        World World { get; }

        /// <summary>
        /// Initialize the system when world is created
        /// </summary>
        /// <param name="world">World that this system is initialized on</param>
        void Initialize(World world);

        #endregion Public Methods
    }
}