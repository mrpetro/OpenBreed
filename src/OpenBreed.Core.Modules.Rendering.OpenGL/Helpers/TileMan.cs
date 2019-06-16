using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    public class TileMan : ITileMan
    {
        public ITileAtlas Create(ITexture texture, int tileSize, int tileColumns, int tileRows)
        {
            return new TileAtlas(texture, tileSize, tileColumns, tileRows);
        }

        public ITileAtlas GetById(string id)
        {
            throw new NotImplementedException();
        }

        public void UnloadAll()
        {
            throw new NotImplementedException();
        }
    }
}
