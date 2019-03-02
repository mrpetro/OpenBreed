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
    public class ABTAMAPFormat : IDataFormatType
    {
        #region Public Constructors

        public ABTAMAPFormat()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public object Load(AssetBase asset, List<FormatParameter> parameters)
        {
            //Remember to set source stream to begining
            asset.Stream.Seek(0, SeekOrigin.Begin);

            var mapBuilder = MapBuilder.NewMapModel();
            MAPReader mapReader = new MAPReader(mapBuilder, MAPFormat.ABTA);
            return mapReader.Read(asset.Stream);
        }

        public void Save(AssetBase asset, object model, List<FormatParameter> parameters)
        {
            if (asset.Stream == null)
                throw new InvalidOperationException("Asset stream not opened.");

            //Remember to clear the stream before writing
            asset.Stream.SetLength(0);

            MAPWriter mapWriter = new MAPWriter(asset.Stream, MAPFormat.ABTA);
            mapWriter.Write((MapModel)model);
        }

        #endregion Public Methods
    }
}