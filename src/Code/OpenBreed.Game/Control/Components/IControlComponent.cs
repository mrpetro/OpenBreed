﻿using OpenBreed.Game.Common.Components;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Control.Components
{
    public interface IControlComponent : IEntityComponent
    {
        void ProcessInputs(KeyboardState keyState);
    }
}
