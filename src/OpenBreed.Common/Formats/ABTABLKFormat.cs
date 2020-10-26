using OpenBreed.Common.DataSources;
using OpenBreed.Database.Interface.Items.Assets;
using System;
using System.Collections.Generic;
using System.IO;
using OpenBreed.Model.Tiles;
using OpenBreed.Reader.Legacy.Tiles.BLK;

namespace OpenBreed.Common.Formats
{
    public class ABTABLKFormat : IDataFormatType
    {
        #region Public Constructors

        public ABTABLKFormat()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public object Load(DataSourceBase ds, List<FormatParameter> parameters)
        {
            //Remember to set source stream to begining
            ds.Stream.Seek(0, SeekOrigin.Begin);

            var tileSetBuilder = TileSetBuilder.NewTileSet();
            var blkReader = new BLKReader(tileSetBuilder);
            return blkReader.Read(ds.Stream);
        }

        public void Save(DataSourceBase ds, object model, List<FormatParameter> parameters)
        {
            throw new NotImplementedException("ABTABLK Write");
        }

        #endregion Public Methods
    }
}