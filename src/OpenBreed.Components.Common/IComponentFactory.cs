using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Components.Common
{
    public interface IComponentFactory
    {
        IEntityComponent Create(IComponentTemplate data);
    }
}
