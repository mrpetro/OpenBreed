using OpenBreed.Common.Assets;
using OpenBreed.Common.XmlDatabase.Items.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Assets
{
    public class FileAssetVM : AssetVM
    {
        #region Private Fields

        private string _filePath;

        #endregion Private Fields

        #region Public Properties

        public string FilePath
        {
            get { return _filePath; }
            set { SetProperty(ref _filePath, value); }
        }

        #endregion Public Properties

        #region Internal Methods

        internal override void Load(IAssetEntry source)
        {
            base.Load(source);
            Load((IFileAssetEntry)source);
        }

        #endregion Internal Methods

        #region Private Methods

        private void Load(IFileAssetEntry source)
        {
            FilePath = source.FilePath;
        }

        #endregion Private Methods
    }
}
