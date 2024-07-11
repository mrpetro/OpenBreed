using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Database.Interface.Items.Tiles
{
    public interface IDbTileAtlasFromAcbm : IDbTileAtlas
    {
        string DataRef { get; }
        int TileSize { get; }
        int BitPlanesNo { get; }
    }
}
