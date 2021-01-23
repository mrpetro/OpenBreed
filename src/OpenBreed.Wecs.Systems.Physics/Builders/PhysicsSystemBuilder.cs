using OpenBreed.Core;
using OpenBreed.Wecs.Systems;

namespace OpenBreed.Wecs.Systems.Physics
{
    public class PhysicsSystemBuilder : ISystemBuilder<PhysicsSystem>
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