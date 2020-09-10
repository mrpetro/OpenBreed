﻿using OpenBreed.Common.Helpers;
using OpenBreed.Common.Model.Maps.Blocks;
using OpenBreed.Common.Model.Sprites;
using OpenBreed.Common.Builders.Sprites;
using OpenBreed.Database.Interface.Items.Sprites;
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
            return provider.GetData<SpriteSetModel>(entry.DataRef);
        }

        public static SpriteSetModel FromImageModel(DataProvider provider, ISpriteSetFromImageEntry entry)
        {
            var image = provider.GetData<Bitmap>(entry.DataRef);

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
