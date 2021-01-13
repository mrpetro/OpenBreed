using OpenBreed.Core;
using OpenBreed.Core.Modules.Rendering.Systems;
using OpenBreed.Systems.Physics;

namespace OpenBreed.Systems.Rendering
{
    public class PhysicsSystemsFactory
    {
        #region Private Fields

        private readonly ICore core;
        private readonly MovementSystemBuilder movementSystemBuilder;
        private readonly PhysicsSystemBuilder physicsSystemBuilder;

        #endregion Private Fields

        #region Public Constructors

        public PhysicsSystemsFactory(ICore core)
        {
            this.core = core;
            movementSystemBuilder = new MovementSystemBuilder(core);
            physicsSystemBuilder = new PhysicsSystemBuilder(core);
        }

        #endregion Public Constructors

        #region Public Methods

        public MovementSystem CreateMovementSystem()
        {
            return movementSystemBuilder.Build();
        }

        public PhysicsSystem CreatePhysicsSystem()
        {
            return physicsSystemBuilder.Build();
        }

        #endregion Public Methods
    }
}