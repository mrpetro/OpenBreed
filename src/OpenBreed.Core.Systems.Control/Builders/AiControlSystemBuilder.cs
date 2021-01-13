using OpenBreed.Core;
using OpenBreed.Core.Builders;

namespace OpenBreed.Systems.Control.Builders
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