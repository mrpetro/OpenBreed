using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Modules.Animation.Systems.Control.Systems;
using OpenBreed.Core.Modules.Rendering.Systems;

namespace OpenBreed.Core.Modules.Physics.Builders
{
    public class AiControlSystemBuilder : IWorldSystemBuilder<AiControlSystem>
    {
        #region Internal Fields

        internal ICore core;

        #endregion Internal Fields

        #region Public Constructors

        public AiControlSystemBuilder(ICore core)
        {
            this.core = core;
        }

        #endregion Public Constructors

        #region Public Methods

        public AiControlSystem Build()
        {
            return new AiControlSystem(this);
        }

        #endregion Public Methods
    }
}