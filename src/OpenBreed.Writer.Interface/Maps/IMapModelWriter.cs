using OpenBreed.Model.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenBreed.Writer.Interface.Maps
{
    public interface IMapModelWriter
    {
        void Write(MapModel map);
    }
}
