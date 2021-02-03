using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Systems.Control.Systems;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Core;
using OpenBreed.Input.Interface;

namespace OpenBreed.Wecs.Systems.Control.Systems
{
    public class DigitalJoyInputHandler : IInputHandler
    {
        public const string WALK_LEFT = "Left";
        public const string WALK_RIGHT = "Right";
        public const string WALK_UP = "Up";
        public const string WALK_DOWN = "Down";

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
                case WALK_LEFT:
                    input.AxisX = -1.0f;
                    break;
                case WALK_RIGHT:
                    input.AxisX = 1.0f;
                    break;
                case WALK_UP:
                    input.AxisY = 1.0f;
                    break;
                case WALK_DOWN:
                    input.AxisY = -1.0f;
                    break;
                default:
                    break;
            }
        }
    }
}
