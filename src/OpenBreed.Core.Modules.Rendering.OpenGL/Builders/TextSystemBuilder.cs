using OpenBreed.Core.Builders;
using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Modules.Rendering.Systems;

namespace OpenBreed.Core.Modules.Physics.Builders
{
    public class TextSystemBuilder : IWorldSystemBuilder<TextSystem>
    {
        #region Internal Fields

        internal ICore core;

        #endregion Internal Fields

        #region Public Constructors

        public TextSystemBuilder(ICore core)
        {
            this.core = core;
        }

        #endregion Public Constructors

        #region Public Methods

        public TextSystem Build()
        {
            return new TextSystem(this);
        }

        #endregion Public Methods
    }
}