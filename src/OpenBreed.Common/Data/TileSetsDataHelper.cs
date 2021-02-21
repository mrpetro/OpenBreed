using OpenBreed.Model.Tiles;
using OpenBreed.Database.Interface.Items.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Data
{
    internal class TileSetsDataHelper
    {
        public static TileSetModel FromBlkModel(IModelsProvider dataProvider, ITileSetFromBlkEntry entry)
        {
            return dataProvider.GetModel<TileSetModel>(entry.DataRef);
        }

        public static TileSetModel FromImageModel(IModelsProvider dataProvider, ITileSetFromImageEntry entry)
        {
            return null;
        }
    }
}
