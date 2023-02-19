﻿using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Tools;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Model;
using OpenBreed.Model.Palettes;
using OpenBreed.Model.Sprites;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Data;
using OpenBreed.Rendering.Interface.Managers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.OpenGL.Data
{
    internal class SpriteAtlasDataLoader : ISpriteAtlasDataLoader
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;
        private readonly AssetsDataProvider assetsDataProvider;
        private readonly ITextureMan textureMan;
        private readonly ISpriteMan spriteMan;
        private readonly ISpriteMerger spriteMerger;

        #endregion Private Fields

        #region Public Constructors

        public SpriteAtlasDataLoader(IRepositoryProvider repositoryProvider,
                                 AssetsDataProvider assetsDataProvider,
                                 ITextureMan textureMan,
                                 ISpriteMan spriteMan,
                                 ISpriteMerger spriteMerger)
        {
            this.repositoryProvider = repositoryProvider;
            this.assetsDataProvider = assetsDataProvider;
            this.textureMan = textureMan;
            this.spriteMan = spriteMan;
            this.spriteMerger = spriteMerger;
        }

        #endregion Public Constructors

        #region Public Methods

        public object LoadObject(string entryId) => Load(entryId);

        public ISpriteAtlas Load(string entryId, params object[] args)
        {
            if (spriteMan.GetByName(entryId) != null)
                return null;

            var paletteModel = args.FirstOrDefault() as PaletteModel;

            var entry = repositoryProvider.GetRepository<IDbSpriteAtlas>().GetById(entryId) as IDbSpriteAtlasFromSpr;
            if (entry == null)
                throw new Exception("Sprite atlas error: " + entryId);

            var spriteSet = assetsDataProvider.LoadModel(entry.DataRef) as SpriteSetModel;

            byte[] outData = default;
            int outWidth = -1;
            int outHeight = -1;
            List<(int X, int Y, int Width, int Height)> bounds = default;
            spriteMerger.Merge(spriteSet.Sprites, out outData, out outWidth, out outHeight, out bounds);

            var bitmap = BitmapHelper.FromBytes(outWidth, outHeight, outData);
            if (paletteModel != null)
            {
                //BitmapHelper.DumpColors(@"D:\c4pal.pal", paletteModel.Data);
                //TODO: Pass mask color with SpriteSetModel
                paletteModel.Data[0] = Color.FromArgb(0, 0, 0, 0);

                //paletteModel.SetColors(0, paletteModel.Data.Skip(16).Take(32).ToArray());

                BitmapHelper.SetPaletteColors(bitmap, paletteModel.Data);
            }

            var texture = textureMan.Create(entryId, bitmap);

            var atlasBuilder = spriteMan.CreateAtlas().SetName(entryId)
                .SetTexture(texture.Id);

            for (int i = 0; i < bounds.Count; i++)
            {
                var bound = bounds[i];

                atlasBuilder.AppendCoord(
                    bound.X,
                    bound.Y,
                    bound.Width,
                    bound.Height);

            }

            return atlasBuilder.Build(); ;
        }

        #endregion Public Methods
    }
}
