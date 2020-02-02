using OpenBreed.Core.Builders;
using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Modules.Rendering.Systems;

namespace OpenBreed.Core.Modules.Physics.Builders
{
    public class SpriteSystemBuilder : IWorldSystemBuilder<SpriteSystem>
    {
        #region Internal Fields

        internal ICore core;

        #endregion Internal Fields

        #region Public Constructors

        public SpriteSystemBuilder(ICore core)
        {
            this.core = core;
        }

        #endregion Public Constructors

        #region Public Methods

        public SpriteSystem Build()
        {
            return new SpriteSystem(this);
        }

        #endregion Public Methods
    }
}