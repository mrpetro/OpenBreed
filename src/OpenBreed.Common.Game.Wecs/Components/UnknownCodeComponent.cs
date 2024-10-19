using OpenBreed.Wecs.Components;

namespace OpenBreed.Common.Game.Wecs.Components
{
    public class UnknownCodeComponent : IEntityComponent
    {
        #region Public Constructors

        public UnknownCodeComponent(int code)
        {
            Code = code;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Code { get; }

        #endregion Public Properties
    }
}