using OpenBreed.Core.Systems.Common.Components;
using OpenBreed.Core.Systems.Control.Components;
using OpenTK.Input;

namespace OpenBreed.Core.Systems.Control.Components
{
    public interface IKeyboardController : IControllerComponent
    {
        #region Public Methods

        void ProcessInputs(KeyboardState keyState);

        #endregion Public Methods
    }
}