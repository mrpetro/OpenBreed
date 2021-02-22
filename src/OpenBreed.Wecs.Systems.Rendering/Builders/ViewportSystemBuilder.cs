using OpenBreed.Core;
using OpenBreed.Wecs.Systems;
using OpenBreed.Rendering.Interface;
using OpenBreed.Wecs.Systems.Rendering;

namespace OpenBreed.Wecs.Systems.Rendering.Builders
{
    public class ViewportSystemBuilder : ISystemBuilder<ViewportSystem>
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
            return new ViewportSystem(this,
                                      core.GetManager<IPrimitiveRenderer>());
        }

        #endregion Public Methods
    }
}