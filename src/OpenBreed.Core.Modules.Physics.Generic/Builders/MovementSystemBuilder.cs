using OpenBreed.Core.Builders;
using OpenBreed.Core.Modules.Physics.Systems;

namespace OpenBreed.Core.Modules.Physics.Builders
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