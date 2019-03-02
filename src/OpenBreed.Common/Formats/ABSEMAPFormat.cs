using OpenBreed.Common.Assets;
using OpenBreed.Common.Maps;
using OpenBreed.Common.Maps.Builders;
using OpenBreed.Common.Maps.Readers.MAP;
using OpenBreed.Common.Maps.Writers.MAP;
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

        public object Load(AssetBase source, List<FormatParameter> parameters)
        {
            //Remember to set source stream to begining
            source.Stream.Seek(0, SeekOrigin.Begin);

            var mapBuilder = MapBuilder.NewMapModel();
            MAPReader mapReader = new MAPReader(mapBuilder, MAPFormat.ABSE);
            return mapReader.Read(source.Stream);
        }

        public void Save(AssetBase source, object model, List<FormatParameter> parameters)
        {
            if (source.Stream == null)
                throw new InvalidOperationException("Source stream not opened.");

            //Remember to clear the stream before writing
            source.Stream.SetLength(0);

            MAPWriter mapWriter = new MAPWriter(source.Stream, MAPFormat.ABSE);
            mapWriter.Write((MapModel)model);
        }

        #endregion Public Methods

    }
}