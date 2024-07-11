using OpenBreed.Model.Tiles;
using OpenBreed.Database.Interface.Items.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Interface.Data;

namespace OpenBreed.Common.Data
{
    internal class TileAtlasDataHelper
    {
        public static TileSetModel FromBlkModel(IModelsProvider dataProvider, IDbTileAtlasFromBlk entry)
        {
            return dataProvider.GetModel<IDbTileAtlasFromBlk, TileSetModel>(entry);
        }

        public static TileSetModel FromImageModel(IModelsProvider dataProvider, IDbTileAtlasFromAcbm entry)
        {
            return null;
        }
    }
}
