using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Logging;
using OpenBreed.Common.Tools;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Model.Palettes;
using OpenBreed.Model.Tiles;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Data;
using OpenBreed.Rendering.Interface.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.OpenGL.Data
{
    internal class TileAtlasDataLoader : ITileAtlasDataLoader
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;
        private readonly AssetsDataProvider assetsDataProvider;
        private readonly ITextureMan textureMan;
        private readonly ITileMan tileMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public TileAtlasDataLoader(IRepositoryProvider repositoryProvider,
                                 AssetsDataProvider assetsDataProvider,
                                 ITextureMan textureMan,
                                 ITileMan tileMan,
                                 ILogger logger)
        {
            this.repositoryProvider = repositoryProvider;
            this.assetsDataProvider = assetsDataProvider;
            this.textureMan = textureMan;
            this.tileMan = tileMan;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public object LoadObject(string entryId) => Load(entryId);

        public ITileAtlas Load(string tileAtlasName, params object[] args)
        {
            var tileAtlas = tileMan.GetByName(tileAtlasName);

            //If atlas is already loaded then return it;
            if (tileAtlas != null)
                return tileAtlas;

            var paletteModel = args.FirstOrDefault() as PaletteModel;

            var entry = repositoryProvider.GetRepository<IDbTileAtlas>().GetById(tileAtlasName) as IDbTileAtlasFromBlk;
            if (entry == null)
                throw new Exception("Tile atlas error: " + tileAtlasName);

            var tileAtlasModel = assetsDataProvider.LoadModel(entry.DataRef) as TileSetModel;

            if (paletteModel != null)
                BitmapHelper.SetPaletteColors(tileAtlasModel.Bitmap, paletteModel.Data);

            var texture = textureMan.Create(entry.DataRef, tileAtlasModel.Bitmap);

            var builder = tileMan.CreateAtlas()
                                 .SetName(tileAtlasName)
                                 .SetTexture(texture.Id)
                                 .SetTileSize(tileAtlasModel.TileSize);

            foreach (var tile in tileAtlasModel.Tiles)
                builder.AppendCoords(tile.Rectangle.X, tile.Rectangle.Y);

            tileAtlas = builder.Build();

            logger.Verbose($"Tile atlas '{tileAtlasName}' loaded.");

            return tileAtlas;
        }

        #endregion Public Methods
    }
}
