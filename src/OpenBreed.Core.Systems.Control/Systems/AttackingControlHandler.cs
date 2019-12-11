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
    public class AttackControlHandler : IControlHandler
    {
        public const string ATTACK_PRIMARY = "Primary";
        public const string ATTACK_SECONDARY = "Secondary";

        public string ControlType => "Attacking";

        public void HandleKeyDown(Player player, float value, string actionName)
        {
        }

        public void HandleKeyUp(Player player, float value, string actionName)
        {
        }

        public void HandleKeyPressed(Player player, string actionName)
        {
            var input = player.Inputs.OfType<AttackingPlayerInput>().FirstOrDefault();

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
