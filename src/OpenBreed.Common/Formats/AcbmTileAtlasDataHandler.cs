using OpenBreed.Common.Data;
using OpenBreed.Common.DataSources;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Readers.Images.IFF;
using OpenBreed.Database.Interface.Items.Assets;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Model.Images;
using OpenBreed.Model.Sounds;
using OpenBreed.Model.Tiles;
using OpenBreed.Reader.Legacy.Images.ACBM;
using OpenBreed.Reader.Legacy.Tiles.ACBM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace OpenBreed.Common.Formats
{
    public class AcbmTileAtlasDataHandler : AssetDataHandlerBase<IDbTileAtlasFromAcbm>
    {
        #region Private Fields

        private readonly IBitmapProvider bitmapProvider;

        private readonly DataSourceProvider dataSourceProvider;

        #endregion Private Fields

        #region Public Constructors

        public AcbmTileAtlasDataHandler(
            IBitmapProvider bitmapProvider,
            DataSourceProvider dataSourceProvider)
        {
            this.bitmapProvider = bitmapProvider;
            this.dataSourceProvider = dataSourceProvider;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void Save(IDbTileAtlasFromAcbm dbEntry, object model)
        {
            throw new NotImplementedException(nameof(Save));
        }

        protected override object Load(IDbTileAtlasFromAcbm dbEntry)
        {
            var ds = dataSourceProvider.GetDataSource(dbEntry.DataRef);

            if (ds is null)
            {
                throw new Exception($"Unknown DataSourceRef '{dbEntry.DataRef}'.");
            }

            return Load(ds, dbEntry.TileSize, dbEntry.BitPlanesNo);
        }

        #endregion Protected Methods

        #region Private Methods

        private object Load(DataSourceBase ds, int tileSize, int bitPlanesNo)
        {
            //Remember to set source stream to begining
            ds.Stream.Seek(0, SeekOrigin.Begin);

            var tileSetBuilder = TileSetBuilder.NewTileSet();
            var reader = new ACBMTileSetReader(tileSetBuilder, tileSize, bitPlanesNo);
            return reader.Read(ds.Stream);
        }

        #endregion Private Methods
    }
}