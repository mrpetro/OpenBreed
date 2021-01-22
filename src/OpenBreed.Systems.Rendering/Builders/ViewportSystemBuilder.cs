using OpenBreed.Core;
using OpenBreed.Ecsw.Systems;
using OpenBreed.Rendering.Interface;
using OpenBreed.Systems.Rendering;

namespace OpenBreed.Systems.Rendering.Builders
{
    public class ViewportSystemBuilder : IWorldSystemBuilder<ViewportSystem>
    {
        #region Internal Fields

        internal ICore core;
        internal IRenderModule renderModule;

        #endregion Internal Fields

        #region Public Constructors

        public ViewportSystemBuilder(ICore core)
        {
            this.core = core;
            this.renderModule = core.GetModule<IRenderModule>();
        }

        #endregion Public Constructors

        #region Public Methods

        public ViewportSystem Build()
        {
            return new ViewportSystem(this);
        }

        #endregion Public Methods
    }
}