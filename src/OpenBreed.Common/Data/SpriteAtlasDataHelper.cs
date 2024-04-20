using OpenBreed.Model.Maps.Blocks;
using OpenBreed.Model.Sprites;
using OpenBreed.Database.Interface.Items.Sprites;

using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Drawing;

namespace OpenBreed.Common.Data
{
    internal class SpriteAtlasDataHelper
    {
        public static SpriteSetModel FromSprModel(IModelsProvider dataProvider, IDbSpriteAtlasFromSpr entry)
        {
            return dataProvider.GetModel<SpriteSetModel>(entry.DataRef);
        }

        public static SpriteSetModel FromImageModel(
            IModelsProvider dataProvider,
            IBitmapProvider bitmapProvider,
            IDbSpriteAtlasFromImage entry)
        {
            var image = dataProvider.GetModel<IBitmap>(entry.DataRef);

            var spriteSetBuilder = SpriteSetBuilder.NewSpriteSet();

            for (int i = 0; i < entry.Sprites.Count; i++)
            {
                var spriteDef = entry.Sprites[i];

                var spriteBuilder = SpriteBuilder.NewSprite();
                spriteBuilder.SetIndex(i);
                spriteBuilder.SetSize(spriteDef.Width, spriteDef.Height);
                var cutout = new MyRectangle(spriteDef.X, spriteDef.Y, spriteDef.Width, spriteDef.Height);
                var bytes = bitmapProvider.ToBytes(image, cutout);
                spriteBuilder.SetData(bytes);

                spriteSetBuilder.AddSprite(spriteBuilder.Build());
            }


            return spriteSetBuilder.Build();
        }
    }
}
