using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenBreed.Common.Maps.Writers
{
    public interface IMapModelWriter
    {
        void Write(MapModel map);
    }
}
