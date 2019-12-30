using OpenBreed.Common.Data;
using System.IO;

namespace OpenBreed.Common.DataSources
{
    public class FileDataSource : DataSourceBase
    {
        #region Public Constructors

        public FileDataSource(DataSourceProvider provider, string id, string filePath) :
            base(provider, id)
        {
            FilePath = filePath;
        }

        #endregion Public Constructors

        #region Public Properties

        public string FilePath { get; }

        #endregion Public Properties

        #region Protected Methods

        protected override void Close()
        {
            Stream.Close();

            base.Close();
        }

        protected override Stream CreateStream()
        {
            string filePath = DataSourceProvider.ExpandVariables(FilePath);
            return File.Open(filePath, FileMode.Open, FileAccess.ReadWrite);
        }

        #endregion Protected Methods
    }
}