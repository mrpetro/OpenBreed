using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.TileStamps;
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
    internal class TileStampDataLoader : ITileStampDataLoader
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;
        private readonly AssetsDataProvider assetsDataProvider;
        private readonly ITextureMan textureMan;
        private readonly IStampMan stampMan;
        private readonly ITileAtlasDataLoader tileAtlasDataLoader;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public TileStampDataLoader(IRepositoryProvider repositoryProvider,
                                 AssetsDataProvider assetsDataProvider,
                                 ITextureMan textureMan,
                                 IStampMan stampMan,
                                 ITileAtlasDataLoader tileAtlasDataLoader,
                                 ILogger logger)
        {
            this.repositoryProvider = repositoryProvider;
            this.assetsDataProvider = assetsDataProvider;
            this.textureMan = textureMan;
            this.stampMan = stampMan;
            this.tileAtlasDataLoader = tileAtlasDataLoader;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public ITileStamp Load(IDbTileStamp dbTileStamp)
        {
            if (stampMan.TryGetByName(dbTileStamp.Id, out ITileStamp tileStamp))
            {
                return tileStamp;
            }

            var stampBuilder = stampMan.Create();

            stampBuilder.ClearTiles();
            stampBuilder.SetName(dbTileStamp.Id);
            stampBuilder.SetSize(dbTileStamp.Width, dbTileStamp.Height);
            stampBuilder.SetOrigin(dbTileStamp.CenterX, dbTileStamp.CenterY);

            foreach (var cell in dbTileStamp.Cells)
            {
                var ts = tileAtlasDataLoader.Load(cell.TsId);
                stampBuilder.AddTile(cell.X, cell.Y, ts.Id, cell.TsTi);
            }

            tileStamp = stampBuilder.Build();

            logger.LogTrace("Tile stamp '{0}' loaded.", dbTileStamp.Id);

            return tileStamp;
        }

        public ITileStamp Load(string dbEntryId)
        {
            if (stampMan.TryGetByName(dbEntryId, out ITileStamp tileStamp))
            {
                return tileStamp;
            }

            var entry = repositoryProvider.GetRepository<IDbTileStamp>().GetById(dbEntryId);

            if (entry is null)
            {
                throw new Exception("Tilestamp error: " + dbEntryId);
            }

            return Load(entry);
        }

        #endregion Public Methods
    }
}
