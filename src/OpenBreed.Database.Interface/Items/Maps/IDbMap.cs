using OpenBreed.Database.Interface.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Interface.Items.Maps
{
    public interface IDbMap : IDbEntry
    {
        string DataRef { get; }

        string Format { get; }

        string TileSetRef { get; }
        string ActionSetRef { get; set; }
        string ScriptRef { get; set; }

        List<string> SpriteSetRefs { get; }
        List<string> PaletteRefs { get; }
    }
}
