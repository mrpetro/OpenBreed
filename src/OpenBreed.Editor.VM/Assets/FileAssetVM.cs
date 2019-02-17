using OpenBreed.Common;
using OpenBreed.Common.Assets;
using OpenBreed.Common.XmlDatabase.Items.Assets;
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

        internal override void FromEntry(IEntry entry)
        {
            base.FromEntry(entry);
            FromEntry((IFileAssetEntry)entry);
        }

        internal override void ToEntry(IEntry entry)
        {
            base.ToEntry(entry);
            ToEntry((IFileAssetEntry)entry);
        }

        #endregion Internal Methods

        #region Private Methods

        private void FromEntry(IFileAssetEntry source)
        {
            FilePath = source.FilePath;
        }

        private void ToEntry(IFileAssetEntry source)
        {
            source.FilePath = FilePath;
        }

        #endregion Private Methods

    }
}
