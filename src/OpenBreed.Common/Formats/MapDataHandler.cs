using OpenBreed.Common.Data;
using OpenBreed.Common.DataSources;
using OpenBreed.Common.Extensions;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Readers.Images.IFF;
using OpenBreed.Database.Interface.Items.Assets;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Model.Images;
using OpenBreed.Model.Maps;
using OpenBreed.Model.Sounds;
using OpenBreed.Reader.Legacy.Images.ACBM;
using OpenBreed.Reader.Legacy.Maps.MAP;
using OpenBreed.Writer.Legacy.Maps.MAP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace OpenBreed.Common.Formats
{
    public class MapDataHandler : AssetDataHandlerBase<IDbMap>
    {
        #region Private Fields

        private readonly IBitmapProvider bitmapProvider;
        private readonly IDrawingFactory drawingFactory;
        private readonly DataSourceProvider dataSourceProvider;

        #endregion Private Fields

        #region Public Constructors

        public MapDataHandler(
            IBitmapProvider bitmapProvider,
            IDrawingFactory drawingFactory,
            DataSourceProvider dataSourceProvider)
        {
            this.bitmapProvider = bitmapProvider;
            this.drawingFactory = drawingFactory;
            this.dataSourceProvider = dataSourceProvider;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void Save(IDbMap dbEntry, object model)
        {
            var ds = dataSourceProvider.GetDataSource(dbEntry.DataRef);

            if (ds is null)
            {
                throw new Exception($"Unknown DataSourceRef '{dbEntry.DataRef}'.");
            }

            Save(ds, dbEntry.Format.ToMapFormat(), model);
        }

        protected override object Load(IDbMap dbEntry)
        {
            var ds = dataSourceProvider.GetDataSource(dbEntry.DataRef);

            if (ds is null)
            {
                throw new Exception($"Unknown DataSourceRef '{dbEntry.DataRef}'.");
            }

            return Load(ds, dbEntry.Format.ToMapFormat());
        }

        #endregion Protected Methods

        #region Private Methods

        private void Save(DataSourceBase ds, MAPFormat mapFormat, object model)
        {
            if (ds.Stream is null)
            {
                throw new InvalidOperationException("Asset stream not opened.");
            }

            //Remember to clear the stream before writing
            ds.Stream.SetLength(0);

            var mapWriter = new MAPWriter(ds.Stream, mapFormat);
            mapWriter.Write((MapModel)model);
        }

        private object Load(DataSourceBase ds, MAPFormat mapFormat)
        {
            //Remember to set source stream to beginning
            ds.Stream.Seek(0, SeekOrigin.Begin);

            var mapBuilder = MapBuilder.NewMapModel(drawingFactory);
            var mapReader = new MAPReader(mapBuilder, mapFormat);
            return mapReader.Read(ds.Stream);
        }

        #endregion Private Methods
    }
}