using OpenBreed.Common.Logging;
using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;

namespace OpenBreed.Physics.Generic.Managers
{
    public class BroadphaseGridFactory : IBroadphaseGridFactory
    {
        #region Private Fields

        private readonly ILogger logger;

        #endregion Private Fields

        #region Internal Constructors

        internal BroadphaseGridFactory(ILogger logger)
        {
            this.logger = logger;
        }

        #endregion Internal Constructors

        #region Public Methods

        public IBroadphaseGrid CreateGrid(int width, int height, int cellSize)
        {
            return new BroadphaseGrid(width, height, cellSize);
        }

        #endregion Public Methods
    }
}