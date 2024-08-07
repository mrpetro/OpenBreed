﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components
{
    public interface IComponentFactory
    {
        IEntityComponent Create(IComponentTemplate data);
    }

    public interface IComponentFactory<TComponentTemplate> : IComponentFactory where TComponentTemplate : IComponentTemplate
    {
    }
}
