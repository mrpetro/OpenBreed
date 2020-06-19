using OpenBreed.Core.Inputs;
using OpenBreed.Core.Modules.Animation.Systems.Control.Commands;
using OpenBreed.Core.Systems.Control.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Systems.Control.Systems
{
    public class WalkingPlayerInput : IPlayerInput
    {
        public float OldAxisX { get; set; }
        public float OldAxisY { get; set; }
        public float AxisX { get; set; }
        public float AxisY { get; set; }

        public void Reset(Player player)
        {
            AxisX = 0.0f;
            AxisY = 0.0f;
        }

        public void Apply(Player player)
        {
            if (AxisX == OldAxisX && AxisY == OldAxisY)
                return;

            foreach (var entity in player.ControlledEntities)
            {
                var control = entity.TryGet<WalkingControl>();

                if (control == null)
                    continue;

                Console.WriteLine($"{player.Name} -> Walk({AxisX},{AxisY})");
                entity.Core.Commands.Post(new WalkingControlCommand(entity.Id, new OpenTK.Vector2(AxisX, AxisY)));
            }

            OldAxisX = AxisX;
            OldAxisY = AxisY;
        }
    }
}
