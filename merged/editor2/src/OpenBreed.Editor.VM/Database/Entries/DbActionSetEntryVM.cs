using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Actions;
using System;

namespace OpenBreed.Editor.VM.Database.Entries
{
    public class DbActionSetEntryVM : DbEntryVM
    {
        #region Private Fields

        private IDbActionSet _entry;

        #endregion Private Fields

        #region Public Constructors

        public DbActionSetEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override IDbEntry Entry { get { return _entry; } }

        #endregion Public Properties

        #region Public Methods

        public override void Load(IDbEntry entry)
        {
            _entry = entry as IDbActionSet ?? throw new InvalidOperationException($"Expected {nameof(IDbActionSet)}");

            base.Load(entry);
        }

        #endregion Public Methods
    }
}