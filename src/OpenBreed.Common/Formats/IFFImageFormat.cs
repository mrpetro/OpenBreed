using OpenBreed.Common.DataSources;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Readers.Images.IFF;
using OpenBreed.Database.Interface.Items.Assets;
using OpenBreed.Model.Images;
using System;
using System.Collections.Generic;
using System.IO;

namespace OpenBreed.Common.Formats
{
    public class IFFImageFormat : IDataFormatType
    {
        #region Private Fields

        private readonly IBitmapProvider bitmapProvider;

        #endregion Private Fields

        #region Public Constructors

        public IFFImageFormat(IBitmapProvider bitmapProvider)
        {
            this.bitmapProvider = bitmapProvider;
        }

        #endregion Public Constructors

        #region Public Methods

        public object Load(DataSourceBase ds, List<FormatParameter> parameters)
        {
            //Remember to set source stream to begining
            ds.Stream.Seek(0, SeekOrigin.Begin);

            var imageBuilder = ImageBuilder.NewImage(bitmapProvider);
            var reader = new LBMImageReader(imageBuilder);
            return reader.Read(ds.Stream);
        }

        public void Save(DataSourceBase ds, object model, List<FormatParameter> parameters)
        {
            throw new NotImplementedException("LBMImageFormat Write");
        }

        #endregion Public Methods
    }
}