using OpenBreed.Core.Systems.Common.Components;
using OpenTK.Input;

namespace OpenBreed.Core.Systems.Control
{
    public interface IControlComponent : IEntityComponent
    {
        #region Public Methods

        void ProcessInputs(KeyboardState keyState);

        #endregion Public Methods
    }
}