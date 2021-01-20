using OpenBreed.Components.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Core;
using OpenBreed.Systems.Control.Commands;
using OpenBreed.Input.Interface;

namespace OpenBreed.Systems.Control.Systems
{
    public class WalkingPlayerInput : IPlayerInput
    {
        public float OldAxisX { get; set; }
        public float OldAxisY { get; set; }
        public float AxisX { get; set; }
        public float AxisY { get; set; }

        public void Reset(IPlayer player)
        {
            AxisX = 0.0f;
            AxisY = 0.0f;
        }

        public void Apply(IPlayer player)
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
