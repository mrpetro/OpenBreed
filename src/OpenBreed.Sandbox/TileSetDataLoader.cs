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

namespace OpenBreed.Sandbox
{
    internal class TileSetDataLoader : IDataLoader<ITileAtlas>
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;
        private readonly AssetsDataProvider assetsDataProvider;
        private readonly ITextureMan textureMan;
        private readonly ITileMan tileMan;

        #endregion Private Fields

        #region Public Constructors

        public TileSetDataLoader(IRepositoryProvider repositoryProvider,
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

            var entry = repositoryProvider.GetRepository<ITileSetEntry>().GetById(entryId) as ITileSetFromBlkEntry;
            if (entry == null)
                throw new Exception("Tileset error: " + entryId);

            var tileSet = assetsDataProvider.LoadModel(entry.DataRef) as TileSetModel;

            if(paletteModel != null)
                BitmapHelper.SetPaletteColors(tileSet.Bitmap, paletteModel.Data);

            var texture = textureMan.Create(entry.DataRef, tileSet.Bitmap);

            return tileMan.Create(entryId, texture.Id, tileSet.TileSize, tileSet.Tiles.Select(tile => new Point(tile.Rectangle.X, tile.Rectangle.Y)).ToArray()); ;
        }

        #endregion Public Methods
    }
}