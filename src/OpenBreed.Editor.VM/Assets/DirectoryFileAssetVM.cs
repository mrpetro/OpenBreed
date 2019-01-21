using OpenBreed.Common.Assets;
using OpenBreed.Common.XmlDatabase.Items.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Assets
{
    public class DirectoryFileAssetVM : AssetVM
    {
        #region Private Fields

        private string _directoryPath;
        private string _fileName;

        #endregion Private Fields

        #region Public Properties

        public string DirectoryPath
        {
            get { return _directoryPath; }
            set { SetProperty(ref _directoryPath, value); }
        }

        public string FileName
        {
            get { return _fileName; }
            set { SetProperty(ref _fileName, value); }
        }

        #endregion Public Properties

        #region Internal Methods

        internal override void Load(IAssetEntry source)
        {
            base.Load(source);
            Load((IDirectoryFileAssetEntry)source);
        }

        #endregion Internal Methods

        #region Private Methods

        private void Load(IDirectoryFileAssetEntry source)
        {
            DirectoryPath = source.DirectoryPath;
            FileName = source.FileName;
        }

        #endregion Private Methods
    }
}
