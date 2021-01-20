using OpenBreed.Core;
using OpenBreed.Core.Systems;
using OpenBreed.Systems.Rendering;

namespace OpenBreed.Systems.Rendering.Builders
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