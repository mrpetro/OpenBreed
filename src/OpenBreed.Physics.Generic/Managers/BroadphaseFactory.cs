using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using System.Runtime.CompilerServices;

namespace OpenBreed.Physics.Generic.Managers
{
    internal class BroadphaseFactory : IBroadphaseFactory
    {
        #region Private Fields

        private readonly ILogger logger;

        #endregion Private Fields

        #region Internal Constructors

        public BroadphaseFactory(ILogger logger)
        {
            this.logger = logger;
        }

        #endregion Internal Constructors

        #region Public Methods

        public IBroadphase CreateDynamic(int width, int height, int cellSize)
        {
            return new Broadphase(width, height, cellSize);
        }

        #endregion Public Methods
    }
}