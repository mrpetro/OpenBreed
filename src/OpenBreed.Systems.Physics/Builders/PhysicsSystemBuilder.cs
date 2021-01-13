using OpenBreed.Core;
using OpenBreed.Core.Builders;

namespace OpenBreed.Systems.Physics
{
    public class PhysicsSystemBuilder : IWorldSystemBuilder<PhysicsSystem>
    {
        #region Internal Fields

        internal ICore core;
        internal int gridWidth;
        internal int gridHeight;

        #endregion Internal Fields

        #region Public Constructors

        public PhysicsSystemBuilder(ICore core)
        {
            this.core = core;
        }

        #endregion Public Constructors

        #region Public Methods

        public PhysicsSystemBuilder SetGridSize(int width, int height)
        {
            gridWidth = width;
            gridHeight = height;

            return this;
        }

        public PhysicsSystem Build()
        {
            return new PhysicsSystem(this);
        }

        #endregion Public Methods
    }
}