using OpenBreed.Common;
using OpenBreed.Common.DataSources;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.DataSources;

namespace OpenBreed.Editor.VM.DataSources
{
    public class FileDataSourceVM : DataSourceVM
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
            FromEntry((IFileDataSourceEntry)entry);
        }

        internal override void ToEntry(IEntry entry)
        {
            base.ToEntry(entry);
            ToEntry((IFileDataSourceEntry)entry);
        }

        #endregion Internal Methods

        #region Private Methods

        private void FromEntry(IFileDataSourceEntry source)
        {
            FilePath = source.FilePath;
        }

        private void ToEntry(IFileDataSourceEntry source)
        {
            source.FilePath = FilePath;
        }

        #endregion Private Methods
    }
}