﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Components
{
    public interface IComponentFactory
    {
        public IEntityComponent Create(IComponentTemplate data);
    }
}
