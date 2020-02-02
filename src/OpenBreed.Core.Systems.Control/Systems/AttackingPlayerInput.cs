﻿using OpenBreed.Core.Inputs;
using OpenBreed.Core.Modules.Animation.Systems.Control.Commands;
using OpenBreed.Core.Systems.Control.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Systems.Control.Systems
{
    public class AttackingPlayerInput : IPlayerInput
    {
        public bool OldPrimary { get; set; }
        public bool OldSecondary { get; set; }
        public bool Primary { get; set; }
        public bool Secondary { get; set; }

        public void Reset(Player player)
        {
            Primary = false;
            Secondary = false;
        }

        public void Apply(Player player)
        {
            if (Primary == OldPrimary && Secondary == OldSecondary)
                return;

            foreach (var entity in player.ControlledEntities)
            {
                var control = entity.Components.OfType<AttackControl>().FirstOrDefault();

                if (control == null)
                    continue;

                Console.WriteLine($"{player.Name} -> Attack({Primary},{Secondary})");
                entity.PostCommand(new AttackControlCommand(entity.Id, entity, Primary, Secondary));
            }

            OldPrimary = Primary;
            OldSecondary = Secondary;
        }
    }
}
