using OpenBreed.Common.Data;
using OpenBreed.Common.DataSources;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Readers.Images.IFF;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Model.Images;
using OpenBreed.Model.Sounds;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace OpenBreed.Common.Formats
{
    public class IffImageDataHandler : AssetDataHandlerBase<IDbIffImage>
    {
        #region Private Fields

        private readonly IBitmapProvider bitmapProvider;

        private readonly DataSourceProvider dataSourceProvider;

        #endregion Private Fields

        #region Public Constructors

        public IffImageDataHandler(
            IBitmapProvider bitmapProvider,
            DataSourceProvider dataSourceProvider)
        {
            this.bitmapProvider = bitmapProvider;
            this.dataSourceProvider = dataSourceProvider;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Save(IDbIffImage dbEntry, object model)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override object Load(IDbIffImage dbEntry)
        {
            var ds = dataSourceProvider.GetDataSource(dbEntry.DataRef);

            if (ds is null)
            {
                throw new Exception($"Unknown DataSourceRef '{dbEntry.DataRef}'.");
            }

            return Load(ds);
        }

        #endregion Protected Methods

        #region Private Methods

        private object Load(DataSourceBase ds)
        {
            //Remember to set source stream to begining
            ds.Stream.Seek(0, SeekOrigin.Begin);

            var imageBuilder = ImageBuilder.NewImage(bitmapProvider);
            var reader = new LBMImageReader(imageBuilder);
            return reader.Read(ds.Stream);
        }

        #endregion Private Methods
    }
}