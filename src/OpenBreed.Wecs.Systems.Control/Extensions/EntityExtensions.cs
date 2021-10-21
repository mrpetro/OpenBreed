using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Control.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Control.Extensions
{
    public static class EntityExtensions
    {
        public static void StartPrimaryAttack(this Entity entity)
        {
            var control = entity.Get<AttackControlComponent>();

            if (control.AttackPrimary)
                return;

            control.AttackPrimary = true;
            entity.RaiseEvent(new ControlFireChangedEvenrArgs(control.AttackPrimary));
        }

        public static void StopPrimaryAttack(this Entity entity)
        {
            var control = entity.Get<AttackControlComponent>();

            if (!control.AttackPrimary)
                return;

            control.AttackPrimary = false;
            entity.RaiseEvent(new ControlFireChangedEvenrArgs(control.AttackPrimary));
        }
    }
}
