﻿using Microsoft.Extensions.Logging;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Logging;
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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.OpenGL.Data
{
    internal class TileAtlasDataLoader : ITileAtlasDataLoader
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;
        private readonly TileAtlasDataProvider tileAtlasDataProvider;
        private readonly ITextureMan textureMan;
        private readonly ITileMan tileMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public TileAtlasDataLoader(IRepositoryProvider repositoryProvider,
                                 TileAtlasDataProvider tileAtlasDataProvider,
                                 ITextureMan textureMan,
                                 ITileMan tileMan,
                                 ILogger logger)
        {
            this.repositoryProvider = repositoryProvider;
            this.tileAtlasDataProvider = tileAtlasDataProvider;
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
            if (tileAtlas is not null)
            {
                return tileAtlas;
            }

            var entry = repositoryProvider.GetRepository<IDbTileAtlas>().GetById(tileAtlasName) as IDbTileAtlasFromBlk;

            if (entry is null)
            {
                throw new Exception("Tile atlas error: " + tileAtlasName);
            }

            var tileAtlasModel = tileAtlasDataProvider.GetTileAtlas(entry);

            var textureWidth = tileAtlasModel.TilesNoX * tileAtlasModel.TileSize;
            var textureHeight = tileAtlasModel.TilesNoY * tileAtlasModel.TileSize;

            var texture = textureMan.Create(
                entry.DataRef,
                textureWidth,
                textureHeight,
                tileAtlasModel.Bitmap);

            var builder = tileMan.CreateAtlas()
                                 .SetName(tileAtlasName)
                                 .SetTexture(texture.Id)
                                 .SetTileSize(tileAtlasModel.TileSize);

            foreach (var tile in tileAtlasModel.Tiles)
                builder.AppendCoords(tile.Rectangle.X, tile.Rectangle.Y);

            tileAtlas = builder.Build();

            logger.LogTrace("Tile atlas '{0}' loaded.", tileAtlasName);

            return tileAtlas;
        }

        #endregion Public Methods
    }
}
