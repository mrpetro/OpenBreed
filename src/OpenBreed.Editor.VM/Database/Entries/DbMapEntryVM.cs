using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Maps;
using System;

namespace OpenBreed.Editor.VM.Database.Entries
{
    public class DbMapEntryVM : DbEntryVM
    {
        #region Private Fields

        private IDbMap _entry;

        #endregion Private Fields

        #region Public Constructors

        public DbMapEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override IDbEntry Entry { get { return _entry; } }

        #endregion Public Properties

        #region Public Methods

        public override void Load(IDbEntry entry)
        {
            _entry = entry as IDbMap ?? throw new InvalidOperationException($"Expected {nameof(IDbMap)}");

            base.Load(entry);
        }

        #endregion Public Methods
    }
}