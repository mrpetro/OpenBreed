using OpenBreed.Core;
using OpenBreed.Ecsw.Systems;

namespace OpenBreed.Systems.Physics
{
    public class MovementSystemBuilder : IWorldSystemBuilder<MovementSystem>
    {
        #region Internal Fields

        internal ICore core;

        #endregion Internal Fields

        #region Public Constructors

        public MovementSystemBuilder(ICore core)
        {
            this.core = core;
        }

        #endregion Public Constructors

        #region Public Methods

        public MovementSystem Build()
        {
            return new MovementSystem(this);
        }

        #endregion Public Methods
    }
}