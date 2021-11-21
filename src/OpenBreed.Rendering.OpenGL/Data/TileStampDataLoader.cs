using OpenBreed.Common;
using OpenBreed.Common.Data;
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
        private readonly ITileMan tileMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public TileStampDataLoader(IRepositoryProvider repositoryProvider,
                                 AssetsDataProvider assetsDataProvider,
                                 ITextureMan textureMan,
                                 IStampMan stampMan,
                                 ITileMan tileMan,
                                 ILogger logger)
        {
            this.repositoryProvider = repositoryProvider;
            this.assetsDataProvider = assetsDataProvider;
            this.textureMan = textureMan;
            this.stampMan = stampMan;
            this.tileMan = tileMan;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public object LoadObject(string entryId) => Load(entryId);

        public ITileStamp Load(string tileStampName, params object[] args)
        {
            var entry = repositoryProvider.GetRepository<IDbTileStamp>().GetById(tileStampName);
            if (entry == null)
                throw new Exception("Tilestamp error: " + tileStampName);

            var stampBuilder = stampMan.Create();

            stampBuilder.ClearTiles();
            stampBuilder.SetName(entry.Id);
            stampBuilder.SetSize(entry.Width, entry.Height);
            stampBuilder.SetOrigin(entry.CenterX, entry.CenterY);

            foreach (var cell in entry.Cells)
            {
                var ts = tileMan.GetByName(cell.TsId);
                stampBuilder.AddTile(cell.X, cell.Y, ts.Id, cell.TsTi);
            }

            var tileStamp = stampBuilder.Build();

            logger.Verbose($"Tile stamp '{tileStampName}' loaded.");

            return tileStamp;
        }

        #endregion Public Methods
    }
}
