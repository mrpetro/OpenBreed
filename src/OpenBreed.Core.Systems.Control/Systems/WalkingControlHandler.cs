using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Inputs;
using OpenBreed.Core.Modules.Animation.Systems.Control.Commands;
using OpenBreed.Core.Systems.Control.Components;
using OpenBreed.Core.Systems.Control.Systems;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Modules.Animation.Systems.Control.Systems
{
    public class WalkingControlHandler : IControlHandler
    {
        public const string WALK_LEFT = "Left";
        public const string WALK_RIGHT = "Right";
        public const string WALK_UP = "Up";
        public const string WALK_DOWN = "Down";

        public string ControlType => "Walking";

        public void HandleKeyDown(Player player, float value, string actionName)
        {
        }

        public void HandleKeyUp(Player player, float value, string actionName)
        {
        }

        public void HandleKeyPressed(Player player, string actionName)
        {
            var input = player.Inputs.OfType<WalkingPlayerInput>().FirstOrDefault();

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
