﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Control
{
    public class BaseControl 
    {
        protected BaseControl()
        {
        }

        public bool ShowMenu { get; }
        public bool PrimaryAction { get; }
        public bool SecondaryAction { get; }
    }
}
