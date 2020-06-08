using OpenBreed.Core.Common.Systems.Components;

namespace OpenBreed.Core.Common.Components
{
    public class TextDataComponent : IEntityComponent
    {
        #region Public Constructors

        public TextDataComponent()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public string Data { get; set; }

        #endregion Public Properties
    }
}