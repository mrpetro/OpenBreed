using OpenBreed.Common.Assets;
using OpenBreed.Common.XmlDatabase.Items.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Assets
{
    public class EPFArchiveFileAssetVM : AssetVM
    {

        #region Private Fields

        private string _archivePath;
        private string _entryName;

        #endregion Private Fields

        #region Public Properties

        public string ArchivePath
        {
            get { return _archivePath; }
            set { SetProperty(ref _archivePath, value); }
        }

        public string EntryName
        {
            get { return _entryName; }
            set { SetProperty(ref _entryName, value); }
        }

        #endregion Public Properties

        #region Internal Methods

        internal override void Load(IAssetEntry source)
        {
            base.Load(source);
            Load((EPFArchiveFileAssetDef)source);
        }

        #endregion Internal Methods

        #region Private Methods

        private void Load(EPFArchiveFileAssetDef source)
        {
            ArchivePath = source.ArchivePath;
            EntryName = source.EntryName;
        }

        #endregion Private Methods

    }
}
