using OpenBreed.Core.Systems.Common.Components;
using OpenTK.Input;

namespace OpenBreed.Core.Systems.Control.Components
{
    public interface IMouseController : IControllerComponent
    {
        #region Public Methods

        void ProcessInputs(float dt, MouseState mouseState);

        #endregion Public Methods
    }
}