using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Props
{
    public interface IPropSetEntry : IEntry
    {
        List<IPropertyEntry> Items { get; }
    }
}
