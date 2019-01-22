using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace OpenBreed.Common.Assets
{
    public class FileAsset : AssetBase
    {

        #region Private Fields

        public string FilePath { get; }

        #endregion Private Fields

        #region Public Constructors

        public FileAsset(AssetsDataProvider manager, string name, string filePath) :
            base(manager, name)
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
