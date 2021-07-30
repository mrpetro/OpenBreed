using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Tools;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Model.Palettes;
using OpenBreed.Model.Sprites;
using OpenBreed.Model.Tiles;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;

namespace OpenBreed.Sandbox.Loaders
{
    internal class SpriteAtlasDataLoader : IDataLoader<ISpriteAtlas>
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;
        private readonly AssetsDataProvider assetsDataProvider;
        private readonly ITextureMan textureMan;
        private readonly ISpriteMan spriteMan;

        #endregion Private Fields

        #region Public Constructors

        public SpriteAtlasDataLoader(IRepositoryProvider repositoryProvider,
                                 AssetsDataProvider assetsDataProvider,
                                 ITextureMan textureMan,
                                 ISpriteMan spriteMan)
        {
            this.repositoryProvider = repositoryProvider;
            this.assetsDataProvider = assetsDataProvider;
            this.textureMan = textureMan;
            this.spriteMan = spriteMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public object LoadObject(string entryId) => Load(entryId);

        public ISpriteAtlas Load(string entryId, params object[] args)
        {
            var paletteModel = args.FirstOrDefault() as PaletteModel;

            var entry = repositoryProvider.GetRepository<IDbSpriteAtlas>().GetById(entryId) as IDbSpriteAtlasFromSpr;
            if (entry == null)
                throw new Exception("Sprite atlas error: " + entryId);

            var spriteSet = assetsDataProvider.LoadModel(entry.DataRef) as SpriteSetModel;

            foreach (var sprite in spriteSet.Sprites)
            {
                var spriteAtlasKey = $"{entryId}#{sprite.Index}";
                var bitmap = ToBitmap(sprite.Width, sprite.Height, sprite.Data);

                if (paletteModel != null)
                    BitmapHelper.SetPaletteColors(bitmap, paletteModel.Data);

                var texture = textureMan.Create(spriteAtlasKey, bitmap);

                spriteMan.CreateAtlas()
                    .SetName(spriteAtlasKey)
                    .SetTexture(texture.Id)
                    .AppendCoordsFromGrid(sprite.Width, sprite.Height, 1, 1)
                    .Build();
            }

            return null;
        }

        public static Bitmap ToBitmap(int width, int height, byte[] data)
        {
            var bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            //Create a BitmapData and Lock all pixels to be written
            var bmpData = bitmap.LockBits(new Rectangle(0, 0, width, height),
                                                    ImageLockMode.WriteOnly, bitmap.PixelFormat);

            //Copy the data from the byte array into BitmapData.Scan0
            Marshal.Copy(data, 0, bmpData.Scan0, data.Length);

            //Unlock the pixels
            bitmap.UnlockBits(bmpData);
            return bitmap;
        }

        #endregion Public Methods
    }
}