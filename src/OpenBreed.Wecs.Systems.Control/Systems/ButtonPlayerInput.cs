using OpenBreed.Wecs.Components.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Core;
using OpenBreed.Wecs.Systems.Control.Commands;
using OpenBreed.Input.Interface;

namespace OpenBreed.Wecs.Systems.Control.Systems
{
    public class ButtonPlayerInput : IPlayerInput
    {
        public bool OldPrimary { get; set; }
        public bool OldSecondary { get; set; }
        public bool Primary { get; set; }
        public bool Secondary { get; set; }

        public bool Changed => Primary != OldPrimary || Secondary != OldSecondary;

        public void Reset(IPlayer player)
        {
            OldPrimary = Primary;
            OldSecondary = Secondary;
            Primary = false;
            Secondary = false;
        }
    }
}
