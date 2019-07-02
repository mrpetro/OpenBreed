using OpenBreed.Core.Modules;
using OpenBreed.Core.Modules.Physics.Systems;
using OpenBreed.Core.Systems;
using System;

namespace OpenBreed.Core.Modules.Physics
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
            return new PhysicsSystem(Core, gridWidth, gridHeight);
        }

        #endregion Public Properties
    }
}