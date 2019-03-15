using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Common.Data;
using OpenBreed.Common.Formats;

namespace OpenBreed.Common.Assets
{
    public class FileAsset : AssetBase
    {

        #region Private Fields

        public string FilePath { get; }

        #endregion Private Fields

        #region Public Constructors

        public FileAsset(AssetsDataProvider manager, string id, IDataFormatType formatType, List<FormatParameter> formatParameters, string filePath) :
            base(manager, id, formatType, formatParameters)
        {
            FilePath = filePath;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void Close()
        {
            Stream.Close();

            base.Close();
        }

        protected override Stream CreateStream()
        {
            string filePath = AssetsDataProvider.ExpandVariables(FilePath);
            return File.Open(filePath, FileMode.Open, FileAccess.ReadWrite);
        }

        #endregion Protected Methods

    }
}
