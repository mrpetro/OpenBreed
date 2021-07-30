using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Tools;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.TileStamps;
using OpenBreed.Model.Palettes;
using OpenBreed.Model.Tiles;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using System;
using System.Drawing;
using System.Linq;

namespace OpenBreed.Sandbox.Loaders
{
    internal class TileStampDataLoader : IDataLoader<ITileStamp>
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;
        private readonly AssetsDataProvider assetsDataProvider;
        private readonly ITextureMan textureMan;
        private readonly IStampMan stampMan;
        private readonly ITileMan tileMan;

        #endregion Private Fields

        #region Public Constructors

        public TileStampDataLoader(IRepositoryProvider repositoryProvider,
                                 AssetsDataProvider assetsDataProvider,
                                 ITextureMan textureMan,
                                 IStampMan stampMan,
                                 ITileMan tileMan)
        {
            this.repositoryProvider = repositoryProvider;
            this.assetsDataProvider = assetsDataProvider;
            this.textureMan = textureMan;
            this.stampMan = stampMan;
            this.tileMan = tileMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public object LoadObject(string entryId) => Load(entryId);

        public ITileStamp Load(string entryId, params object[] args)
        {
            var entry = repositoryProvider.GetRepository<ITileStampEntry>().GetById(entryId);
            if (entry == null)
                throw new Exception("Tilestamp error: " + entryId);

            var stampBuilder = stampMan.Create();

            stampBuilder.ClearTiles();
            stampBuilder.SetName(entry.Id);
            stampBuilder.SetSize(entry.Width, entry.Height);
            stampBuilder.SetOrigin(entry.CenterX, entry.CenterY);

            foreach (var cell in entry.Cells)
            {
                var ts = tileMan.GetByAlias(cell.TsId);
                stampBuilder.AddTile(cell.X, cell.Y, ts.Id, cell.TsTi);
            }

            return stampBuilder.Build();
        }

        #endregion Public Methods
    }
}