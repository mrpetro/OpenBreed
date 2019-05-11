using OpenBreed.Core.Systems.Control.Components;
using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Systems.Control
{
    /// <summary>
    /// Control system should be used to control actions
    /// </summary>
    public class ControlSystem : WorldSystem<IControlComponent>, IControlSystem
    {
        #region Private Fields

        private List<IMovementController> controllers;

        #endregion Private Fields

        #region Public Constructors

        public ControlSystem()
        {
            controllers = new List<IMovementController>();
        }

        #endregion Public Constructors

        #region Internal Methods

        public override void Update(float dt)
        {
            var keyState = Keyboard.GetState();

            for (int i = 0; i < controllers.Count; i++)
            {
                controllers[i].ProcessInputs(keyState);
            }
        }

        #endregion Internal Methods

        #region Protected Methods

        protected override void AddComponent(IControlComponent component)
        {
            if (component is IMovementController)
                AddMovementControl((IMovementController)component);
            else
                throw new NotImplementedException($"{component}");
        }

        protected override void RemoveComponent(IControlComponent component)
        {
            throw new NotImplementedException();
        }

        #endregion Protected Methods

        #region Private Methods

        private void AddMovementControl(IMovementController controller)
        {
            controllers.Add(controller);
        }

        #endregion Private Methods
    }
}