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
    public class AttackingPlayerInput : IPlayerInput
    {
        public bool OldPrimary { get; set; }
        public bool OldSecondary { get; set; }
        public bool Primary { get; set; }
        public bool Secondary { get; set; }

        public void Reset(IPlayer player)
        {
            Primary = false;
            Secondary = false;
        }

        public void Apply(IPlayer player)
        {
            if (Primary == OldPrimary && Secondary == OldSecondary)
                return;

            foreach (var entity in player.ControlledEntities)
            {
                var control = entity.TryGet<AttackControl>();

                if (control == null)
                    continue;

                Console.WriteLine($"{player.Name} -> Attack({Primary},{Secondary})");
                entity.Core.Commands.Post(new AttackControlCommand(entity.Id, entity, Primary, Secondary));
            }

            OldPrimary = Primary;
            OldSecondary = Secondary;
        }
    }
}
