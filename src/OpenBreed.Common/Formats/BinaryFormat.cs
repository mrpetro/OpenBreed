using OpenBreed.Common.DataSources;
using OpenBreed.Database.Interface.Items.Assets;
using OpenBreed.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpenBreed.Common.Formats
{
    public class BinaryFormat : IDataFormatType
    {
        #region Public Constructors

        public BinaryFormat()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public object Load(DataSourceBase ds, List<FormatParameter> parameters)
        {
            //Remember to set source stream to begining
            ds.Stream.Seek(0, SeekOrigin.Begin);

            using (var br = new BinaryReader(ds.Stream, Encoding.Default, true))
                return new BinaryModel(br.ReadBytes((int)ds.Stream.Length));
        }

        public void Save(DataSourceBase ds, object model, List<FormatParameter> parameters)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}