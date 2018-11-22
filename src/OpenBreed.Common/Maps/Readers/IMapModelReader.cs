using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace OpenBreed.Common.Maps.Readers
{
    public interface IMapModelReader
    {
        MapModel Read(Stream stream);
    }
}
