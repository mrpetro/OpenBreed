using OpenBreed.Common.DataSources;
using OpenBreed.Common.Maps;
using OpenBreed.Common.Maps.Builders;
using OpenBreed.Common.Maps.Readers.MAP;
using OpenBreed.Common.Maps.Writers.MAP;
using OpenBreed.Database.Interface.Items.Assets;
using System;
using System.Collections.Generic;
using System.IO;

namespace OpenBreed.Common.Formats
{
    public class ABSEMAPFormat : IDataFormatType
    {
        #region Public Constructors

        public ABSEMAPFormat()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public object Load(DataSourceBase ds, List<FormatParameter> parameters)
        {
            //Remember to set source stream to begining
            ds.Stream.Seek(0, SeekOrigin.Begin);

            var mapBuilder = MapBuilder.NewMapModel();
            MAPReader mapReader = new MAPReader(mapBuilder, MAPFormat.ABSE);
            return mapReader.Read(ds.Stream);
        }

        public void Save(DataSourceBase ds, object model, List<FormatParameter> parameters)
        {
            if (ds.Stream == null)
                throw new InvalidOperationException("Source stream not opened.");

            //Remember to clear the stream before writing
            ds.Stream.SetLength(0);

            MAPWriter mapWriter = new MAPWriter(ds.Stream, MAPFormat.ABSE);
            mapWriter.Write((MapModel)model);
        }

        #endregion Public Methods
    }
}