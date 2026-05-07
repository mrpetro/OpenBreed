using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.EntityClasses;
using System;

namespace OpenBreed.Editor.VM.Database.Entries
{
    public class DbEntityClassEntryVM : DbEntryVM
    {
        #region Private Fields

        private IDbEntityClass _entry;

        #endregion Private Fields

        #region Public Constructors

        public DbEntityClassEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override IDbEntry Entry { get { return _entry; } }

        #endregion Public Properties

        #region Public Methods

        public override void Load(IDbEntry entry)
        {
            _entry = entry as IDbEntityClass ?? throw new InvalidOperationException($"Expected {nameof(IDbEntityClass)}");

            base.Load(entry);
        }

        #endregion Public Methods
    }
}