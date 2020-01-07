using OpenBreed.Common.Helpers;
using OpenBreed.Common.Maps;
using OpenBreed.Common.Maps.Blocks;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.Palettes.Builders;
using OpenBreed.Common.Palettes.Readers;
using OpenBreed.Common.Sprites;
using OpenBreed.Common.Sprites.Builders;
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
            var image = provider.GetData(entry.DataRef) as Bitmap;

            var spriteSetBuilder = SpriteSetBuilder.NewSpriteSet();

            for (int i = 0; i < entry.Sprites.Count; i++)
            {
                var spriteDef = entry.Sprites[i];

                var spriteBuilder = SpriteBuilder.NewSprite();
                spriteBuilder.SetIndex(i);
                spriteBuilder.SetSize(spriteDef.Width, spriteDef.Height);
                var cutout = new Rectangle(spriteDef.X, spriteDef.Y, spriteDef.Width, spriteDef.Height);
                var bytes = BitmapHelper.ToBytes(image, cutout);
                spriteBuilder.SetData(bytes);

                spriteSetBuilder.AddSprite(spriteBuilder.Build());
            }


            return spriteSetBuilder.Build();
        }
    }
}
