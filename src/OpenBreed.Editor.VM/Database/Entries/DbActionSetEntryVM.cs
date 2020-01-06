using OpenBreed.Common;
using OpenBreed.Common.Actions;
using System;

namespace OpenBreed.Editor.VM.Database.Entries
{
    public class DbActionSetEntryVM : DbEntryVM
    {
        #region Private Fields

        private IActionSetEntry _entry;

        #endregion Private Fields

        #region Public Constructors

        public DbActionSetEntryVM()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEntry Entry { get { return _entry; } }

        #endregion Public Properties

        #region Public Methods

        public override void Load(IEntry entry)
        {
            _entry = entry as IActionSetEntry ?? throw new InvalidOperationException($"Expected {nameof(IActionSetEntry)}");

            base.Load(entry);
        }

        #endregion Public Methods
    }
}