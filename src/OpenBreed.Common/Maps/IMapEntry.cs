using OpenBreed.Common.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Maps
{
    public interface IMapEntry : IEntry
    {
        string DataRef { get; }

        string TileSetRef { get; }
        string ActionSetRef { get; set; }

        List<string> SpriteSetRefs { get; }
        List<string> PaletteRefs { get; }
    }
}
