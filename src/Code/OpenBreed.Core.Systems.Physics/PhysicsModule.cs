using OpenBreed.Core.Modules;
using System;

namespace OpenBreed.Core.Systems.Physics
{
    public class PhysicsModule : IPhysicsModule
    {
        #region Public Constructors

        public PhysicsModule(ICore core)
        {
            Core = core ?? throw new ArgumentNullException(nameof(core));
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        /// <summary>
        /// Creates physics system and return it
        /// </summary>
        /// <param name="gridWidth">Map grid width size</param>
        /// <param name="gridHeight">Map grid height size</param>
        /// <returns>Physics system interface</returns>
        public IPhysicsSystem CreatePhysicsSystem(int gridWidth, int gridHeight)
        {
            return new PhysicsSystem(gridWidth, gridHeight);
        }

        #endregion Public Properties
    }
}