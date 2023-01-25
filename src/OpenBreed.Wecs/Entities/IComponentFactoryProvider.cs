using OpenBreed.Wecs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Entities
{
    public interface IComponentFactoryProvider
    {
        IComponentFactory GetFactory(Type componentType);
    }
}
