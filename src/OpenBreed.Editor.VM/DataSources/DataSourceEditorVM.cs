using OpenBreed.Common;
using OpenBreed.Common.DataSources;
using System;

namespace OpenBreed.Editor.VM.DataSources
{
    public class DataSourceEditorVM : EntryEditorBaseVM<IDataSourceEntry, DataSourceVM>
    {
        #region Public Constructors

        public DataSourceEditorVM(IRepository repository) : base(repository)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override string EditorName { get { return "Data Source Editor"; } }

        #endregion Public Properties

        #region Protected Methods

        protected override DataSourceVM CreateVM(IDataSourceEntry entry)
        {
            if (entry is IFileDataSourceEntry)
                return new FileDataSourceVM();
            else if (entry is IEPFArchiveDataSourceEntry)
                return new EPFArchiveFileDataSourceVM();
            else
                throw new NotImplementedException();
        }

        protected override void UpdateEntry(DataSourceVM source, IDataSourceEntry target)
        {
            base.UpdateEntry(source, target);
        }

        protected override void UpdateVM(IDataSourceEntry source, DataSourceVM target)
        {
            base.UpdateVM(source, target);
        }

        #endregion Protected Methods
    }
}