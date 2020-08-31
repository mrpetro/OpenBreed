using OpenBreed.Common.Assets;
using OpenBreed.Common.DataSources;
using OpenBreed.Common.Maps;
using OpenBreed.Common.Maps.Readers.MAP;
using OpenBreed.Common.Maps.Writers.MAP;
using OpenBreed.Common.Model.Maps;
using OpenBreed.Common.Model.Maps.Builders;
using OpenBreed.Database.Interface.Items.Assets;
using System;
using System.Collections.Generic;
using System.IO;

namespace OpenBreed.Common.Formats
{
    public class ABHCMAPFormat : IDataFormatType
    {
        #region Public Constructors

        public ABHCMAPFormat()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public object Load(DataSourceBase ds, List<FormatParameter> parameters)
        {
            //Remember to set source stream to begining
            ds.Stream.Seek(0, SeekOrigin.Begin);

            var mapBuilder = MapBuilder.NewMapModel();
            MAPReader mapReader = new MAPReader(mapBuilder, MAPFormat.ABHC);
            return mapReader.Read(ds.Stream);
        }

        public void Save(DataSourceBase ds, object model, List<FormatParameter> parameters)
        {
            if (ds.Stream == null)
                throw new InvalidOperationException("Asset stream not opened.");

            //Remember to clear the stream before writing
            ds.Stream.SetLength(0);

            MAPWriter mapWriter = new MAPWriter(ds.Stream, MAPFormat.ABHC);
            mapWriter.Write((MapModel)model);
        }

        #endregion Public Methods
    }
}