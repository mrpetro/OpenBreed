using OpenBreed.Common.DataSources;
using OpenBreed.Model.Maps;
using OpenBreed.Database.Interface.Items.Assets;
using System;
using System.Collections.Generic;
using System.IO;
using OpenBreed.Reader.Legacy.Maps.MAP;
using OpenBreed.Writer.Legacy.Maps.MAP;
using OpenBreed.Common.Interface.Drawing;

namespace OpenBreed.Common.Formats
{
    public class ABHCMAPFormat : IDataFormatType
    {
        #region Private Fields

        private readonly IDrawingFactory drawingFactory;

        #endregion Private Fields

        #region Public Constructors

        public ABHCMAPFormat(IDrawingFactory drawingFactory)
        {
            this.drawingFactory = drawingFactory;
        }

        #endregion Public Constructors

        #region Public Methods

        public object Load(DataSourceBase ds, List<FormatParameter> parameters)
        {
            //Remember to set source stream to begining
            ds.Stream.Seek(0, SeekOrigin.Begin);

            var mapBuilder = MapBuilder.NewMapModel(drawingFactory);
            var mapReader = new MAPReader(mapBuilder, MAPFormat.ABHC);
            return mapReader.Read(ds.Stream);
        }

        public void Save(DataSourceBase ds, object model, List<FormatParameter> parameters)
        {
            if (ds.Stream == null)
                throw new InvalidOperationException("Asset stream not opened.");

            //Remember to clear the stream before writing
            ds.Stream.SetLength(0);

            var mapWriter = new MAPWriter(ds.Stream, MAPFormat.ABHC);
            mapWriter.Write((MapModel)model);
        }

        #endregion Public Methods
    }
}