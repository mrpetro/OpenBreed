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
using OpenBreed.Input.Generic;

namespace OpenBreed.Wecs.Systems.Control.Systems
{
    public class AttackControlHandler : IInputHandler
    {
        public const string ATTACK_PRIMARY = "Primary";
        public const string ATTACK_SECONDARY = "Secondary";

        public string InputType => "Attacking";

        public void HandleKeyDown(IPlayer player, float value, string actionName)
        {
        }

        public void HandleKeyUp(IPlayer player, float value, string actionName)
        {
        }

        public void HandleKeyPressed(IPlayer player, string actionName)
        {
            var input = player.Inputs.OfType<ButtonPlayerInput>().FirstOrDefault();

            if (input == null)
                throw new InvalidOperationException($"Input {input} not registered");

            switch (actionName)
            {
                case ATTACK_PRIMARY:
                    input.Primary = true;
                    break;
                case ATTACK_SECONDARY:
                    input.Secondary = true;
                    break;
                default:
                    break;
            }
        }
    }
}
