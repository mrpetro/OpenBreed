using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Tools;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Model.Palettes;
using OpenBreed.Model.Tiles;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using System;
using System.Drawing;
using System.Linq;

namespace OpenBreed.Sandbox.Loaders
{
    internal class TileAtlasDataLoader : IDataLoader<ITileAtlas>
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;
        private readonly AssetsDataProvider assetsDataProvider;
        private readonly ITextureMan textureMan;
        private readonly ITileMan tileMan;

        #endregion Private Fields

        #region Public Constructors

        public TileAtlasDataLoader(IRepositoryProvider repositoryProvider,
                                 AssetsDataProvider assetsDataProvider,
                                 ITextureMan textureMan,
                                 ITileMan tileMan)
        {
            this.repositoryProvider = repositoryProvider;
            this.assetsDataProvider = assetsDataProvider;
            this.textureMan = textureMan;
            this.tileMan = tileMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public object LoadObject(string entryId) => Load(entryId);

        public ITileAtlas Load(string entryId, params object[] args)
        {
            var paletteModel = args.FirstOrDefault() as PaletteModel;

            var entry = repositoryProvider.GetRepository<IDbTileAtlas>().GetById(entryId) as IDbTileAtlasFromBlk;
            if (entry == null)
                throw new Exception("Tile atlas error: " + entryId);

            var tileAtlas = assetsDataProvider.LoadModel(entry.DataRef) as TileSetModel;

            if(paletteModel != null)
                BitmapHelper.SetPaletteColors(tileAtlas.Bitmap, paletteModel.Data);

            var texture = textureMan.Create(entry.DataRef, tileAtlas.Bitmap);

            var builder = tileMan.CreateAtlas()
                                 .SetName(entryId)
                                 .SetTexture(texture.Id)
                                 .SetTileSize(tileAtlas.TileSize);

            foreach (var tile in tileAtlas.Tiles)
                builder.AppendCoords(tile.Rectangle.X, tile.Rectangle.Y);

            return builder.Build();


            //return tileMan.Create(entryId, texture.Id, tileAtlas.TileSize, tileAtlas.Tiles.Select(tile => new Point(tile.Rectangle.X, tile.Rectangle.Y)).ToArray()); ;
        }

        #endregion Public Methods
    }
}