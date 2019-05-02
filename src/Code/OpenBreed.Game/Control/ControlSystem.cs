using OpenBreed.Game.Common;
using OpenBreed.Game.Control.Components;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Control
{
    /// <summary>
    /// Control system should be used to control actions
    /// </summary>
    public class ControlSystem : WorldSystem<IControlComponent>
    {
        private List<MovementController> controllers;

        public ControlSystem()
        {
            controllers = new List<MovementController>();
        }

        private void AddMovementControl(MovementController controller)
        {
            controllers.Add(controller);
        }

        protected override void AddComponent(IControlComponent component)
        {
            if(component is MovementController)
                AddMovementControl((MovementController)component);
            else
                throw new NotImplementedException($"{component}");
        }

        protected override void RemoveComponent(IControlComponent component)
        {
            throw new NotImplementedException();
        }

        internal void ProcessInputs(double dt)
        {
            var keyState = Keyboard.GetState();

            for (int i = 0; i < controllers.Count; i++)
            {
                controllers[i].ProcessInputs(keyState);
            }
        }
    }
}
