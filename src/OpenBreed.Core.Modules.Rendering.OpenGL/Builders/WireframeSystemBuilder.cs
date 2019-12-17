using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Modules.Rendering.Systems;

namespace OpenBreed.Core.Modules.Physics.Builders
{
    public class WireframeSystemBuilder : IWorldSystemBuilder<WireframeSystem>
    {
        #region Internal Fields

        internal ICore core;

        #endregion Internal Fields

        #region Public Constructors

        public WireframeSystemBuilder(ICore core)
        {
            this.core = core;
        }

        #endregion Public Constructors

        #region Public Methods

        public WireframeSystem Build()
        {
            return new WireframeSystem(this);
        }

        #endregion Public Methods
    }
}