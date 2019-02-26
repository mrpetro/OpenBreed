using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Maps
{
    public interface IMapDataBlock
    {
        string Name { get; }
        int Length { get; }

    }
}
