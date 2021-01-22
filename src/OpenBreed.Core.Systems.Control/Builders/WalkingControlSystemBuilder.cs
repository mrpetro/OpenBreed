using OpenBreed.Core;
using OpenBreed.Ecsw.Systems;

namespace OpenBreed.Systems.Control.Builders
{
    public class WalkingControlSystemBuilder : IWorldSystemBuilder<WalkingControlSystem>
    {
        #region Internal Fields

        internal ICore core;

        #endregion Internal Fields

        #region Public Constructors

        public WalkingControlSystemBuilder(ICore core)
        {
            this.core = core;
        }

        #endregion Public Constructors

        #region Public Methods

        public WalkingControlSystem Build()
        {
            return new WalkingControlSystem(this);
        }

        #endregion Public Methods
    }
}