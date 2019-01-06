using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace OpenBreed.Common.Assets
{
    public class DirectoryFileAsset : AssetBase
    {

        #region Private Fields

        public string DirectoryPath { get; }

        #endregion Private Fields

        #region Public Constructors

        public DirectoryFileAsset(AssetsDataProvider manager, string directoryPath, string name) :
            base(manager, name)
        {
            DirectoryPath = directoryPath;
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
            string filePath = Path.Combine(AssetsDataProvider.ExpandVariables(DirectoryPath), Name);
            return File.Open(filePath, FileMode.Open, FileAccess.ReadWrite);
        }

        #endregion Protected Methods

    }
}
