using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Model.Maps;

namespace OpenBreed.Common.Readers.Maps
{
    public interface IMapModelReader
    {
        MapModel Read(Stream stream);
    }
}
