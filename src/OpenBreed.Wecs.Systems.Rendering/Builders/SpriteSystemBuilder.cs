using OpenBreed.Core;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Rendering;

namespace OpenBreed.Wecs.Systems.Rendering.Builders
{
    public class SpriteSystemBuilder : ISystemBuilder<SpriteSystem>
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