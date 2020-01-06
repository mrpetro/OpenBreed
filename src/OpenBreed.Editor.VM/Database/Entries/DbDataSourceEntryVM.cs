using OpenBreed.Common;
using OpenBreed.Common.DataSources;
using System;

namespace OpenBreed.Editor.VM.Database.Entries
{
    public class DbDataSourceEntryVM : DbEntryVM
    {
        #region Private Fields

        private IDataSourceEntry _entry;

        #endregion Private Fields

        #region Public Constructors

        public DbDataSourceEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEntry Entry { get { return _entry; } }

        #endregion Public Properties

        #region Public Methods

        public override void Load(IEntry entry)
        {
            _entry = entry as IDataSourceEntry ?? throw new InvalidOperationException($"Expected {nameof(IDataSourceEntry)}");

            base.Load(entry);
        }

        #endregion Public Methods
    }
}