using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Props
{
    public interface IPropSetEntity : IEntity
    {
        List<IPropertyEntity> Items { get; }
    }
}
