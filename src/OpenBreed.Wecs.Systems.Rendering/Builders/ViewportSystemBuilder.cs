using OpenBreed.Core;
using OpenBreed.Wecs.Systems;
using OpenBreed.Rendering.Interface;
using OpenBreed.Wecs.Systems.Rendering;
using OpenBreed.Wecs.Entities;

namespace OpenBreed.Wecs.Systems.Rendering.Builders
{
    public class ViewportSystemBuilder : ISystemBuilder<ViewportSystem>
    {
        #region Internal Fields

        internal ICore core;

        #endregion Internal Fields

        #region Public Constructors

        public ViewportSystemBuilder(ICore core)
        {
            this.core = core;
        }

        #endregion Public Constructors

        #region Public Methods

        public ViewportSystem Build()
        {
            return new ViewportSystem(core,
                                      core.GetManager<IEntityMan>(),
                                      core.GetManager<IPrimitiveRenderer>(),
                                      core.GetManager<ICoreClient>());
        }

        #endregion Public Methods
    }
}