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
        private List<IAutoController> autoControllers;

        #endregion Private Fields

        #region Public Constructors

        public ControlSystem(ICore core) : base(core)
        {
            keyboardControllers = new List<IKeyboardController>();
            mouseControllers = new List<IMouseController>();
            autoControllers = new List<IAutoController>();
        }

        #endregion Public Constructors

        #region Internal Methods

        public override void Update(float dt)
        {
            var keyState = Keyboard.GetState();
            var mouseState = Mouse.GetCursorState();

            for (int i = 0; i < keyboardControllers.Count; i++)
                keyboardControllers[i].ProcessInputs(dt, keyState);

            for (int i = 0; i < mouseControllers.Count; i++)
                mouseControllers[i].ProcessInputs(dt, mouseState);

            for (int i = 0; i < autoControllers.Count; i++)
                autoControllers[i].Update(dt);
        }

        #endregion Internal Methods

        #region Protected Methods

        protected override void AddComponent(IControllerComponent component)
        {
            if (component is IKeyboardController)
                AddKeyboardController((IKeyboardController)component);
            else if (component is IMouseController)
                AddMouseController((IMouseController)component);
            else if (component is IAutoController)
                AddAutoController((IAutoController)component);
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

        private void AddAutoController(IAutoController controller)
        {
            autoControllers.Add(controller);
        }

        #endregion Private Methods
    }
}