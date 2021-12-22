using OpenBreed.Common.Logging;
using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;

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

        public IBroadphaseStatic CreateStatic(int width, int height, int cellSize)
        {
            return new BroadphaseStaticGrid(width, height, cellSize);
        }

        public IBroadphaseDynamic CreateDynamic()
        {
            return new BroadphaseDynamicSwepAndPrune();
        }

        #endregion Public Methods
    }
}