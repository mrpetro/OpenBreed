using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Control;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Core;
using OpenBreed.Input.Interface;
using OpenBreed.Wecs.Systems.Control.Inputs;

namespace OpenBreed.Wecs.Systems.Control.Handlers
{
    public class DigitalJoyInputHandler : IInputHandler
    {
        public const string LEFT = "Left";
        public const string RIGHT = "Right";
        public const string UP = "Up";
        public const string DOWN = "Down";

        public string InputType => "Walking";

        public void HandleKeyDown(IPlayer player, float value, string actionName)
        {
        }

        public void HandleKeyUp(IPlayer player, float value, string actionName)
        {
        }

        public void HandleKeyPressed(IPlayer player, string actionName)
        {
            var input = player.Inputs.OfType<DigitalJoyPlayerInput>().FirstOrDefault();

            if (input == null)
                throw new InvalidOperationException($"Input {input} not registered");

            switch (actionName)
            {
                case LEFT:
                    input.AxisX = -1.0f;
                    break;
                case RIGHT:
                    input.AxisX = 1.0f;
                    break;
                case UP:
                    input.AxisY = 1.0f;
                    break;
                case DOWN:
                    input.AxisY = -1.0f;
                    break;
                default:
                    break;
            }
        }
    }
}
