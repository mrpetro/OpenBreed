using OpenBreed.Database.Interface.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface.Items.Maps
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
