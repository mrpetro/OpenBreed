using OpenBreed.Common.Data;
using OpenBreed.Common.DataSources;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Readers.Images.IFF;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Database.Interface.Items.Texts;
using OpenBreed.Model.Images;
using OpenBreed.Model.Sounds;
using OpenBreed.Model.Texts;
using OpenBreed.Reader.Legacy.Images.ACBM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace OpenBreed.Common.Formats
{
    public class FileTextDataHandler : AssetDataHandlerBase<IDbTextFromFile>
    {
        #region Private Fields

        private readonly DataSourceProvider dataSourceProvider;

        #endregion Private Fields

        #region Public Constructors

        public FileTextDataHandler(
            DataSourceProvider dataSourceProvider)
        {
            this.dataSourceProvider = dataSourceProvider;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void Save(IDbTextFromFile dbEntry, object model)
        {
            var ds = dataSourceProvider.GetDataSource(dbEntry.DataRef);

            if (ds is null)
            {
                throw new Exception($"Unknown DataSourceRef '{dbEntry.DataRef}'.");
            }

            Save(ds, model);
        }

        protected override object Load(IDbTextFromFile dbEntry)
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
            ds.Stream.Seek(0, SeekOrigin.Begin);

            using (var sr = new StreamReader(ds.Stream, Encoding.UTF8, true, 1024, true))
            {
                var builder = TextBuilder.NewTextModel();
                builder.SetText(sr.ReadToEnd());
                return builder.Build();
            }
        }

        private void Save(DataSourceBase ds, object model)
        {
            if (ds.Stream is null)
            {
                throw new InvalidOperationException("Data source stream not opened.");
            }

            //Remember to clear the stream before writing
            ds.Stream.SetLength(0);

            using (var sw = new StreamWriter(ds.Stream, Encoding.UTF8, 1024, true))
            {
                sw.Write(((TextModel)model).Text);
            }
        }

        #endregion Private Methods
    }
}