using OpenBreed.Common.DataSources;
using OpenBreed.Common.Model.Tiles.Builders;
using OpenBreed.Common.Tiles.Readers.ACBM;
using OpenBreed.Database.Interface.Items.Assets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OpenBreed.Common.Formats
{
    public class ACBMTileSetFormat : IDataFormatType
    {
        #region Public Constructors

        public ACBMTileSetFormat()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public object Load(DataSourceBase ds, List<FormatParameter> parameters)
        {
            var tileSize = (int)parameters.FirstOrDefault(item => item.Name == "TILE_SIZE").Value;
            var bitPlanesNo = (int)parameters.FirstOrDefault(item => item.Name == "BIT_PLANES_NO").Value;

            //Remember to set source stream to begining
            ds.Stream.Seek(0, SeekOrigin.Begin);

            var tileSetBuilder = TileSetBuilder.NewTileSet();
            var reader = new ACBMTileSetReader(tileSetBuilder, tileSize, bitPlanesNo);
            return reader.Read(ds.Stream);
        }

        public void Save(DataSourceBase ds, object model, List<FormatParameter> parameters)
        {
            throw new NotImplementedException("ACBMTileSet Write");
        }

        #endregion Public Methods
    }
}