using OpenBreed.Model.Tiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Reader.Tiles
{
    public interface ITileSetReader
    {
        TileSetModel Read(Stream stream);
    }
}
