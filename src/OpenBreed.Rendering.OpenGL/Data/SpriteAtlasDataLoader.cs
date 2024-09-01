using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
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
        private readonly SpriteAtlasDataProvider spriteAtlasDataProvider;
        private readonly ITextureMan textureMan;
        private readonly ISpriteMan spriteMan;
        private readonly ISpriteMerger spriteMerger;

        #endregion Private Fields

        #region Public Constructors

        public SpriteAtlasDataLoader(IRepositoryProvider repositoryProvider,
                                 SpriteAtlasDataProvider spriteAtlasDataProvider,
                                 ITextureMan textureMan,
                                 ISpriteMan spriteMan,
                                 ISpriteMerger spriteMerger)
        {
            this.repositoryProvider = repositoryProvider;
            this.spriteAtlasDataProvider = spriteAtlasDataProvider;
            this.textureMan = textureMan;
            this.spriteMan = spriteMan;
            this.spriteMerger = spriteMerger;
        }

        #endregion Public Constructors

        #region Public Methods

        public ISpriteAtlas Load(IDbSpriteAtlas dbSpriteAtlas)
        {
            if (spriteMan.TryGetByName(dbSpriteAtlas.Id, out ISpriteAtlas spriteAtlas))
            {
                return spriteAtlas;
            }

            var spriteSet = spriteAtlasDataProvider.GetSpriteAtlas(dbSpriteAtlas);

            spriteMerger.Merge(
                spriteSet.Sprites,
                out byte[] outData,
                out int outWidth,
                out int outHeight,
                out List<(int X, int Y, int Width, int Height)> bounds);

            var texture = textureMan.Create(dbSpriteAtlas.Id, outWidth, outHeight, outData, maskIndex: 0);

            var atlasBuilder = spriteMan.CreateAtlas()
                .SetName(dbSpriteAtlas.Id)
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

            spriteAtlas = atlasBuilder.Build();

            return spriteAtlas;
        }

        public ISpriteAtlas Load(string dbEntryId)
        {
            if (spriteMan.TryGetByName(dbEntryId, out ISpriteAtlas spriteAtlas))
            {
                return spriteAtlas;
            }

            var entry = repositoryProvider.GetRepository<IDbSpriteAtlas>().GetById(dbEntryId);

            if (entry is null)
            {
                throw new Exception("Sprite atlas error: " + dbEntryId);
            }

            return Load(entry);
        }

        #endregion Public Methods
    }
}
