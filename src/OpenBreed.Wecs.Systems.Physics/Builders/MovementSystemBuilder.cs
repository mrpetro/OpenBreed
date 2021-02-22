using OpenBreed.Core;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;

namespace OpenBreed.Wecs.Systems.Physics
{
    public class MovementSystemBuilder : ISystemBuilder<MovementSystem>
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
            return new MovementSystem(this, core.GetManager<IEntityMan>());
        }

        #endregion Public Methods
    }
}