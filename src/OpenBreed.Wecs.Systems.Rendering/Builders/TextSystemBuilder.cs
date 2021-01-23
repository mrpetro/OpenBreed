using OpenBreed.Core;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Rendering;

namespace OpenBreed.Wecs.Systems.Rendering.Builders
{
    public class TextSystemBuilder : ISystemBuilder<TextSystem>
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