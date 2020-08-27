using OpenBreed.Database.Interface.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface.Items.Tiles
{
    public interface ITileSetEntry : IEntry
    {
        List<string> PaletteRefs { get; }
    }
}
