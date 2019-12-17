using OpenBreed.Core.Builders;
using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Modules.Animation.Systems.Control.Systems;
using OpenBreed.Core.Modules.Rendering.Systems;

namespace OpenBreed.Core.Modules.Physics.Builders
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