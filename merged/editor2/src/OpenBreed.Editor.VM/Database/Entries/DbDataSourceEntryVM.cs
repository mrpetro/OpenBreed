using OpenBreed.Common;
using OpenBreed.Common.DataSources;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.DataSources;
using System;

namespace OpenBreed.Editor.VM.Database.Entries
{
    public class DbDataSourceEntryVM : DbEntryVM
    {
        #region Private Fields

        private IDbDataSource _entry;

        #endregion Private Fields

        #region Public Constructors

        public DbDataSourceEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override IDbEntry Entry { get { return _entry; } }

        #endregion Public Properties

        #region Public Methods

        public override void Load(IDbEntry entry)
        {
            _entry = entry as IDbDataSource ?? throw new InvalidOperationException($"Expected {nameof(IDbDataSource)}");

            base.Load(entry);
        }

        #endregion Public Methods
    }
}