using OpenBreed.Common.Maps;
using OpenBreed.Common.Maps.Blocks;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.Palettes.Builders;
using OpenBreed.Common.Palettes.Readers;
using OpenBreed.Common.Sprites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Data
{
    internal class SpriteSetsDataHelper
    {
        public static SpriteSetModel FromSprModel(DataProvider provider, ISpriteSetFromSprEntry entry)
        {
            return provider.GetData(entry.DataRef) as SpriteSetModel;
        }

        public static SpriteSetModel FromImageModel(DataProvider provider, ISpriteSetFromImageEntry entry)
        {
            return null;
        }
    }
}
