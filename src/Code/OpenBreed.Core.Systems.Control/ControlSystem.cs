using OpenBreed.Core.Systems.Control.Components;
using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Systems.Control
{
    /// <summary>
    /// Control system should be used to control actions
    /// </summary>
    public class ControlSystem : WorldSystem<IControllerComponent>, IControlSystem
    {
        #region Private Fields

        private List<IKeyboardController> keyboardControllers;
        private List<IMouseController> mouseControllers;

        #endregion Private Fields

        #region Public Constructors

        public ControlSystem()
        {
            keyboardControllers = new List<IKeyboardController>();
            mouseControllers = new List<IMouseController>();
        }

        #endregion Public Constructors

        #region Internal Methods

        public override void Update(float dt)
        {
            var keyState = Keyboard.GetState();
            var mouseState = Mouse.GetCursorState();

            for (int i = 0; i < keyboardControllers.Count; i++)
                keyboardControllers[i].ProcessInputs(keyState);

            for (int i = 0; i < mouseControllers.Count; i++)
                mouseControllers[i].ProcessInputs(mouseState);
        }

        #endregion Internal Methods

        #region Protected Methods

        protected override void AddComponent(IControllerComponent component)
        {
            if (component is IKeyboardController)
                AddKeyboardController((IKeyboardController)component);
            else if (component is IMouseController)
                AddMouseController((IMouseController)component);
            else
                throw new NotImplementedException($"{component}");
        }

        protected override void RemoveComponent(IControllerComponent component)
        {
            throw new NotImplementedException();
        }

        #endregion Protected Methods

        #region Private Methods

        private void AddKeyboardController(IKeyboardController controller)
        {
            keyboardControllers.Add(controller);
        }

        private void AddMouseController(IMouseController controller)
        {
            mouseControllers.Add(controller);
        }

        #endregion Private Methods
    }
}