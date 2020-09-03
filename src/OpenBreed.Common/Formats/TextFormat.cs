using OpenBreed.Common.Builders.Texts;
using OpenBreed.Common.DataSources;
using OpenBreed.Common.Model;
using OpenBreed.Common.Model.Texts;
using OpenBreed.Database.Interface.Items.Assets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpenBreed.Common.Formats
{
    public class TextFormat : IDataFormatType
    {
        #region Public Constructors

        public TextFormat()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public object Load(DataSourceBase ds, List<FormatParameter> parameters)
        {
            //Remember to set source stream to begining
            ds.Stream.Seek(0, SeekOrigin.Begin);

            using (var sr = new StreamReader(ds.Stream, Encoding.UTF8, true, 1024 , true))
            {
                var builder = TextBuilder.NewTextModel();
                builder.Text = sr.ReadToEnd();
                return builder.Build();
            }
        }

        public void Save(DataSourceBase ds, object model, List<FormatParameter> parameters)
        {
            if (ds.Stream == null)
                throw new InvalidOperationException("Asset stream not opened.");

            //Remember to clear the stream before writing
            ds.Stream.SetLength(0);

            using (var sw = new StreamWriter(ds.Stream, Encoding.UTF8, 1024, true))
            {
                sw.Write(((TextModel)model).Text);
            }
        }

        #endregion Public Methods
    }
}